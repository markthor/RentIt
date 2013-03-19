<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="RentItClient.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RentIt Login</title>
    <link rel="stylesheet" type="text/css" href="RentItStyle.css" />
</head>
<body>
    <form id="form1" runat="server">
            <h1 id="loginTitle">RentIt</h1>
        <div class="labelTextMargin">
            <asp:Label ID="lbl_loginTitle" runat="server" Text="Login" CssClass="loginTitle"></asp:Label>
            <div class="clear">       
                <asp:Label ID="lbl_username" runat="server" Text="Username" CssClass="loginLabel" Width="70px"></asp:Label>
                <asp:TextBox ID="tbx_username" runat="server" CssClass="loginTextbox"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_username" runat="server" ControlToValidate="tbx_username" ErrorMessage="*" ForeColor="Red" />
            </div>
            <div class="clear">
                <asp:Label ID="lbl_password" runat="server" Text="Password" CssClass="loginLabel" Width="70px"></asp:Label>           
                <asp:TextBox ID="tbx_password" runat="server" CssClass="loginTextbox" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_password" runat="server" ControlToValidate="tbx_password" ErrorMessage="*" ForeColor="Red" />
            </div>
            <asp:Button ID="btn_login" runat="server" Text="Login" OnClick="btn_login_Click" />
        </div> 
    <div class="labelTextMargin">
        <asp:Label ID="lbl_createUserTitle" runat="server" Text="Create User" CssClass="loginTitle"></asp:Label>
        <div class="clear">
            <asp:Label ID="lbl_createUsername" runat="server" Text="Username" CssClass="loginLabel" Width="70px"></asp:Label>
            <asp:TextBox ID="tbx_createUsername" runat="server" CssClass="loginTextbox"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfv_createUsername" runat="server" ControlToValidate="tbx_createUsername" ErrorMessage="*" ForeColor="Red" />
        </div>
        <div class="clear">
            <asp:Label ID="lbl_createPassword" runat="server" Text="Password" CssClass ="loginLabel"  Width="70px"></asp:Label>
            <asp:TextBox ID="tbx_createPassword" runat="server" CssClass="loginTextbox" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfv_createPassword" runat="server" ControlToValidate="tbx_createPassword" ErrorMessage="*" ForeColor="Red" />
        </div>
        <div class="clear">
            <asp:Label ID="lbl_confirmPassword" runat="server" Text="(Confirm)" CssClass ="loginLabel" Width="70px"></asp:Label>
            <asp:TextBox ID="tbx_confirmPassword" runat="server" CssClass="loginTextbox" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfv_confirmPassword" runat="server" ControlToValidate="tbx_confirmPassword" ErrorMessage="*" ForeColor="Red" />
            <asp:CompareValidator ID="cmp_passwords" runat="server" ControlToValidate="tbx_createPassword" ControlToCompare="tbx_confirmPassword" Operator="Equal" Type="String" ErrorMessage="The two passwords must be equal!" ForeColor="Red" />
        </div>
        <div class="clear">
            <asp:Label ID="lbl_email" runat="server" Text="E-mail" CssClass="loginLabel" Width="70px"></asp:Label>
            <asp:TextBox ID="tbx_email" runat="server" CssClass="loginTextbox"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfv_email" runat="server" ControlToValidate="tbx_email" ErrorMessage="*" ForeColor="Red" />
            <asp:RegularExpressionValidator ID="rev_email" runat="server" ControlToValidate="tbx_email" ErrorMessage="Invalid email!" ForeColor="Red" ValidationExpression="[\w\d\._-]+@[\w\d\._-]+\.[\w]+" />
        </div>
        <asp:Button ID="btn_createUser" runat="server" Text="Create user" OnClick="btn_createUser_Click" />
    </div>
    </form>
</body>
</html>
