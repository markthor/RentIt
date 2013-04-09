<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchBar.ascx.cs" Inherits="RentItClient.SearchBar" %>

<asp:TextBox ID="tbx_searchField" runat="server" width="300px"/>
<asp:Button ID="btn_search" runat="server" Text="Search" OnClick="btn_search_Click" />
<asp:Button ID="bth_searchAdvanced" runat="server" Text="Adv. Search" OnClick="bth_searchAdvanced_Click" />