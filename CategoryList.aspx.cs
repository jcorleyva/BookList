using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USAToday.Booklist.Business;

namespace USATodayBookList
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public partial class CategoryList : System.Web.UI.Page
    {
        /// <summary>
        /// Gets or sets the last sort key.
        /// </summary>
        /// <value>The last sort key.</value>
        /// <remarks></remarks>
        public string LastSortKey
        {
            get { return ViewState["LastSortKey"].ToString(); }
            set { ViewState["LastSortKey"] = value; }
        }

        /// <summary>
        /// Gets or sets the last sort direction.
        /// </summary>
        /// <value>The last sort direction.</value>
        /// <remarks></remarks>
        public string LastSortDirection
        {
            get { return ViewState["LastSortDirection"].ToString(); }
            set { ViewState["LastSortDirection"] = value; }
        }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<USAToday.Booklist.Business.Category> GetCategories()
        {
            using (var data = new USATodayBookListEntities())
            {
                var categories = data.Categories;
                return categories.ToList<USAToday.Booklist.Business.Category>();

            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.BindData();

                // Set the sort info 
                LastSortDirection = string.Empty;
                LastSortKey = string.Empty;
            }

            // Hide buttons
            if (AppCode.AppCache.GetUser(User.Identity.Name).CanWrite == false)
            {
                this.AddButton.Visible = false;
                this.AddTextBox.Visible = false;
                this.CategoryGridView.Columns[2].Visible = false;
                this.CategoryGridView.Columns[3].Visible = false;
            }
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <remarks></remarks>
        private void BindData()
        {
            // Get the links
            List<USAToday.Booklist.Business.Category> categories = GetCategories();

            // Bind
            CategoryGridView.DataSource = categories;
            CategoryGridView.DataBind();
        }

        /// <summary>
        /// Handles the Click event of the Filter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected virtual void Filter_Click(object sender, EventArgs e)
        {
            if (this.CategoryTextBox.Text != "")
            {
                using (var data = new USATodayBookListEntities())
                {
                    var categories = from a in data.Categories
                                where a.CategoryDescription.Contains(this.CategoryTextBox.Text.Trim())
                                select a;
                    if (categories.Count() == 0)
                        CategoryGridView.AllowSorting = false;

                    CategoryGridView.DataSource = categories;
                    CategoryGridView.DataBind();
                }
            }
            else
            {
                CategoryGridView.DataSource = GetCategories();
                CategoryGridView.DataBind();
            }
        }

        /// <summary>
        /// Handles the RowEditing event of the CategoryGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewEditEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void CategoryGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            CategoryGridView.EditIndex = e.NewEditIndex;
            this.BindData();
        }

        /// <summary>
        /// Handles the RowCancelingEdit event of the CategoryGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewCancelEditEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void CategoryGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            CategoryGridView.EditIndex = -1;
            this.BindData();
        }

        /// <summary>
        /// Handles the RowUpdating event of the CategoryGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewUpdateEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void CategoryGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
            {                
                int Id = int.Parse(e.Keys[0].ToString());
                var category = data.Categories.Single(l => l.Id == Id);
                category.CategoryDescription = e.NewValues[0].ToString();
                data.SaveChanges();
                Audit(Id);
            }

            CategoryGridView.EditIndex = -1;
            this.BindData();
        }

        /// <summary>
        /// Audits the specified category id.
        /// </summary>
        /// <param name="CategoryId">The category id.</param>
        /// <remarks></remarks>
        private void Audit(int CategoryId)
        {
            Audit audit = new Audit();

            audit.TableName = "Category";
            audit.UpdatedBy = User.Identity.Name;
            audit.Updated = DateTime.Now;
            audit.UpdateRowId = CategoryId;

            Global.BookEntities.Audits.AddObject(audit);
            Global.BookEntities.SaveChanges();
        }

        /// <summary>
        /// Handles the RowDeleting event of the CategoryGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewDeleteEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void CategoryGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
            {
                int Id = int.Parse(e.Keys[0].ToString());
                var category = data.Categories.Single(l => l.Id == Id);
                data.Categories.DeleteObject(category);
                data.SaveChanges();
            }
            CategoryGridView.EditIndex = -1;
            this.BindData();
        }

        /// <summary>
        /// Handles the Sorting event of the CategoryGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void CategoryGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Get the list of authors from the session 
            List<USAToday.Booklist.Business.Category> categories = default(List<USAToday.Booklist.Business.Category>);
            categories = GetCategories();

            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
                LastSortDirection = string.Empty;

            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "CategoryDescription":
                    categories = Sort(categories, category => category.CategoryDescription);
                    break;
            }

            LastSortKey = e.SortExpression;

            // Rebind 
            CategoryGridView.DataSource = categories;
            CategoryGridView.DataBind();
        }

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.Category> Sort(List<USAToday.Booklist.Business.Category> list,
                Func<USAToday.Booklist.Business.Category, string> sortKey)
        {
            if (LastSortDirection == "ASC")
            {
                list = list.OrderByDescending(sortKey).ToList();
                LastSortDirection = "DESC";
            }
            else
            {
                list = list.OrderBy(sortKey).ToList();
                LastSortDirection = "ASC";
            }
            return list;
        }

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.Category> Sort(List<USAToday.Booklist.Business.Category> list,
                Func<USAToday.Booklist.Business.Category, decimal> sortKey)
        {
            if (LastSortDirection == "ASC")
            {
                list = list.OrderByDescending(sortKey).ToList();
                LastSortDirection = "DESC";
            }
            else
            {
                list = list.OrderBy(sortKey).ToList();
                LastSortDirection = "ASC";
            }
            return list;
        }


        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (AddTextBox.Text.Trim() == "")
                ErrorDiv.Visible = true;
            else
            {
                ErrorDiv.Visible = false;
                using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
                {
                    USAToday.Booklist.Business.Category category = new USAToday.Booklist.Business.Category();

                    category.CategoryDescription = AddTextBox.Text;

                    data.Categories.AddObject(category);
                    data.SaveChanges();

                    Audit(category.Id);

                    //clear text box
                    this.AddTextBox.Text = "";

                    //rebind data to show changes
                    this.BindData();
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the ClearFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ClearFilter_Click(object sender, EventArgs e)
        {
            this.CategoryTextBox.Text = "";
            this.BindData();
        }
    }
}