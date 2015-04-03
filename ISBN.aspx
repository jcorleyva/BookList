<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ISBN.aspx.cs" Inherits="USATodayBookList.ISBN" %>
<%@ Register  TagPrefix="uc" TagName="LinkableTextBox" Src="~/Controls/LinkableTextBox.ascx"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">
        function addBook(isbnId, isbn) {
            returnValue = Page.ShowModal("BookLookup.aspx?IsbnId=" + isbnId + "&isbn=" + isbn, "940px", "390px", true);
            Page.SetClientParam("RefreshBook", returnValue, true);
        }

        function ClientValidate(source, arguments)
        {
            var ddlPublisher = document.getElementById("MainContent_PublisherDropDownList");
            var tbImprint = document.getElementById("MainContent_tbImprint_txtbox");
            if (tbImprint.value != "" && ddlPublisher.value == 0)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server"> 
    <h1>Edit ISBN</h1>
    <table cellpadding="2" cellspacing="0" border="0">
        <tr>
            <td style="width:50%" valign="top">
                <fieldset>
                    <legend>Book ISBN Information</legend>
                    <p>
                        <asp:Label ID="ISBNLabel" runat="server" >ISBN:</asp:Label><br />
                        <asp:TextBox ID="ISBNTextBox" runat="server" CssClass="textEntry" Width="200px" ></asp:TextBox>               
                        <asp:RequiredFieldValidator ID="RFV1" CssClass="error" EnableClientScript="true" ControlToValidate="ISBNTextBox" InitialValue=""
                         SetFocusOnError="true" ErrorMessage="*required" ValidationGroup="VG1" runat="server"></asp:RequiredFieldValidator>
                    </p>
                    <p>
                        <asp:Label ID="BookLabel" runat="server" >Book:&nbsp;</asp:Label>
                        <asp:LinkButton ID="AddBookLink" Font-Underline="true" Text="Add Book" runat="server"></asp:LinkButton>&nbsp;
                        <asp:HyperLink ID="EditBookLink" Text="Edit Book" NavigateUrl="" runat="server"></asp:HyperLink>
                    </p>
                    <p>
                        <asp:Label ID="PriceLabel" runat="server" >Price:</asp:Label><br />
                        <asp:TextBox ID="PriceTextBox" runat="server" CssClass="textEntry"  Width="200px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RFV2" CssClass="error" EnableClientScript="true" ControlToValidate="PriceTextBox" InitialValue=""
                         SetFocusOnError="true" ErrorMessage="*required" ValidationGroup="VG1" runat="server"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="REV1" CssClass="error" EnableClientScript="true" ControlToValidate="PriceTextBox" InitialValue=""
                         SetFocusOnError="true" ErrorMessage="*invalid number" ValidationExpression="\d+(\.\d+)?" ValidationGroup="VG1" runat="server"></asp:RegularExpressionValidator>
                    </p>
                    <p style="clear:both;">
                        <div style="margin-left:10px;">
                            Publisher:<br />
                            <asp:DropDownList ID="PublisherDropDownList" CssClass="control" AutoPostBack="true" OnSelectedIndexChanged="PublisherDropDown_SelectedIndexChanged" runat="server"></asp:DropDownList>
                            <uc:LinkableTextBox id="tbPublisher" LinkText="Add" InitialValue="" ErrorMessage="*required" runat="server"></uc:LinkableTextBox>
                        </div>
                    </p>
                    <p style="clear:both;">
                       <div style="margin-left:10px;">
                        Imprint:<br />
                           <asp:DropDownList ID="ImprintDropDownList" CssClass="control" runat="server">
                            <asp:ListItem Text="---Select---" Value="0"></asp:ListItem>
                           </asp:DropDownList>
                           <uc:LinkableTextBox ID="tbImprint" LinkText="Add" InitialValue="" ErrorMessage="*required"
                            CustomControlToValidate="MainContent_PublisherDropDownList" CustomErrorMessage="Select Publisher Plz!" CustomClientValidationFunction="ClientValidate" runat="server" />
                        </div>
                    </p>
                    <br />
                    <p style="clear:both;">
                        <asp:Label ID="MediaFormatLabel" runat="server" >Media Format:</asp:Label><br />
                        <asp:DropDownList ID="MediaFormatDropDownList" runat="server"></asp:DropDownList>                       
                    </p>
                    <p>
                        <asp:Label ID="PermExcludedLabel" runat="server" >Permanently Exclude:</asp:Label>&nbsp;
                        <asp:CheckBox ID="PermExcludeCheckBox" runat="server" />
                    </p>
                    <p>
                        <asp:Label ID="EditStatusLabel" runat="server" >Edit Status:</asp:Label><br />
                        <asp:DropDownList id="EditStatusDropDownList" runat="server"></asp:DropDownList>
                    </p>
                    <p class="saveButton">
                        <asp:LinkButton ID="SaveButton" runat="server" ValidationGroup="VG1" OnClick="SaveButton_Click"  Text="Save ISBN" />&nbsp;
                        <a href="ISBNList.aspx">Manage Book ISBNs</a>
                    </p>
                </fieldset>
            </td>
            <td valign="top" style="width:50%;">
                <fieldset>
                    <legend>Research</legend>
                        <asp:GridView ID="LinksGridView" AutoGenerateColumns="False" ShowHeader="false"
                             GridLines="None" PageSize="10" AllowPaging="true"
                             OnPageIndexChanging="LinksGridView_PageIndexChanging"
                             runat="server">
                            <Columns>
                                <asp:HyperLinkField DataNavigateUrlFields="URL" DataNavigateUrlFormatString="{0}" 
                                    DataTextField="LinkDesc" Target="_blank" ItemStyle-HorizontalAlign="Left" />
                           </Columns>
                             <EmptyDataTemplate>
                                <div style="color:red">There are currently no research links set up.</div>
                             </EmptyDataTemplate>                           
                        </asp:GridView>
                </fieldset>
                <fieldset>
                    <legend>Sales By Provider</legend>
                     <asp:GridView ID="SalesGridView" AutoGenerateColumns="False" ShowHeader="true" 
                      Width="100%" ShowHeaderWhenEmpty="true" runat="server">
                     <Columns>
                         <asp:BoundField HeaderText="Provider" DataField="ProviderName" ItemStyle-Width="250" ItemStyle-HorizontalAlign="Left" />
                         <asp:BoundField HeaderText="ISBN Sales" DataField="ISBNSales" DataFormatString="{0:#,###}" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                     </Columns>
                        <EmptyDataTemplate>
                                <div style="color:red">No data returned.</div>
                        </EmptyDataTemplate>                           
                     </asp:GridView>                    
                     <br />
                <table cellpadding="0" cellspacing="0" border="0" style="border:1px solid #C0C0C0;" width="100%">
                    <tr>
                        <td style="border-bottom:1px solid #C0C0C0; border-right:1px solid #C0C0C0; width:250px;"><b>Total Sales for ISBN</b></td>
                        <td style="border-bottom:1px solid #C0C0C0;" align="center" id="tdTotalISBNSales" runat="server"></td>
                    </tr>
                    <tr >
                        <td style="border-right:1px solid #C0C0C0; width:250px;"><b>Book Sales</b></td>
                        <td align="center" id="tdTotalBookSales" runat="server"></td>
                    </tr>
                </table>

                </fieldset>

            </td>

        </tr>
    </table>
    
</asp:Content>
