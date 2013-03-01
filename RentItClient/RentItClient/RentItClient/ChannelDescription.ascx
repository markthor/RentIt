<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChannelDescription.ascx.cs" Inherits="RentItClient.ChannelDescription" %>

<link rel="stylesheet" type="text/css" href="RentItStyle.css" />

<div id="div_channelName" style="text-align:center">
    <h1>Channel Name</h1>
</div>
<div style="float:left; text-align:right; width:50%;">
    <div style="margin-right:5px">
        <asp:Label ID="lbl_plays" runat="server" Text="Plays:" />
        <br />
        <asp:Label ID="lbl_uptokes" runat="server" Text="Uptokes:" />
        <br />
        <asp:Label ID="lbl_downtokes" runat="server" Text="Downtokes:" />
    </div>
</div>
<div style="float:left; text-align:left; width:50%">
    <div style="margin-left:5px">
        <asp:Label ID="lbl_numberOfPlays" runat="server" Text="123.234.346" />
        <br />
        <asp:Label ID="lbl_numberOfUptokes" runat="server" Text="173.726.258" />
        <br />
        <asp:Label ID="lbl_numberOfDowntokes" runat="server" Text="173.726.258" />
    </div>
</div>
<div>
    <h3>Description</h3>
    <p>
        Lorem ipsum dolor sit amet, consectetur adipiscing elit.
        Nam iaculis eros a est luctus eget pretium justo semper.
        Donec vel dui vitae elit tincidunt facilisis.
        Aliquam eget risus ut tortor egestas adipiscing.
        Pellentesque egestas enim vitae ligula faucibus vulputate.
        Pellentesque eu eros mauris, non placerat urna.
        Praesent egestas risus sed lacus bibendum laoreet. Nunc ac tempus dolor.
        Sed id neque eu elit feugiat rutrum fermentum in magna. In congue tortor a dui viverra vestibulum.
    </p>
</div>
<div>
    <h3>Tracklist</h3>
    <asp:ListBox ID="lbx_tracklist" runat="server" Width="100%" Rows="10"/>
</div>
<div>
    <h3>Genrelist</h3>
    <asp:ListBox ID="lbx_genrelist" runat="server" Width="100%" Rows="5" />
</div>
<div>
    <asp:Button ID="btn_play" runat="server" Text="Play" />
    <asp:Button ID="btn_subscribe" runat="server" Text="Subscribe" />
</div>