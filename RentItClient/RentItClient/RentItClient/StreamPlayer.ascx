<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StreamPlayer.ascx.cs" Inherits="RentItClient.StreamPlayer" %>

<div class="centered">
    <h1>Stream Player</h1>
</div>
<div class="centered">
    <asp:ImageButton ID="img_playPause" runat="server" ImageUrl="~/Images/play_pause_24x24.png"/>
</div>


<div id="div_track1" style="width:100%">
    <div style="float:left; width:75%">
        <asp:Label ID="lbl_track1" runat="server" Text="Dr. Dre - Still DRE ft. Snoop Dogg" style="float:right; text-overflow:clip; width:50px" />
    </div>
    <div style="width:25%">
        <asp:ImageButton ID="img_uptokeTrack1" runat="server" ImageUrl="~/Images/UpToke-25x25.jpg" CssClass="upDownToke" />
        <asp:ImageButton ID="img_downtokeTrack1" runat="server" ImageUrl="~/Images/DownToke-25x25.jpg" CssClass="upDownToke" />
    </div>
    
    
</div>
<div id="div_track2">
    <asp:Label ID="lbl_track2" runat="server" Text="track2" />
    <asp:ImageButton ID="img_uptokeTrack2" runat="server" ImageUrl="~/Images/UpToke-25x25.jpg" />
    <asp:ImageButton ID="img_downtokeTrack2" runat="server" ImageUrl="~/Images/DownToke-25x25.jpg" />
</div>
<div id="div_track3">
    <asp:Label ID="lbl_track3" runat="server" Text="track3" />
    <asp:ImageButton ID="img_uptokeTrack3" runat="server" ImageUrl="~/Images/UpToke-25x25.jpg" />
    <asp:ImageButton ID="img_downtokeTrack3" runat="server" ImageUrl="~/Images/DownToke-25x25.jpg" />
</div>
<div id="div_track4">
    <asp:Label ID="lbl_track4" runat="server" Text="track4" />
    <asp:ImageButton ID="img_uptokeTrack4" runat="server" ImageUrl="~/Images/UpToke-25x25.jpg" />
    <asp:ImageButton ID="img_downtokeTrack4" runat="server" ImageUrl="~/Images/DownToke-25x25.jpg" />
</div>
<div id="div_track5">
    <asp:Label ID="lbl_track5" runat="server" Text="track5" />
    <asp:ImageButton ID="img_uptokeTrack5" runat="server" ImageUrl="~/Images/UpToke-25x25.jpg" />
    <asp:ImageButton ID="img_downtokeTrack5" runat="server" ImageUrl="~/Images/DownToke-25x25.jpg" />
</div>