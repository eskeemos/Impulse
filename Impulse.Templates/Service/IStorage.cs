using System.Collections.Generic;

namespace Impulse.Shared.Domain.Service
{
    public interface IStorage
    {
        void SaveValue(decimal value);

        ICollection<decimal> GetValues();

        void SetPath(string path);
    }
}
