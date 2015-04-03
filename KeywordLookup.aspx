<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="KeywordLookup.aspx.cs" Inherits="USATodayBookList.KeywordLookup" %>
<%@ Register Assembly="CommonWebControl" Namespace="CommonWebControl" TagPrefix="com" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/CommonApp.js"></script>
</head>
<body style="margin-top:0px; padding-left:10px; padding-right:10px; background-color:White">
    <form id="Form1" runat="server">
    <br />
        <fieldset style="margin:-10px 0px 0px 0px">
            <legend style="color:Black">Keyword Lookup</legend>
                <table cellpadding="0" cellspacing="5" border="0" width="100%">
                    <tr>
                        <td class="formCaption">Keyword:</td>
                        <td>
                            <asp:TextBox ID="KeywordTextBox" runat="server"></asp:TextBox>&nbsp;
                            <asp:Button ID="FilterButton" Text="Filter" runat="server" onclick="FilterButton_Click" />&nbsp;
                            <asp:Button ID="ClearFilterButton" Text="Clear Filter" runat="server" onclick="ClearFilterButton_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="formCaption" style="width:200px;">Keyword Assignments:</td>
                        <td>
                             <com:AddRemoveList id="arlKeyword"  runat="server" Rows="10" ShowAllButtons="false" 
                                            SourceText="UnAssigned Keywords" TargetText="Assigned Keywords" />                        
                        </td>
                    </tr>
                </table>
       </fieldset>
                <br />
                <div align="right">
                    <asp:Button runat="server" ID="SaveButton" Text="Save Changes" onclick="SaveButton_Click" />
                        &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="CancelButton" Text="Cancel" OnClientClick="javascript:self.close();" runat="server" />
                </div>
    </form>
</body>
</html>                 
