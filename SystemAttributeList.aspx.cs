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
    public partial class SystemAttributeList : System.Web.UI.Page
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
        /// Gets the system attributes.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<USAToday.Booklist.Business.SystemAttribute> GetSystemAttributes()
        {
            using (var data = new USATodayBookListEntities())
            {
                var attributes = data.SystemAttributes;
                return attributes.ToList<USAToday.Booklist.Business.SystemAttribute>();
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
                // Get attributes
                List<USAToday.Booklist.Business.SystemAttribute> attributes = GetSystemAttributes();

                if (attributes.Count() == 0)
                    SystemAttributeGridView.AllowSorting = false;

                // Bind
                SystemAttributeGridView.DataSource = attributes;
                SystemAttributeGridView.DataBind();

                // Store in session variable
                Session.Add("Attributes", attributes);

                // Set the sort info 
                LastSortDirection = string.Empty;
                LastSortKey = string.Empty;

            }

            // Set up the events
            SystemAttributeGridView.PageIndexChanging += SystemAttributeGridView_PageIndexChanging;
            SystemAttributeGridView.Sorting += SystemAttributeGridView_Sorting;

            // Hide buttons
            if (AppCode.AppCache.GetUser(User.Identity.Name).CanWrite == false)
            {
                this.AddButton.Visible = false;
            }
            
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the SystemAttributeGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void SystemAttributeGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Get the list of attributes from the session 
            List<USAToday.Booklist.Business.SystemAttribute> attributes = default(List<USAToday.Booklist.Business.SystemAttribute>);
            attributes = Session["Attributes"] as List<USAToday.Booklist.Business.SystemAttribute>;

            // Set the index 
            SystemAttributeGridView.PageIndex = e.NewPageIndex;
            // Rebind 
            SystemAttributeGridView.DataSource = attributes;
            SystemAttributeGridView.DataBind();
        }

        /// <summary>
        /// Handles the Sorting event of the SystemAttributeGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void SystemAttributeGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Get the list of attributes from the session 
            List<USAToday.Booklist.Business.SystemAttribute> attributes = default(List<USAToday.Booklist.Business.SystemAttribute>);
            attributes = Session["Attributes"] as List<USAToday.Booklist.Business.SystemAttribute>;


            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
            {
                LastSortDirection = string.Empty;
            }

            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "AttrDesc":
                    attributes = Sort(attributes, p => p.AttrDesc);
                    break;
                case "AttrValue":
                    attributes = Sort(attributes, p => p.AttrValue);
                    break;
            }

            LastSortKey = e.SortExpression;
            // Rebind 
            SystemAttributeGridView.DataSource = attributes;
            SystemAttributeGridView.DataBind();
        }

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.SystemAttribute> Sort(List<USAToday.Booklist.Business.SystemAttribute> list,
                Func<USAToday.Booklist.Business.SystemAttribute, string> sortKey)
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
        /// Handles the Click event of the AddButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void AddButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("SystemAttribute.aspx?Id=0");
        }
    }
}