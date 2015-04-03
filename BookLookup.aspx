<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="BookLookup.aspx.cs" Inherits="USATodayBookList.BookListLookup" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function addBook(isbn)
        {
            returnValue = Page.ShowModal("Book.aspx?Id=0&isdialog=true&isbn=" + isbn, "1000px", "900px", true);
            Page.SetClientParam("RefreshSearch", returnValue, true);
        }
   </script>    
</head>
<body style="margin-top:0px; padding-left:10px; padding-right:10px; background-color:White">
    <form id="Form1" runat="server">
    <br />
        <fieldset style="margin:-10px 0px 0px 0px">
            <legend style="color:Black">Book Lookup</legend>
                 <asp:Label ID="TitleLabel" runat="server" AssociatedControlID="TitleTextBox">Title:</asp:Label>
                 <asp:TextBox ID="TitleTextBox" runat="server"> </asp:TextBox>&nbsp;
                 <asp:Label ID="AuthorNameLabel" runat="server" AssociatedControlID="AuthorNameTextBox">Author Name:</asp:Label>
                 <asp:TextBox ID="AuthorNameTextBox" runat="server"> </asp:TextBox>&nbsp;
                 <asp:Button ID="FilterButton" Text="Filter" runat="server" OnClick="FilterButton_Click" />&nbsp;
                 <asp:Button ID="ClearFilterButton" Text="Clear Filter" runat="server" onclick="ClearFilterButton_Click" />                 
                 <br /><br />
                 <asp:GridView ID="BookGridView" AutoGenerateColumns="False" ShowHeader="true"
                             HorizontalAlign="left" ShowHeaderWhenEmpty="true" AllowSorting="true" Width="100%"
                             AllowPaging="true" PageSize="1000" DataKeyNames="Id"
                             OnRowDataBound="BookGridView_RowDataBound"
                             OnPageIndexChanging="BookGridView_PageIndexChanging" 
                             OnSorting="BookGridView_Sorting"
                             OnRowCommand="BookGridView_RowCommand"
                             runat="server">
                 <Columns>
                     <asp:CommandField ShowSelectButton="true"  ButtonType="Link" SelectText="" ItemStyle-Width="250" HeaderText="Title" SortExpression="Title" />
                     <asp:BoundField HeaderText="Author Name" DataField="AuthorName" ItemStyle-Width="200" SortExpression="AuthorName" />
                     <asp:BoundField HeaderText="Publish Date" DataField="PubDate" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" SortExpression="PubDate" />
                 </Columns>
                     <EmptyDataTemplate>
                           <div class="error">No records found.</div>
                     </EmptyDataTemplate>
                 </asp:GridView>
       </fieldset>
                <div align="right" style="padding-top:10px">
                    <asp:Button ID="AddBookButton" runat="server" Text="Add Book" />                    
                    &nbsp;
                    <asp:Button ID="CancelButton" Text="Cancel" OnClientClick="javascript:self.close();" runat="server" />
                </div>
    </form>
</body>
</html>                 
