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
    public partial class KeywordLookup : CommonApp.WebPageBase
    {
        /// <summary>
        /// Gets the book id.
        /// </summary>
        /// <remarks></remarks>
        public int BookId
        {
            get { return int.Parse(Request["bookId"]); }
        }

        /// <summary>
        /// Gets the assigned keywords.
        /// </summary>
        /// <remarks></remarks>
        public List<Keyword> AssignedKeywords
        {
            get
            {
                if (Session["AssignedKeywords" + this.BookId.ToString()] == null)
                    Session["AssignedKeywords" + this.BookId.ToString()] = GetAssignedKeywords(this.BookId);

                return (List<Keyword>)Session["AssignedKeywords" + this.BookId.ToString()];

            }
        }

        /// <summary>
        /// Gets the un assigned keywords.
        /// </summary>
        /// <remarks></remarks>
        public List<Keyword> UnAssignedKeywords
        {
            get
            {
                if (Session["UnAssignedKeywords" + this.BookId.ToString()] == null)
                    Session["UnAssignedKeywords" + this.BookId.ToString()] = GetUnAssignedKeywords(this.BookId);

                return (List<Keyword>)Session["UnAssignedKeywords" + this.BookId.ToString()];
            }
        }

        /// <summary>
        /// Gets the assigned keywords.
        /// </summary>
        /// <param name="BookId">The book id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<Keyword> GetAssignedKeywords(int BookId)
        {
            /* USATodayBookListEntities data = new USATodayBookListEntities();
            USAToday.Booklist.Business.Book book = new USAToday.Booklist.Business.Book();
            book = data.Books.SingleOrDefault(l => l.Id == BookId);
            return (book == null ? new USAToday.Booklist.Business.Book().Keywords.ToList() : book.Keywords.ToList());*/

            using (var data = new USATodayBookListEntities()) {
                var keywords = from k in data.Keywords
                               from b in k.Books
                               where b.Id == BookId
                               orderby k.KeywordDescription
                               select k;

                return keywords.ToList<USAToday.Booklist.Business.Keyword>();
            }
        }

        /// <summary>
        /// Gets the un assigned keywords.
        /// </summary>
        /// <param name="BookId">The book id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<Keyword> GetUnAssignedKeywords(int BookId)
        {
            using (var data = new USATodayBookListEntities()) {
                /* USAToday.Booklist.Business.Book book = new USAToday.Booklist.Business.Book();
                book = Global.BookEntities.Books.SingleOrDefault(l => l.Id == BookId);
                book = (book == null ? new USAToday.Booklist.Business.Book() : book);

                List<Keyword> keywords = Global.BookEntities.Keywords.ToList<USAToday.Booklist.Business.Keyword>();

                foreach (USAToday.Booklist.Business.Keyword selectedKeyword in book.Keywords)
                {
                    keywords.Remove(selectedKeyword);
                }

                return keywords;
                 */
                var assignedKeywords = from k in data.Keywords
                                       from b in k.Books
                                       where b.Id == BookId
                                       orderby k.KeywordDescription
                                       select k;
                var keywords= data.Keywords.Except(assignedKeywords);

                return keywords.ToList<USAToday.Booklist.Business.Keyword>();
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
                Session["AssignedKeywords" + this.BookId.ToString()] = null;
                Session["UnAssignedKeywords" + this.BookId.ToString()] = null;

                arlKeyword.SourceListBox.DataTextField = "KeywordDescription";
                arlKeyword.SourceListBox.DataValueField = "Id";
                arlKeyword.SourceListBox.DataSource = UnAssignedKeywords;
                arlKeyword.SourceListBox.DataBind();

                arlKeyword.TargetListBox.DataTextField = "KeywordDescription";
                arlKeyword.TargetListBox.DataValueField = "Id";
                arlKeyword.TargetListBox.DataSource = AssignedKeywords;
                arlKeyword.TargetListBox.DataBind();
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
            AssignedKeywords.Clear();
            foreach (ListItem li in arlKeyword.TargetListBox.Items)
            {
                //add keyword to Assigned list
                Keyword k = new Keyword();
                k.Id = int.Parse(li.Value);
                k.KeywordDescription = li.Text;
                AssignedKeywords.Add(k);

                //Remove keyword from UnAssigned list
                if(UnAssignedKeywords.Exists(l => l.Id == k.Id))
                    UnAssignedKeywords.RemoveAt(UnAssignedKeywords.FindIndex(l => l.Id == k.Id));
            }

            //check to make sure UnAssignedKeyword matches with Items in UnAssigned List Box
            foreach (ListItem li in arlKeyword.SourceListBox.Items)
            {
                Keyword k = new Keyword();
                k.Id = int.Parse(li.Value);
                k.KeywordDescription = li.Text;
                if (!UnAssignedKeywords.Exists(l => l.Id == k.Id))
                    UnAssignedKeywords.Add(k);

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
            KeywordTextBox.Text = "";
            Response.Redirect("KeywordLookup.aspx?bookId=" + this.BookId);
        }

        /// <summary>
        /// Handles the Click event of the FilterButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void FilterButton_Click(object sender, EventArgs e)
        {
            List<USAToday.Booklist.Business.Keyword> keywords = GetUnAssignedKeywords(this.BookId);
            List<USAToday.Booklist.Business.Keyword> filteredKeywords = new List<Keyword>();

            foreach(Keyword k in keywords)
            {
                if (k.KeywordDescription.ToLower().Contains(this.KeywordTextBox.Text.Trim().ToLower()))
                    filteredKeywords.Add(k);
            }

            arlKeyword.SourceListBox.DataTextField = "KeywordDescription";
            arlKeyword.SourceListBox.DataValueField = "Id";
            arlKeyword.SourceListBox.DataSource = filteredKeywords;
            arlKeyword.SourceListBox.DataBind();

        }
    }
}