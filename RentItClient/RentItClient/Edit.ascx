<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Edit.ascx.cs" Inherits="RentItClient.Edit" %>

<link rel="stylesheet" type="text/css" href="RentItStyle.css" />

<div class="centered">
    <h1>Edit Channel</h1>
    <asp:Label ID="Label1" runat="server" Text="Channel name:" CssClass="loginLabel"></asp:Label>
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
    <h3 style="margin-top: 20px;">Track List</h3>
    <asp:ListBox ID="lbx_trackList" runat="server" Width="95%" CssClass="genreBox" ViewStateMode="Enabled"></asp:ListBox>
</div>

<div class="channelTextMargin">
    <h3 style="margin-top: 20px;">Upload File</h3>
    <asp:FileUpload ID="flu_trackUpload" runat="server" />
    <br />
    <asp:Button ID="btn_uploadSong" runat="server" Text="Upload track" Style="margin-top: 5px;" Onclick="btn_uploadSong_Click"/>
</div>
<div class="channelTextMargin">
    <asp:Button ID="btn_saveChanges" runat="server" Text="Save Changes" OnClick="btn_saveChanges_Click" />
    <asp:Button ID="btn_discardChanges" runat="server" Text="Discard Changes" OnClick="btn_discardChanges_Click" />
</div>