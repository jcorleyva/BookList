using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USAToday.Booklist.Business;
using USATodayBookList.AppCode;

namespace USATodayBookList
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public partial class CategoryLookup : PageBase
    {
        /// <summary>
        /// Gets the book id.
        /// </summary>
        /// <remarks></remarks>
        public int BookId 
        { 
            get{return int.Parse(Request["bookId"]);}
        }

        /// <summary>
        /// Gets the assigned categories.
        /// </summary>
        /// <remarks></remarks>
        public List<BookCategoryView> AssignedCategories
        {
            get 
            { 
                if (Session["AssignedCategories" + this.BookId.ToString()] == null)
                    Session["AssignedCategories" + this.BookId.ToString()] = GetAssignedBookCategories(this.BookId);

                return (List<BookCategoryView>)Session["AssignedCategories" + this.BookId.ToString()];
            }
        }

        /// <summary>
        /// Gets the un assigned categories.
        /// </summary>
        /// <remarks></remarks>
        public List<Category> UnAssignedCategories
        {
            get {
                if (Session["UnAssignedCategories" + this.BookId.ToString()] == null)
                    Session["UnAssignedCategories" + this.BookId.ToString()] = GetUnAssignedCategories(this.BookId);

                return (List<Category>)Session["UnAssignedCategories" + this.BookId.ToString()]; 
                }
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
                var categories = data.Categories.OrderBy("it.CategoryDescription", new System.Data.Objects.ObjectParameter[] { });
                return categories.ToList<USAToday.Booklist.Business.Category>();
            }
        }

        /// <summary>
        /// Gets the primary category.
        /// </summary>
        /// <param name="BookId">The book id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Category GetPrimaryCategory(int BookId)
        {
            try
            {
                using (var data = new USATodayBookListEntities())
                {
                    var cat = from c in data.Categories
                              from b in data.Books
                              where b.Id == BookId && b.PrimaryCategoryID == c.Id
                              select c;
                    return cat.ToList<USAToday.Booklist.Business.Category>().First();
                }
            }
            catch { return null; }             
        }

        public static List<USAToday.Booklist.Business.BookCategoryView> GetAssignedBookCategories(int bookId)
        {
            using (var data = new USATodayBookListEntities())
            {
                var categories = from c in data.BookCategoryViews
                                 where c.BookID == bookId
                                 orderby c.Category
                                 select c;

                return categories.ToList<USAToday.Booklist.Business.BookCategoryView>();
            }
        } 
        /// <summary>
        /// Gets the un assigned categories.
        /// </summary>
        /// <param name="BookId">The book id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<USAToday.Booklist.Business.Category> GetUnAssignedCategories(int BookId)
        {
            using (var data = new USATodayBookListEntities())
            {
                /* var categories = from c in data.Categories
                                 where !data.Books.Any(b => b.Id == BookId)
                                         orderby c.CategoryDescription
                                 select c; */
                var assignedCategories = 
                    from c in data.Categories
                    from b in c.Books
                    where b.Id == BookId
                    select c;

                var categories = data.Categories.Except(assignedCategories);

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
                Session["AssignedCategories" + this.BookId.ToString()] = null;
                Session["UnAssignedCategories" + this.BookId.ToString()] = null;

                BookCategoryView primaryCategory = AssignedCategories.Find(l => l.PrimaryCategory == true);

                PrimaryDropDownList.DataTextField = "CategoryDescription";
                PrimaryDropDownList.DataValueField = "Id";
                PrimaryDropDownList.DataSource = GetCategories();
                PrimaryDropDownList.DataBind();

                PrimaryDropDownList.Items.Insert(0, new ListItem("---Select---","0"));
                PrimaryDropDownList.SelectedValue = (primaryCategory == null ? "0" : primaryCategory.CategoryID.ToString());
                PrimaryCategoryLabel.Text = (primaryCategory == null ? "" : primaryCategory.Category);


                arlCategory.SourceListBox.DataTextField = "CategoryDescription";
                arlCategory.SourceListBox.DataValueField = "Id";
                arlCategory.SourceListBox.DataSource = UnAssignedCategories;
                arlCategory.SourceListBox.DataBind();

                arlCategory.TargetListBox.DataTextField = "Category";
                arlCategory.TargetListBox.DataValueField = "CategoryId";
                arlCategory.TargetListBox.DataSource = AssignedCategories;
                arlCategory.TargetListBox.DataBind();
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the Primary control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void Primary_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if user selects ---Select---(first option in list) then do not do anything to Add Remove List
            if (this.PrimaryDropDownList.SelectedIndex != 0)
            {
                //Category c = new Category();
                //c.Id = int.Parse(PrimaryDropDownList.SelectedValue);
                ////remove Primary Category from UnAssigned List
                //int index = UnAssignedCategories.FindIndex(l => l.Id == c.Id);
                //if (index > -1)
                //    UnAssignedCategories.RemoveAt(index);
                arlCategory.SourceListBox.Items.Remove(new ListItem(PrimaryDropDownList.SelectedItem.Text, PrimaryDropDownList.SelectedValue));

                //BookCategoryView bc = new BookCategoryView();
                //bc.CategoryID = int.Parse(PrimaryDropDownList.SelectedValue);
                //bc.Category = PrimaryDropDownList.SelectedItem.Text;
                //bc.PrimaryCategory = true;

                ////set previously selected primary category to false
                //if (AssignedCategories.Exists(l => l.PrimaryCategory == true && l.CategoryID != bc.CategoryID))
                //    AssignedCategories.Find(l => l.PrimaryCategory == true).PrimaryCategory = false;

                ////if primary category already exists in Assigned List then just set its PrimaryCategory property to true
                //if (AssignedCategories.Exists(l => l.CategoryID == int.Parse(PrimaryDropDownList.SelectedValue)))
                //    AssignedCategories.Find(l => l.CategoryID == int.Parse(PrimaryDropDownList.SelectedValue)).PrimaryCategory = true;
                //else
                //{
                //    //Primary Category does not exists in Assigned List. Add it to the Assigned List.
                //    AssignedCategories.Add(bc);
                    arlCategory.TargetListBox.Items.Add(new ListItem(PrimaryDropDownList.SelectedItem.Text, PrimaryDropDownList.SelectedValue));
                    //hdnAssignedPrimaryCategory.Value = PrimaryDropDownList.SelectedValue;
                //}
            }
        }

        /// <summary>
        /// Handles the Click event of the SaveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            AssignedCategories.Clear();
            foreach (ListItem li in arlCategory.TargetListBox.Items)
            {
                //add category to Assigned list
                BookCategoryView c = new BookCategoryView();
                c.CategoryID = int.Parse(li.Value);
                c.Category = li.Text;
                c.PrimaryCategory = (this.PrimaryDropDownList.SelectedValue == li.Value ? true : false);
                AssignedCategories.Add(c);

                //Remove keyword from UnAssigned list
                if (UnAssignedCategories.Exists(l => l.Id == c.CategoryID))
                    UnAssignedCategories.RemoveAt(UnAssignedCategories.FindIndex(l => l.Id == c.CategoryID));
            }

            //check to make sure UnAssignedCategory matches with Items in UnAssigned List Box
            foreach (ListItem li in arlCategory.SourceListBox.Items)
            {
                Category c = new Category();
                c.Id = int.Parse(li.Value);
                c.CategoryDescription = li.Text;
                if (!UnAssignedCategories.Exists(l => l.Id == c.Id))
                    UnAssignedCategories.Add(c);
            }

            base.CloseClientWindow();
        }

        /// <summary>
        /// Handles the Click event of the ClearFilterButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ClearFilterButton_Click(object sender, EventArgs e)
        {
            CategoryTextBox.Text = "";
            Response.Redirect("CategoryLookup.aspx?bookId=" + BookId);
        }

        /// <summary>
        /// Handles the Click event of the FilterButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void FilterButton_Click(object sender, EventArgs e)
        {
            using (var data = new USATodayBookListEntities())
            {
                var categories = from c in data.Categories
                                 from b in data.Books
                                    where b.Id == BookId
                                         && c.CategoryDescription.Contains(CategoryTextBox.Text)
                                 select c;

                PrimaryDropDownList.DataTextField = "CategoryDescription";
                PrimaryDropDownList.DataValueField = "Id";
                PrimaryDropDownList.DataSource = categories;
                PrimaryDropDownList.DataBind();

                PrimaryDropDownList.Items.Insert(0, new ListItem("---Select---","0"));
                
                arlCategory.SourceListBox.DataTextField = "CategoryDescription";
                arlCategory.SourceListBox.DataValueField = "Id";
                arlCategory.SourceListBox.DataSource = categories;
                arlCategory.SourceListBox.DataBind();
            }

        }

        /// <summary>
        /// Handles the Click event of the AddButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void AddButton_Click(object sender, EventArgs e)
        {
            if (CategoryTextBox.Text != "")
            {
                using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
                {
                    USAToday.Booklist.Business.Category category = new USAToday.Booklist.Business.Category();

                    category.CategoryDescription = CategoryTextBox.Text;

                    data.Categories.AddObject(category);
                    data.SaveChanges();

                    Audit(category.Id);

                    BookCategoryView primaryCategory = AssignedCategories.Find(l => l.PrimaryCategory == true);

                    PrimaryDropDownList.DataTextField = "CategoryDescription";
                    PrimaryDropDownList.DataValueField = "Id";
                    PrimaryDropDownList.DataSource = GetCategories();
                    PrimaryDropDownList.DataBind();

                    PrimaryDropDownList.Items.Insert(0, new ListItem("---Select---", "0"));
                    PrimaryDropDownList.SelectedValue = (primaryCategory == null ? "0" : primaryCategory.CategoryID.ToString());

                    //add category to Assigned list
                    BookCategoryView c = new BookCategoryView();
                    c.CategoryID = category.Id;
                    c.Category = CategoryTextBox.Text;
                    c.PrimaryCategory = false;
                    AssignedCategories.Add(c);

                    arlCategory.TargetListBox.DataTextField = "Category";
                    arlCategory.TargetListBox.DataValueField = "CategoryId";
                    arlCategory.TargetListBox.DataSource = AssignedCategories;
                    arlCategory.TargetListBox.DataBind();

                    //clear text box
                    this.CategoryTextBox.Text = "";
                }
            }
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
    }
}