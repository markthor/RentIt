<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StreamPlayer.ascx.cs" Inherits="RentItClient.StreamPlayer" %>
<%@ Register TagPrefix="UC" TagName="TrackWithUpDownToke" Src="~/TrackWithUpDownToke.ascx" %>

<div class="centered">
    <h1>Stream Player</h1>
</div>
<div class="centered">
    <asp:ImageButton ID="img_playPause" runat="server" ImageUrl="~/Images/play_pause_24x24.png"/>
</div>

<br /><br />

<UC:TrackWithUpDownToke ID="uc_track1" runat="server" />
<br /><br />
<UC:TrackWithUpDownToke ID="uc_track2" runat="server" />
<br /><br />
<UC:TrackWithUpDownToke ID="uc_track3" runat="server" />
<br /><br />
<UC:TrackWithUpDownToke ID="uc_track4" runat="server" />
<br /><br />
<UC:TrackWithUpDownToke ID="uc_track5" runat="server" />