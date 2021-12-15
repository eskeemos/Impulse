using System.Collections.Generic;

namespace Impulse.Shared.Domain.Service
{
    public interface IStorage
    {
        // TODO
        void SaveValue(decimal value);

        // TODO
        ICollection<decimal> GetValues();

        // TODO
        void SetPath(string path);
    }
}
