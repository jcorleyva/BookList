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
    public partial class MediaFormatList : System.Web.UI.Page
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
        /// Gets the media formats.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<USAToday.Booklist.Business.MediaFormat> GetMediaFormats()
        {
            using (var data = new USATodayBookListEntities())
            {
                var mediaFormats = data.MediaFormats;
                return mediaFormats.ToList<USAToday.Booklist.Business.MediaFormat>();

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
                this.MediaFormatGridView.Columns[2].Visible = false;
                this.MediaFormatGridView.Columns[3].Visible = false;

            }
        }

        /// <summary>
        /// Binds the data.
        /// </summary>
        /// <remarks></remarks>
        private void BindData()
        {
            // Get the links
            List<USAToday.Booklist.Business.MediaFormat> mediaFormat = GetMediaFormats();

            // Bind
            MediaFormatGridView.DataSource = mediaFormat;
            MediaFormatGridView.DataBind();

        }

        /// <summary>
        /// Handles the Click event of the Filter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected virtual void Filter_Click(object sender, EventArgs e)
        {
            if (this.MediaFormatTextBox.Text != "")
            {
                using (var data = new USATodayBookListEntities())
                {
                    var mediaFormat = from a in data.MediaFormats
                                      where a.MediaFormatDescription.Contains(this.MediaFormatTextBox.Text.Trim())
                                   select a;
                    if (mediaFormat.Count() == 0)
                        MediaFormatGridView.AllowSorting = false;

                    MediaFormatGridView.DataSource = mediaFormat;
                    MediaFormatGridView.DataBind();
                }
            }
            else
            {
                MediaFormatGridView.DataSource = GetMediaFormats();
                MediaFormatGridView.DataBind();
            }
        }

        /// <summary>
        /// Handles the RowEditing event of the MediaFormatGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewEditEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void MediaFormatGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            MediaFormatGridView.EditIndex = e.NewEditIndex;
            this.BindData();
        }

        /// <summary>
        /// Handles the RowCancelingEdit event of the MediaFormatGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewCancelEditEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void MediaFormatGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            MediaFormatGridView.EditIndex = -1;
            this.BindData();
        }

        /// <summary>
        /// Handles the RowUpdating event of the MediaFormatGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewUpdateEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void MediaFormatGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
            {
                int Id = int.Parse(e.Keys[0].ToString());
                var MediaFormat = data.MediaFormats.Single(l => l.Id == Id);
                MediaFormat.MediaFormatDescription = e.NewValues[0].ToString();
                data.SaveChanges();
            }

            MediaFormatGridView.EditIndex = -1;
            this.BindData();
        }

        /// <summary>
        /// Handles the RowDeleting event of the MediaFormatGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewDeleteEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void MediaFormatGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            using (var data = new USAToday.Booklist.Business.USATodayBookListEntities())
            {
                int Id = int.Parse(e.Keys[0].ToString());
                var MediaFormat = data.MediaFormats.Single(l => l.Id == Id);
                data.MediaFormats.DeleteObject(MediaFormat);
                data.SaveChanges();
            }
            MediaFormatGridView.EditIndex = -1;
            this.BindData();
        }

        /// <summary>
        /// Handles the Sorting event of the MediaFormatGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void MediaFormatGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Get the list of authors from the session 
            List<USAToday.Booklist.Business.MediaFormat> mediaFormat = default(List<USAToday.Booklist.Business.MediaFormat>);
            mediaFormat = GetMediaFormats();

            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
                LastSortDirection = string.Empty;

            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "MediaFormatDescription":
                    mediaFormat = Sort(mediaFormat, MediaFormat => MediaFormat.MediaFormatDescription);
                    break;
            }

            LastSortKey = e.SortExpression;

            // Rebind 
            MediaFormatGridView.DataSource = mediaFormat;
            MediaFormatGridView.DataBind();
        }

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.MediaFormat> Sort(List<USAToday.Booklist.Business.MediaFormat> list,
                Func<USAToday.Booklist.Business.MediaFormat, string> sortKey)
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
        private List<USAToday.Booklist.Business.MediaFormat> Sort(List<USAToday.Booklist.Business.MediaFormat> list,
                Func<USAToday.Booklist.Business.MediaFormat, decimal> sortKey)
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
                    USAToday.Booklist.Business.MediaFormat MediaFormat = new USAToday.Booklist.Business.MediaFormat();

                    MediaFormat.MediaFormatDescription = AddTextBox.Text;

                    data.MediaFormats.AddObject(MediaFormat);
                    data.SaveChanges();

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
            this.MediaFormatTextBox.Text = "";
            this.BindData();
        }     
    }
}