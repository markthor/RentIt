<%@ Page Title="" Language="C#" MasterPageFile="~/MainPageMaster.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="BootstrapAsp.Index" %>
<%@ Register tagPrefix="UC" tagName="StreamPlayer" src="StreamPlayer.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2"  ContentPlaceHolderID="up" runat="server">
    
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="left" runat="server">
    <UC:StreamPlayer runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="center" runat="server">
    
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="right" runat="server">
    
</asp:Content>
