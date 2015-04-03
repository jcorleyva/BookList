using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace USATodayBookList
{
    public partial class ImportData : System.Web.UI.Page
    {
        /*
        protected void Page_Load(object sender, EventArgs e)
        {
            List<ImportedData> data = new List<ImportedData>();

            data.Add(new ImportedData("Barnes & Nobles", 37197, 36119,400, DateTime.Today,"Successfully Imported"));
            data.Add(new ImportedData("Amazon", 912, 1000,400,DateTime.Today,"Successfully Imported"));
            data.Add(new ImportedData("Amazon Book", 0, 5,0,DateTime.Today,"Unknown Error. Please check thelog"));
            data.Add(new ImportedData("Walmart", 26484, 27929, 350, DateTime.Today,"Successfully Imported"));

            ProviderGridView.DataSource = data;
            ProviderGridView.DataBind();
        }

        protected virtual void FilterClick( object sender, EventArgs e)
        {
            this.ProviderTextBox.Text = "DD";
        }
        protected void btnImport_Click(object sender, EventArgs e)
        {
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
        }
         */
    }

    public class ImportedData
    {
        /*
        public string ProviderName { get; set; }
        public int ThisWeekSales { get; set; }
        public int LastWeekSales { get; set; }
        public int RowsImported { get; set; }
        public DateTime DateImported { get; set; }
        public string Status { get; set; }

        public ImportedData(string Name, int ThisWeekSales, int LastWeekSales, int RowsImported, DateTime DateImported, string Status)
        {
            this.ProviderName = Name;
            this.ThisWeekSales = ThisWeekSales;
            this.LastWeekSales = LastWeekSales;
            this.RowsImported = RowsImported;
            this.DateImported = DateImported;
            this.Status = Status;
        }
         */
    }
}