<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PublisherList.aspx.cs" Inherits="USATodayBookList.PublisherList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<h1>Publisher List</h1><br />
     
        <table cellpadding="0" cellspacing="3" border="0">
            <tr>
                <td valign="top">
                     <asp:Label ID="PublisherLabel" runat="server" AssociatedControlID="PublisherTextBox">Publisher:</asp:Label>&nbsp;
                </td>
                <td>
                     <asp:TextBox ID="PublisherTextBox" runat="server"> </asp:TextBox>&nbsp;
                </td>
                <td valign="top">                
                    <asp:Button ID="FilterButton" Text="Filter" runat="server" OnClick="Filter_Click" />&nbsp;
                    <asp:Button ID="ClearFilterButton" Text="Clear Filter" runat="server" OnClick="ClearFilter_Click" />
                </td>
            </tr>
        </table>
     <asp:GridView ID="PublisherGridView" AutoGenerateColumns="False" AllowPaging="false" 
                 AllowSorting="true" ShowHeaderWhenEmpty="true" Width="100%" runat="server">
        <Columns>
            <asp:HyperLinkField DataTextField="PublisherName" DataNavigateUrlFormatString="~/Publisher.aspx?ID={0}" 
                        DataNavigateUrlFields="ID" SortExpression="PublisherName" HeaderText="Publisher Name" ItemStyle-Width="200" />
            <asp:BoundField HeaderText="Contact Name" DataField="ContactName" ItemStyle-Width="200" SortExpression="ContactName" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Phone" DataField="Phone" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" SortExpression="Phone" />
            <asp:BoundField HeaderText="Email" DataField="Email" ItemStyle-Width="200" ItemStyle-HorizontalAlign="Center" SortExpression="Email" />
        </Columns>
        <EmptyDataTemplate>
            <div style="color:red">No Publishers found.</div>
        </EmptyDataTemplate>
     </asp:GridView>
     <br />
    <asp:LinkButton ID="AddButton" Text="Add Publisher" OnClick="AddButton_Click" runat="server" />           
</asp:Content>
