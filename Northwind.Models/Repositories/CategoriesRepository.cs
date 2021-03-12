using Northwind.Models;
using Northwind.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Repositories
{
    public class CategoriesRepository : IRepository<CategoryVM>
    {
        private NorthwindEntities db = null;

        public CategoriesRepository(NorthwindEntities context = null)
        {
            if (context == null)
            {
                db = new NorthwindEntities();
            }
            else
            {
                db = context;
            }
        }

        ~CategoriesRepository()
        {
            db = null;
        }

        public CategoryVM Add(CategoryVM model)
        {
            Category r = model.ToModel();
            db.Categories.Add(r);

            db.SaveChanges();
            db.Entry(r).Reload();

            return r.ToViewModel();
        }

        public CategoryVM Delete(int Id)
        {
            CategoryVM result = null;

            Category r = db.Categories.Where(m => m.CategoryID == Id).FirstOrDefault();
            if (r != null)
            {
                result = r.ToViewModel();
                db.Categories.Remove(r);

                db.SaveChanges();
            }

            return result;
        }

        public CategoryVM Delete(CategoryVM model)
        {
            return Delete(model.CategoryID);
        }

        public CategoryVM Edit(CategoryVM model)
        {
            Category r = db.Categories.Where(m => m.CategoryID == model.CategoryID).FirstOrDefault();
            if (r != null)
            {
                r.CategoryName = model.CategoryName;
                r.Description = model.Description;
                r.Picture = model.Picture;

                db.SaveChanges();
            }

            return model;
        }

        public IList<CategoryVM> Get(Expression<Func<Category, bool>> WhereFilter)
        {
            List<Category> r = db.Categories.AsNoTracking().Where(WhereFilter).OrderBy(m => m.CategoryName).ToList();

            return new List<CategoryVM>(r.Select(m => m.ToViewModel()));
        }

        public IList<CategoryVM> GetAll()
        {
            return Get(m => true);
        }

        public CategoryVM GetByID(int Id)
        {
            CategoryVM result = null;

            Category r = db.Categories.AsNoTracking().Where(m => m.CategoryID == Id).FirstOrDefault();
            if (r != null)
            {
                result = r.ToViewModel();
            }

            return result;
        }

        public CategoryVM GetByID(CategoryVM model)
        {
            return GetByID(model.CategoryID);
        }
    }
}
