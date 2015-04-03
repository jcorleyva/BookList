<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProviderList.aspx.cs" Inherits="USATodayBookList.ProviderList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<h1>Provider List</h1><br />
     
        <table cellpadding="0" cellspacing="3" border="0">
            <tr>
                <td valign="top">
                     <asp:Label ID="ProviderLabel" runat="server" AssociatedControlID="ProviderTextBox">Provider:</asp:Label>&nbsp;
                </td>
                <td>
                     <asp:TextBox ID="ProviderTextBox" runat="server"> </asp:TextBox><br />
                </td>
                <td valign="top">                
                    <asp:LinkButton ID="BtnFilter" Text="Filter" runat="server" OnClick="Filter_Click" />&nbsp;
                    <asp:LinkButton ID="BtnClearFilter" Text="Clear Filter" runat="server" OnClick="ClearFilter_Click" />
                </td>
            </tr>
        </table>
     <asp:GridView ID="ProviderGridView" AutoGenerateColumns="False" AllowPaging="false" 
                 AllowSorting="true" ShowHeaderWhenEmpty="true" Width="100%" runat="server">
        <Columns>
            <asp:HyperLinkField DataTextField="ProviderName" DataNavigateUrlFormatString="~/Provider.aspx?ID={0}" 
                        DataNavigateUrlFields="ID" SortExpression="ProviderName" HeaderText="Provider Name" ItemStyle-Width="150" />
            <asp:BoundField HeaderText="Systems Contact" DataField="SystemsContact" ItemStyle-Width="200" SortExpression="SystemContact" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Phone" DataField="Phone" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" SortExpression="Phone" />
            <asp:BoundField HeaderText="Email" DataField="Email" ItemStyle-Width="200" ItemStyle-HorizontalAlign="Center" SortExpression="Email" />
        </Columns>
        <EmptyDataTemplate>
            <div style="color:red">No Provider found.</div>
        </EmptyDataTemplate>
     </asp:GridView>
     <br />
    <asp:LinkButton ID="AddButton" Text="Add" OnClick="btnAdd_Click" runat="server" />
</asp:Content>
