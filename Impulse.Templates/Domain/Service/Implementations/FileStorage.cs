using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impulse.Shared.Domain.Service.Implementations
{
    public class FileStorage : IStorage
    {
        #region Var

        // Storage path
        private readonly string filePath;

        #endregion

        #region Constructor

        // Constructor
        public FileStorage(string _filePath)
        {
            // Set up var's
            filePath = _filePath;
        }

        #endregion

        #region Public

        // Obtain values from store file
        public ICollection<decimal> GetValues()
        {
            // init empty values list
            var list = new List<decimal>();
            // use streamReader to interpret file
            using (StreamReader reader = new StreamReader(filePath))
            {
                // init empty val
                string line;
                // interpret file values until false
                while ((line = reader.ReadLine()) != null)
                {
                    // add value to file
                    list.Add(Convert.ToDecimal(line));
                }
            }
            // return obtained values
            return list;
        }

        // Save new value to file
        public void SaveValue(decimal val)
        {
            // obtain writer reference
            using StreamWriter writer = new StreamWriter(filePath, true);
            // overwrite file
            writer.WriteLine(val);
        }

        #endregion
    }
}
