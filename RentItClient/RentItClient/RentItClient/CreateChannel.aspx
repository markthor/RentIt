<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateChannel.aspx.cs" Inherits="RentItClient.CreateChannel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="RentItStyle.css" />
</head>
<body class="createBody">
    <form id="form1" runat="server">
        <h1 id="loginTitle">Create Channel</h1>
    <div class="channelTextMargin">
        <asp:Label ID="lbl_loginTitle" runat="server" Text="Create Channel" CssClass="loginTitle"></asp:Label>
            <div class="clear">
                <asp:Label ID="lbl_channelName" runat="server" Text="Channel name" CssClass ="loginLabel"></asp:Label>
                <asp:TextBox ID="tbx_channelName" runat="server" CssClass="loginTextbox"></asp:TextBox>     
            </div>
        <asp:Button ID="btn_createChannel" runat="server" Text="Create" OnClick="btn_createChannel_Click" />
    </div>
    </form>
</body>
</html>
