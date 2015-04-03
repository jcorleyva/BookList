<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="SalesByProvider.aspx.cs" Inherits="USATodayBookList.SalesByProvider" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin-top:0px; padding-left:10px; padding-right:10px; background-color:White">
    <form id="Form1" runat="server">
    <br />
        <fieldset style="margin:-10px 0px 0px 0px">
            <legend style="color:Black">Sales By Provider</legend>
                <asp:Label ID="ISBNLabel" Font-Bold="true" runat="server">ISBN:</asp:Label>&nbsp;<span id="spanISBN" runat="server"></span>
                <br />
                <asp:Label ID="BookLabel" Font-Bold="true" runat="server">Book:</asp:Label>&nbsp;<span id="spanBook" runat="server"></span>
                <br />
                <asp:GridView ID="SalesGridView" AutoGenerateColumns="False" ShowHeader="true" 
                    Width="100%" ShowHeaderWhenEmpty="true" runat="server">
                   <Columns>
                    <asp:BoundField HeaderText="Provider" DataField="ProviderName" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="ISBN Sales" DataField="ISBNSales"  DataFormatString="{0:#,###}" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                   </Columns>
                   <EmptyDataTemplate>
                    <div style="color:red">No data returned.</div>
                   </EmptyDataTemplate>                           
                </asp:GridView>                    
                <br />
                <table cellpadding="0" cellspacing="0" border="0" style="border:1px solid #C0C0C0;" width="100%">
                    <tr>
                        <td style="border-bottom:1px solid #C0C0C0; border-right:1px solid #C0C0C0; width:250px;"><b>Total Sales for ISBN</b></td>
                        <td style="border-bottom:1px solid #C0C0C0;" align="center" id="tdTotalISBNSales" runat="server"></td>
                    </tr>
                    <tr >
                        <td style="border-right:1px solid #C0C0C0; width:250px;"><b>Book Sales</b></td>
                        <td align="center" id="tdTotalBookSales" runat="server"></td>
                    </tr>
                </table>
       </fieldset>
    </form>
</body>
</html>                 
