<%@ Page Title="" Language="C#" MasterPageFile="~/BootstrapASP.Master" AutoEventWireup="true" CodeBehind="CreateChannel.aspx.cs" Inherits="BootstrapAsp.CreateChannel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <h1>Choose a name for your radio channel</h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="control-group">
           <label class="control-label" for="input">Channel name</label>
           <div class="controls">
             <input type="text" runat="server" id="inputEmail" placeholder="Email">
           </div>
        </div>
        <div class="control-group">
           <div class="controls">
             <button type="submit" runat="server" class="btn" id="btn_create" onclick="SignUp">Create</button>
           </div>
        </div> 
</asp:Content>
