﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services.Protocols;
using Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution;
using System.IO;
using System.Data;
using System.Configuration;
using System.Net;
using USATodayBookList.AppCode;
using System.Threading;

namespace USATodayBookList
{
    public partial class ResetWorkflow : PageBase
    {
        protected delegate void DoAsyncWork();
        private TimeSpan _workTime = TimeSpan.FromSeconds(15);

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnArchive_Click(object sender, EventArgs e)
        {            
            var task = new PageAsyncTask(BeginRequest, EndRequest, TimedOutOp, null);
            RegisterAsyncTask(task);
        }

        IAsyncResult BeginRequest(Object sender, EventArgs e,
                                  AsyncCallback cb, object state)
        {
            //spanStatus.InnerText += "Begin Asyn";
            DoAsyncWork asyncWork = new DoAsyncWork(Archive);
            return asyncWork.BeginInvoke(cb, state);
        }

        void EndRequest(IAsyncResult asyncResult)
        {
            //spanStatus.InnerText += " End Asyn";
            base.ShowClientMessage("Booklist archive request submitted sucessfully :)");            
        }

        void TimedOutOp(IAsyncResult ar)
        {
            base.ShowClientMessage("Booklist archive request submitted sucessfully :)");
            //spanStatus.InnerText += "Timed Out";
        }

        void Archive()
        {
            try
            {
                USATodayBookList.AppCode.DABooklist da = new AppCode.DABooklist();
                da.Archive();
            }
            catch (Exception ex)
            {
                base.ShowClientMessage("Error occurred while archiving. " + ex.Message + " - " + ex.StackTrace);
            }
        }
    }
}