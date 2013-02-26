<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="RentItClient.Home" %>
<%@ Register TagPrefix="userControl" TagName="searchBar" Src="~/SearchBar.ascx"%>
<%@ Register TagPrefix="userControl" TagName="userInfo" Src="~/UserInfo.ascx" %>
<%@ Register TagPrefix="userControl" TagName="logo" Src="~/Logo.ascx" %>
<%@ Register TagPrefix="userControl" TagName="searchResult" Src="~/SearchResult.ascx" %>
<%@ Register TagPrefix="userControl" TagName="ChannelDescription" Src="~/ChannelDescription.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="RentItStyle.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="div_logo" class="logo">
            <userControl:logo ID="logo" runat="server"/>
        </div>
        <div id="div_search" class="search">
            <div class="centered" style="margin: 10px 0px 0px;">
                <userControl:searchBar id="searchBar" runat="server" />    
            </div>
        </div>
        <div id="div_userInfo" class="userInfo">
            <div class="centered" style="margin: 10px 0px 0px;">
                <userControl:userInfo id="userInfo" runat="server" />
            </div>
        </div>
        <div id="div_streamPlayer" class="streamPlayer">
            <p>StreamPlayer</p>
        </div>
        <div id="div_homeScreen" runat="server" class="homeScreen">
            <!--<userControl:searchResult id="searchResult" runat="server" />-->
            <userControl:ChannelDescription ID="channelDescription" runat="server" />
        </div>
        <div id="div_subscriptions" class="subscriptions">
            
        </div>
    </form>
</body>
</html>
<%--  --%>