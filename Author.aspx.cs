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
    public partial class Author : PageBase
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

        private List<USAToday.Booklist.Business.Book> Books
        {
            get { return (List<USAToday.Booklist.Business.Book>)Session["AuthorBooks"+this.Id.ToString()]; }
            set { Session["AuthorBooks"+this.Id.ToString()] = value; }
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
            }

            if (!Page.IsPostBack)
            {
                if (AddMode == false)
                    FillData();

            }

            // Hide buttons
            if (AppCode.AppCache.GetUser(User.Identity.Name).CanWrite == false)
            {
                this.SaveButton.Visible = false;
                this.BookGridView.Columns[2].Visible = false;
            }

        }

        /// <summary>
        /// Fills the data.
        /// </summary>
        /// <remarks></remarks>
        private void FillData() {

            var data= new USATodayBookListEntities();
            var author = data.Authors.Single(l => l.Id == Id);

            this.FirstNameTextBox.Text = author.FirstName;
            this.InitialTextBox.Text = author.Initial;
            this.LastNameTextBox.Text = author.LastName;
            this.SuffixTextBox.Text = author.Suffix;
            this.DisplayNameTextBox.Text = author.DisplayName;
            this.DescriptionTextBox.Text = author.AuthorDesc;

            var books =
                from b in data.Books
                from a in b.Authors
                where a.Id == Id
                orderby b.Title
                select b;

            Books = books.ToList<USAToday.Booklist.Business.Book>();

            BookGridView.DataSource = Books;
            BookGridView.DataBind();

        }

        /// <summary>
        /// Handles the Click event of the SaveButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
                if (AddMode == true)
                {
                    using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
                    {
                        USAToday.Booklist.Business.Author author = new USAToday.Booklist.Business.Author();
                        author.FirstName = this.FirstNameTextBox.Text.Trim();
                        author.Initial = this.InitialTextBox.Text.Trim();
                        author.LastName = this.LastNameTextBox.Text.Trim();
                        author.Suffix = this.SuffixTextBox.Text;
                        author.DisplayName = this.DisplayNameTextBox.Text;
                        author.AuthorDesc = this.DescriptionTextBox.Text;

                        data.Authors.AddObject(author);
                        data.SaveChanges();
                        Audit(author.Id);

                        if (base.IsDialog)
                            base.CloseClientWindow(author.FirstName + "|" + author.LastName);
                        else
                            Response.Redirect("Author.aspx?id=" + author.Id.ToString());
                    }
                }
                else
                {
                        var data = new USATodayBookListEntities();
                        USAToday.Booklist.Business.Author author = data.Authors.Include("Books").Single(l => l.Id == Id);
                        data.AttachTo("Authors", author);

                        author.FirstName = this.FirstNameTextBox.Text.Trim();
                        author.Initial = this.InitialTextBox.Text.Trim();
                        author.LastName = this.LastNameTextBox.Text.Trim();
                        author.Suffix = this.SuffixTextBox.Text;
                        author.DisplayName = this.DisplayNameTextBox.Text;
                        author.AuthorDesc = this.DescriptionTextBox.Text;

                        author.Books = new List<USAToday.Booklist.Business.Book>();

                        foreach (var book in Books)
                            author.Books.Add(data.Books.Single(b => b.Id == book.Id));

                        data.SaveChanges();

                        Audit(author.Id);

                        if (base.IsDialog)
                            base.CloseClientWindow(author.FirstName + "|" + author.LastName);
                        else
                            Response.Redirect("Author.aspx?id=" + author.Id.ToString());
                }
        }

        private void Audit(int AuthorId)
        {
            Audit audit = new Audit();

            audit.TableName = "Author";
            audit.UpdatedBy = User.Identity.Name;
            audit.Updated = DateTime.Now;
            audit.UpdateRowId = AuthorId;

            Global.BookEntities.Audits.AddObject(audit);
            Global.BookEntities.SaveChanges();
        }

        /// <summary>
        /// Handles the Click event of the ManageAuthorsButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ManageAuthorsButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("AuthorList.aspx");
        }

        /// <summary>
        /// Handles the RowDeleting event of the BookGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewDeleteEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void BookGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //remove book association to this author
            Books.RemoveAt(Books.FindIndex(l => l.Id == int.Parse(e.Keys[0].ToString())));

            this.BookGridView.DataSource = Books;
            this.BookGridView.DataBind();
        }

    }
}