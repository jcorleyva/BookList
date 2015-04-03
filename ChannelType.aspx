<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ChannelType.aspx.cs" Inherits="USATodayBookList.ChannelType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1>Edit Channel Type</h1>
        <fieldset>
            <legend>Channel Type Information</legend>
            <p>
                <asp:Label ID="ChannelTypeLabel" runat="server" >Channel Type:</asp:Label><br />
                <asp:TextBox ID="ChannelTypeTextBox" runat="server" CssClass="textEntry"></asp:TextBox>&nbsp;
                <asp:RequiredFieldValidator ID="RFV1" CssClass="error" EnableClientScript="true" ControlToValidate="ChannelTypeTextBox" InitialValue=""
                 SetFocusOnError="true" ErrorMessage="*required" runat="server"></asp:RequiredFieldValidator>

            </p>
            <p>
                <asp:Label ID="WeightedSalesLabel" runat="server" >Weighted Sales:</asp:Label><br />
                <asp:TextBox ID="WeightedSalesTextBox" runat="server" CssClass="textEntry"></asp:TextBox>&nbsp;
                <asp:RequiredFieldValidator ID="RFV2" CssClass="error" EnableClientScript="true" ControlToValidate="WeightedSalesTextBox" InitialValue=""
                 SetFocusOnError="true" ErrorMessage="*required" runat="server"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="REV1" CssClass="error" EnableClientScript="true" ControlToValidate="WeightedSalesTextBox" InitialValue=""
                 SetFocusOnError="true" ErrorMessage="*invalid number" ValidationExpression="\d+(\.\d+)?" runat="server"></asp:RegularExpressionValidator>
            </p>
             <p class="saveButton">
                    <asp:LinkButton ID="SaveButton" runat="server" OnClick="SaveButtonClick"  Text="Save Channel Type" />
                    &nbsp;&nbsp;
                    <a href="ChannelTypeList.aspx">Manage Channel Types</a>
             </p>
        </fieldset>
</asp:Content>
