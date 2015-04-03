<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ConfirmImport.aspx.cs" Inherits="USATodayBookList.ConfirmImport" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin-top:0px; padding-left:10px; padding-right:10px; background-color:White">
    <form id="Form1" runat="server">        
        <fieldset>
            <legend>Confirm Import</legend>
            <table cellpadding="0" cellspacing="0" border="0" align="center" >
                <tr>
                    <td>
                        Please verify that you will be importing data from these providers.<br /><br />
                        <asp:GridView ID="ConfirmGridView" AutoGenerateColumns="False" runat="server">
                            <Columns>
                                <asp:BoundField HeaderText="Provider Name" DataField="ProviderName" ItemStyle-Width="300" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField HeaderText="File Extension" DataField="FileExtension" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField HeaderText="Current Import File" DataField="CurrentImportFile" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </asp:GridView>
                    
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td align="center" style="height:30px;"> 
                        Are you sure, you want to continue importing data?                    
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnYes" Text="Yes" runat="server" />&nbsp;&nbsp;
                        <asp:Button ID="Button1" Text="No" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </form>
</body>
</html>                 