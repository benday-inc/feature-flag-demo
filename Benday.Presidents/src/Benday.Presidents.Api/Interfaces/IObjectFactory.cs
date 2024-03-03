using System;
using System.Collections.Generic;
using System.Linq;

namespace Benday.Presidents.Common
{
    public interface IObjectFactory
    {
        T Create<T>();
        void RegisterInstance<T>(T instance);
    }
}
