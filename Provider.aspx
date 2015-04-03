<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Provider.aspx.cs" Inherits="USATodayBookList.Provider" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1>Edit Provider</h1>
    <div >
        <fieldset >
            <legend>Provider Information</legend>
            <p>
                <asp:Label ID="ProviderNameLabel" runat="server" >Provider Name:</asp:Label><br />
                <asp:TextBox ID="ProviderNameTextBox" runat="server" CssClass="textEntry"></asp:TextBox>                
                <asp:RequiredFieldValidator ID="RFV1" CssClass="error" EnableClientScript="true" ControlToValidate="ProviderNameTextBox" InitialValue=""
                             SetFocusOnError="true" ErrorMessage="*required" runat="server"></asp:RequiredFieldValidator>
            </p>
            <p>
                <asp:Label ID="ContactLabel" runat="server" >Contact Person:</asp:Label><br />
                <asp:TextBox ID="ContactTextBox" runat="server" CssClass="textEntry"></asp:TextBox>               
            </p>
            <p>
                <asp:Label ID="PhoneLabel" runat="server" >Phone:</asp:Label><br />
                <asp:TextBox ID="PhoneTextBox" runat="server" CssClass="textEntry"></asp:TextBox>               
            </p>
            <p>
                <asp:Label ID="EmailLabel" runat="server" >Email:</asp:Label><br />
                <asp:TextBox ID="EmailTextBox" runat="server" CssClass="textEntry"></asp:TextBox>               
            </p>
            <p>
                <asp:Label ID="ActiveLabel" runat="server" >Active:</asp:Label>&nbsp;
                <asp:CheckBox ID="ActiveCheckBox" runat="server" />
            </p>
            <p>
                <asp:Label ID="Label1" runat="server" >Channel Type:</asp:Label><br />
                <asp:DropDownList ID="ChannelTypeDropDownList" runat="server"></asp:DropDownList>
            </p>
            <p>
                <asp:Label ID="FileFormatLabel" runat="server" >File Format:</asp:Label><br />
                <asp:DropDownList ID="FileFormatDropDownList" runat="server">
                    <asp:ListItem Text="Delimited" Value="tsv"></asp:ListItem>
                    <asp:ListItem Text="Excel" Value="xls"></asp:ListItem>
                    <asp:ListItem Text="Fixed Field" Value="txt"></asp:ListItem>
                </asp:DropDownList>
            </p>
            <p>
                <asp:Label ID="RawFileNameLabel" runat="server" >Raw File Name:</asp:Label><br />
                <asp:TextBox ID="RawFileNameTextBox" runat="server" CssClass="textEntry"></asp:TextBox>               
            </p>
            <p>
                <asp:Label ID="MemoLabel" runat="server" >Notes:</asp:Label><br />
                <asp:TextBox ID="MemoTextBox" runat="server" CssClass="largeTextEntry"  TextMode="MultiLine"  Width="400" Height="80"></asp:TextBox>               
            </p>
            <p>
                <asp:Label ID="ImportNotesLabel" runat="server" >ImportNotes:</asp:Label><br />
                <asp:TextBox ID="ImportNotestTextBox" runat="server" CssClass="largeTextEntry"  TextMode="MultiLine"  Width="400" Height="80"></asp:TextBox>               
            </p>
             <p class="saveButton">
                        <asp:LinkButton ID="SaveButton" runat="server" OnClick="SaveButton_Click"  Text="Save Provider" />&nbsp;
                        <a href="ProviderList.aspx">Manage Providers</a>
             </p>          
        </fieldset>
    </div>
    
</asp:Content>
