using System;
using System.Collections.Generic;
using System.Linq;

namespace Benday.Presidents.Common
{
    public interface IRepository<T> where T : IInt32Identity
    {
        IList<T> GetAll();
        T GetById(int id);
        void Save(T saveThis);
        void Delete(T deleteThis);
    }
}
