<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="BookWorkingList.aspx.cs" Inherits="USATodayBookList.BookWorkingList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">
        function editBook(bookId) {
            returnValue = Page.ShowModal("Book.aspx?ID=" + bookId + "&isdialog=true", "1000px", "800px", true);
            if (returnValue != undefined)
                Page.SetClientParam("RefreshGrid", returnValue, true);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Book Working List</h1>
    <br />
    <asp:Label ID="TitleLabel" runat="server" >Title:</asp:Label>&nbsp;
    <asp:TextBox ID="TitleTextBox" runat="server"> </asp:TextBox>
    <asp:Label ID="FirstNameLabel" runat="server" >First Name:</asp:Label>&nbsp;
    <asp:TextBox ID="FirstNameTextBox" runat="server"> </asp:TextBox>
    <asp:Label ID="LastNameLabel" runat="server" >Last Name:</asp:Label>&nbsp;
    <asp:TextBox ID="LastNameTextBox" runat="server"> </asp:TextBox>
    
    <asp:LinkButton ID="FilterButton" Text="Filter" runat="server" OnClick="Filter_Click" />&nbsp;
    <asp:LinkButton ID="ClearFilterButton" Text="Clear Filter" runat="server" OnClick="ClearFilter_Click" /><br />
    <asp:Label ID="Label1" runat="server">Show Top:</asp:Label>
    <asp:TextBox ID="MaxRecordTextBox" runat="server" Text="400" Width="35" />
    <br />
    <asp:RadioButtonList ID="FilterTypeRadioButtonList" AutoPostBack="true" align="left"        
        RepeatDirection="Horizontal" runat="server"
        OnSelectedIndexChanged="FilterType_SelectedIndexChanged">
        <asp:ListItem Text="All Books" Value="All" Selected="True"></asp:ListItem>
        <asp:ListItem Text="New" Value="New" ></asp:ListItem>
        <asp:ListItem Text="Needs Editing" Value="Needs Editing"></asp:ListItem>
        <asp:ListItem Text="Done Editing" Value="Done Editing"></asp:ListItem>
        <asp:ListItem Text="Missing Categories" Value="Missing Categories"></asp:ListItem>         
    </asp:RadioButtonList>
    <br />
    <br />
     <asp:GridView ID="BookWorkingListGridView" AutoGenerateColumns="False" 
                  AllowSorting="true" ShowHeaderWhenEmpty="true" 
        Width="100%" runat="server"
        OnRowDataBound="BookWorkingListGridView_RowDataBound"
        >
        <Columns>
            <asp:BoundField HeaderText="Rank" DataField="Rank" ItemStyle-Width="15" ItemStyle-HorizontalAlign="Center" SortExpression="Rank" />
            <asp:BoundField HeaderText="Last Rank" DataField="LastWeekRank" ItemStyle-Width="15" SortExpression="LastWeekRank"
                ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="Sales" DataField="Sales" SortExpression="Sales" ItemStyle-Width="35" ItemStyle-HorizontalAlign="Center" />
            <asp:TemplateField HeaderText="Title" ItemStyle-Width="150px" SortExpression="Title">
                <ItemTemplate>
                    <div class="links" onclick="editBook(<%# ((USAToday.Booklist.Business.BookWorkingList)Container.DataItem).BookId %>);">
                    <%#((USAToday.Booklist.Business.BookWorkingList)Container.DataItem).Title %>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="AuthorDisplay" HeaderText="Author" ItemStyle-Width="150" SortExpression="AuthorDisplay" />
            <asp:BoundField DataField="EditStatus" HeaderText="Status" ItemStyle-HorizontalAlign="Center" SortExpression="EditStatus" ItemStyle-ForeColor="Blue" ItemStyle-Width="100" />
            <asp:BoundField DataField="CurrentBookStatus" HeaderText="Include" ItemStyle-HorizontalAlign="Center"
                SortExpression="CurrentBookStatus" ItemStyle-Width="100" />
        </Columns>
        <EmptyDataTemplate>
            <div style="color:red">No Records found.</div>
        </EmptyDataTemplate>
    </asp:GridView>
</asp:Content>
