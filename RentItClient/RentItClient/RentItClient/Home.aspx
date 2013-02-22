<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="RentItClient.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="RentItStyle.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="logo" class="logo">
            <asp:ImageButton ID="img_Logo" runat="server" ImageUrl="~/Images/Logo-300x50.jpg" OnClick="img_Logo_Click"/>
        </div>
        <div id="search" class="search">
            <asp:TextBox ID="tbx_searchBar" runat="server" width="63%" CssClass="element"/>
            <asp:Button ID="btn_search" runat="server" Text="Search" CssClass="element"/>    
            <asp:Button ID="btn_advancedSearch" runat="server" Text="Adv. search" CssClass="element"/>
        </div>
        <div id="userInfo" class="userInfo">
            <asp:Button ID="btn_createChannel" runat="server" Text="Create Channel" />
            <asp:Label ID="lbl_loggedInUser" runat="server" Text="LoggedInUser" />
            <asp:Button ID="btn_logout" runat="server" Text="Logout" />
        </div>
        <div id="streamPlayer" class="streamPlayer">
            <p>StreamPlayer</p>
        </div>
        <div id="homeScreen" runat="server" class="homeScreen">
            <p>HomeScreen</p>
        </div>
        <div id="subscriptions" class="subscriptions">
            
        </div>
    </form>
</body>
</html>
<%--  --%>