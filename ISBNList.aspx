<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ISBNList.aspx.cs" Inherits="USATodayBookList.ISBNList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<h1>ISBN List</h1>
<br />
     <asp:Label ID="ISBNLabel" runat="server" AssociatedControlID="ISBNTextBox">ISBN:</asp:Label>
     <asp:TextBox ID="ISBNTextBox" runat="server"> </asp:TextBox>
     <asp:LinkButton ID="BtnFilter" Text="Filter" runat="server" OnClick="Filter_Click"></asp:LinkButton>&nbsp;
     <asp:LinkButton ID="BtnClearFilter" Text="Clear Filter" runat="server" OnClick="ClearFilter_Click" />
     &nbsp;<br /><br />
     <asp:GridView ID="ISBNGridView" AutoGenerateColumns="False" AllowPaging="true" 
                 PageSize="1000" AllowSorting="true" ShowHeaderWhenEmpty="true" Width="100%" runat="server">
        <Columns>
            <asp:HyperLinkField DataTextField="ISBN" DataNavigateUrlFormatString="~/ISBN.aspx?ID={0}" 
                        DataNavigateUrlFields="ID" SortExpression="ISBN" HeaderText="ISBN" ItemStyle-Width="150" />
            <asp:BoundField HeaderText="Raw Title" DataField="Title" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" SortExpression="Title" />
            <asp:BoundField HeaderText="Raw Author" DataField="Author" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" SortExpression="Author" />
        </Columns>
        <EmptyDataTemplate>
            <div style="color:red">No ISBN found.</div>
        </EmptyDataTemplate>
     </asp:GridView>
     <br />
     <asp:LinkButton ID="AddButton" Text="Add" OnClick="btnAddClick" runat="server" />
</asp:Content>
