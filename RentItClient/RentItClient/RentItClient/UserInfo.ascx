<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserInfo.ascx.cs" Inherits="RentItClient.UserInfo" %>
<asp:Button ID="btn_createChannel" runat="server" Text="Create Channel" OnClick="btn_createChannel_Click"/>
<asp:Label ID="lbl_loggedInUser" runat="server" Text="LoggedInUser" />
<asp:Button ID="btn_logout" runat="server" Text="Logout" OnClick="btn_logout_Click"/>