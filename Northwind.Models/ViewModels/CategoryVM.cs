using Northwind.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.ViewModels
{
    public class CategoryVM : DataErrorInfoBase, IValidatableObject
    {

        [DisplayName("α/α")]
        [Required]
        public int CategoryID { get; set; }

        [DisplayName("Κατηγορία")]
        [Required]
        [StringLength(15)]
        public string CategoryName { get; set; }

        [DisplayName("Περιγραφή")]
        public string Description { get; set; }

        [DisplayName("Φωτογραφία")]
        public byte[] Picture { get; set; }

        #region ' IValidatableObject '

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (validationContext != null)
            {
                CategoryVM model = validationContext.ObjectInstance as CategoryVM;

                if (string.IsNullOrEmpty(model.Description))
                {
                    yield return new ValidationResult("Η εισαγωγή στο πεδίο 'Περιγραφή' είναι υποχρεωτική.", new[] { "Description" });
                }
            }
        }

        #endregion

    }


    public static partial class Extensions
    {
        public static CategoryVM ToViewModel(this Category model)
        {
            CategoryVM result = null;

            if (model != null)
            {
                result = new CategoryVM();
                result.CategoryID = model.CategoryID;
                result.CategoryName = model.CategoryName;
                result.Description = model.Description;
                result.Picture = model.Picture;
            }

            return result;
        }

        public static Category ToModel(this CategoryVM model)
        {
            Category result = null;

            if (model != null)
            {
                result = new Category();
                result.CategoryID = model.CategoryID;
                result.CategoryName = model.CategoryName;
                result.Description = model.Description;
                result.Picture = model.Picture;
            }

            return result;
        }

    }
}
