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
    public abstract class DataErrorInfoBase : IDataErrorInfo
    {
        #region ' IDataErrorInfo Members '

        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string columnName]
        {
            get
            {

                List<ValidationAttribute> validationAttributes = new List<ValidationAttribute>();

                MetadataTypeAttribute metadataType = this.GetType().GetCustomAttributes(true).OfType<MetadataTypeAttribute>().FirstOrDefault();
                if (metadataType != null)
                {
                    PropertyInfo p = metadataType.MetadataClassType.GetProperty(columnName);
                    if (p != null)
                    {
                        validationAttributes = p.GetCustomAttributes(true).OfType<ValidationAttribute>().ToList();
                    }
                }

                PropertyInfo currentProperty = this.GetType().GetProperty(columnName);
                validationAttributes.AddRange(currentProperty.GetCustomAttributes(true).OfType<ValidationAttribute>().ToList());

                foreach (ValidationAttribute currentAttribute in validationAttributes)
                {
                    try
                    {
                        currentAttribute.Validate(currentProperty.GetValue(this, null), columnName);
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }

                return string.Empty;
            }
        }

        #endregion

    }

    public static partial class Extensions
    {
        public static bool IsValid(this IValidatableObject o)
        {
            ValidationContext ctx = new ValidationContext(o, null, null);
            var errors = new List<ValidationResult>();
            return Validator.TryValidateObject(o, ctx, errors, true);
        }

        public static bool IsValid(this IValidatableObject o, out List<ValidationResult> errors)
        {
            ValidationContext ctx = new ValidationContext(o, null, null);
            errors = new List<ValidationResult>();
            return Validator.TryValidateObject(o, ctx, errors, true);
        }
    }
}
