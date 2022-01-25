using StudentsDiary.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public partial class Main : Form
    {
        private FileHelper<List<Student>> fileHelper = new FileHelper<List<Student>>(Program.FilePath);
        private FileHelper<List<Group>> fileHelperGroup = new FileHelper<List<Group>>(Program.FilePathGroup);
        private int _studentId;
        private bool flIsGroupNameColumn = false;
        private List<Student> _listFromFile;
        private List<Group> _listGroupFromFile;

        public static Color FontColor
        {
            get
            {
                return Settings.Default.FontColor;
            }
            set
            {
                Settings.Default.FontColor = value;
            }
        }

        public bool IsMaximize
        {
            get
            {
                return Settings.Default.IsMaximize;
            }
            set
            {
                Settings.Default.IsMaximize = value;
            }
        }

        public Main()
        {
            InitializeComponent();
            RefreshDiary();
            //MokInitializeStudents();
            if (IsMaximize)
                WindowState = FormWindowState.Maximized;
            Text = "Dzienik uczniów";
            SetColumnLabel();
            Group grupa = new Group();
            grupa.MokGroup();
            SetGroupNameFromId();
        }

        private void RefreshDiary()
        {
            _listFromFile = fileHelper.DeserialisedFromFile();
            var listStudent = new List<Student>();
            if (cmbFiltrGroup.SelectedValue == null || cmbFiltrGroup.SelectedValue.ToString() == "Wszystkie" || cmbFiltrGroup.SelectedValue.ToString().Length == 0)
                listStudent = _listFromFile.OrderBy(x => x.Id).ToList();
            else
                listStudent = _listFromFile.OrderBy(x => x.Id).Where(x => x.StudentGroupName == cmbFiltrGroup.SelectedValue.ToString()).ToList();
            dgvDiary.DataSource = listStudent;
            FillComboGroup();
            if (flIsGroupNameColumn)
                FillColumnGroupName();
        }

        private void FillColumnGroupName()
        {
            _listGroupFromFile = fileHelperGroup.DeserialisedFromFile();
            foreach (DataGridViewRow row in dgvDiary.Rows)
            {
                var tmpGruop = _listGroupFromFile.FirstOrDefault(x => x.Id.ToString() == row.Cells["StudentGroupId"].Value.ToString());
                if (tmpGruop != null)
                    row.Cells["GroupName"].Value = tmpGruop.GroupName;
            }
                
        }

        private void FillComboGroup()
        {
            //_listGroupFromFile
            var lista = new List<string>() { "Wszystkie" };
            var listaGroup = _listFromFile.Select(x => x.StudentGroupName).Where(x => x != null).Where(x => x.Length > 0).Distinct();
            //var lista2 = _listFromFile.Select(x)
            lista.AddRange(listaGroup);
            cmbFiltrGroup.DataSource = lista;
            cmbFiltrGroup.DisplayMember = "StudentGroupName";
            cmbFiltrGroup.SelectedIndex = -1;
        }
        private List<Student> MokInitializeStudents()
        {
            var students = new List<Student>();
            students.Add(new Student { FirstName = "Piotr" });
            students.Add(new Student { FirstName = "Magdalena" });
            students.Add(new Student { FirstName = "Matylda" });
            var list = students.FindAll(x => x.FirstName == "Piotr");
            return students;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            StudentForm studentForm = new StudentForm();
            //studentForm.FormClosed += StudentForm_FormClosed;
            studentForm.ShowDialog();
            RefreshDiary();
        }

        private void StudentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FillComboGroup();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!IsSelectedStudent("edycji"))
                return;
            StudentForm studentForm = new StudentForm(_studentId);
            //studentForm.FormClosed += StudentForm_FormClosed;
            studentForm.ShowDialog();
            RefreshDiary();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!IsSelectedStudent("usunięcia"))
                return;
            var ans = MessageBox.Show($"Czy jesteś pewien usunięcia Studenta ", "Usuwanie Studenta", MessageBoxButtons.YesNo);
            if (ans == DialogResult.No)
                return;
            var students = fileHelper.DeserialisedFromFile();
            students.RemoveAll(x => x.Id == _studentId);
            fileHelper.SerializedToFile(students);
            RefreshDiary();
        }

        private bool IsSelectedStudent(string info)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show($"Wybierz studenta do {info}.", "Brak wybranego Studenta");
                return false;
            }
            else
                _studentId = Convert.ToInt32(dgvDiary.SelectedRows[0].Cells["Id"].Value.ToString());
            return true;
        }

        private void SetColumnLabel()
        {
            dgvDiary.Columns["Id"].HeaderText = "LP";
            dgvDiary.Columns["FirstName"].HeaderText = "Imię";
            dgvDiary.Columns["LastName"].HeaderText = "Nazwisko";
            dgvDiary.Columns["Math"].HeaderText = "Matematyka";
            dgvDiary.Columns["History"].HeaderText = "Historia";
            dgvDiary.Columns["Tech"].HeaderText = "Technika";
            dgvDiary.Columns["PolishLang"].HeaderText = "język polski";
            dgvDiary.Columns["ForeignLang"].HeaderText = "język obcy";
            dgvDiary.Columns["AdditionalActivities"].HeaderText = "zajęcia dodatkowe";
            dgvDiary.Columns["Comments"].HeaderText = "Uwagi";
            dgvDiary.Columns["StudentGroupName"].HeaderText = "Grupa";
            dgvDiary.Columns["StudentGroupId"].Visible = false;
        }

        private void SetGroupNameFromId()
        {
            DataGridViewColumn txbGroupName = new DataGridViewTextBoxColumn();
            txbGroupName.Name = "GroupName";
            txbGroupName.DataPropertyName = "GroupName";
            dgvDiary.Columns.Add(txbGroupName);
            flIsGroupNameColumn = true;
            FillColumnGroupName();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            IsMaximize = false;
            if (WindowState == FormWindowState.Maximized)
                IsMaximize = true;
            Settings.Default.Save();
        }
    }
}
