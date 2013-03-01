<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditChannel.aspx.cs" Inherits="RentItClient.EditChannel" %>
<%@ Register TagPrefix="userControl" TagName="topBar" Src="~/TopBar.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RentIt - Edit Channel</title>
    <link rel="stylesheet" type="text/css" href="RentItStyle.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <userControl:topBar ID="topBar" runat="server" />
        </div>
        <div class="centered">
            <h1>Edit Channel</h1>
            <asp:Label runat="server" Text="Channel name:" CssClass="loginLabel"></asp:Label>
            <asp:Label ID="lbl_channelName" runat="server" CssClass="loginLabel" Text=""></asp:Label>
        </div>
        <br />
        <div class="channelTextMargin">
            <div style="float:left; width:50%">
                <asp:Label ID="Label3" runat="server" Text="Available genres" CssClass="loginLabel"></asp:Label>
                <asp:ListBox ID="lbx_availableGenres" runat="server" Width="95%" CssClass="genreBox" ViewStateMode="Enabled"></asp:ListBox>
            </div>
            <div style="float:left; width:50%">
                <asp:Label ID="Label4" runat="server" Text="Chosen genres" CssClass="loginLabel"></asp:Label>
                <asp:ListBox ID="lbx_chosenGenres" runat="server" Width="95%" style="float:right" CssClass="genreBox" ViewStateMode="Enabled"></asp:ListBox>
            </div>
            <div style="float:left; width:50%">
                <asp:Button ID="btn_addGenre" runat="server" Text="Add" style="float:right;margin-right:15px" CssClass="genreBtn" OnClick="btn_addGenre_Click"/>
            </div>
            <div style="float:left; width:50%">
                <asp:Button ID="btn_deleteGenre" runat="server" Text="Remove" style="float:left;margin-left:15px" CssClass="genreBtn" OnClick="btn_deleteGenre_Click" />
            </div>
            
        </div>
        <div class="channelTextMargin">
            <h3>Description</h3>
            <asp:TextBox ID="tbx_description" runat="server" Text="" Rows="8" TextMode="MultiLine" Width="100%"/>
        </div>
        <div class="channelTextMargin">
            <asp:Button ID="btn_saveChanges" runat="server" Text="Save Changes" OnClick="btn_saveChanges_Click" />
            <asp:Button ID="btn_discardChanges" runat="server" Text="Discard Changes" OnClick="btn_discardChanges_Click" />
        </div>
        
    </form>
</body>
</html>
