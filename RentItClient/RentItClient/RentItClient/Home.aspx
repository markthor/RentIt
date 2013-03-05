<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="RentItClient.Home" %>
<%@ Register TagPrefix="UC" TagName="SearchResult" Src="~/SearchResult.ascx" %>
<%@ Register TagPrefix="UC" TagName="ChannelDescription" Src="~/ChannelDescription.ascx" %>
<%@ Register TagPrefix="UC" TagName="TopBar" Src="~/TopBar.ascx" %>
<%@ Register TagPrefix="UC" TagName="StreamPlayer" Src="~/StreamPlayer.ascx" %>
<%@ Register TagPrefix="UC" TagName="Subscriptions" Src="~/Subscriptions.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="RentItStyle.css" />
</head>
<body>
    <form runat="server">
        <div>
            <UC:TopBar ID="topBar" runat="server" />
        </div>
        <div id="div_streamPlayer" class="streamPlayer">
            <UC:StreamPlayer ID="streamPlayer" runat="server" />
        </div>
        <div id="div_homeScreen" runat="server" class="homeScreen">
            <!--<UC:SearchResult id="searchResult" runat="server" />-->
            <UC:ChannelDescription ID="channelDescription" runat="server" />
        </div>
        <div id="div_subscriptions" class="subscriptions">
            <UC:Subscriptions ID="uc_subscriptions" runat="server" />
        </div>
    </form>
</body>
</html>