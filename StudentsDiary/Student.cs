using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsDiary
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Math { get; set; }
        public string Tech { get; set; }
        public string PolishLang { get; set; }
        public string History { get; set; }
        public string ForeignLang { get; set; }
        public string Comments { get; set; }
        public bool AdditionalActivities { get; set; }
        public int? StudentGroupId { get; set; }
        //public string StudentGroupName { get; set; }
        //public Group StudentGroup { get; set; }
    }
}
