<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChannelDescription.ascx.cs" Inherits="RentItClient.ChannelDescription" %>

<link rel="stylesheet" type="text/css" href="RentItStyle.css" />

<div id="div_channelName" class="centered">
    <h1>Channel Name</h1>
</div>
    <asp:Table ID="tbl_main" runat="server">
        <asp:TableRow>
            <asp:TableCell ID="tbc_description" Width="65%">
                <p>
                    Some text. Some text. Some text. Some text. Some text. Some text. Some text. Some text. 
                    Some text. Some text. Some text. Some text. Some text. Some text. Some text. Some text. 
                    Some text. Some text. Some text. Some text. Some text. Some text. Some text. Some text. 
                    Some text. Some text. Some text. Some text. Some text. Some text. Some text. Some text. 
                </p>
            </asp:TableCell>
            <asp:TableCell Width="35%">
                <asp:Table ID="tbl_playsUptokesDowntokes" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label runat="server" Text="Plays:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lbl_numberOfPlays" runat="server" Text="123.234.346" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label runat="server" Text="Uptokes:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lbl_numberOfUptokes" runat="server" Text="173.726.258" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label runat="server" Text="Downtokes:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lbl_numberOfDowntokes" runat="server" Text="173.726.258" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <h3>Tracklist</h3>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:ListBox ID="lbx_tracklist" runat="server" width="100%"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <h3>Genrelist</h3>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:ListBox ID="lbx_genrelist" runat="server" Width="100%" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button ID="btn_play" runat="server" Text="Play" />
                <asp:Button ID="btn_subscribe" runat="server" Text="Subscribe" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>