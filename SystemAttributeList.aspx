<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SystemAttributeList.aspx.cs" Inherits="USATodayBookList.SystemAttributeList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<h1>System Attribute List</h1><br />
     <asp:GridView ID="SystemAttributeGridView" AutoGenerateColumns="False" AllowPaging="true" CellPadding="2" 
                 PageSize="10" AllowSorting="true" ShowHeaderWhenEmpty="true" Width="100%" runat="server">
        <Columns>
            <asp:HyperLinkField DataTextField="AttrDesc" DataNavigateUrlFormatString="~/SystemAttribute.aspx?ID={0}" 
                        DataNavigateUrlFields="ID" SortExpression="AttrDesc" HeaderText="Attribute Description" ItemStyle-Width="200" />
            <asp:BoundField HeaderText="Value" DataField="AttrValue" ItemStyle-Width="200" ItemStyle-HorizontalAlign="Left" SortExpression="AttrValue" />
        </Columns>
        <EmptyDataTemplate>
            <div style="color:red">No Attributes found.</div>
        </EmptyDataTemplate>
     </asp:GridView>
     <br />
    <asp:LinkButton ID="AddButton" Text="Add System Attribute" OnClick="AddButton_Click" runat="server" />
</asp:Content>
