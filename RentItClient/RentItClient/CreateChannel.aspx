<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateChannel.aspx.cs" Inherits="RentItClient.CreateChannel" %>
<%@ Register TagPrefix="userControl" TagName="topBar" Src="~/TopBar.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="RentItStyle.css" />
</head>
<body>
    <form runat="server">
        <div>
            <userControl:topBar ID="topBar" runat="server" />
        </div>
        <div id="div_createChannelTitle" class="centered">
            <h1>Create Channel</h1>
        </div>
        <div class="centered">
            <asp:Label ID="lbl_channelName" runat="server" Text="Channel name" CssClass ="loginLabel"></asp:Label>
            <asp:TextBox ID="tbx_channelName" runat="server" CssClass="loginTextbox"></asp:TextBox>     
        </div>
        <div class="centered">
            <asp:Button ID="btn_createChannel" runat="server" Text="Create" OnClick="btn_createChannel_Click" />
            <asp:Button ID="btn_cancelCreateChannel" runat="server" Text="Cancel" OnClick="btn_cancelCreateChannel_Click" />
        </div>
    </form>
</body>
</html>
