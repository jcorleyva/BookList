using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace USATodayBookList
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public partial class ChannelType : System.Web.UI.Page
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
                if (AddMode == false)
                {
                    FillData();
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
                var channel = data.ChannelTypes.Single(l => l.Id == Id);

                this.ChannelTypeTextBox.Text = channel.ChannelTypeDescription;
                this.WeightedSalesTextBox.Text = channel.WeightedSales.ToString();
            }

        }

        /// <summary>
        /// Saves the button click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void SaveButtonClick(object sender, EventArgs e)
        {
            if (AddMode == true)
            {
                using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
                {
                    USAToday.Booklist.Business.ChannelType channel = new USAToday.Booklist.Business.ChannelType();
                    channel.ChannelTypeDescription = this.ChannelTypeTextBox.Text;
                    channel.WeightedSales = decimal.Parse( this.WeightedSalesTextBox.Text);

                    data.ChannelTypes.AddObject(channel);
                    data.SaveChanges();
                    Response.Redirect("ChannelType.aspx?id=" + channel.Id.ToString());
                }
            }
            else
            {
                using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
                {
                    var channel = data.ChannelTypes.Single(l => l.Id == Id);

                    channel.ChannelTypeDescription = this.ChannelTypeTextBox.Text;
                    channel.WeightedSales = decimal.Parse(this.WeightedSalesTextBox.Text);
                    data.SaveChanges();
                    Response.Redirect("ChannelType.aspx?id=" + channel.Id.ToString());
                }
            }
        }
    }
}