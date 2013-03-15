<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopBar.ascx.cs" Inherits="RentItClient.TopBar" %>
<%@ Register TagPrefix="userControl" TagName="logo" Src="~/Logo.ascx" %>
<%@ Register TagPrefix="userControl" TagName="searchBar" Src="~/SearchBar.ascx" %>
<%@ Register TagPrefix="userControl" TagName="userInfo" Src="~/UserInfo.ascx" %>

<link rel="stylesheet" type="text/css" href="RentItStyle.css" />

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
