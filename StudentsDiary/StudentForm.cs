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

namespace StudentsDiary
{
    public partial class StudentForm : Form
    {
        private FileHelper<List<Student>> fileHelper = new FileHelper<List<Student>>(Program.FilePath);
        private FileHelper<List<Group>> fileHelperGroup = new FileHelper<List<Group>>(Program.FilePathGroup);
        private int _studentId;

        public StudentForm(int id = 0)
        {
            InitializeComponent();
            _studentId = id;
            FillComboGroup();
            if (_studentId != 0)
            {
                Text = "Edytuj dane Studenta";
                FillStudentsData();
            }
            //btnCancel.BackColor = Main.FontColor;
            txbFirstName.Focus();
        }

        private void FillComboGroup()
        {
            var lista = new List<Group>();
            lista = fileHelperGroup.DeserialisedFromFile();
            cmbGroup.DataSource = lista;
            cmbGroup.DisplayMember = "GroupName";
            cmbGroup.ValueMember = "Id";
            cmbGroup.SelectedValue = 0;
        }

        private void FillStudentsData()
        {
            Student student = new Student();
            var students = fileHelper.DeserialisedFromFile();
            student = students.FirstOrDefault(x => x.Id == _studentId);
            txbFirstName.Text = student.FirstName;
            txbName.Text = student.LastName;
            txbMath.Text = student.Math;
            txbHistory.Text = student.History;
            txbTech.Text = student.Tech;
            txbForeignLang.Text = student.ForeignLang;
            txbPolish.Text = student.PolishLang;
            rtxComments.Text = student.Comments;
            cbAdditionalActivities.Checked = student.AdditionalActivities;
            if (student.StudentGroupId != null)
                cmbGroup.SelectedValue = student.StudentGroupId;
            //cmbGroup.SelectedText = student.StudentGroupName;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = fileHelper.DeserialisedFromFile();
            if (_studentId != 0)
                students.RemoveAll(x => x.Id == _studentId);
            else
                AssignIdToNewStudent();
            students.Add(AddStudent());
            fileHelper.SerializedToFile(students);
            Close();
        }

        private Student AddStudent()
        {
            var groupName = "";
            if (cmbGroup.SelectedItem != null)
                groupName = ((Group)cmbGroup.SelectedItem).GroupName;
            var student = new Student()
            {
                Id = _studentId,
                FirstName = txbFirstName.Text,
                LastName = txbName.Text,
                Math = txbMath.Text,
                Tech = txbTech.Text,
                PolishLang = txbPolish.Text,
                ForeignLang = txbForeignLang.Text,
                Comments = rtxComments.Text,
                History = txbHistory.Text,
                AdditionalActivities = cbAdditionalActivities.Checked,
                StudentGroupId = Convert.ToInt32(cmbGroup.SelectedValue)
                //,StudentGroupName = groupName
            };
            return student;
        }

        private void AssignIdToNewStudent()
        {
            var students = fileHelper.DeserialisedFromFile();
            var studentMaxId = students.OrderByDescending(x => x.Id).FirstOrDefault();
            _studentId = studentMaxId == null ? 1 : studentMaxId.Id + 1;
        }
    }
}
