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
    public partial class Publisher : System.Web.UI.Page
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
                BindGrid();
                if (AddMode == false)
                    FillData();
            }

            // Hide buttons
            if (AppCode.AppCache.GetUser(User.Identity.Name).CanWrite == false)
            {
                this.SaveButton.Visible = false;
                this.AddImprintButton.Visible = false;
                this.ImprintGridView.Columns[1].Visible = false;
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
                var publisher = data.Publishers.Single(l => l.Id == Id);

                this.PublisherNameTextBox.Text = publisher.PublisherName;
                this.ContactNameTextBox.Text = publisher.ContactName;
                this.PhoneTextBox.Text = publisher.Phone;
                this.EmailTextBox.Text = publisher.Email;
                this.NotesTextBox.Text = publisher.Notes;
            }
        }
        /// <summary>
        /// Binds the grid.
        /// </summary>
        /// <remarks></remarks>
        private void BindGrid()
        {
            USAToday.Booklist.Business.Publisher publisher = new USAToday.Booklist.Business.Publisher();
            var data = new USATodayBookListEntities();
            var imprints = from p in data.Imprints
                           where p.PublisherId == this.Id
                           select p;
            //publisher = data.Publishers.SingleOrDefault(l => l.Id == this.Id);
//            List<USAToday.Booklist.Business.Imprint> imprints = 

//                (publisher == null ? new USAToday.Booklist.Business.Publisher().Imprints.ToList() : publisher.Imprints.ToList());

            ImprintGridView.DataSource = imprints.ToList<USAToday.Booklist.Business.Imprint>();
            ImprintGridView.DataBind();
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
                    USAToday.Booklist.Business.Publisher publisher = new USAToday.Booklist.Business.Publisher();
                    publisher.PublisherName = this.PublisherNameTextBox.Text;
                    publisher.ContactName = this.ContactNameTextBox.Text;
                    publisher.Phone = this.PhoneTextBox.Text;
                    publisher.Email = this.EmailTextBox.Text;
                    publisher.Notes = this.NotesTextBox.Text;

                    data.Publishers.AddObject(publisher);
                    data.SaveChanges();
                    Audit(publisher.Id);
                    Response.Redirect("Publisher.aspx?Id=" + publisher.Id.ToString());
                }
            }
            else
            {
                using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
                {
                    var publisher = data.Publishers.Single(l => l.Id == Id);
                    publisher.PublisherName = this.PublisherNameTextBox.Text;
                    publisher.ContactName = this.ContactNameTextBox.Text;
                    publisher.Phone = this.PhoneTextBox.Text;
                    publisher.Email = this.EmailTextBox.Text;
                    publisher.Notes = this.NotesTextBox.Text;

                    data.SaveChanges();
                    Audit(publisher.Id);
                    Response.Redirect("Publisher.aspx?Id=" + publisher.Id.ToString());

                }
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

        /// <summary>
        /// Handles the RowDeleting event of the ImprintGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewDeleteEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void ImprintGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //remove imprint association to this publisher from database
            int imprintId = int.Parse(e.Keys[0].ToString());
            USAToday.Booklist.Business.Imprint imprint = Global.BookEntities.Imprints.Single(l => l.Id == imprintId);
            Global.BookEntities.Imprints.DeleteObject(imprint);            
            Global.BookEntities.SaveChanges();
            BindGrid();
        }

        /// <summary>
        /// Handles the Click event of the AddButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void AddButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Imprint.aspx?Id=0");
        }
    }
}