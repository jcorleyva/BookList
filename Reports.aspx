<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="USATodayBookList.Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function btnPubllishClick() {
            var divMessage = document.getElementById("MainContent_divMessage");
            if (confirm("Are you sure you want to publish this report?")) {
                divMessage.innerHTML = "Please wait...<br/>Sending data...";
                event.returnValue = true;
            }
            else {
                event.returnValue = false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Reports</h1>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width:65%">
                <asp:GridView ID="ReportsGridView" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" ForeColor="#333333" GridLines="None" 
                    OnRowDataBound="ReportsGridView_RowDataBound">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField HeaderText="List of Reports">
                            <ItemTemplate>
                                <asp:HyperLink ID="Top5HyperLink" runat="server" 
                                    NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "ReportUrl") %>'
                                    Target="Top5">
                                    <%# DataBinder.Eval(Container.DataItem, "ReportDisplayName")%>
                                    </asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle Width="300px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnPublish" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "ButtonText") %>' 
                                CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ReportName") %>' 
                                OnClick="btnPublish_Click" OnClientClick="btnPubllishClick();" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="150px" />
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>            
            </td>
            <td valign="top" style="padding-left:4px; color:Red">
                <div id="divMessage" runat="server"></div>
            </td>
        </tr>        
    </table>
</asp:Content>
