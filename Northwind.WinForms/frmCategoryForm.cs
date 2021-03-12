using Northwind.Models;
using Northwind.Repositories;
using Northwind.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Northwind.WinForms
{
    public partial class frmCategoryForm : Form
    {
        private bool HasErrors = false;
        private CategoriesRepository Repository = null;
        private CategoryVM Model = null;
        private int ModelId = -1;

        public frmCategoryForm(int Id = -1)
        {
            InitializeComponent();

            this.ModelId = Id;
        }

        private void InitializeLookups()
        {

        }

        private void InitializeModel()
        {
            this.Repository = new CategoriesRepository();

            if (this.ModelId > 0)
            {
                this.Model = this.Repository.GetByID(this.ModelId);
            }
            else
            {
                this.Model = new CategoryVM();
            }

            if (this.Model != null)
            {
                categoryVMBindingSource.DataSource = this.Model;
            }

        }

        private void frmCategoryFormOnFormClosed(object sender, FormClosedEventArgs e)
        {
            this.Model = null;
            this.ModelId = -1;
            this.Repository = null;
        }

        private void frmCategoryFormOnFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = HasErrors;
        }

        private void frmCategoryFormOnLoad(object sender, EventArgs e)
        {
            InitializeLookups();
            InitializeModel();
        }

        private void btnOkOnClick(object sender, EventArgs e)
        {
            try
            {
                List<ValidationResult> validationErrors = new List<ValidationResult>();
                if (Model.IsValid(out validationErrors))
                {
                    if (this.ModelId > 0)
                    {
                        this.Repository.Edit(this.Model);
                    }
                    else
                    {
                        this.Repository.Add(this.Model);
                    }

                    HasErrors = false;
                    this.Close();
                }
                else
                {
                    StringBuilder errorMessages = new StringBuilder();
                    foreach (ValidationResult validationError in validationErrors)
                    {
                        errorMessages.AppendLine(string.Format("- {0}", validationError.ErrorMessage));
                        HasErrors = true;
                    }

                    MessageBox.Show(errorMessages.ToString(), "Σφάλμα", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                StringBuilder errorMessages = new StringBuilder();
                HasErrors = false;
                foreach (Control formControl in categoryVMErrorProvider.ContainerControl.Controls)
                {
                    string errorMessage = categoryVMErrorProvider.GetError(formControl);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        errorMessages.AppendLine(string.Format("- {0}", errorMessage));
                        HasErrors = true;
                    }
                }

                if (!HasErrors)
                {
                    errorMessages.AppendLine(string.Format("- {0}", ex.Message));

                    if (ex.InnerException != null)
                    {
                        errorMessages.AppendLine(string.Format("- {0}", ex.InnerException.Message));
                    }
                }

                MessageBox.Show(errorMessages.ToString(), "Σφάλμα", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelOnClick(object sender, EventArgs e)
        {
            HasErrors = false;
        }

    }
}
