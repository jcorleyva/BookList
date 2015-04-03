<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AuthorList.aspx.cs" Inherits="USATodayBookList.AuthorList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<h1>Author List</h1><br />
                <asp:Label ID="FirstNameLabel" runat="server">First Name:</asp:Label>
                 <asp:TextBox ID="FirstNameTextBox" runat="server"> </asp:TextBox>
                 <asp:Label ID="LastNameLabel" runat="server" >Last Name:</asp:Label>
                 <asp:TextBox ID="LastNameTextBox" runat="server"> </asp:TextBox>&nbsp;
                 <asp:LinkButton ID="BtnFilter" Text="Filter" runat="server" OnClick="FilterClick" />&nbsp;
                 <asp:LinkButton ID="BtnClearFilter" Text="Clear Filter" runat="server" OnClick="ClearFilter_Click" />&nbsp;
                 <br /><br />
                 <asp:GridView ID="AuthorsGridView" AutoGenerateColumns="False" AllowPaging="true" DataKeyNames="Id" 
                 PageSize="1000" AllowSorting="true" ShowHeaderWhenEmpty="true" Width="100%" runat="server"
                 OnRowDeleting="AuthorsGridView_RowDeleting"
                 >                 
                 <Columns>
                     <asp:HyperLinkField DataTextField="LastName" DataNavigateUrlFormatString="~/Author.aspx?ID={0}" 
                        DataNavigateUrlFields="ID" SortExpression="LastName" HeaderText="Last Name" ItemStyle-Width="150" />
                     <asp:BoundField HeaderText="First Name" DataField="FirstName" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" SortExpression="FirstName" />
                     <asp:BoundField HeaderText="Display Name" DataField="DisplayName" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" SortExpression="DisplayName" />
                     <asp:CommandField ButtonType="Button" ShowDeleteButton="true" DeleteText="Delete" ItemStyle-Width="100px" />
                 </Columns>
                 <EmptyDataTemplate>
                    <div style="color:red">No author found.</div>
                 </EmptyDataTemplate>
                 </asp:GridView>
        <br />
    <!-- <asp:LinkButton ID="AddButton" Text="Add Author" OnClick="btnAddClick" runat="server" /> -->
    <a id="MainContent_AddButton" href="Author.aspx?ID=0" target="_blank">Add Author</a>
</asp:Content>
