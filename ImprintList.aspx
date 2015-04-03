<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImprintList.aspx.cs" Inherits="USATodayBookList.ImprintList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<h1>Imprint List</h1><br />
     
        <table cellpadding="0" cellspacing="3" border="0">
            <tr>
                <td valign="top">
                     <asp:Label ID="ImprintLabel" runat="server">Imprint:</asp:Label>&nbsp;
                </td>
                <td>
                     <asp:TextBox ID="ImprintTextBox" runat="server"> </asp:TextBox>&nbsp;
                </td>
                <td valign="top">                
                    <asp:Button ID="FilterButton" Text="Filter" runat="server" OnClick="Filter_Click" />&nbsp;
                    <asp:Button ID="ClearFilterButton" Text="Clear Filter" runat="server" OnClick="ClearFilter_Click" />
                </td>
            </tr>
        </table>
     <asp:GridView ID="ImprintGridView" AutoGenerateColumns="False" AllowPaging="false" 
                 AllowSorting="true" ShowHeaderWhenEmpty="true" Width="100%" runat="server">
        <Columns>
            <asp:HyperLinkField DataTextField="ImprintName" DataNavigateUrlFormatString="~/Imprint.aspx?ID={0}" 
                        DataNavigateUrlFields="ID" SortExpression="ImprintName" HeaderText="Imprint Name" ItemStyle-Width="200" />
            <asp:BoundField HeaderText="Publisher Name" DataField="PublisherName" ItemStyle-Width="200" SortExpression="PublisherName" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Contact Name" DataField="ContactName" ItemStyle-Width="200" SortExpression="ContactName" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Email" DataField="Email" ItemStyle-Width="200" ItemStyle-HorizontalAlign="Left" SortExpression="Email" />
            <asp:BoundField HeaderText="Phone" DataField="Phone" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" SortExpression="Phone" />
        </Columns>
        <EmptyDataTemplate>
            <div style="color:red">No Imprint found.</div>
        </EmptyDataTemplate>
     </asp:GridView>
    <br />
    <asp:LinkButton ID="AddButton" Text="Add Imprint" OnClick="AddButton_Click" runat="server" />                 
</asp:Content>
