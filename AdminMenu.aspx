<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminMenu.aspx.cs" Inherits="USATodayBookList.AdminMenu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h1>Data Administration Menu</h1>
    <a href="AuthorList.aspx">Manage Authors</a><br /> 
    <a href="BookList.aspx">Manage Books</a><br /> 
    <a href="CategoryList.aspx">Manage Categories</a><br /> 
    <a href="KeywordList.aspx">Manage Keywords</a><br /> 
    <a href="MediaFormat.aspx">Manage Media Formats</a><br />
    <a href="ISBNList.aspx">Manage ISBNs</a><br />
    <a href="ProviderList.aspx">Manage Providers</a><br />
    <a href="PublisherList.aspx">Manage Publishers</a><br />
    <a href="ImprintList.aspx">Manage Imprints</a><br />
    <a href="LinkList.aspx">Manage Research Links</a><br />
    <a href="ChannelTypeList.aspx">Manage Channel Types</a><br />
    <a href="SystemAttributeList.aspx">Manage System Attributes</a><br />
    <a href="ImportLog.aspx">Import Log</a><br />
    <a href="Reports.aspx">Reports</a><br />
    <a href="ResetWorkflow.aspx">Reset Workflow</a>
</asp:Content>
