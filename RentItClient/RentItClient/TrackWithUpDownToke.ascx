<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackWithUpDownToke.ascx.cs" Inherits="TrackWithUpDownToke" %>

<div style="float:left; width:80%">
    <asp:Label ID="lbl_trackName" runat="server" Text="Arctic Monkeys - You Probably Couldn't See for the" style="float:right; width:240px; font-family:monospace" />
</div>
<div style="float:right; width:18%;">
    <asp:Button ID="btn_uptoke" runat="server" Text="Uptoke" />
    <asp:Button ID="btn_downtoke" runat="server" Text="Downtoke" />
</div>