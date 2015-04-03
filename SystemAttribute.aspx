<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SystemAttribute.aspx.cs" Inherits="USATodayBookList.SystemAttribute" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1>Edit System Attribute</h1>
        <fieldset>
            <legend>System Attribute Information</legend>
            <p>
                <asp:Label ID="ChannelTypeLabel" runat="server" >Attribute Name:</asp:Label><br />
                <asp:TextBox ID="AttributeNameTextBox" runat="server" Width="350px" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFV1" CssClass="error" EnableClientScript="true" ControlToValidate="AttributeNameTextBox" InitialValue=""
                 SetFocusOnError="true" ErrorMessage="*required" runat="server"></asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="AttributeDescriptionLabel" runat="server" >Attribute Description:</asp:Label><br />
                <asp:TextBox ID="AttributeDescriptionTextBox" TextMode="MultiLine" Rows="5" Width="350px" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFV2" CssClass="error" EnableClientScript="true" ControlToValidate="AttributeDescriptionTextBox" InitialValue=""
                 SetFocusOnError="true" ErrorMessage="*required" runat="server"></asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="AttributeValueLabel" runat="server" >Value:</asp:Label><br />
                <asp:TextBox ID="AttributeValueTextBox" TextMode="MultiLine" Rows="5" Width="350px" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFV3" CssClass="error" EnableClientScript="true" ControlToValidate="AttributeValueTextBox" InitialValue=""
                 SetFocusOnError="true" ErrorMessage="*required" runat="server"></asp:RequiredFieldValidator>
            </p>
             <p class="saveButton">
                <asp:LinkButton ID="SaveButton" runat="server" OnClick="SaveButton_Click"  Text="Save System Attribute" />&nbsp;
                <a href="SystemAttributeList.aspx">Manage System Attribute</a>
             </p>
        </fieldset>
</asp:Content>
