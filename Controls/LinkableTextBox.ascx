<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LinkableTextBox.ascx.cs" Inherits="USATodayBookList.Controls.LinkableTextBox" %>
<script type="text/javascript">
    function show(id)
    {
        var div = document.getElementById(id);
        if (div.style.display == 'block')
            div.style.display = 'none';
        else
            div.style.display = 'block';       
    }
</script>
<div id="divMain" style="float:left; cursor:hand; padding-left:4px;" runat="server" >
    <asp:Label ID="labelLink" ForeColor="Blue" Font-Underline="true" Text="" runat="server"></asp:Label>
<div id="divTextBox" style="background-color:Beige; border:1px solid black; width:163px; margin-top:2px; padding: 5px 0 0 5px; height:40px; float:left; position:absolute; display:none;" runat="server">
    <asp:TextBox ID="txtbox" runat="server"></asp:TextBox>&nbsp;
    <div style="float:left; width:163px;">
        <asp:RequiredFieldValidator ID="RFV1" ControlToValidate="txtbox" ForeColor="Red" EnableClientScript="true" SetFocusOnError="true" runat="server" ></asp:RequiredFieldValidator>
        <span style="margin-left:-60px; position:absolute;">
            <asp:CustomValidator ID="CV1" ForeColor="Red" EnableClientScript="true" SetFocusOnError="true" Visible="false" runat="server"></asp:CustomValidator>
        </span>
        <span style="float:right; padding-right:8px;">
            <asp:LinkButton ID="btnSave" Text="Save" runat="server"></asp:LinkButton>
        </span>
    </div>
</div>
</div>
 
