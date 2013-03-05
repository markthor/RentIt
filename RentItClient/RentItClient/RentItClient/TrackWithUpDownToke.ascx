<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackWithUpDownToke.ascx.cs" Inherits="RentItClient.TrackWithUpDownToke" %>

<div style="float:left; width:80%">
    <asp:Label ID="lbl_trackName" runat="server" Text="Arctic Monkeys - You Probably Couldn't See for the" style="float:right; width:240px; font-family:monospace" />
</div>
<div style="float:right; width:18%;">
    <asp:ImageButton ID="img_uptokeTrack1" runat="server" ImageUrl="~/Images/UpToke-25x25.jpg" CssClass="upDownToke" />
    <asp:ImageButton ID="img_downtokeTrack1" runat="server" ImageUrl="~/Images/DownToke-25x25.jpg" CssClass="upDownToke" />
</div>