<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="true" CodeBehind="CategoryList.aspx.cs" Inherits="USATodayBookList.CategoryList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1>Category List</h1><br />
     <asp:Label ID="CategoryLabel" runat="server" AssociatedControlID="CategoryTextBox">Category:</asp:Label>
                 <asp:TextBox ID="CategoryTextBox" runat="server"> </asp:TextBox>
                  
                 <asp:LinkButton ID="FilterButton" Text="Filter" runat="server" OnClick="Filter_Click" />&nbsp;
                 <asp:LinkButton ID="ClearFilterButton" Text="Clear Filter" runat="server" OnClick="ClearFilter_Click" />
                 <br /><br />
                 <asp:GridView ID="CategoryGridView" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" 
                   DataKeyNames="Id" AllowSorting="true"
                    AllowPaging="false"
                    OnRowEditing="CategoryGridView_RowEditing"
                    OnRowUpdating="CategoryGridView_RowUpdating"
                    OnRowCancelingEdit="CategoryGridView_RowCancelingEdit"
                    OnRowDeleting="CategoryGridView_RowDeleting"
                    OnSorting="CategoryGridView_Sorting"
                 runat="server">
                 <Columns>
                     <asp:BoundField HeaderText="Id" DataField="Id" Visible="false" />
                     <asp:BoundField HeaderText="Category" DataField="CategoryDescription" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" SortExpression="CategoryDescription" />
                     <asp:CommandField ShowEditButton="true" ItemStyle-Width="50px" ButtonType="Link" />
                     <asp:CommandField ShowDeleteButton="true" ItemStyle-Width="50px" ButtonType="Link" />
                 </Columns>
                 <EmptyDataTemplate>
                    <div class="error">No records found.</div>
                 </EmptyDataTemplate>
                 </asp:GridView>
                 <br />
                 <div id="ErrorDiv" runat="server" style="vertical-align:bottom" visible="false" class="error">*Enter Category</div>
                 <div>
                    <asp:TextBox ID="AddTextBox" Width="250px" runat="server"></asp:TextBox>&nbsp;
                    <asp:LinkButton ID="AddButton" Text="Add" runat="server" onclick="btnAdd_Click" />
                 </div>

</asp:Content>
