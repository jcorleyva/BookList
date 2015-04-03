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
    public partial class ISBN : PageBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether [add mode].
        /// </summary>
        /// <value><c>true</c> if [add mode]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool AddMode { get; set; }
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        /// <remarks></remarks>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the associated book.
        /// </summary>
        /// <value>The associated book.</value>
        /// <remarks></remarks>
        private USAToday.Booklist.Business.Book AssociatedBook
        {
            get { return (USAToday.Booklist.Business.Book)Session["AssociatedBook" + this.Id.ToString()]; }
            set { Session["AssociatedBook" + this.Id.ToString()] = value; }
        }


        /// <summary>
        /// Gets the publishers.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private static List<USAToday.Booklist.Business.Publisher> GetPublishers()
        {
            using (var data = new USATodayBookListEntities())
            {
                var publishers = from e in data.Publishers
                                 orderby e.PublisherName ascending
                                 select e;
                return publishers.ToList<USAToday.Booklist.Business.Publisher>();
            }
        }

        /// <summary>
        /// Gets the imprints.
        /// </summary>
        /// <param name="publisherId">The publisher id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static List<USAToday.Booklist.Business.Imprint> GetImprints(int publisherId)
        {
            using (var data = new USATodayBookListEntities())
            {
                var imprints = from e in data.Imprints
                                 where e.PublisherId == publisherId
                                 orderby e.ImprintName ascending
                                 select e;
                return imprints.ToList<USAToday.Booklist.Business.Imprint>();
            }
        }

        /// <summary>
        /// Gets the media formats.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private static List<USAToday.Booklist.Business.MediaFormat> GetMediaFormats()
        {
            using (var data = new USATodayBookListEntities())
            {
                var mediaFormat = from e in data.MediaFormats
                                  select e;
                return mediaFormat.ToList<USAToday.Booklist.Business.MediaFormat>();
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
        /// Gets the links.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private static List<USAToday.Booklist.Business.Link> GetLinks()
        {
            using (var data = new USATodayBookListEntities())
            {
                var links = from l in data.Links
                            select l;

                return links.ToList<USAToday.Booklist.Business.Link>();
            }
        }

        private static List<USAToday.Booklist.Business.ISBNSalesByProvider> GetISBNSales(string ISBN)
        {
            using (var data = new USATodayBookListEntities())
            {
                var isbnSales = data.GetISBNSales(ISBN.Trim());
                return isbnSales.ToList<USAToday.Booklist.Business.ISBNSalesByProvider>();
            }
        }

        private static List<USAToday.Booklist.Business.TotalISBNSales> GetTotalISBNSales(string ISBN)
        {
            using (var data = new USATodayBookListEntities())
            {
                var isbnSales = data.GetTotalISBNSales(ISBN.Trim());
                return isbnSales.ToList<USAToday.Booklist.Business.TotalISBNSales>();
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
                SaveButton.Text = "Add";
            }
            else
            {
                AddMode = false;
                SaveButton.Text = "Save";
                ISBNTextBox.ReadOnly = true;
            }

            if (!Page.IsPostBack)
            {
                
                AssociatedBook = null;

                //fill up Links Grid View with links from database
                LinksGridView.DataSource = GetLinks();
                LinksGridView.DataBind(); 

                FillDropDownLists();
                FillISBNSalesByProvider();

                if (AddMode == false)
                    FillData();
                else
                    this.AddBookLink.OnClientClick = "addBook(" + this.Id.ToString() + ",'');";
            }
            else
            {
                if (base.GetClientParamName() == "RefreshBook")
                {
                    //user clicked on Add Book button on Book Lookup dialog
                    if (base.GetClientParamValue() == "RedirectToBook")
                        Response.Redirect("Book.aspx?Id=0");
                    else
                    {
                        //user selected book on Book Lookup dialog
                        if (AssociatedBook != null)
                        {
                            AddBookLink.Text = AssociatedBook.Title;
                            EditBookLink.NavigateUrl = "Book.aspx?Id=" + AssociatedBook.Id.ToString();
                        }
                    }
                }
            }

            // Hide buttons
            if (AppCode.AppCache.GetUser(User.Identity.Name).CanWrite == false)
            {
                this.SaveButton.Visible = false;
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
                var isbn = data.BookISBNs.Single(l => l.Id == Id);

                this.AddBookLink.OnClientClick = "addBook(" + this.Id.ToString() + ",'" + isbn.ISBN + "');"; 
                
                this.ISBNTextBox.Text = isbn.ISBN;
                this.PriceTextBox.Text = isbn.Price.ToString();
                this.PublisherDropDownList.SelectedValue = (isbn.PublisherID.ToString());

                //fill imprint drop down list with values if there are any that belong to this publisher
                this.ImprintDropDownList.DataTextField = "ImprintName";
                this.ImprintDropDownList.DataValueField = "Id";
                this.ImprintDropDownList.DataSource = GetImprints((isbn.PublisherID));
                this.ImprintDropDownList.DataBind();
                this.ImprintDropDownList.Items.Insert(0, new ListItem("---Select---", "0"));

                this.ImprintDropDownList.SelectedValue = (isbn.ImprintId.ToString());
                this.MediaFormatDropDownList.SelectedValue = isbn.MediaFormatId.ToString();
                if (isbn.Excluded)
                    this.PermExcludeCheckBox.Checked = true;
                else
                    this.PermExcludeCheckBox.Checked = false;
                this.EditStatusDropDownList.SelectedValue = isbn.EditStatusId.ToString();

                AssociatedBook = (USAToday.Booklist.Business.Book)data.Books.SingleOrDefault(b => b.Id == isbn.BookID);
                AddBookLink.Text = ((AssociatedBook == null || AssociatedBook.Title == "") ? "Associate Book" : AssociatedBook.Title);

                if (AssociatedBook == null)
                {
                    EditBookLink.Visible = false;
                }
                else
                {
                    EditBookLink.Visible = true;
                    EditBookLink.NavigateUrl = "Book.aspx?Id=" + isbn.BookID.ToString();
                }


            }
        }

        private void FillISBNSalesByProvider()
        {
            using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
            {
                var bookISBN = data.BookISBNs.SingleOrDefault(l => l.Id == Id);
                string isbn = (bookISBN == null ? "0" : bookISBN.ISBN);

                //Get ISBN Sales By Provider
                SalesGridView.DataSource = GetISBNSales(isbn);
                SalesGridView.DataBind();

                List<USAToday.Booklist.Business.TotalISBNSales> p = GetTotalISBNSales(isbn);
                this.tdTotalISBNSales.InnerText = (p.Single().ISBNSales == null) ? "0" : p.Single().ISBNSales.Value.ToString("#,###");
                this.tdTotalBookSales.InnerText = (p.Single().BookSales == null) ? "0" : p.Single().BookSales.Value.ToString("#,###");
            }
        }

        /// <summary>
        /// Fills the drop down lists.
        /// </summary>
        /// <remarks></remarks>
        private void FillDropDownLists()
        {            
            this.MediaFormatDropDownList.DataTextField = "MediaFormatDescription";
            this.MediaFormatDropDownList.DataValueField = "Id";
            this.MediaFormatDropDownList.DataSource = GetMediaFormats();
            this.MediaFormatDropDownList.DataBind();
        
            this.EditStatusDropDownList.DataTextField = "EditStatusDesc";
            this.EditStatusDropDownList.DataValueField = "Id";
            this.EditStatusDropDownList.DataSource = GetEditStatus();
            this.EditStatusDropDownList.DataBind();

            this.FillPublisherDropDownList();
        }

        private void FillPublisherDropDownList()
        {
            this.PublisherDropDownList.DataTextField = "PublisherName";
            this.PublisherDropDownList.DataValueField = "Id";
            this.PublisherDropDownList.DataSource = GetPublishers();
            this.PublisherDropDownList.DataBind();
            this.PublisherDropDownList.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        private void FillImprintDropDownList()
        {
            this.ImprintDropDownList.DataTextField = "ImprintName";
            this.ImprintDropDownList.DataValueField = "Id";
            this.ImprintDropDownList.DataSource = GetImprints(int.Parse(this.PublisherDropDownList.SelectedValue));
            this.ImprintDropDownList.DataBind();
            this.ImprintDropDownList.Items.Insert(0, new ListItem("---Select---", "0"));
        }

        /// <summary>
        /// Handles the Click event of the SaveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            decimal price = 0;
            if (AddMode == true)
            {
                using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
                {
                    USAToday.Booklist.Business.BookISBN isbn = new USAToday.Booklist.Business.BookISBN();
                    isbn.ISBN = this.ISBNTextBox.Text;
                    if (AssociatedBook != null)
                    {
                        isbn.BookID = AssociatedBook.Id;
                        //isbn.Title = AssociatedBook.Title;
                    }
                    decimal.TryParse(this.PriceTextBox.Text.Trim(),out price);
                    isbn.Price = price;
                    if (this.PublisherDropDownList.SelectedValue == "0")
                        isbn.PublisherID = 1;
                    else
                        isbn.PublisherID = int.Parse(this.PublisherDropDownList.SelectedValue);
                    if (this.ImprintDropDownList.SelectedValue == "0")
                        isbn.ImprintId = null;
                    else
                        isbn.ImprintId = int.Parse(this.ImprintDropDownList.SelectedValue);
                    isbn.MediaFormatId = int.Parse(this.MediaFormatDropDownList.SelectedValue);
                    isbn.Excluded = this.PermExcludeCheckBox.Checked;
                    isbn.EditStatusId = byte.Parse(this.EditStatusDropDownList.SelectedValue);                   

                    data.BookISBNs.AddObject(isbn);
                    data.SaveChanges();
                    Response.Redirect("ISBN.aspx?Id=" + isbn.Id.ToString());
                }
            }
            else
            {
                using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
                {
                    var isbn = data.BookISBNs.Include("Book").Single(l => l.Id == Id);

                    isbn.ISBN = this.ISBNTextBox.Text;


                    if (AssociatedBook != null)
                    {
                        isbn.BookID = AssociatedBook.Id;
                        //isbn.Title = AssociatedBook.Title;
                    }

                    decimal.TryParse(this.PriceTextBox.Text.Trim(), out price);
                    isbn.Price = price;
                    if(this.PublisherDropDownList.SelectedValue == "0")
                        isbn.PublisherID = 1;
                    else
                        isbn.PublisherID = int.Parse(this.PublisherDropDownList.SelectedValue);
                    if(this.ImprintDropDownList.SelectedValue == "0") 
                        isbn.ImprintId = null;
                    else
                        isbn.ImprintId = int.Parse(this.ImprintDropDownList.SelectedValue);
                    isbn.MediaFormatId = int.Parse(this.MediaFormatDropDownList.SelectedValue);
                    isbn.Excluded = this.PermExcludeCheckBox.Checked;
                    isbn.EditStatusId = byte.Parse(this.EditStatusDropDownList.SelectedValue);

                    data.SaveChanges();
                    if (base.IsDialog)
                        base.CloseClientWindow("Refresh");
                    else
                        Response.Redirect("ISBN.aspx?Id=" + isbn.Id.ToString());

                }
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the PublisherDropDown control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void PublisherDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.FillImprintDropDownList();
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the LinksGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void LinksGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LinksGridView.PageIndex = e.NewPageIndex;
            LinksGridView.DataSource = GetLinks();
            LinksGridView.DataBind();
        }

        /// <summary>
        /// Handles the Click event of the ManageISBNsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ManageISBNsButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ISBNList.aspx");
        }

        protected override void OnInit(EventArgs e)
        {
            this.tbPublisher.onSaveClick += new EventHandler(tbPublisher_SaveClick);
            this.tbImprint.onSaveClick +=new EventHandler(tbImprint_SaveClick);
            base.OnInit(e);
        }

        private void tbPublisher_SaveClick(object sender, EventArgs e)
        {
            using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
            {
                USAToday.Booklist.Business.Publisher publisher = new USAToday.Booklist.Business.Publisher();
                publisher.PublisherName = this.tbPublisher.Text;

                data.Publishers.AddObject(publisher);
                data.SaveChanges();
                this.Audit(publisher.Id);

                this.FillPublisherDropDownList();
                this.PublisherDropDownList.SelectedValue = publisher.Id.ToString();
                this.FillImprintDropDownList();
                this.tbPublisher.Text = "";
            }
        }

        private void tbImprint_SaveClick(object sender, EventArgs e)
        {
            using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
            {
                USAToday.Booklist.Business.Imprint imprint = new USAToday.Booklist.Business.Imprint();
                imprint.ImprintName = this.tbImprint.Text;
                imprint.PublisherId = int.Parse(this.PublisherDropDownList.SelectedValue);

                data.Imprints.AddObject(imprint);
                data.SaveChanges();

                this.FillImprintDropDownList();
                this.ImprintDropDownList.SelectedValue = imprint.Id.ToString();
                this.tbImprint.Text = "";
            }
        }

        private void Audit(int PublisherId)
        {
            Audit audit = new Audit();

            audit.TableName = "Publisher";
            audit.UpdatedBy = User.Identity.Name;
            audit.Updated = DateTime.Now;
            audit.UpdateRowId = PublisherId;

            Global.BookEntities.Audits.AddObject(audit);
            Global.BookEntities.SaveChanges();
        }

    }

}