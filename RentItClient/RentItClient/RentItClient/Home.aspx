<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="RentItClient.Home" %>
<%@ Register TagPrefix="userControl" TagName="searchResult" Src="~/SearchResult.ascx" %>
<%@ Register TagPrefix="userControl" TagName="channelDescription" Src="~/ChannelDescription.ascx" %>
<%@ Register TagPrefix="userControl" TagName="topBar" Src="~/TopBar.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="RentItStyle.css" />
</head>
<body>
    <form runat="server">
        <div>
            <userControl:topBar ID="topBar" runat="server" />
        </div>
        <div id="div_streamPlayer" class="streamPlayer">
            <p>StreamPlayer</p>
        </div>
        <div id="div_homeScreen" runat="server" class="homeScreen">
            <!--<userControl:searchResult id="searchResult" runat="server" />-->
            <userControl:channelDescription ID="channelDescription" runat="server" />
        </div>
        <div id="div_subscriptions" class="subscriptions">
            
        </div>
    </form>
</body>
</html>