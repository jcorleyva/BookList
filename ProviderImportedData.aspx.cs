using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace USATodayBookList
{
    public partial class ProviderImportedData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<BookWeeklyData> data = new List<BookWeeklyData>();
            data.Add(new BookWeeklyData("9780000001","The Help", "Stockett K", 38000));
            data.Add(new BookWeeklyData("9780000002","Stolen Life", "Gugard J", 5000));
            data.Add(new BookWeeklyData("9780000003","Hunger Games Hunger Games Series", "Collins S", 300));

            ProvidersGridView.DataSource = data;
            ProvidersGridView.DataBind();
        }
    }

    public class BookWeeklyData
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Sales { get; set; }

        public BookWeeklyData(string ISBN, string Title, string Author, int Sales)
        {
            this.ISBN = ISBN;
            this.Title = Title;
            this.Author = Author;
            this.Sales = Sales;
        }
    }

}