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
    public partial class ProviderList : System.Web.UI.Page
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
        /// Gets the providers.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<USAToday.Booklist.Business.Provider> GetProviders()
        {
            using (var data = new USATodayBookListEntities())
            {
                var providers = data.Providers;
                return providers.ToList<USAToday.Booklist.Business.Provider>();
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
                // Get providers
                List<USAToday.Booklist.Business.Provider> providers = GetProviders();

                if (providers.Count() == 0)
                    ProviderGridView.AllowSorting = false;

                // Bind
                ProviderGridView.DataSource = providers;
                ProviderGridView.DataBind();

                // Store in session variable
                Session.Add("Providers", providers);

                // Set the sort info 
                LastSortDirection = string.Empty;
                LastSortKey = string.Empty;

            }

            // Set up the events
            ProviderGridView.Sorting += ProviderGridView_Sorting;

            // Hide buttons
            if (AppCode.AppCache.GetUser(User.Identity.Name).CanWrite == false)
            {
                this.AddButton.Visible = false;
            }
        }

        /// <summary>
        /// Handles the Sorting event of the ProviderGridView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewSortEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void ProviderGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // Get the list of providers from the session 
            List<USAToday.Booklist.Business.Provider> providers = default(List<USAToday.Booklist.Business.Provider>);
            providers = Session["Providers"] as List<USAToday.Booklist.Business.Provider>;


            // Sort key is different, clear the last sort direction 
            if (LastSortKey != e.SortExpression)
            {
                LastSortDirection = string.Empty;
            }

            // Perform the sort using Linq 
            switch (e.SortExpression)
            {
                case "ProviderName":
                    providers = Sort(providers, p => p.ProviderName);
                    break;
                case "SystemContact":
                    providers = Sort(providers, p => p.SystemsContact);
                    break;
                case "Phone":
                    providers = Sort(providers, p => p.Phone);
                    break;
                case "Email":
                    providers = Sort(providers, p => p.Email);
                    break;
            }

            LastSortKey = e.SortExpression;
            // Rebind 
            ProviderGridView.DataSource = providers;
            ProviderGridView.DataBind();
            // Store in a session variable 
            //Session.Add("Providers", providers);
        }

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sortKey">The sort key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private List<USAToday.Booklist.Business.Provider> Sort(List<USAToday.Booklist.Business.Provider> list,
                Func<USAToday.Booklist.Business.Provider, string> sortKey)
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
            if (this.ProviderTextBox.Text != "")
            {
                using (var data = new USATodayBookListEntities())
                {
                    var a = from p in data.Providers
                                    where p.ProviderName.Contains(this.ProviderTextBox.Text.Trim())
                                select p;
                    if (a.Count() == 0)
                        ProviderGridView.AllowSorting = false;
                    else
                        ProviderGridView.AllowSorting = true;

                    List<USAToday.Booklist.Business.Provider> providers = a.ToList<USAToday.Booklist.Business.Provider>();
                    ProviderGridView.DataSource = providers;
                    ProviderGridView.DataBind();

                    Session.Add("Providers", providers);
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
            Response.Redirect("ProviderList.aspx");
        }
        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("Provider.aspx?Id=0");
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class ProviderData
    {
        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        /// <value>The name of the provider.</value>
        /// <remarks></remarks>
        public string ProviderName { get; set; }
        /// <summary>
        /// Gets or sets the systems contact.
        /// </summary>
        /// <value>The systems contact.</value>
        /// <remarks></remarks>
        public string SystemsContact { get; set; }
        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>The phone.</value>
        /// <remarks></remarks>
        public string Phone { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        /// <remarks></remarks>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        /// <value>The file extension.</value>
        /// <remarks></remarks>
        public string FileExtension { get; set; }
        /// <summary>
        /// Gets or sets the current import file.
        /// </summary>
        /// <value>The current import file.</value>
        /// <remarks></remarks>
        public string CurrentImportFile { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderData"/> class.
        /// </summary>
        /// <param name="ProviderName">Name of the provider.</param>
        /// <param name="SystemsContact">The systems contact.</param>
        /// <param name="Phone">The phone.</param>
        /// <param name="Email">The email.</param>
        /// <remarks></remarks>
        public ProviderData(string ProviderName, string SystemsContact, string Phone, string Email)
        {
            this.ProviderName = ProviderName;
            this.SystemsContact = SystemsContact;
            this.Phone = Phone;
            this.Email = Email;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderData"/> class.
        /// </summary>
        /// <param name="ProviderName">Name of the provider.</param>
        /// <param name="FileExtension">The file extension.</param>
        /// <param name="CurrentImportFile">The current import file.</param>
        /// <remarks></remarks>
        public ProviderData(string ProviderName, string FileExtension, string CurrentImportFile)
        {
            this.ProviderName = ProviderName;
            this.FileExtension = FileExtension;
            this.CurrentImportFile = CurrentImportFile;
        }
    }
}