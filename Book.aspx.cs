using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using AmazonProductAdvtApi;
using USAToday.Booklist.Business;
using USATodayBookList.AppCode;


namespace USATodayBookList
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public partial class Book1 : PageBase
    {
        private const string MY_AWS_ACCESS_KEY_ID = "AKIAIV4OH56VWFOJXAUQ";
        private const string MY_AWS_SECRET_KEY = "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";
        private const string DESTINATION = "ecs.amazonaws.com";
        private const string NAMESPACE = "http://webservices.amazon.com/AWSECommerceService/2009-03-31";
        private const string ITEM_ID = "0545010225";

        /// <summary>
        /// Gets or sets a value indicating whether [add mode].
        /// </summary>
        /// <value><c>true</c> if [add mode]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        private bool AddMode { get; set; }
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        /// <remarks></remarks>
        private int Id { get; set; }

        /// <summary>
        /// Gets or sets the last sort key.
        /// </summary>
        /// <value>The last sort key.</value>
        /// <remarks></remarks>
        private string LastSortKey
        {
            get { return ViewState["LastSortKey"].ToString(); }
            set { ViewState["LastSortKey"] = value; }
        }

        /// <summary>
        /// Gets or sets the last sort direction.
        /// </summary>
        /// <value>The last sort direction.</value>
        /// <remarks></remarks>
        private string LastSortDirection
        {
            get { return ViewState["LastSortDirection"].ToString(); }
            set { ViewState["LastSortDirection"] = value; }
        }

        /// <summary>
        /// Gets or sets the un assigned categories.
        /// </summary>
        /// <value>The un assigned categories.</value>
        /// <remarks></remarks>
        //private List<USAToday.Booklist.Business.BookCategoryView> UnAssignedCategories
        //{
        //    get { return (List<USAToday.Booklist.Business.BookCategoryView>)Session["UnAssignedCategories" + this.Id.ToString()]; }
        //    set { Session["UnAssignedCategories" + this.Id.ToString()] = value; }
        //}

        /// <summary>
        /// Gets or sets the assigned categories.
        /// </summary>
        /// <value>The assigned categories.</value>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.BookCategoryView> AssignedCategories
        {
            get {
                    if (Session["AssignedCategories" + this.Id.ToString()] == null)
                        Session["AssignedCategories" + this.Id.ToString()] = GetAssignedBookCategories(this.Id);
                
                return (List<USAToday.Booklist.Business.BookCategoryView>)Session["AssignedCategories" + this.Id.ToString()]; 
                }
            set { Session["AssignedCategories" + this.Id.ToString()] = value; }
        }

        /// <summary>
        /// Gets or sets the un assigned keywords.
        /// </summary>
        /// <value>The un assigned keywords.</value>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.Keyword> UnAssignedKeywords
        {
            get { return (List<USAToday.Booklist.Business.Keyword>)Session["UnAssignedKeywords" + this.Id.ToString()]; }
            set { Session["UnAssignedKeywords" + this.Id.ToString()] = value; }
        }

        /// <summary>
        /// Gets or sets the assigned keywords.
        /// </summary>
        /// <value>The assigned keywords.</value>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.Keyword> AssignedKeywords
        {
            get {
                if (Session["AssignedKeywords" + this.Id.ToString()] == null)
                    Session["AssignedKeywords" + this.Id.ToString()] = GetBookKeywords(this.Id);
                     
                return (List<USAToday.Booklist.Business.Keyword>)Session["AssignedKeywords" + this.Id.ToString()]; 
                }
            set { Session["AssignedKeywords" + this.Id.ToString()] = value; }
        }

        /// <summary>
        /// Gets or sets the assigned authors.
        /// </summary>
        /// <value>The assigned authors.</value>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.Author> AssignedAuthors
        {
            get {
                if (Session["AssignedAuthors" + this.Id.ToString()] == null)
                        Session["AssignedAuthors" + this.Id.ToString()] = GetBookAuthors(this.Id);
                
                return (List<USAToday.Booklist.Business.Author>)Session["AssignedAuthors" + this.Id.ToString()]; 
                }
            set { Session["AssignedAuthors" + this.Id.ToString()] = value; }
        }
        /// <summary>
        /// Gets or sets the assigned ISB ns.
        /// </summary>
        /// <value>The assigned ISB ns.</value>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.BookISBNView> AssignedISBNs
        {
            get { 
                    if(Session["AssignedISBNs" + this.Id.ToString()] == null)
                        Session["AssignedISBNs" + this.Id.ToString()] = GetBookISBNs(this.Id);

                    return (List<USAToday.Booklist.Business.BookISBNView>)Session["AssignedISBNs" + this.Id.ToString()]; 
                }
            set { Session["AssignedISBNs" + this.Id.ToString()] = value; }
        }

        private static List<USAToday.Booklist.Business.BookSalesByProvider> GetBookSales(int bookId)
        {
            using (var data = new USATodayBookListEntities())
            {
                var isbnSales = data.GetBookSales(bookId);
                return isbnSales.ToList<USAToday.Booklist.Business.BookSalesByProvider>();
            }
        }

        /// <summary>
        /// Gets the book categories.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
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
        /// Gets the book keywords.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static List<USAToday.Booklist.Business.Keyword> GetBookKeywords(int bookId)
        {
            /*
            //USATodayBookListEntities data = new USATodayBookListEntities();
            USAToday.Booklist.Business.Book book = new USAToday.Booklist.Business.Book();
            book = Global.BookEntities.Books.SingleOrDefault(l => l.Id == bookId);           
            return (book == null ? new USAToday.Booklist.Business.Book().Keywords.ToList() : book.Keywords.ToList());
             */
            
            // copied from KeywordLookup.GetAssignedKeywords -- should merge the two methods since they do the same thing 
            using (var data = new USATodayBookListEntities()) {
                var keywords = from k in data.Keywords
                               from b in k.Books
                               where b.Id == bookId
                               orderby k.KeywordDescription
                               select k;

                return keywords.ToList<USAToday.Booklist.Business.Keyword>();
            }
        }

        /// <summary>
        /// Gets the book authors.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static List<USAToday.Booklist.Business.Author> GetBookAuthors(int bookId)
        {
            
            /* USAToday.Booklist.Business.Book book = new USAToday.Booklist.Business.Book();
            book = Global.BookEntities.Books.SingleOrDefault(l => l.Id == bookId);
            return (book == null ? new USAToday.Booklist.Business.Book().Authors.ToList() : book.Authors.ToList()); */
            using (var data= new USATodayBookListEntities()) {
                var authors= from a in data.Authors
                             from b in a.Books
                             where b.Id == bookId
                             orderby a.DisplayName
                             select a;

                return authors.ToList<USAToday.Booklist.Business.Author>();
            }
        }
        /// <summary>
        /// Gets the book ISB ns.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static List<USAToday.Booklist.Business.BookISBNView> GetBookISBNs(int bookId)
        {
            using (var data = new USATodayBookListEntities())
            {
                var bookISBNs = from b in data.BookISBNViews
                                where b.BookID == bookId
                                select b;

                return bookISBNs.ToList<USAToday.Booklist.Business.BookISBNView>();
            }
        }
        /// <summary>
        /// Gets the edit status.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private static List<USAToday.Booklist.Business.EditStatus> GetEditStatus()
        {
            using (var data = new USATodayBookListEntities())
            {
                var editStatus = from e in data.EditStatus
                                 select e;
                return editStatus.ToList<USAToday.Booklist.Business.EditStatus>();
            }
        }

        /// <summary>
        /// Gets the book class.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private static List<USAToday.Booklist.Business.BookClass> GetBookClass()
        {
            using (var data = new USATodayBookListEntities())
            {
                var bookClass = from e in data.BookClasses
                                 select e;
                return bookClass.ToList<USAToday.Booklist.Business.BookClass>();
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
            Id = int.Parse(Request["ID"]);

            if (Id == 0)
            {
                AddMode = true;
                SaveButton.Text = "Add Book";
            }
            else
            {
                AddMode = false;
                SaveButton.Text = "Save Book";
            }
            
            if (!Page.IsPostBack)
            {

                this.PurchaseISBNTextBox.Text = (Request["isbn"] == null ? string.Empty : Request["isbn"]);

                AddCategoryButton.OnClientClick = "addCategory(" + this.Id + ");";
                AddKeywordButton.OnClientClick = "addKeyword(" + this.Id + ");";
                AddAuthorButton.OnClientClick = "addAuthor(" + this.Id + ");";
                AddISBNButton.OnClientClick = "addISBN(" + this.Id + ");";

                //UnAssignedCategories = null;
                AssignedCategories = null; Session["AssignedCategories" + this.Id] = null;
                UnAssignedKeywords = null; Session["UnAssignedKeywords" + this.Id] = null;
                AssignedKeywords = null; Session["AssignedKeywords" + this.Id] = null;
                AssignedISBNs = null; Session["AssignedISBNs" + this.Id] = null;
                AssignedAuthors = null; Session["AssignedAuthors" + this.Id] = null;

                FillDropDowns();
                FillGridViews();
                FillISBNSalesByProvider();

                if (AddMode == false)
                    FillData();
                else {
                    var id= ""+Session["RedirectToBookIsbnId"];
                    int ID;
                    if (int.TryParse(id, out ID)) {
                        using (var data = new USATodayBookListEntities()) {
                            // FIXME: not tested: what happens when the id is not found?
                            var isbn= data.BookISBNs.Single(i => i.Id == ID);
                            TitleTextBox.Text= isbn.Title;
                            AuthorDisplayTextBox.Text= isbn.Author;
                            PurchaseISBNTextBox.Text= isbn.ISBN;
                        }
                    }
                }


            }
            else
            {
                if (base.GetClientParamName() == "RefreshCategories")
                {
                    if (base.GetClientParamValue() != "undefined")
                    {
                        // Rebind
                        CategoryGridView.DataSource = AssignedCategories;
                        CategoryGridView.DataBind();
                    }

                    //hide error msg in case if it is visible
                    this.CategoryErrorLabel.Visible = false;
                }
                else if (base.GetClientParamName() == "RefreshKeywords")
                {
                    if (base.GetClientParamValue() != "undefined")
                    {
                        //Rebind
                        KeywordGridView.DataSource = AssignedKeywords;
                        KeywordGridView.DataBind();
                    }
                }
                else if (base.GetClientParamName() == "RefreshAuthors")
                {
                    if (base.GetClientParamValue() == "RedirectToAuthor")
                    {
                        Response.Redirect("Author.aspx?Id=0");
                    }
                    else if (base.GetClientParamValue() != "undefined")
                    {
                        //Rebind
                        AuthorGridView.DataSource = AssignedAuthors;
                        AuthorGridView.DataBind();
                    }
                }
                else if (base.GetClientParamName() == "RefreshISBNs")
                {
                    if (base.GetClientParamValue() != "undefined")
                    {
                        //Rebind
                        //set it to null so it gets a fresh copy from database
                        ISBNGridView.DataSource = AssignedISBNs;
                        ISBNGridView.DataBind();
                    }
                }
                else if (base.GetClientParamName() == "RefreshISBN")
                {
                    if (base.GetClientParamValue() != "undefined")
                    {
                        //Rebind
                        //set it to null so it gets a fresh copy from database
                        AssignedISBNs = null;
                        ISBNGridView.DataSource = AssignedISBNs;
                        ISBNGridView.DataBind();
                    }
                }

                base.ResetClientParam();
            }

            // Hide buttons
            if (AppCode.AppCache.GetUser(User.Identity.Name).CanWrite == false)
            {
                this.SaveButton.Visible = false;
                this.AddCategoryButton.Visible = false;
                this.AddKeywordButton.Visible = false;
                this.AddAuthorButton.Visible = false;
                this.AddISBNButton.Visible = false;

                ISBNGridView.Columns[3].Visible = false;
                AuthorGridView.Columns[1].Visible = false;
            }
        }


        private void FillISBNSalesByProvider()
        {
            using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
            {
                //Get book ISBN Sales By Provider
                SalesGridView.DataSource = GetBookSales(this.Id);
                SalesGridView.DataBind();

                TotalBookSales s = data.GetTotalBookSales(this.Id).SingleOrDefault();
                this.tdTotalISBNSales.InnerText = (s.BookSales == null) ? "0" : s.BookSales.Value.ToString("#,###");
            }
        }


        /// <summary>
        /// Fills the data.
        /// </summary>
        /// <remarks></remarks>
        private void FillData()
        {
            using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
            {
                var book = data.Books.Single(l => l.Id == this.Id);
                
                this.TitleTextBox.Text = book.Title;
                this.AuthorDisplayTextBox.Text = book.AuthorDisplay;
                this.PublishDateTextBox.Text = (book.PubDate == null || book.PubDate.Value == DateTime.MinValue ? "" : book.PubDate.Value.ToShortDateString());
                this.LastEditedDate.Text = book.LastEdited.ToString("MM/dd/yyyy hh:mm tt");
                this.FirstOccuranceDate.Text = book.Created.ToShortDateString();

                this.ClassDropDownList.SelectedValue = book.BookClassId.ToString();
                this.StatusDropDownList.SelectedValue = book.StatusId.ToString();

                this.BriefDescriptionTextBox.Text = book.BriefDescription;
                this.LongDescriptionTextBox.Text = book.BookDesc;
                this.BackgroundInfoTextBox.Text = book.BackgroundInfo;
                this.BookCoverURLTextBox.Text = book.URL;
                this.cbBookCoverUrl.Checked = book.URLCompleted;
                this.PurchaseISBNTextBox.Text = book.PurchaseISBN;
                this.ASINTextBox.Text = book.ASIN;

                this.BookCoverImage.ImageUrl = book.URL;

                this.EditStatusDropDownList.SelectedValue = book.EditStatusId.ToString();
             
            }
        }

        /// <summary>
        /// Fills the drop downs.
        /// </summary>
        /// <remarks></remarks>
        private void FillDropDowns()
        {
               this.EditStatusDropDownList.DataTextField = "EditStatusDesc";
                this.EditStatusDropDownList.DataValueField = "Id";
                this.EditStatusDropDownList.DataSource = GetEditStatus();
                this.EditStatusDropDownList.DataBind();

                this.ClassDropDownList.DataTextField = "BookClassDescription";
                this.ClassDropDownList.DataValueField = "id";
                this.ClassDropDownList.DataSource = GetBookClass();
                this.ClassDropDownList.DataBind();

                this.StatusDropDownList.DataTextField = "StatusDesc";
                this.StatusDropDownList.DataValueField = "Id";
                this.StatusDropDownList.DataSource = GetStatus();
                this.StatusDropDownList.DataBind();
                
        }

        /// <summary>
        /// Fills the grid views.
        /// </summary>
        /// <remarks></remarks>
        private void FillGridViews()
        {
            CategoryGridView.DataSource = AssignedCategories;
            CategoryGridView.DataBind();

            KeywordGridView.DataSource = AssignedKeywords;
            KeywordGridView.DataBind();

            AuthorGridView.DataSource = AssignedAuthors;
            AuthorGridView.DataBind();

            ISBNGridView.DataSource = AssignedISBNs;
            ISBNGridView.DataBind();
        }

        /// <summary>
        /// Handles the Sorting event of the CategoryGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void CategoryGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Get the list of categories 
            List<USAToday.Booklist.Business.BookCategoryView> categories = default(List<USAToday.Booklist.Business.BookCategoryView>);
            categories = GetAssignedBookCategories(this.Id);

            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
                LastSortDirection = string.Empty;

            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "CategoryDescription":
                    categories = Sort(categories, category => category.Category);
                    break;
            }

            LastSortKey = e.SortExpression;

            // Rebind 
            CategoryGridView.DataSource = categories;
            CategoryGridView.DataBind();
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private static List<USAToday.Booklist.Business.Status> GetStatus()
        {
            using (var data = new USATodayBookListEntities())
            {
                var status = from e in data.Status
                             select e;
                return status.ToList<USAToday.Booklist.Business.Status>();
            }
        }

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.BookCategoryView> Sort(List<USAToday.Booklist.Business.BookCategoryView> list,
                Func<USAToday.Booklist.Business.BookCategoryView, string> sortKey)
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
        private List<USAToday.Booklist.Business.BookCategoryView> Sort(List<USAToday.Booklist.Business.BookCategoryView> list,
                Func<USAToday.Booklist.Business.BookCategoryView, decimal> sortKey)
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
        /// Handles the PageIndexChanging event of the CategoryGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void CategoryGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CategoryGridView.PageIndex = e.NewPageIndex;
            CategoryGridView.DataSource = AssignedCategories;
            CategoryGridView.DataBind();

        }

        /// <summary>
        /// Handles the PageIndexChanging event of the KeywordGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void KeywordGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            KeywordGridView.PageIndex = e.NewPageIndex;
            KeywordGridView.DataSource = AssignedKeywords;
            KeywordGridView.DataBind();

        }

        /// <summary>
        /// Handles the PageIndexChanging event of the AuthorGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void AuthorGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AuthorGridView.PageIndex = e.NewPageIndex;
            AuthorGridView.DataSource = AssignedAuthors;
            AuthorGridView.DataBind();

        }
        /// <summary>
        /// Handles the PageIndexChanging event of the ISBNGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ISBNGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ISBNGridView.PageIndex = e.NewPageIndex;
            ISBNGridView.DataSource = AssignedISBNs;
            ISBNGridView.DataBind();

        }

        /// <summary>
        /// Handles the RowDeleting event of the AuthorGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewDeleteEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void AuthorGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //remove author from session
            AssignedAuthors.RemoveAt(AssignedAuthors.FindIndex(l => l.Id == int.Parse(e.Keys[0].ToString())));

            //Rebind
            AuthorGridView.DataSource = AssignedAuthors;
            AuthorGridView.DataBind();
        }
        /// <summary>
        /// Handles the RowDeleting event of the ISBNGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewDeleteEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ISBNGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //remove author from session
            AssignedISBNs.RemoveAt(AssignedISBNs.FindIndex(l => l.Id == int.Parse(e.Keys[0].ToString())));

            //Rebind
            ISBNGridView.DataSource = AssignedISBNs;
            ISBNGridView.DataBind();
        }

        /// <summary>
        /// Handles the Click event of the SaveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void SaveButton_Click(object sender, EventArgs e) {
            //validate Category
            if (AssignedCategories == null || AssignedCategories.Count() == 0) {
                CategoryErrorLabel.Visible = true;
                return;
            }

            CategoryErrorLabel.Visible = false;

            var context= new USATodayBookListEntities();
            USAToday.Booklist.Business.Book book;
            if (AddMode) {
                book= new USAToday.Booklist.Business.Book();
                book.Created = DateTime.Now;
            } else {
                book = context.Books
                    .Include("BookISBNs")
                    .Include("Categories")
                    .Include("Keywords")
                    .Include("Authors")
                    .Single(l => l.Id == Id);
                context.AttachTo("Books", book);
            }

            book.Title = this.TitleTextBox.Text;
            book.AuthorDisplay = this.AuthorDisplayTextBox.Text;
            if (!string.IsNullOrEmpty(this.PublishDateTextBox.Text))
                book.PubDate = Convert.ToDateTime(this.PublishDateTextBox.Text);
            book.BookClassId = byte.Parse(this.ClassDropDownList.SelectedValue);
            book.BriefDescription = this.BriefDescriptionTextBox.Text;
            book.BookDesc = this.LongDescriptionTextBox.Text;
            book.BackgroundInfo = this.BackgroundInfoTextBox.Text;
            book.URL = this.BookCoverURLTextBox.Text;
            book.URLCompleted = this.cbBookCoverUrl.Checked;
            book.PurchaseISBN = this.PurchaseISBNTextBox.Text;
            book.ASIN = this.ASINTextBox.Text;

            book.EditStatusId = byte.Parse(this.EditStatusDropDownList.SelectedValue);
            book.StatusId = byte.Parse(StatusDropDownList.SelectedValue);

            book.Categories= new List<Category>();
            foreach (var aCat in AssignedCategories) {
                book.Categories.Add(context.Categories.Single(c => c.Id == aCat.CategoryID));
                if (aCat.PrimaryCategory) book.PrimaryCategoryID= aCat.CategoryID;
            }

            // if we use book.Keywords= AssignedKeywords we get a foreign key constraint from trying to append a new keyword name that's already in use
            book.Keywords= new List<Keyword>();
            foreach (var key in AssignedKeywords) {
                book.Keywords.Add(context.Keywords.Single(k => k.Id == key.Id));
            }

            // need to bring authors into this context, also
            book.Authors = new List<USAToday.Booklist.Business.Author>();
            foreach (var author in AssignedAuthors)
                book.Authors.Add(context.Authors.Single(a => a.Id == author.Id));
            

            // if we use book.BookISBNs = AssignedISBNs we get a multiplicity violation when re-using an existing isbn
            book.BookISBNs= new List<BookISBN>();
            foreach (var isbn in AssignedISBNs) {
                book.BookISBNs.Add(context.BookISBNs.Single(i => i.Id == isbn.Id));
            }
            

            book.LastEdited = DateTime.Now;
            if (AddMode)
                context.Books.AddObject(book);
            context.DetectChanges();
            context.SaveChanges();            
            context.MaybeAdjustBookStatus(book.Id);
            Audit(book.Id);

            if (base.IsDialog)
                base.CloseClientWindow(book.Title);
            else
                Response.Redirect("Book.aspx?Id=" + book.Id.ToString());
        }

        protected void ISBNGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //set the text value of select link button to the book title
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                USAToday.Booklist.Business.BookISBNView b = ((USAToday.Booklist.Business.BookISBNView)e.Row.DataItem);

                if (b.EditStatusDescription != null && b.EditStatusDescription.ToUpper() == "DONE EDITING")
                    e.Row.Cells[3].Style.Add("color", "green");
            }
        }


        protected string GetPrimaryCategory(object o)
        {
            if ( o != null && bool.Parse(o.ToString()) == true)
                return "Yes";
            else
                return "No";
        }

        /// <summary>
        /// Audits the specified book id.
        /// </summary>
        /// <param name="BookId">The book id.</param>
        /// <remarks></remarks>
        private void Audit(int BookId)
        {
            Audit audit = new Audit();

            audit.TableName = "Books";
            audit.UpdatedBy = User.Identity.Name;
            audit.Updated = DateTime.Now;
            audit.UpdateRowId = BookId;

            var context= new USATodayBookListEntities();
            context.Audits.AddObject(audit);
            context.SaveChanges();
        }

        /// <summary>
        /// Handles the Click event of the ManageBooksButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ManageBooksButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("BookList.aspx");
        }


        protected void RefreshBookUrlButton_Click(object sender, EventArgs e)
        {
            if (this.ASINTextBox.Text == string.Empty)
                this.ErrorLabel.Text = "Please enter ASIN to retrieve Book Cover URL.";
            else
            {
                updateImages(new SignedRequestHelper(MY_AWS_ACCESS_KEY_ID, MY_AWS_SECRET_KEY, DESTINATION));
                BookCoverImage.ImageUrl = BookCoverURLTextBox.Text.Trim();
            }
        }

        protected void updateImages(SignedRequestHelper helper)
        {
            String requestUrl;            

            IDictionary<string, string> r1 = new Dictionary<string, String>();
            r1["Service"] = "AWSECommerceService";
            r1["Version"] = "2009-03-31";
            r1["Operation"] = "ItemSearch";
            r1["SearchIndex"] = "Books";
            r1["Keywords"] = this.ASINTextBox.Text;
            r1["ResponseGroup"] = "Images";
            r1["AssociateTag"] = "ut056-20";


            requestUrl = helper.Sign(r1);

            string imageUrl = string.Empty;
            FetchImage(requestUrl, out imageUrl);

            //if fetch by ASIN fails then try to fetch images by title + author
            if (imageUrl.Length == 0)
            {
                r1["Keywords"] = this.TitleTextBox.Text + " + " + this.AuthorDisplayTextBox.Text;
                requestUrl = helper.Sign(r1);
                FetchImage(requestUrl, out imageUrl);
            }


            ErrorLabel.Text = "";
            this.BookCoverURLTextBox.Text = imageUrl;
            string error = string.Empty;

            if (imageUrl.Length == 0)
                error += "Could not retrieve image URL.";

            ErrorLabel.Text = error;

        }

        /// <summary>
        /// Fetches the title.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="imageUrl">The image URL.</param>
        /// <remarks></remarks>
        protected void FetchImage(string url, out string imageUrl)
        {
            string retImageUrl = string.Empty;
            try
            {
                WebRequest request = HttpWebRequest.Create(url);                
                WebResponse response = request.GetResponse();                
                XmlDocument doc = new XmlDocument();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                doc.Load(reader.BaseStream);

                string page = doc.InnerXml;
                XmlNodeList errorMessageNodes = doc.GetElementsByTagName("Message", NAMESPACE);
                if (errorMessageNodes != null && errorMessageNodes.Count > 0)
                {
                    imageUrl = retImageUrl;
                    return;
                }

                int imagesIndex = page.IndexOf("<ImageSet Category=\"primary\"><SwatchImage><URL>");
                int srcIndex = 0;
                if (imagesIndex > 0)
                {
                    srcIndex = page.IndexOf("</URL>", imagesIndex, 200);
                    if (srcIndex > 0)
                    {
                        retImageUrl = page.Substring(imagesIndex + 47, srcIndex - imagesIndex - 47);
                        retImageUrl = retImageUrl.Replace("SL30_", "SL300");
                    }
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Caught Exception: " + e.Message);
                System.Console.WriteLine("Stack Trace: " + e.StackTrace);
            }
            imageUrl = retImageUrl;
        }
    }
}