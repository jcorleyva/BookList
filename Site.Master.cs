using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace USATodayBookList
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            AppCode.USATodayUser user = AppCode.AppCache.GetUser(System.Web.HttpContext.Current.User.Identity.Name);

            // If you do not have access redirect to NoAccess.htm
            if (user.HasAccess == false)
            {
                Response.Redirect("NoAccess.htm");
            }

        }
    }
}
