<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="AuthorLookup.aspx.cs" Inherits="USATodayBookList.AuthorLookup" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function addAuthor() {
            returnValue = Page.ShowModal("Author.aspx?ID=0&isdialog=true", "1000px", "900px", true);
            Page.SetClientParam("RefreshSearch", returnValue, true);
        }
   </script>
</head>
<body style="margin-top:0px; padding-left:10px; padding-right:10px; background-color:White">
    <form id="Form1" runat="server">
        <fieldset style="margin:-10px 0px 0px 0px">
            <legend style="color:Black">Author Lookup</legend>
                 <asp:Label ID="FirstNameLabel" runat="server" AssociatedControlID="FirstNameTextBox">First Name:</asp:Label>
                 <asp:TextBox ID="FirstNameTextBox" runat="server"></asp:TextBox>
                 <asp:Label ID="LastNameLabel" runat="server" AssociatedControlID="LastNameTextBox">Last Name:</asp:Label>
                 <asp:TextBox ID="LastNameTextBox" runat="server"> </asp:TextBox>
                 <asp:Button ID="FilterButton" Text="Filter" runat="server" OnClick="FilterButton_Click" />&nbsp;
                 <asp:Button ID="ClearFilterButton" Text="Clear Filter" runat="server" onclick="ClearFilterButton_Click" />                 
                 <br /><br />
                 <asp:GridView ID="AuthorGridView" AutoGenerateColumns="False" ShowHeader="true"
                             HorizontalAlign="left" ShowHeaderWhenEmpty="true" AllowSorting="true"
                             AllowPaging="false" DataKeyNames="Id"
                             OnRowDataBound="AuthorGrid_RowDataBound"
                             OnPageIndexChanging="AuthorGridView_PageIndexChanging" 
                             OnSorting="AuthorGridView_Sorting"
                             OnRowCommand="AuthorGridView_RowCommand"
                             runat="server" Width="100%">
                     <Columns>
                         <asp:CommandField ShowSelectButton="true"  ButtonType="Link" SelectText="" ItemStyle-Width="150" HeaderText="Last Name" SortExpression="LastName" />
                         <asp:BoundField HeaderText="First Name" DataField="FirstName" ItemStyle-Width="150" SortExpression="FirstName" />
                         <asp:BoundField HeaderText="Middle Initial" DataField="Initial" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" SortExpression="Initial" />
                         <asp:BoundField HeaderText="Suffix" DataField="Suffix" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" SortExpression="Suffix" />
                     </Columns>
                     <EmptyDataTemplate>
                           <div class="error">No records found.</div>
                     </EmptyDataTemplate>
                  </asp:GridView>
                  
       </fieldset>
                <div align="right" style="padding-top:10px">
                    <asp:Button ID="AddAuthorButton" OnClientClick="addAuthor();" Text="Add Author" runat="server" /> 
                    &nbsp;
                    <asp:Button ID="CancelButton" Text="Cancel" OnClientClick="javascript:self.close();" runat="server" />
                </div>
    </form>
</body>
</html>                 
