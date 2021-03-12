using Northwind.Infrastructure;
using Northwind.Repositories;
using Northwind.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Northwind.WinForms
{
    public partial class frmCategoriesList : Form
    {
        private CategoriesRepository Repository = null;
        private IList<CategoryVM> Model = null;

        public frmCategoriesList()
        {
            InitializeComponent();
            InitializeModel();
        }

        private void InitializeModel()
        {
            this.Repository = new CategoriesRepository();
            BindModelToControls();
        }

        private void BindModelToControls()
        {
            this.Model = this.Repository.Get(m => true);
            categoryVMBindingSource.DataSource = new SortableBindingList<CategoryVM>(this.Model);
        }

        private void categoriesDataGridViewOnCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridView dataGridView = sender as DataGridView;
                CategoryVM model = dataGridView.Rows[e.RowIndex].DataBoundItem as CategoryVM;

                using (frmCategoryForm f = new frmCategoryForm(model.CategoryID))
                {
                    f.ShowDialog();
                }

                BindModelToControls();
            }
        }

        private void insertCategoryToolStripMenuItemOnClick(object sender, EventArgs e)
        {
            using (frmCategoryForm f = new frmCategoryForm(-1))
            {
                f.ShowDialog();
            }

            BindModelToControls();
        }

        private void editCategoryToolStripMenuItemOnClick(object sender, EventArgs e)
        {
            if (categoriesDataGridView.CurrentRow != null && categoriesDataGridView.CurrentRow.Index > -1)
            {
                CategoryVM model = categoriesDataGridView.Rows[categoriesDataGridView.CurrentRow.Index].DataBoundItem as CategoryVM;

                using (frmCategoryForm f = new frmCategoryForm(model.CategoryID))
                {
                    f.ShowDialog();
                }

                BindModelToControls();
            }
        }

        private void deleteCategoryToolStripMenuItemOnClick(object sender, EventArgs e)
        {
            if (categoriesDataGridView.CurrentRow != null && categoriesDataGridView.CurrentRow.Index > -1)
            {
                CategoryVM model = categoriesDataGridView.Rows[categoriesDataGridView.CurrentRow.Index].DataBoundItem as CategoryVM;

                if (MessageBox.Show("Are you sure?", "Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    this.Repository.Delete(model.CategoryID);
                }

                BindModelToControls();
            }
        }

        private void categoriesContextMenuStripOnOpening(object sender, CancelEventArgs e)
        {
            if (categoriesDataGridView.CurrentRow != null && categoriesDataGridView.CurrentRow.Index > -1)
            {
                deleteCategoryToolStripMenuItem.Enabled = true;
                editCategoryToolStripMenuItem.Enabled = true;
            }
            else
            {
                deleteCategoryToolStripMenuItem.Enabled = false;
                editCategoryToolStripMenuItem.Enabled = false;
            }
        }
    }
}
