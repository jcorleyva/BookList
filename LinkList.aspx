<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LinkList.aspx.cs" Inherits="USATodayBookList.LinkList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<h1>Research Links</h1><br />
                 <asp:GridView ID="LinksGridView" AutoGenerateColumns="False" runat="server" 
                 Width="100%" ShowHeaderWhenEmpty="true"
                 AllowPaging="true" PageSize="10" AllowSorting="true">
                 <Columns>
                     <asp:HyperLinkField DataTextField="LinkDesc" DataNavigateUrlFormatString="~/Link.aspx?ID={0}"
                            dataNavigateurlfields="ID" HeaderText="Description" SortExpression="LinkDesc" ItemStyle-Width="250" />    
                     <asp:BoundField HeaderText="URL" DataField="URL" ItemStyle-HorizontalAlign="left" SortExpression="URL" />
                 </Columns>
                 <EmptyDataTemplate>
                     <div style="color:red">There are currently no research links set up.</div>
                 </EmptyDataTemplate>  
                 </asp:GridView>
        <br />
    <asp:LinkButton ID="AddButton" Text="Add Link" OnClick="btnAddClick" runat="server" />                 
</asp:Content>
