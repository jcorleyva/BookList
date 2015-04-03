<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="USATodayBookList._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        USA Today Booklist
    </h2>
    <br />
    <br />
    <br />
    <a href="ImportData.aspx"><h3>Import Data</h3></a><br />

    <a href="ISBNBookMatching.aspx"><h3>ISBN Working List</h3></a><br />
    <a href="BookWorkingList.aspx"><h3>Book Working List</h3></a><br />
        <a href="AdminMenu.aspx"><h3>Data Administration</h3></a><br />
</asp:Content>
