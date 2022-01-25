using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StudentsDiary
{
    class FileHelper<T> where T : new()     //Powoduje, że dopuszcamy tylko takie bez parametrów
    {
        private string _path;

        public FileHelper(string path)
        {
            _path = path;
        }

        public void SerializedToFile(T students)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var streamWriter = new StreamWriter(_path))
            {
                serializer.Serialize(streamWriter, students);
                streamWriter.Close();
            }
        }

        public T DeserialisedFromFile()
        {
            if (!File.Exists(_path))
                return new T();
            var serializer = new XmlSerializer(typeof(T));
            using (var streamReader = new StreamReader(_path))
            {
                var students = (T)serializer.Deserialize(streamReader);
                streamReader.Close();
                return students;
            }
        }
    }
}
