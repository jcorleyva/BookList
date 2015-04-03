using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace USATodayBookList.AppCode
{
    public class PageBase : CommonApp.WebPageBase
    {
        public bool IsDialog
        {
            get
            {
                bool isdialog = false;
                bool.TryParse(Request.Params["isdialog"], out isdialog);
                return isdialog;
            }
        }


        public PageBase()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            HtmlGenericControl jsCommonApp = new HtmlGenericControl("script");
            jsCommonApp.Attributes.Add("type", "text/javascript");
            jsCommonApp.Attributes.Add("src", "Scripts/CommonApp.js");
            base.Header.Controls.Add(jsCommonApp);
            base.PersistScrollPosition = true;

            base.OnInit(e);
        }

     
    }
}