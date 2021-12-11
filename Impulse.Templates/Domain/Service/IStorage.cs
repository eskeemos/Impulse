using System.Collections.Generic;

namespace Impulse.Shared.Domain.Service
{
    public interface IStorage
    {
        /// <summary>
        /// Save value into file
        /// </summary>
        /// <param name="value">Price average from specific symbol and interval</param>
        void SaveValue(decimal value);

        /// <summary>
        /// Obtain average prices of a specific symbol in range
        /// </summary>
        /// <returns>Average prices list</returns>
        ICollection<decimal> GetValues();
    }
}
