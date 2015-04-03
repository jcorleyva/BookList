<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChannelTypeList.aspx.cs" Inherits="USATodayBookList.ChannelTypeList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<h1>Channel Type List</h1><br />
                 <asp:GridView ID="ChannelTypeGridView" AutoGenerateColumns="False" runat="server"
                  AllowPaging="true" PageSize="10" AllowSorting="true">
                 <Columns>
                     <asp:HyperLinkField DataTextField="ChannelTypeDescription" 
                     dataNavigateurlfields="ID" SortExpression="ChannelTypeDescription"     
                     DataNavigateUrlFormatString="~/ChannelType.aspx?ID={0}"  HeaderText="Channel Type" ItemStyle-Width="200" />
                     <asp:BoundField HeaderText="Weighted Sales" DataField="WeightedSales" SortExpression="WeightedSales" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                 </Columns>
                 </asp:GridView>
        <br />
    <asp:LinkButton ID="AddButton" Text="Add" OnClick="btnAddClick" runat="server" />                 
</asp:Content>
