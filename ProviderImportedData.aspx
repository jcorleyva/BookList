<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ProviderImportedData.aspx.cs" Inherits="USATodayBookList.ProviderImportedData" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin-top:0px; padding-left:10px; padding-right:10px; background-color:White">
    <form id="Form1" runat="server">        
        <fieldset>
            <legend>Provider Imported Data</legend>
        <div>
            <b>Provider:</b> Barnes & Nobles
        </div>
        <br />
        <asp:GridView ID="ProvidersGridView" AutoGenerateColumns="False" runat="server">
            <Columns>
                <asp:BoundField HeaderText="ISBN" DataField="ISBN" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Title" DataField="Title" ItemStyle-Width="200" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Author" DataField="Author" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Sales" DataField="Sales" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
            </Columns>
        </asp:GridView>
        </fieldset>
    </form>
</body>
</html>                 
