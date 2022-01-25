using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentsDiary
{
    static class Program
    {
        public static string FilePath = Path.Combine(Environment.CurrentDirectory, "StudentsFile.txt");
        public static string FilePathGroup = Path.Combine(Environment.CurrentDirectory, "GroupFile.txt");
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
