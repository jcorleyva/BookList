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
    public partial class Imprint : System.Web.UI.Page
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

        //Get All Publishers to show in drop down list
        /// <summary>
        /// Gets the publishers.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private static List<USAToday.Booklist.Business.Publisher> GetPublishers()
        {
            using (var data = new USATodayBookListEntities())
            {
                var publishers = from p in data.Publishers
                                 select p;

                return publishers.ToList<USAToday.Booklist.Business.Publisher>();
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
            }

            if (!Page.IsPostBack)
            {
                //bind Publishers Drop Down List
                this.PublisherDropDownList.DataTextField = "PublisherName";
                this.PublisherDropDownList.DataValueField = "Id";
                this.PublisherDropDownList.DataSource = GetPublishers();
                this.PublisherDropDownList.DataBind();

                if (AddMode == false)
                    FillData();
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
                var imprint = data.Imprints.Single(l => l.Id == Id);

                this.ImprintNameTextBox.Text = imprint.ImprintName;
                this.PublisherDropDownList.SelectedValue = imprint.PublisherId.ToString();
                this.ContactNameTextBox.Text = imprint.ContactName;
                this.PhoneTextBox.Text = imprint.Phone;
                this.EmailTextBox.Text = imprint.Email;
                this.NotesTextBox.Text = imprint.Notes;
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
            if (AddMode == true)
            {
                using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
                {
                    USAToday.Booklist.Business.Imprint imprint = new USAToday.Booklist.Business.Imprint();
                    imprint.ImprintName = this.ImprintNameTextBox.Text;
                    imprint.PublisherId = int.Parse(this.PublisherDropDownList.SelectedValue);
                    imprint.ContactName = this.ContactNameTextBox.Text;
                    imprint.Phone = this.PhoneTextBox.Text;
                    imprint.Email = this.EmailTextBox.Text;
                    imprint.Notes = this.NotesTextBox.Text;

                    data.Imprints.AddObject(imprint);
                    data.SaveChanges();
                    Response.Redirect("Imprint.aspx?Id=" + imprint.Id.ToString());
                }
            }
            else
            {
                    var imprint = Global.BookEntities.Imprints.Single(l => l.Id == Id);
                    imprint.ImprintName = this.ImprintNameTextBox.Text;
                    imprint.PublisherId = int.Parse(this.PublisherDropDownList.SelectedValue);
                    imprint.ContactName = this.ContactNameTextBox.Text;
                    imprint.Phone = this.PhoneTextBox.Text;
                    imprint.Email = this.EmailTextBox.Text;
                    imprint.Notes = this.NotesTextBox.Text;

                    Global.BookEntities.SaveChanges();
                    Response.Redirect("Imprint.aspx?Id=" + imprint.Id.ToString());

            }
        }
    }
}