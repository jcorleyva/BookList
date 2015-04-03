<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Publisher.aspx.cs" Inherits="USATodayBookList.Publisher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <fieldset >
            <legend>Publisher Information</legend>
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td style="width:50%" valign="top">
                        <p>
                            <asp:Label ID="PublisherNameLabel" runat="server" >Publisher Name:</asp:Label><br />
                            <asp:TextBox ID="PublisherNameTextBox" runat="server" CssClass="textEntry"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RFV1" CssClass="error" EnableClientScript="true" ControlToValidate="PublisherNameTextBox" InitialValue=""
                             SetFocusOnError="true" ErrorMessage="*required" runat="server"></asp:RequiredFieldValidator>
                        </p>
                        <p>
                            <asp:Label ID="ContactNameLabel" runat="server" >Contact Name:</asp:Label><br />
                            <asp:TextBox ID="ContactNameTextBox" runat="server" CssClass="textEntry"></asp:TextBox>               
                        </p>
                        <p>
                            <asp:Label ID="EmailLabel" runat="server" >Email:</asp:Label><br />
                            <asp:TextBox ID="EmailTextBox" runat="server" CssClass="textEntry"></asp:TextBox>               
                        </p>
                        <p>
                            <asp:Label ID="PhoneLabel" runat="server" >Phone:</asp:Label><br />
                            <asp:TextBox ID="PhoneTextBox" runat="server" CssClass="textEntry"></asp:TextBox>               
                        </p>
                        <p>
                            <asp:Label ID="NotesLabel" runat="server" >Notes:</asp:Label><br />
                            <asp:TextBox ID="NotesTextBox" runat="server" CssClass="largeTextEntry"  TextMode="MultiLine"  Width="400" Height="80"></asp:TextBox>               
                        </p>
                         <p class="saveButton">
                            <asp:LinkButton ID="SaveButton" runat="server" OnClick="SaveButton_Click"  Text="Save Publisher" />&nbsp;
                            <a href="PublisherList.aspx">Manage Publishers</a>
                         </p>
                    </td>
                    <td valign="top" style="width:50%">
                        <fieldset>
                            <legend style="margin-top:-20px">Imprints</legend>
                            <asp:GridView ID="ImprintGridView" AutoGenerateColumns="False" ShowHeader="true"
                                             HorizontalAlign="left" ShowHeaderWhenEmpty="true" AllowPaging="false"
                                             DataKeyNames="Id"
                                             OnRowDeleting="ImprintGridView_RowDeleting"
                                             runat="server" Width="100%">
                                <Columns>
                                    <asp:HyperLinkField DataTextField="ImprintName" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/Imprint.aspx?Id={0}"  HeaderText="Imprint Name" ItemStyle-Width="250" />
                                    <asp:CommandField ShowDeleteButton="true" ButtonType="Link" ItemStyle-Width="100px" DeleteText="remove" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="error">No Imprints found.</div>
                                </EmptyDataTemplate>
                           </asp:GridView>
                        </fieldset>
                         <asp:LinkButton ID="AddImprintButton" Text="Add Imprint" CausesValidation="false" OnClick="AddButton_Click" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
    
</asp:Content>
