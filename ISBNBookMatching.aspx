<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ISBNBookMatching.aspx.cs" Inherits="USATodayBookList.ISBNBookMatching" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">
        function addBook(isbnId,isbn) {
            returnValue = Page.ShowModal("BookLookup.aspx?isbnId=" + isbnId + "&isbn=" + isbn, "940px", "390px", true);
            //Page.SetClientParam("RefreshBook", returnValue, true);
        }
        function showSalesByProvider(isbn,bookId) {
            window.showModalDialog("SalesByProvider.aspx?isbn=" + isbn + "&bookId=" + bookId, "", "dialogHeight:500px;dialogWidth:400px;");
        }
        function editISBN(isbnId) {
            returnValue = Page.ShowModal("ISBN.aspx?ID=" + isbnId + "&isdialog=true", "1000px", "700px", true);
            if (returnValue == "Refresh")
                Page.SetClientParam("RefreshISBN", returnValue, true);

        }
        function editBook(bookId) {
            returnValue = Page.ShowModal("Book.aspx?ID=" + bookId + "&isdialog=true", "1000px", "800px", true);
            Page.SetClientParam("RefreshISBN", returnValue, true);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>ISBN Working List</h1><br />
    <asp:Label ID="ISBNLabel" runat="server" AssociatedControlID="ISBNTextBox">ISBN:</asp:Label>
    <asp:TextBox ID="ISBNTextBox" runat="server"> </asp:TextBox>&nbsp;
    <asp:Label ID="TitleLabel" runat="server" AssociatedControlID="TitleTextBox">Title:</asp:Label>
    <asp:TextBox ID="TitleTextBox" runat="server"> </asp:TextBox>&nbsp;
    <asp:Label ID="AuthorLabel" runat="server" AssociatedControlID="AuthorTextBox">Author:</asp:Label>
    <asp:TextBox ID="AuthorTextBox" runat="server"> </asp:TextBox>&nbsp;
    <asp:LinkButton ID="FilterButton" Text="Filter" runat="server" OnClick="Filter_Click" />&nbsp;
    <asp:LinkButton ID="ClearFilterButton" Text="Clear Filter" runat="server" OnClick="ClearFilter_Click" />
    <br />
        <asp:Label ID="Label1" runat="server" AssociatedControlID="TitleTextBox">Show Top:</asp:Label>
        <asp:TextBox ID="MaxRecordTextBox" runat="server" Text="400" Width="35" />
    <br />
        <asp:RadioButtonList ID="FilterTypeRadioButtonList" AutoPostBack="true" align="left"
            RepeatDirection="Horizontal" runat="server" 
            OnSelectedIndexChanged="FilterType_SelectedIndexChanged">
                        <asp:ListItem Text="All ISBNs" Selected="True" Value="All"> </asp:ListItem>
                        <asp:ListItem Text="New" Value="New"></asp:ListItem>
                        <asp:ListItem Text="Needs Editing" Value="Needs Editing"></asp:ListItem>
                        <asp:ListItem Text="ISBNs Not Matched to Books" Value="Not Matched"></asp:ListItem>
                        <asp:ListItem Text="Excluded ISBNs" Value="Excluded"></asp:ListItem>
        </asp:RadioButtonList>
    <br />
    <br />
     <asp:GridView ID="ISBNMatchingGridView" AutoGenerateColumns="False" AllowPaging="false" 
                  AllowSorting="true" ShowHeaderWhenEmpty="true" Width="100%" runat="server"
                OnRowCommand="ISBNMatchingGridView_RowCommand"  
                OnRowDataBound="ISBNMatchingGridView_RowDataBound"               
                 >
        <Columns>
            <asp:BoundField HeaderText="Sales" DataField="Sales" SortExpression="Sales" ItemStyle-Width="35" ItemStyle-HorizontalAlign="Center" />
            <asp:TemplateField HeaderText="Raw ISBN" ItemStyle-Width="150" SortExpression="ISBN" ItemStyle-CssClass="links">
                <ItemTemplate>
                    <div onclick="editISBN(<%# ((USAToday.Booklist.Business.ISBNWorkingList)Container.DataItem).BookISBNId.ToString() %>);">
                        <%# DataBinder.Eval(Container.DataItem, "ISBN") %>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Title"   HeaderText="Raw Title" SortExpression="Title" ItemStyle-Width="150" />
            <asp:BoundField DataField="Author"   HeaderText="Raw Author" SortExpression="Author" ItemStyle-Width="150" />
            <asp:TemplateField HeaderText="Book" ItemStyle-Width="150" SortExpression="BookTitle">
                <ItemTemplate>
                    <div class="links" onclick="editBook(<%# ((USAToday.Booklist.Business.ISBNWorkingList)Container.DataItem).BookId.ToString() %>);">
                        <%# DataBinder.Eval(Container.DataItem, "BookTitle") %>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:ButtonField ButtonType="Link" Text="Assign Bk" CommandName="ASSIGN" HeaderText="Assign Bk" ItemStyle-Width="250" />
            <asp:TemplateField HeaderText="Providers" ItemStyle-Width="250px">
                <ItemTemplate>
                    <div class="links" onclick="showSalesByProvider(<%# ((USAToday.Booklist.Business.ISBNWorkingList)Container.DataItem).ISBN + ',' + GetBookId(((USAToday.Booklist.Business.ISBNWorkingList)Container.DataItem).BookId) %>);">
                        Sales By Provider
                    </div>                
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Ptotal" DataField="Providers" SortExpression="Providers"
                ItemStyle-Width="15" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField HeaderText="Etotal" DataField="eProviders" SortExpression="eProviders"
                ItemStyle-Width="15" ItemStyle-HorizontalAlign="Right" />
            <asp:BoundField DataField="BookStatus" HeaderText="Status" ItemStyle-ForeColor="Blue" ItemStyle-HorizontalAlign="Center"
                SortExpression="BookStatus" ItemStyle-Width="100" />
        </Columns>
        <EmptyDataTemplate>
            <div style="color:red">No Records found.</div>
        </EmptyDataTemplate>

    </asp:GridView>
    <p>
        <asp:LinkButton ID="AddButton" Text="Add Book" runat="server" OnClick="AddBook_Click" />
    </p>


</asp:Content>
