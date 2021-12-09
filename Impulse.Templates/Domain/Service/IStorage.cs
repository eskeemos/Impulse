using System.Collections.Generic;

namespace Impulse.Shared.Domain.Service
{
    public interface IStorage
    {
        // Values save
        void SaveValue(decimal val);

        // Values obtain
        ICollection<decimal> GetValues();
    }
}
