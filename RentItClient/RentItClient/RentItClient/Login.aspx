<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="RentItClient.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="RentItStyle.css" />
    <title>RentIt Login</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login">
            <asp:Label ID="lbl_username" runat="server" Text="Username"></asp:Label>
        </div>
        <div>
            <asp:TextBox ID="tbx_username" runat="server" Height="16px"></asp:TextBox>
        </div>
    </form>
</body>
</html>
