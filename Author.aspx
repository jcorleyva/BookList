<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Author.aspx.cs" Inherits="USATodayBookList.Author" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function textChanged() 
        {
            var firstName = document.getElementById("MainContent_FirstNameTextBox");
            var lastName = document.getElementById("MainContent_LastNameTextBox");
            var displayName = document.getElementById("MainContent_DisplayNameTextBox");
            displayName.value = firstName.value + " " + lastName.value;         
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Edit Author</h1>
<table cellpadding="2" cellspacing="0" border="0">
        <tr>
            <td style="width:50%">
    <div >
        <fieldset >
            <legend>Author Information</legend>
            <p>
                <asp:Label ID="FirstNameLabel" runat="server" >First Name:</asp:Label><br />
                <asp:TextBox ID="FirstNameTextBox" runat="server" CssClass="textEntry" onChange="textChanged();" ></asp:TextBox>
            </p>
            <p>
                <asp:Label ID="LastNameLabel" runat="server" >Last Name:</asp:Label><br />
                <asp:TextBox ID="LastNameTextBox" runat="server" CssClass="textEntry" onChange="textChanged();" ></asp:TextBox>               
                <asp:RequiredFieldValidator ID="RFV2" CssClass="error" EnableClientScript="true" ControlToValidate="LastNameTextBox" InitialValue=""
                 SetFocusOnError="true" ErrorMessage="*required" runat="server"></asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="InitialLabel" runat="server" >Initial:</asp:Label><br />
                <asp:TextBox ID="InitialTextBox" MaxLength="2" runat="server" CssClass="textEntry"></asp:TextBox>               
            </p>
            <p>
                <asp:Label ID="SuffixLabel" runat="server" >Suffix:</asp:Label><br />
                <asp:TextBox ID="SuffixTextBox" runat="server" CssClass="textEntry"></asp:TextBox>               
            </p>
            <p>
                <asp:Label ID="DisplayNameLabel" runat="server" >Display Name:</asp:Label><br />
                <asp:TextBox ID="DisplayNameTextBox" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFV3" CssClass="error" EnableClientScript="true" ControlToValidate="DisplayNameTextBox" InitialValue=""
                 SetFocusOnError="true" ErrorMessage="*required" runat="server"></asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="DescriptionLabel" runat="server" >Description:</asp:Label><br />
                <asp:TextBox ID="DescriptionTextBox" runat="server" CssClass="largeTextEntry"  TextMode="MultiLine"  Width="400" Height="80"></asp:TextBox>               
            </p> 
            <p>
             <p class="saveButton">
                <asp:LinkButton ID="SaveButton" runat="server" OnClick="SaveButton_Click"  Text="Save Author" />&nbsp;
                <asp:LinkButton ID="ManageAuthorsButton" Text="Manage Authors" CausesValidation="false" OnClick="ManageAuthorsButton_Click" runat="server"></asp:LinkButton>
             </p>
            </p>         
        </fieldset>
    </div>
    </td>
    <td valign="top">
        <fieldset >
            <legend>Books</legend>
            <asp:GridView ID="BookGridView" AutoGenerateColumns="False" ShowHeader="true"
                             HorizontalAlign="left" ShowHeaderWhenEmpty="true" AllowPaging="false" DataKeyNames="Id"
                             OnRowDeleting="BookGridView_RowDeleting"
                             runat="server" Width="100%">
                <Columns>
                    <asp:HyperLinkField DataTextField="Title" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/Book.aspx?Id={0}"  HeaderText="Title" ItemStyle-Width="250" />
                    <asp:BoundField HeaderText="Publish Date" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center"
                        DataFormatString="{0:MM/dd/yyyy}" DataField="PubDate" HtmlEncode="false" />
                    <asp:CommandField ShowDeleteButton="true" ButtonType="Link" DeleteText="remove" />
                </Columns>
                <EmptyDataTemplate>
                    <div class="error">No books found.</div>
                </EmptyDataTemplate>
           </asp:GridView>
        </fieldset>
    </td>
</tr>
</table>
    
</asp:Content>
