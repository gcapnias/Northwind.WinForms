using System;
using System.Collections.Generic;

namespace Northwind.Repositories
{
    public interface IRepository<T>
        where T : new()
    {
        T Add(T model);
        T Edit(T model);
        T Delete(T model);
        IList<T> GetAll();
        T GetByID(T model);
    }

}
