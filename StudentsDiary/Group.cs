using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsDiary
{
    public class Group
    {
        private FileHelper<List<Group>> fileHelper = new FileHelper<List<Group>>(Program.FilePathGroup);

        public int Id { get; set; }
        public string GroupName { get; set; }

        public void MokGroup()
        {
            var lista = new List<Group>();
            var j = 1;
            for (int i = 1; i < 17; i++)
            {
                var className = i % 2 != 0 ? "A" : "B";
                var grupa = new Group()
                {
                    Id = i,
                    GroupName = $"Klasa {j}{className}"
                };
                if (className == "B")
                    j++;
                lista.Add(grupa);
            }
            fileHelper.SerializedToFile(lista);
        }
    }
}
