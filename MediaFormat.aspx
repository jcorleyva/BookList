<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MediaFormat.aspx.cs" Inherits="USATodayBookList.MediaFormatList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server"><h1>Media Format</h1><br />
     <asp:Label ID="MediaFormatLabel" runat="server" AssociatedControlID="MediaFormatTextBox">Media Format:</asp:Label>
                 <asp:TextBox ID="MediaFormatTextBox" runat="server"> </asp:TextBox>
                  
                 <asp:LinkButton ID="FilterButton" Text="Filter" runat="server" OnClick="Filter_Click" />&nbsp;
                 <asp:LinkButton ID="ClearFilterButton" Text="Clear Filter" runat="server" OnClick="ClearFilter_Click" />
                 <br /><br />
                <asp:GridView ID="MediaFormatGridView" AutoGenerateColumns="False" 
                    AllowPaging="false" AllowSorting="true"  ShowHeaderWhenEmpty="true" 
                    DataKeyNames="Id" 
                    OnRowEditing="MediaFormatGridView_RowEditing"
                    OnRowUpdating="MediaFormatGridView_RowUpdating"
                    OnRowCancelingEdit="MediaFormatGridView_RowCancelingEdit"
                    OnRowDeleting="MediaFormatGridView_RowDeleting"
                    OnSorting="MediaFormatGridView_Sorting"

                 runat="server">
                 <Columns>                    
                     <asp:BoundField HeaderText="Id" DataField="Id" Visible="false" />
                     <asp:BoundField HeaderText="Media Format" DataField="MediaFormatDescription" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" SortExpression="MediaFormatDescription" />
                     <asp:CommandField ShowEditButton="true" ItemStyle-Width="50px" ButtonType="Link" />
                     <asp:CommandField ShowDeleteButton="true" ItemStyle-Width="50px" ButtonType="Link" />
                 </Columns>
                 <EmptyDataTemplate>
                    <div class="error">No records found.</div>
                 </EmptyDataTemplate>
                 </asp:GridView>
                 <br />
                 <div id="ErrorDiv" runat="server" style="vertical-align:bottom" visible="false" class="error">*Enter Media Format</div>
                 <div>
                    <asp:TextBox ID="AddTextBox" Width="250px" runat="server"></asp:TextBox>&nbsp;
                    <asp:LinkButton ID="AddButton" Text="Add Media Format" runat="server" onclick="btnAdd_Click" />
                 </div>
</asp:Content>
