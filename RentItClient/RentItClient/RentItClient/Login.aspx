<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="RentItClient.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RentIt Login</title>
    <link rel="stylesheet" type="text/css" href="RentItStyle.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1 id="loginTitle">Login</h1>
        </div>
        <div class="labelTextMargin">
            <asp:Label ID="lbl_loginTitle" runat="server" Text="Login" CssClass="loginTitle"></asp:Label>
            <div class="clear">       
                <asp:Label ID="lbl_username" runat="server" Text="Username" CssClass="loginLabel"></asp:Label>
                <asp:TextBox ID="tbx_username" runat="server" CssClass="loginTextbox"></asp:TextBox> 
            </div>
            <div class="clear">
                <asp:Label ID="lbl_password" runat="server" Text="Password" CssClass="loginLabel"></asp:Label>           
                <asp:TextBox ID="tbx_password" runat="server" CssClass="loginTextbox"></asp:TextBox>
            </div>
            <asp:Button ID="btn_login" runat="server" Text="Login" OnClick="btn_login_Click" />
        </div> 
    <div class="labelTextMargin">
        <asp:Label ID="lbl_createUserTitle" runat="server" Text="Create User" CssClass="loginTitle"></asp:Label>
        <div class="clear">
            <asp:Label ID="lbl_createUsername" runat="server" Text="Username" CssClass="loginLabel" Width="70px"></asp:Label>
            <asp:TextBox ID="tbx_createUsername" runat="server" CssClass="loginTextbox"></asp:TextBox>
        </div>
        <div class="clear">
            <asp:Label ID="lbl_createPassword" runat="server" Text="Password" CssClass ="loginLabel"  Width="70px"></asp:Label>
            <asp:TextBox ID="tbx_createPassword" runat="server" CssClass="loginTextbox" TextMode="Password"></asp:TextBox>
        </div>
        <div class="clear">
            <asp:Label ID="lbl_confirmPassword" runat="server" Text="(Confirm)" CssClass ="loginLabel" Width="70px"></asp:Label>
            <asp:TextBox ID="tbx_confirmPassword" runat="server" CssClass="loginTextbox" TextMode="Password"></asp:TextBox>
        </div>
        <div class="clear">
            <asp:Label ID="lbl_email" runat="server" Text="E-mail" CssClass="loginLabel" Width="70px"></asp:Label>
            <asp:TextBox ID="tbx_email" runat="server" CssClass="loginTextbox"></asp:TextBox>
        </div>
        <asp:Button ID="btn_createUser" runat="server" Text="Create user" OnClick="btn_createUser_Click" />
        
    </div>
    </form>
</body>
</html>