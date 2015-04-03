<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Book.aspx.cs" Inherits="USATodayBookList.Book1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">

    function addCategory(bookId)
    {
        returnValue = Page.ShowModal("CategoryLookup.aspx?bookId=" + bookId, "590px", "390px", false);
        Page.SetClientParam("RefreshCategories", returnValue, true);
    }
    function addKeyword(bookId)
    {
        returnValue = Page.ShowModal("KeywordLookup.aspx?bookId=" + bookId, "590px", "365px", false);
        Page.SetClientParam("RefreshKeywords", returnValue, true);
    }
    function addAuthor(bookId)
    {
        returnValue = Page.ShowModal("AuthorLookup.aspx?bookId=" + bookId, "675px", "375px", true);
        Page.SetClientParam("RefreshAuthors", returnValue, true);
    }
    function addISBN(bookId)
    {
        returnValue = Page.ShowModal("ISBNLookup.aspx?bookId=" + bookId, "850px", "375px", true);
        Page.SetClientParam("RefreshISBNs", returnValue, true);
    }
    function editISBN(isbnId) {
        returnValue = Page.ShowModal("ISBN.aspx?ID=" + isbnId + "&isdialog=true", "1000px", "700px", true);
        if (returnValue == "Refresh")
            Page.SetClientParam("RefreshISBN", returnValue, true);

    }

    function confirmBookCover() {
        var txtBookCoverUrl = document.getElementById("MainContent_BookCoverURLTextBox");
        var cbBookCoverUrl = document.getElementById("MainContent_cbBookCoverUrl");
        if (txtBookCoverUrl.value == "") {
            cbBookCoverUrl.checked = !cbBookCoverUrl.checked;
            alert("Cannot Confirm Book Cover. Please enter Book Cover Url to confirm.");
        }
    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server"><h1>Edit Book</h1><br />
    <div >
        <fieldset >
            <legend>Book Information</legend>
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td valign="top">
            <p>
                <asp:Label ID="TitleLabel" runat="server" >Title:</asp:Label><br />
                <asp:TextBox ID="TitleTextBox" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFV1" CssClass="error" EnableClientScript="true" ControlToValidate="TitleTextBox" InitialValue=""
                 SetFocusOnError="true" ErrorMessage="*required" runat="server"></asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="AuthorDisplayLabel" runat="server" >Author Display:</asp:Label><br />
                <asp:TextBox ID="AuthorDisplayTextBox" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFV2" CssClass="error" EnableClientScript="true" ControlToValidate="AuthorDisplayTextBox" InitialValue=""
                 SetFocusOnError="true" ErrorMessage="*required" runat="server"></asp:RequiredFieldValidator>
                  </p>
                   <p>
                <asp:Label ID="Label2" runat="server" >ASIN:</asp:Label><br />
                <asp:TextBox ID="ASINTextBox" runat="server" CssClass="textEntry"></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="Label3" runat="server" >Purchase ISBN:</asp:Label><br />
                <asp:TextBox ID="PurchaseISBNTextBox" runat="server" CssClass="textEntry"></asp:TextBox>            
            </p>
            <p>
                <asp:Label ID="PublishDateLabel" runat="server" >Publish Date:</asp:Label><br />
                <asp:TextBox ID="PublishDateTextBox" runat="server" CssClass="textEntry"></asp:TextBox><asp:CompareValidator ID="CV1" runat="server" CssClass="error" ErrorMessage="*invalid Date" ControlToValidate="PublishDateTextBox"
                Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
            </p>
            <p>
                <asp:Label ID="Label1" runat="server" >Edit Status:</asp:Label><br />
                <asp:DropDownList ID="EditStatusDropDownList" runat="server"></asp:DropDownList>
            </p>

            <p>
                <asp:Label ID="LastEditedLabel" runat="server" >Last Edited:</asp:Label>&nbsp;
                <asp:Label ID="LastEditedDate" runat="server"></asp:Label>
            </p>
            <p>
                <asp:Label ID="FirstOccuranceLabel" runat="server" >First Occurance:</asp:Label>&nbsp;
                <asp:Label ID="FirstOccuranceDate" runat="server"></asp:Label>
            </p>
             <p>
                <asp:Label ID="ClassLabel" Text="Class:" runat="server" ></asp:Label><br />
                <asp:DropDownList ID="ClassDropDownList" runat="server"></asp:DropDownList>
            </p>
            <p>
                <asp:Label ID="StatusLabel" runat="server" >Status:</asp:Label><br />
                <asp:DropDownList id="StatusDropDownList" runat="server"></asp:DropDownList>
            </p>
            <p style="float:left; clear:both;">
                <asp:Label ID="BriefDescriptionLabel" runat="server" >Brief Description:</asp:Label><br />
                <asp:TextBox ID="BriefDescriptionTextBox" runat="server" CssClass="largeTextEntry"  TextMode="MultiLine"  Width="400" Height="80"></asp:TextBox>
            </p>
            <p style="float:left; clear:both;">
                <asp:Label ID="LongDescription" runat="server" >Long Description:</asp:Label><br />
                <asp:TextBox ID="LongDescriptionTextBox" runat="server"  CssClass="largeTextEntry" TextMode="MultiLine" Width="400" Height="80" ></asp:TextBox>               
            </p>
            <p style="float:left; clear:both;">
                <asp:Label ID="BackgroundInfoLabel" runat="server" >Background Info:</asp:Label><br />
                <asp:TextBox ID="BackgroundInfoTextBox" runat="server"  CssClass="largeTextEntry" TextMode="MultiLine" Width="400" Height="80" ></asp:TextBox>               
            </p>
            <p style="float:left; clear:both;">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td valign="top" style="width:300px">
                                    <p>
                                        <asp:Label ID="BookCoverUrlLabel" runat="server" >Book Cover URL:</asp:Label><br />
                                        <asp:TextBox ID="BookCoverURLTextBox" TextMode="MultiLine" Rows="4" Width="300" runat="server" CssClass="largeTextEntry"></asp:TextBox>
                                        <asp:Label ID="ErrorLabel" CssClass="error" runat="server" /><br />
                                            <asp:LinkButton ID="RefreshBookUrlButton" Text="Refresh Image" CausesValidation="false" OnClick="RefreshBookUrlButton_Click" runat="server" />
                                    </p>
                                    <p> 
                                        <asp:Label ID="ConfirmBookCoverUrl" runat="server" >Confirm Book Cover URL:</asp:Label>&nbsp;
                                        <asp:CheckBox ID="cbBookCoverUrl" onChange="confirmBookCover();" runat="server" />
                                    </p>                    
                                </td>
                                <td valign="middle" align="left">
                                    <asp:Image ID="BookCoverImage" BorderWidth="1px" Width="100px" runat="server" />
                                </td>
                            </tr>
                        </table>
            
            </p>
             <p class="saveButton"  style="float:left; clear:both; width:250px;">
                    <asp:LinkButton ID="SaveButton" runat="server" OnClick="SaveButton_Click"  Text="Save Book" />&nbsp;
                    <asp:LinkButton ID="ManageBooksButton" runat="server" CausesValidation="false" OnClick="ManageBooksButton_Click"  Text="Manage Books" />
             </p>            
                    </td>
                    <td valign="top" align="left">
                    <fieldset>
                    <legend>Categories</legend>
                        <div style="text-align:right">
                             <asp:Label ID="CategoryErrorLabel" Text="*required" CssClass="error" Visible="false" runat="server"></asp:Label>                        
                        </div>                        
                         <asp:GridView ID="CategoryGridView" AutoGenerateColumns="False" ShowHeader="true" Width="100%"
                             HorizontalAlign="left" ShowHeaderWhenEmpty="true" AllowSorting="true"
                             AllowPaging="true" PageSize="5"
                             OnSorting="CategoryGridView_Sorting"  
                             OnPageIndexChanging="CategoryGridView_PageIndexChanging"
                             runat="server">
                             <Columns>                                             
                                 <asp:BoundField HeaderText="Category" DataField="Category" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" />
                                 <asp:TemplateField HeaderText="Primary" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# GetPrimaryCategory(DataBinder.Eval(Container.DataItem,"PrimaryCategory")) %>
                                    </ItemTemplate>
                                 </asp:TemplateField>
                             </Columns>
                             <EmptyDataTemplate>
                                <div class="error">This book is not categorized.</div>
                             </EmptyDataTemplate>
                         </asp:GridView>
                         <div style="float:right; clear:both;padding-top:10px;" align="center">
                            <asp:LinkButton ID="AddCategoryButton" Text="Add Category" CausesValidation="false" runat="server" />
                         </div>
                    </fieldset>
                    <fieldset>
                        <legend>Keywords</legend>
                         <asp:GridView ID="KeywordGridView" AutoGenerateColumns="False" ShowHeader="true"
                             HorizontalAlign="left" ShowHeaderWhenEmpty="true"
                             AllowPaging="true" PageSize="5" runat="server" Width="100%">
                             <Columns>                                             
                                 <asp:BoundField HeaderText="Keyword" DataField="KeywordDescription" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" />
                             </Columns> 
                             <EmptyDataTemplate>
                                <div class="error">There are no keywords for this book.</div>
                             </EmptyDataTemplate>
                         </asp:GridView>                       
                         <div style="float:right; clear:both; padding-top:10px;" align="center">
                            <asp:LinkButton ID="AddKeywordButton" Text="Add Keyword" CausesValidation="false" runat="server" />
                         </div>
                    </fieldset>
                     <fieldset>
                        <legend>Authors</legend>
                        <asp:GridView ID="AuthorGridView" AutoGenerateColumns="False" ShowHeader="true" Width="100%"
                             HorizontalAlign="left" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="5"
                             OnPageIndexChanging="AuthorGridView_PageIndexChanging" DataKeyNames="Id"
                             OnRowDeleting="AuthorGridView_RowDeleting"
                             runat="server">
                          <Columns>
                            <asp:HyperLinkField HeaderText="Author" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/Author.aspx?Id={0}" DataTextField="DisplayName" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" />
                            <asp:CommandField ShowDeleteButton="true" ButtonType="Link" DeleteText="remove" />
                         </Columns> 
                         <EmptyDataTemplate>
                             <div class="error">No authors have been added to this book.</div>
                         </EmptyDataTemplate>
                        </asp:GridView>
                         <div style="float:none; clear:both; padding-top:10px;" align="right">
                            <asp:LinkButton ID="AddAuthorButton" Text="Associate Author" CausesValidation="false" runat="server" />
                         </div>
                   </fieldset>
                   <fieldset>
                    <legend>Associated ISBNs</legend>                 
                    <div style="padding-top:20px;">
                        <asp:GridView ID="ISBNGridView" AutoGenerateColumns="False" ShowHeader="true" Width="100%"
                             HorizontalAlign="left" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="5"
                             OnPageIndexChanging="ISBNGridView_PageIndexChanging" DataKeyNames="Id"
                             OnRowDeleting="ISBNGridView_RowDeleting"
                             OnRowDataBound="ISBNGridView_RowDataBound"
                             runat="server"
                        >
                            <Columns>
                                <asp:TemplateField HeaderText="ISBN" ItemStyle-Width="150" SortExpression="ISBN">
                                    <ItemTemplate>
                                        <div class="links" onclick="editISBN(<%# ((USAToday.Booklist.Business.BookISBNView)Container.DataItem).Id.ToString() %>);">
                                            <%# DataBinder.Eval(Container.DataItem, "ISBN") %>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Title" DataField="Title" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField HeaderText="Author" DataField="Author" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" />                                 
                                <asp:BoundField HeaderText="Status" DataField="EditStatusDescription" ItemStyle-HorizontalAlign="left" ItemStyle-ForeColor="Blue" />
                                <asp:CommandField ShowDeleteButton="true" ButtonType="Link" DeleteText="remove" />
                            </Columns> 
                            <EmptyDataTemplate>
                                <div class="error">There are no ISBNs associated with this book.</div>
                            </EmptyDataTemplate>
                         </asp:GridView>
                    </div>
                    <div style="float:none; clear:both; padding-top:10px;" align="right">
                        <asp:LinkButton ID="AddISBNButton" Text="Associate ISBN" CausesValidation="false" runat="server"></asp:LinkButton>
                    </div>
                </fieldset>
                <div>
                <fieldset>
                    <legend>Sales By Provider</legend>
                     <asp:GridView ID="SalesGridView" AutoGenerateColumns="False" ShowHeader="true" 
                      Width="100%" ShowHeaderWhenEmpty="true" runat="server">
                     <Columns>
                         <asp:BoundField HeaderText="Provider" DataField="ProviderName" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" />
                         <asp:BoundField HeaderText="ISBN" DataField="ISBN"  ItemStyle-Width="100" ItemStyle-HorizontalAlign="Left" />
                         <asp:BoundField HeaderText="ISBN Sales" DataField="ISBNSales"  DataFormatString="{0:#,###}" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                     </Columns>
                        <EmptyDataTemplate>
                                <div style="color:red">No data returned.</div>
                        </EmptyDataTemplate>                           
                     </asp:GridView>                    
                     <br />
                    <table cellpadding="0" cellspacing="0" border="0" style="border:1px solid #C0C0C0;" width="100%">
                        <tr>
                            <td style="width:250px; border-right:1px solid #C0C0C0;"><b>Total Sales for Book This Week</b></td>
                            <td style="width:100px;" align="center" id="tdTotalISBNSales" runat="server"></td>
                        </tr>
                    </table>
                </fieldset>
                </div>
                    </td>
                </tr>
         </table>
        </fieldset>
    </div>
</asp:Content>
