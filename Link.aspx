<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Link.aspx.cs" Inherits="USATodayBookList.Link" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1>Edit Research Link</h1>
        <fieldset >
            <legend>Link Information</legend>
            <p>
                <asp:Label ID="DescriptionLabel" runat="server" >Description:</asp:Label><br />
                <asp:TextBox ID="DescriptionTextBox" runat="server" CssClass="textEntry"></asp:TextBox>&nbsp;
                <asp:RequiredFieldValidator ID="RFV1" CssClass="error" EnableClientScript="true" ControlToValidate="DescriptionTextBox" InitialValue=""
                 SetFocusOnError="true" ErrorMessage="*required" runat="server"></asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="URLLabel" runat="server" >URL:</asp:Label><br />
                <asp:TextBox ID="URLTextBox" TextMode="MultiLine" Rows="5" Width="90%" runat="server" CssClass="textEntry"></asp:TextBox>&nbsp;
                <asp:RequiredFieldValidator ID="RFV2" CssClass="error" EnableClientScript="true" ControlToValidate="URLTextBox" InitialValue=""
                 SetFocusOnError="true" ErrorMessage="*required" runat="server"></asp:RequiredFieldValidator>                
            </p>
            
             <p class="saveButton">
                    <asp:LinkButton ID="SaveButton" runat="server" OnClick="SaveButtonClick" Text="Save Link" />&nbsp;
                     <a href="LinkList.aspx">Manage Links</a>
             </p>
            
          
        </fieldset>
</asp:Content>
