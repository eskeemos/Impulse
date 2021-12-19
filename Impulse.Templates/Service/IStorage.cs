using System.Collections.Generic;

namespace Impulse.Shared.Service
{
    public interface IStorage
    {
        void SaveValue(decimal value);
        ICollection<decimal> GetValues();
        void SetPath(string path);
    }
}
