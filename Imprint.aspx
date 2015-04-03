<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Imprint.aspx.cs" Inherits="USATodayBookList.Imprint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div >
        <fieldset >
            <legend>Imprint Information</legend>
                        <p>
                            <asp:Label ID="ImprintNameLabel" runat="server" >Imprint Name:</asp:Label><br />
                            <asp:TextBox ID="ImprintNameTextBox" runat="server" CssClass="textEntry"></asp:TextBox>                
                            <asp:RequiredFieldValidator ID="RFV1" CssClass="error" EnableClientScript="true" ControlToValidate="ImprintNameTextBox" InitialValue=""
                             SetFocusOnError="true" ErrorMessage="*required" runat="server"></asp:RequiredFieldValidator>
                        </p>
                        <p>
                            <asp:Label ID="PublisherNameLabel" runat="server" >Publisher Name:</asp:Label><br />
                            <asp:DropDownList ID="PublisherDropDownList" runat="server"></asp:DropDownList>
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
                            <asp:LinkButton ID="SaveButton" runat="server" OnClick="SaveButton_Click"  Text="Save Imprint" />&nbsp;
                            <a href="ImprintList.aspx">Manage Imprints</a>
                         </p>
        </fieldset>
    </div>
    
</asp:Content>
