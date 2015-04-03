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
    public partial class ImprintList : System.Web.UI.Page
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
        /// Gets the imprints.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<USAToday.Booklist.Business.ImprintPublisherView> GetImprints()
        {
            var imprints = Global.BookEntities.ImprintPublisherViews;
            return imprints.ToList<USAToday.Booklist.Business.ImprintPublisherView>();
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
                // Get imprints
                List<USAToday.Booklist.Business.ImprintPublisherView> imprints = GetImprints();

                if (imprints.Count() == 0)
                    ImprintGridView.AllowSorting = false;

                // Bind
                ImprintGridView.DataSource = imprints;
                ImprintGridView.DataBind();

                // Store in session variable
                Session.Add("Imprints", imprints);

                // Set the sort info 
                LastSortDirection = string.Empty;
                LastSortKey = string.Empty;

            }

            // Set up the events
            ImprintGridView.Sorting += ImprintGridView_Sorting;

            // Hide buttons
            if (AppCode.AppCache.GetUser(User.Identity.Name).CanWrite == false)
            {
                this.AddButton.Visible = false;
            }
        }

        /// <summary>
        /// Handles the Sorting event of the ImprintGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void ImprintGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Get the list of imprints from the session 
            List<USAToday.Booklist.Business.ImprintPublisherView> imprints = default(List<USAToday.Booklist.Business.ImprintPublisherView>);
            imprints = Session["Imprints"] as List<USAToday.Booklist.Business.ImprintPublisherView>;


            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
            {
                LastSortDirection = string.Empty;
            }

            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "ImprintName":
                    imprints = Sort(imprints, p => p.ImprintName);
                    break;
                case "PublisherName":
                    imprints = Sort(imprints, p => p.PublisherName);
                    break;
                case "ContactName":
                    imprints = Sort(imprints, p => p.ContactName);
                    break;
                case "Phone":
                    imprints = Sort(imprints, p => p.Phone);
                    break;
                case "Email":
                    imprints = Sort(imprints, p => p.Email);
                    break;
            }

            LastSortKey = e.SortExpression;
            // Rebind 
            ImprintGridView.DataSource = imprints;
            ImprintGridView.DataBind();
        }

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.ImprintPublisherView> Sort(List<USAToday.Booklist.Business.ImprintPublisherView> list,
                Func<USAToday.Booklist.Business.ImprintPublisherView, string> sortKey)
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
        /// Handles the Click event of the Filter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected virtual void Filter_Click(object sender, EventArgs e)
        {
            if (this.ImprintTextBox.Text != "")
            {
                using (var data = new USATodayBookListEntities())
                {
                    var a = from p in data.ImprintPublisherViews
                            where p.ImprintName.Contains(this.ImprintTextBox.Text.Trim())
                            select p;
                    if (a.Count() == 0)
                        ImprintGridView.AllowSorting = false;
                    else
                        ImprintGridView.AllowSorting = true;

                    List<USAToday.Booklist.Business.ImprintPublisherView> imprints = a.ToList<USAToday.Booklist.Business.ImprintPublisherView>();
                    ImprintGridView.DataSource = imprints;
                    ImprintGridView.DataBind();

                    Session.Add("Imprints", imprints);
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
            Response.Redirect("ImprintList.aspx");
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