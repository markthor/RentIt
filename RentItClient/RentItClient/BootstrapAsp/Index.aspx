<%@ Page Title="" Language="C#" MasterPageFile="~/MainPageMaster.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="BootstrapAsp.Index" %>
<%@ Register tagPrefix="uc" tagName="StreamPlayer" src="StreamPlayer.ascx" %>

<asp:Content ID="content_head" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="content_up"  ContentPlaceHolderID="up" runat="server">
    
</asp:Content>
<asp:Content ID="content_left" ContentPlaceHolderID="left" runat="server">
    <uc:StreamPlayer ID="uc_streamPlayer" runat="server" />
</asp:Content>
<asp:Content ID="content_center" ContentPlaceHolderID="center" runat="server">
    <uc:StreamPlayer ID="StreamPlayer1" runat="server" />
</asp:Content>
<asp:Content ID="content_right" ContentPlaceHolderID="right" runat="server">
<uc:StreamPlayer ID="StreamPlayer2" runat="server" />
</asp:Content>
