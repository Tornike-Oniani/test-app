using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiDesktopApp2.Helpers
{
    public class JsonWriterReader<T>
    {
        private readonly string filePath;

        public JsonWriterReader(string filePath)
        {
            this.filePath = filePath;
        }

        public void WriteData(T data)
        {
            string stringifiedData = JsonConvert.SerializeObject(data);
            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                sw.Write(stringifiedData);
            }
        }
        public T ReadData()
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File was not found", filePath);
            }
            string data = File.ReadAllText(filePath);
            if (String.IsNullOrEmpty(data))
            {
                throw new InvalidDataException("No entries were found");
            }
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
