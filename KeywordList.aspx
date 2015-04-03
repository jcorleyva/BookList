<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="KeywordList.aspx.cs" Inherits="USATodayBookList.KeywordList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1>Keyword List</h1><br />
     <asp:Label ID="KeywordLabel" runat="server" AssociatedControlID="KeywordTextBox">Keyword:</asp:Label>
                 <asp:TextBox ID="KeywordTextBox" runat="server"> </asp:TextBox>
                  
                 <asp:LinkButton ID="FilterButton" Text="Filter" runat="server" OnClick="Filter_Click" />&nbsp;
                 <asp:LinkButton ID="ClearFilterButton" Text="Clear Filter" runat="server" OnClick="ClearFilter_Click" />
                 <br /><br />
                 <asp:GridView ID="KeywordGridView" AutoGenerateColumns="False" 
                    AllowPaging="false" AllowSorting="true" ShowHeaderWhenEmpty="true" 
                    DataKeyNames="Id" 
                    OnRowEditing="KeywordGridView_RowEditing"
                    OnRowUpdating="KeywordGridView_RowUpdating"
                    OnRowCancelingEdit="KeywordGridView_RowCancelingEdit"
                    OnRowDeleting="KeywordGridView_RowDeleting"
                    OnSorting="KeywordGridView_Sorting"
                 runat="server">
                 <Columns>
                     <asp:BoundField HeaderText="Id" DataField="Id" Visible="false" />
                     <asp:BoundField HeaderText="Keyword" DataField="KeywordDescription" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" SortExpression="KeywordDescription" />
                     <asp:CommandField ShowEditButton="true" ItemStyle-Width="50px"  ButtonType="Link" />
                     <asp:CommandField ShowDeleteButton="true" ItemStyle-Width="50px" ButtonType="Link" />
                 </Columns>
                 <EmptyDataTemplate>
                    <div class="error">No records found.</div>
                 </EmptyDataTemplate>
                 </asp:GridView>
                 <br />
                 <div id="ErrorDiv" runat="server" style="vertical-align:bottom" visible="false" class="error">*Enter Keyword</div>
                 <div>
                    <asp:TextBox ID="AddTextBox" Width="250px" runat="server"></asp:TextBox>&nbsp;
                    <asp:LinkButton ID="AddButton" Text="Add" runat="server" onclick="btnAdd_Click" />
                 </div>
</asp:Content>
