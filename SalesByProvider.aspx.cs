using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using USAToday.Booklist.Business;

namespace USATodayBookList
{
    public partial class SalesByProvider : System.Web.UI.Page
    {

        private static List<USAToday.Booklist.Business.ISBNSalesByProvider> GetISBNSales(string ISBN)
        {
            using (var data = new USATodayBookListEntities())
            {
                var isbnSales = data.GetISBNSales(ISBN.Trim());
                return isbnSales.ToList<USAToday.Booklist.Business.ISBNSalesByProvider>();
            }
        }

        private static List<USAToday.Booklist.Business.TotalISBNSales> GetTotalISBNSales(string ISBN)
        {
            using (var data = new USATodayBookListEntities())
            {
                var isbnSales = data.GetTotalISBNSales(ISBN.Trim());
                return isbnSales.ToList<USAToday.Booklist.Business.TotalISBNSales>();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int bookId = 0;
            string isbn = Request["isbn"];
            int.TryParse(Request["bookId"], out bookId);

            if(!IsPostBack)
            {
                this.spanISBN.InnerText = isbn;
                var bk = Global.BookEntities.Books.SingleOrDefault(l => l.Id == bookId);
                this.spanBook.InnerText = (bk == null ? "" : bk.Title);

                SalesGridView.DataSource = GetISBNSales(isbn);
                SalesGridView.DataBind();

                List<USAToday.Booklist.Business.TotalISBNSales> p = GetTotalISBNSales(isbn);
                this.tdTotalISBNSales.InnerText = (p.Single().ISBNSales == null) ? "0" : p.Single().ISBNSales.Value.ToString("#,###");
                this.tdTotalBookSales.InnerText = (p.Single().BookSales == null) ? "0" : p.Single().BookSales.Value.ToString("#,###");

            }
        }
    }
}