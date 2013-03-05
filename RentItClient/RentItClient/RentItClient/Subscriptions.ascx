﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Subscriptions.ascx.cs" Inherits="RentItClient.Subscriptions1" %>

<link rel="stylesheet" type="text/css" href="RentItStyle.css" />

<div class="centered">
            <h1>Channels</h1>
        </div>
    <div> 
        <div style="float:left">
            <h3>Subscriptions</h3>
            <asp:ListBox ID="lbx_subscripedChannels" runat="server" Width="100%"></asp:ListBox>
            <asp:Button ID="btn_selectChannel" runat="server" Text="Select" CssClass="subscripeButton" OnClick="btn_selectChannel_Click"/>
        </div>
        
        <div style="clear:both; margin-top: 15px;">
            <h3>My channels</h3>
            <asp:ListBox ID="lbx_myChannels" runat="server" Width="100%"></asp:ListBox>
            <asp:Button ID="btn_editChannel" runat="server" Text="Edit" CssClass="subscripeButton" OnClick="btn_editChannel_Click"/>
        </div>
        
    </div>