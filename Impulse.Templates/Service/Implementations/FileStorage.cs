using Impulse.Shared.Service;
using System;
using System.Collections.Generic;
using System.IO;

namespace Impulse.Shared.Service.Implementations
{
    public class FileStorage : IStorage
    {
        private string storagePath;
        public ICollection<decimal> GetValues()
        {
            var list = new List<decimal>();

            using (StreamReader reader = new StreamReader(storagePath))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(Convert.ToDecimal(line));
                }
            }

            return list;
        }

        public void SaveValue(decimal value)
        {
            using StreamWriter writer = new StreamWriter(storagePath, true);

            writer.WriteLine(value);
        }

        public void SetPath(string path)
        {
            storagePath = path;
        }
    }
}
