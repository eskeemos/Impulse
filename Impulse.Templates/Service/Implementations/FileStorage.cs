using System;
using System.Collections.Generic;
using System.IO;

namespace Impulse.Shared.Domain.Service.Implementations
{
    public class FileStorage : IStorage
    {
        #region Variables

        // TODO
        private string storagePath;

        #endregion

        #region Implemented functions

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

        #endregion

        public void SetPath(string path)
        {
            storagePath = path;
        }
    }
}
