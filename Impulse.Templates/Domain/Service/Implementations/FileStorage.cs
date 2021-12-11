using System;
using System.Collections.Generic;
using System.IO;

namespace Impulse.Shared.Domain.Service.Implementations
{
    public class FileStorage : IStorage
    {
        #region Variables

        /// <summary>
        /// Storage path
        /// </summary>
        private readonly string storagePath;

        #endregion

        #region Constructor

        /// <summary>
        /// Set up variables
        /// </summary>
        /// <param name="_storagePath">Storage path</param>
        public FileStorage(string _storagePath)
        {
            storagePath = _storagePath;
        }

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
    }
}
