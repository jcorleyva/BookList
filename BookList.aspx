<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookList.aspx.cs" Inherits="USATodayBookList.Book" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<h1>Book List</h1><br />
        <table cellpadding="0" cellspacing="3" border="0">
            <tr>
                <td>
                    <asp:Label ID="TitleLabel" runat="server" AssociatedControlID="TitleTextBox">Title:</asp:Label>&nbsp;
                    <asp:TextBox ID="TitleTextBox" runat="server"></asp:TextBox>&nbsp;
                    <asp:LinkButton ID="BtnFilter" Text="Filter" runat="server" OnClick="FilterClick" />&nbsp;
                    <asp:LinkButton ID="BtnClearFilter" Text="Clear Filter" runat="server" OnClick="ClearFilter_Click" />
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="BooksGridView" AutoGenerateColumns="False" AllowSorting="true" AllowPaging="true"
          PageSize="1000" ShowHeaderWhenEmpty="true" Width="100%"
         runat="server">
            <Columns>
                <asp:HyperLinkField DataTextField="Title"  DataNavigateUrlFormatString="~/Book.aspx?ID={0}"
                 DataNavigateUrlFields="ID" SortExpression="Title" HeaderText="Title" ItemStyle-Width="300px" />
                <asp:BoundField HeaderText="Author" DataField="AuthorDisplay" SortExpression="AuthorDisplay" ItemStyle-Width="300px" />
                <asp:BoundField HeaderText="Publish Date" DataField="PubDate" DataFormatString="{0:MM-dd-yyyy}" SortExpression="PubDate" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" />
            </Columns>
                 <EmptyDataTemplate>
                    <div style="color:red">No book found.</div>
                 </EmptyDataTemplate>
        </asp:GridView>
        <br />
    <asp:LinkButton ID="AddButton" Text="Add Book" runat="server"  OnClick="btnAddClick" />
                 
</asp:Content>
