<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopBarTest.ascx.cs" Inherits="RentItClient.TopBarTest" %>

<link rel="stylesheet" type="text/css" href="RentItStyle.css" />

<div id="div_logo" class="logo">
    <asp:ImageButton ID="img_logo" runat="server" ImageUrl="~/Images/Logo-300x50.jpg" OnClick="img_Logo_Click" />
</div>
<div id="div_search" class="search">
    <div class="centered" style="margin: 10px 0px 0px;">
        <asp:TextBox ID="tbx_searchField" runat="server" width="300px"/>
        <asp:Button ID="btn_search" runat="server" Text="Search" OnClick="btn_search_Click" />
        <asp:Button ID="bth_searchAdvanced" runat="server" Text="Adv. Search" OnClick="bth_searchAdvanced_Click" />
    </div>
</div>
<div id="div_userInfo" class="userInfo">
    <div class="centered" style="margin: 10px 0px 0px;">
        <asp:Button ID="btn_createChannel" runat="server" Text="Create Channel" OnClick="btn_createChannel_Click" />
        <asp:Label ID="lbl_loggedInUser" runat="server" Text="LoggedInUser" />
        <asp:Button ID="btn_logout" runat="server" Text="Logout" OnClick="btn_logout_Click" />
    </div>
</div>