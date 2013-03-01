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
        <div style="margin-bottom: 8px;">
            <asp:Label ID="lbl_channelName" runat="server" Text="Channel name" CssClass ="loginLabel"></asp:Label>
            <asp:TextBox ID="tbx_channelName" runat="server" CssClass="loginTextbox"></asp:TextBox>
        </div>
    <div class="channelTextMargin">
        <div class="genreBox">
            <div class="genreBox">
                <asp:Label ID="Label1" runat="server" Text="Available genres" CssClass="loginLabel"></asp:Label>
                <asp:ListBox ID="lb_genrelist" runat="server" Width="200px" CssClass="genreBox" ViewStateMode="Enabled"></asp:ListBox>
            </div>

            <div class="genreBox">
                <asp:Label ID="Label2" runat="server" Text="Genres" CssClass="loginLabel"></asp:Label>
                <asp:ListBox ID="lb_genres" runat="server" Width="193px" CssClass="genreBox" ViewStateMode="Enabled"></asp:ListBox>
            </div>
        </div> 
            <asp:Button ID="btn_addGenre" runat="server" Text="Add" CssClass="genreBtn" OnClick="btn_addGenre_Click"/>
            <asp:Button ID="btn_deleteGenre" runat="server" Text="Remove" CssClass="genreBtn" OnClick="btn_deleteGenre_Click" />
    </div>
    <div class="channelTextMargin">
        <asp:Label ID="lbl_channelDescription" runat="server" Text="Description" CssClass="loginLabel"></asp:Label>
        <div class="genreBox">
            <asp:TextBox ID="TextBox1" runat="server" Rows="5" TextMode="MultiLine" Width="405px" Height="201px" MaxLength="150" Style="margin:10px;"></asp:TextBox>
        </div>
    </div>
    </form>
</body>
</html>
