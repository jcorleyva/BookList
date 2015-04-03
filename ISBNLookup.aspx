<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ISBNLookup.aspx.cs" Inherits="USATodayBookList.ISBNLookup" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript">
        jQuery.noConflict();
    </script>
</head>
<body style="margin-top:0px; padding-left:10px; padding-right:10px; background-color:White">
    <form id="Form1" runat="server">
        <fieldset style="margin:-10px 0px 0px 0px">
            <legend style="color:Black">ISBN Lookup</legend>
                 <asp:Label ID="ISBNLabel" runat="server" AssociatedControlID="ISBNTextBox">ISBN:</asp:Label>
                 <asp:TextBox ID="ISBNTextBox" runat="server"> </asp:TextBox>
                 <asp:Label ID="TitleLabel" runat="server" AssociatedControlID="TitleTextBox">Title:</asp:Label>
                 <asp:TextBox ID="TitleTextBox" runat="server"> </asp:TextBox>
                 <asp:Label ID="AuthorLabel" runat="server" AssociatedControlID="AuthorTextBox">Author:</asp:Label>
                 <asp:TextBox ID="AuthorTextBox" runat="server"> </asp:TextBox>
                 <asp:Button ID="FilterButton" Text="Filter" runat="server" OnClick="FilterButton_Click" />&nbsp;
                 <asp:Button ID="ClearFilterButton" Text="Clear Filter" runat="server" onclick="ClearFilterButton_Click" />                 
                 <br /><br />
                 <asp:GridView ID="ISBNGridView" AutoGenerateColumns="False" ShowHeader="true"
                             HorizontalAlign="left" ShowHeaderWhenEmpty="true" AllowSorting="true"
                             AllowPaging="true" PageSize="1000" DataKeyNames="Id"
                             OnRowDataBound="ISBNGrid_RowDataBound"
                             OnPageIndexChanging="ISBNGridView_PageIndexChanging" 
                             OnSorting="ISBNGridView_Sorting"
                             OnRowCommand="ISBNGridView_RowCommand"
                             runat="server" Width="100%">
                    <Columns>
                        <asp:CommandField ShowSelectButton="true"  ButtonType="Link" SelectText="" ItemStyle-Width="250" HeaderText="ISBN" SortExpression="ISBN" />
                        <asp:BoundField HeaderText="Title" DataField="Title" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" SortExpression="Title" />
                        <asp:BoundField HeaderText="Author" DataField="Author" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" SortExpression="Author" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="error">No records found.</div>
                    </EmptyDataTemplate>
                 </asp:GridView>
       </fieldset>
       <div align="right" style="padding-top:10px">
        <asp:Button ID="CancelButton" Text="Cancel" OnClientClick="javascript:self.close();" runat="server" />
       </div>
    </form>
<%--    <script type="text/javascript">
        jQuery(function () {
            self.testlist=[];
            /* hack: Mary needs copy and paste, and I cannot figure out how to get that working in a modal... so add a gimmick to do it elsewhere */
            jQuery('.ISBNCommand').each(function () {
                var td=jQuery(this);
                var a=td.find('a');
                var tx=a.text();
                if (tx.match(/[0-9]/)) {
                    a.after('<a onclick="javascript:prompt(\'Copy text:\', \''+tx+'\')">***</a>');
                }
            })
        });
    </script>--%>
</body>
</html>                 
