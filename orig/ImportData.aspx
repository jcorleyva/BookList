<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportData.aspx.cs" Inherits="USATodayBookList.ImportData" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <asp:Label ID="ProviderLabel" runat="server" AssociatedControlID="ProviderTextBox">Provider:</asp:Label>
                 <asp:TextBox ID="ProviderTextBox" runat="server"> </asp:TextBox>

                 <asp:Button ID="BtnFilter" Text="Filter" runat="server" OnClick="FilterClick" />&nbsp;<br /><br />
                 Provider: <asp:DropDownList ID="ddlProviders" runat="server">
                    <asp:ListItem Text="Barnes & Nobles"></asp:ListItem>
                    <asp:ListItem Text="Amazon"></asp:ListItem>
                    <asp:ListItem Text="Amazon Book"></asp:ListItem>
                    <asp:ListItem Text="Walmart"></asp:ListItem>
                 </asp:DropDownList>
                 <asp:Button ID="Button1" Text="Custom Import" OnClientClick="javascript:window.showModalDialog('ConfirmImport.aspx','','dialogWidth:600px;');" runat="server" />&nbsp;<br /><br />
                 <asp:GridView ID="ProviderGridView" AutoGenerateColumns="False" runat="server">
                    <Columns>
                     <asp:HyperLinkField HeaderText="Provider Name" NavigateUrl="javascript:window.showModalDialog('ProviderImportedData.aspx');" DataTextField="ProviderName" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" />
                     <asp:BoundField HeaderText="This Week Sales" DataField="ThisWeekSales" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" />
                     <asp:BoundField HeaderText="Last Week Sales" DataField="ThisWeekSales" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" />
                     <asp:BoundField HeaderText="Rows Imported" DataField="RowsImported" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" />
                     <asp:BoundField HeaderText="Import Date" DataField="DateImported" ItemStyle-Width="200" ItemStyle-HorizontalAlign="Center" />
                     <asp:BoundField HeaderText="Status" DataField="Status" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" />                     
                    </Columns>
                 </asp:GridView>
        <br />
    <asp:Button ID="btnImport" Text="Import" OnClientClick="javascript:window.showModalDialog('ConfirmImport.aspx','','dialogWidth:600px;');" OnClick="btnImport_Click" runat="server" />
    <asp:Button ID="btnClear" Text="Clear Raw Data" OnClick="btnClear_Click" runat="server" />

</asp:Content>
