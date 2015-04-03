<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportLog.aspx.cs" Inherits="USATodayBookList.ImportLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<h1>Import Log</h1><br />
    <asp:Label ID="ProviderLabel" runat="server">Provider:</asp:Label>
    <asp:DropDownList ID="ProviderDropDownList" runat="server"
     DataTextField="ProviderName" DataValueField="Id" AppendDataBoundItems="true">
       <asp:ListItem Text="All Providers" Value="0"  />
    </asp:DropDownList>&nbsp;
    <asp:Button ID="BtnFilter" Text="Filter" runat="server" OnClick="FilterClick" />&nbsp;<br />

                 <asp:GridView ID="LogsGridView" AutoGenerateColumns="False" runat="server"
                 AllowPaging="true" PageSize="500" AllowSorting="true">
                 <Columns>
                     <asp:BoundField HeaderText="Provider Name" DataField="ProviderName" SortExpression="ProviderName" ItemStyle-Width="250" ItemStyle-HorizontalAlign="left" />
                     <asp:BoundField HeaderText="Date Imported" DataField="DateCompleted" SortExpression="DateCompleted" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" />
                     <asp:BoundField HeaderText="Rows Imported" DataField="RowsImported" SortExpression="RowsImported" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" />
                     <asp:BoundField HeaderText="Message" DataField="Messages" SortExpression="Messages" ItemStyle-Width="300" ItemStyle-HorizontalAlign="left" />
                 </Columns>
                 </asp:GridView>                
</asp:Content>
