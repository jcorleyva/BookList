<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetWorkflow.aspx.cs" Async="true" AsyncTimeout="2" Inherits="USATodayBookList.ResetWorkflow" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">
    function ArchiveConfirm() {

        if (confirm("Are you sure you want to proceed with Archive?"))
            event.returnValue = true;
        else
            event.returnValue = false;

    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Reset Workflow</h1>
    <br />
    <asp:Button ID="btnArchive" Text="Archive Booklist" OnClientClick="ArchiveConfirm();" OnClick="btnArchive_Click" runat="server" />
    <br />
    <span id="spanStatus" runat="server"></span>
</asp:Content>
