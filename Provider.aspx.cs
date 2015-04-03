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
    public partial class Provider : System.Web.UI.Page
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
        /// Gets the channel types.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private static List<USAToday.Booklist.Business.ChannelType> GetChannelTypes()
        {
            using (var data = new USATodayBookListEntities())
            {
                var channelTypes = from e in data.ChannelTypes
                                    select e;
                return channelTypes.ToList<USAToday.Booklist.Business.ChannelType>();
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

                //bind Channel Type drop down list
                this.ChannelTypeDropDownList.DataTextField = "ChannelTypeDescription";
                this.ChannelTypeDropDownList.DataValueField = "Id";
                this.ChannelTypeDropDownList.DataSource = GetChannelTypes();
                this.ChannelTypeDropDownList.DataBind();


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
                var provider = data.Providers.Single(l => l.Id == Id);

                this.ProviderNameTextBox.Text = provider.ProviderName;
                this.ContactTextBox.Text = provider.SystemsContact;
                this.PhoneTextBox.Text = provider.Phone;
                this.EmailTextBox.Text = provider.Email;
                this.ActiveCheckBox.Checked = provider.Active;
                this.ChannelTypeDropDownList.SelectedValue = provider.ChannelTypeId.ToString();
                this.FileFormatDropDownList.SelectedValue = provider.FileExtension;
                this.RawFileNameTextBox.Text = provider.CurrentImportFile;
                this.ImportNotestTextBox.Text = provider.ImportNotes;
                this.MemoTextBox.Text = provider.Memo;
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
                    USAToday.Booklist.Business.Provider provider = new USAToday.Booklist.Business.Provider();
                    provider.ProviderName = this.ProviderNameTextBox.Text;
                    provider.SystemsContact = this.ContactTextBox.Text;
                    provider.Phone = this.PhoneTextBox.Text;
                    provider.Email = this.EmailTextBox.Text;
                    provider.Active = this.ActiveCheckBox.Checked;
                    provider.ChannelTypeId = byte.Parse(this.ChannelTypeDropDownList.SelectedValue);
                    provider.FileExtension = this.FileFormatDropDownList.SelectedValue;
                    provider.CurrentImportFile = this.RawFileNameTextBox.Text;
                    provider.Memo = this.MemoTextBox.Text;
                    provider.ImportNotes = this.ImportNotestTextBox.Text ;

                    data.Providers.AddObject(provider);
                    data.SaveChanges();
                    Response.Redirect("Provider.aspx?Id=" + provider.Id.ToString());
                }
            }
            else
            {
                using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
                {
                    var provider = data.Providers.Single(l => l.Id == Id);
                    provider.ProviderName = this.ProviderNameTextBox.Text;
                    provider.SystemsContact = this.ContactTextBox.Text;
                    provider.Phone = this.PhoneTextBox.Text;
                    provider.Email = this.EmailTextBox.Text;
                    provider.Active = this.ActiveCheckBox.Checked;
                    provider.ChannelTypeId = byte.Parse(this.ChannelTypeDropDownList.SelectedValue);
                    provider.FileExtension = this.FileFormatDropDownList.SelectedValue;
                    provider.CurrentImportFile = this.RawFileNameTextBox.Text;
                    provider.Memo = this.MemoTextBox.Text;
                    provider.ImportNotes = this.ImportNotestTextBox.Text;

                    data.SaveChanges();
                    Response.Redirect("Provider.aspx?Id=" + provider.Id.ToString());

                }
            }
        }
    }
}