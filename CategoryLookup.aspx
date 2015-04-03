<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="CategoryLookup.aspx.cs" Inherits="USATodayBookList.CategoryLookup" %>
<%@ Register Assembly="CommonWebControl" Namespace="CommonWebControl" TagPrefix="com" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
    <title>USAToday Best-Selling Books</title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin-top:0px; padding-left:10px; padding-right:10px; background-color:White">
    <form id="Form1" runat="server">
        <fieldset style="margin:-10px 0px 0px 0px;">
            <legend style="color:Black">Category Lookup</legend>
                <table cellpadding="0" cellspacing="5" border="0" width="100%">
                    <tr>
                        <td class="formCaption">Category:</td>
                        <td>
                            <asp:TextBox ID="CategoryTextBox" runat="server"></asp:TextBox>&nbsp;
                            <asp:Button ID="FilterButton" Text="Filter" runat="server" onclick="FilterButton_Click" />&nbsp;
                            <asp:Button ID="ClearFilterButton" Text="Clear Filter" runat="server" onclick="ClearFilterButton_Click" />&nbsp;
                            <asp:Button ID="AddButton" Text="Add" runat="server" onclick="AddButton_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="formCaption">Primary:</td>
                        <td><asp:Label ID="PrimaryCategoryLabel" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="formCaption">Change primary to:</td>
                        <td>
                             <asp:DropDownList ID="PrimaryDropDownList" 
                                DataTextField="CategoryDescription" DataValueField="Id" 
                                OnSelectedIndexChanged="Primary_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                <asp:ListItem Text="---Select---" Value="0"></asp:ListItem>
                             </asp:DropDownList>                        
                        </td>
                    </tr>
                    <tr>
                        <td class="formCaption" style="width:160px;">Category Assignments:</td>
                        <td>
                             <com:AddRemoveList id="arlCategory"  runat="server" Rows="10" ShowAllButtons="false" 
                                            SourceText="UnAssigned Categories" TargetText="Assigned Categories"  />                        
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
