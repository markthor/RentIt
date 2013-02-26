<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchResult.ascx.cs" Inherits="RentItClient.SearchResult" %>
<asp:Table ID="Table1" Width="100%" runat="server">
    <asp:TableRow>
        <asp:TableCell Width="50%">
            <div id="div_channelName">
                <h1>ChannelName</h1>
            </div>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lbl_numberOfPlays" runat="server" Text="Plays: 123.234.756" />
            <br />
            <asp:Label ID="lbl_numberOfSubscribers" runat="server" Text="Subscribers: 184.933" />
        </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <div id="div_description">
                <p>Description</p>
            </div>
        </asp:TableCell>
        <asp:TableCell>
            <asp:Label ID="lbl_genres" runat="server" Text="Genres:" />
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>

