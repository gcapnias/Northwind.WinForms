using Northwind.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Repositories
{

    public class RepositoryBase<T> : IRepository<T>
        where T :object, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual T Add(T model)
        {
            T result = default(T);

            using (NorthwindEntities db = new NorthwindEntities())
            {
                db.Set<T>().Add(model);

                db.SaveChanges();
                db.Entry<T>(model).Reload();

                result = model;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual T Delete(int Id)
        {
            T result = default(T);

            using (NorthwindEntities db = new NorthwindEntities())
            {
                T model = db.Set<T>().Where(m => m.Id == Id).FirstOrDefault();
                if (model != null)
                {
                    db.Set<T>().Remove(model);

                    db.SaveChanges();

                    result = model;
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual T Edit(T model)
        {
            Contract.Ensures(Contract.Result<T>() != null);
            T result = default(T);

            using (NorthwindEntities db = new NorthwindEntities())
            {
                T entity = db.Set<T>().Where(m => m.Id == model.Id).FirstOrDefault();
                if (entity != null)
                {
                    entity = model.MemberwiseClone() as T;

                    db.SaveChanges();
                    db.Entry<T>(entity).Reload();

                    result = entity;
                }
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="function"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedList<T> Get(Expression<Func<T, bool>> WhereFilter, Expression<Func<T, T>> orderSelector, int pageNumber = 1, int pageSize = 15)
        {
            PagedList<T> result = new PagedList<T>(Enumerable.Empty<T>(), m => m, pageNumber, pageSize);

            using (AsaLocalEntities db = new AsaLocalEntities())
            {
                result = db.Set<T>().AsNoTracking().Where(WhereFilter).OrderBy(orderSelector).ToPagedList(pageNumber, pageSize);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedList<T> GetAll(int pageNumber = 1, int pageSize = 15)
        {
            PagedList<T> result = new PagedList<T>(Enumerable.Empty<T>(), m => m, pageNumber, pageSize);

            using (AsaLocalEntities db = new AsaLocalEntities())
            {
                result = db.Set<T>().AsNoTracking().ToPagedList(pageNumber, pageSize);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual T GetByID(int Id)
        {
            T result = default(T);

            using (AsaLocalEntities db = new AsaLocalEntities())
            {
                result = db.Set<T>().AsNoTracking().Where(m => m.Id == Id).FirstOrDefault();
            }

            return result;
        }

    }
}
