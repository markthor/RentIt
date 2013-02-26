<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditChannel.aspx.cs" Inherits="RentItClient.EditChannel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RentIt - Edit Channel</title>
    <link rel="stylesheet" type="text/css" href="RentItStyle.css" />
</head>
<body>
    <form id="form1" runat="server">
        <h1 id="loginTitle">Edit Channel</h1>
    <div class="channelTextMargin">
        <div class="clear">
            <asp:Label ID="lbl_channelName" runat="server" Text="Channel name" CssClass ="loginLabel"></asp:Label>
            <asp:TextBox ID="tbx_channelName" runat="server" CssClass="loginTextbox"></asp:TextBox>
        </div>  
        <div class="genreBox">
            <asp:ListBox ID="lb_genrelist" runat="server" Width="200px" CssClass="genreBox" SelectionMode="Multiple"></asp:ListBox>
            <asp:ListBox ID="lb_genres" runat="server" Width="200px" CssClass="genreBox" SelectionMode="Multiple"></asp:ListBox>
        </div> 
        <asp:Button ID="btn_addGenre" runat="server" Text="Add" OnClick="btn_addGenre_Click"/>    
    </div>
    
    </form>
</body>
</html>
