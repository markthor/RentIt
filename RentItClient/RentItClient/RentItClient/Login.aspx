<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="RentItClient.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RentIt Login</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="login">
            <asp:Label ID="LBL_Username" runat="server" Text="Username"></asp:Label>
        </div>
        <div>
            <asp:TextBox ID="TextBox1" runat="server" Height="16px" style="margin-bottom: 0px"></asp:TextBox>           
        </div>  
        
    </div>
        <p>
            &nbsp;</p>
    </form>
</body>
</html>
