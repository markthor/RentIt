﻿<%@ Page Title="" Language="C#" MasterPageFile="~/BootstrapASP.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BootstrapAsp.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <p>Welcome to the site, you must log in to enter, or create a new user</p>
    <div style="float:left">
        <div class="control-group">
           <label class="control-label" for="inputEmail">Email</label>
           <div class="controls">
             <input type="text" runat="server" id="inputEmail" placeholder="Email">
           </div>
        </div>
        <div class="control-group">
           <label class="control-label" for="inputPassword">Password</label>
           <div class="controls">
             <input type="password" runat="server" id="inputPassword" placeholder="Password">
           </div>
        </div>
        <div class="control-group">
           <div class="controls">
             <label class="checkbox">
               <input type="checkbox"> Remember me
             </label>
             <button type="submit" runat="server" class="btn" id="signup_btn" onclick="SignUp">Sign in</button>
           </div>
        </div> 
    </div>
    <div class="control-group" style="float:right">
        <div class="control-group">
           <label class="control-label" for="inputEmail">Email</label>
           <div class="controls">
             <input type="text" runat="server" id="Text1" placeholder="Email">
           </div>
        </div>
        <div class="control-group">
           <label class="control-label" for="inputEmail">Username</label>
           <div class="controls">
             <input type="text" runat="server" id="Username" placeholder="Username">
           </div>
        </div>
        <div class="control-group">
           <label class="control-label" for="inputPassword">Password</label>
           <div class="controls">
             <input type="password" runat="server" id="Password1" placeholder="Password">
           </div>
        </div>
        <div class="control-group">
           <label class="control-label" for="inputPassword">Confirm Password</label>
           <div class="controls">
             <input type="password" runat="server" id="ConfirmPassword" placeholder="Confirm Password">
           </div>
        </div>
        <div class="control-group">
           <div class="controls">
             <button type="submit" runat="server" class="btn" id="btn_createuser" onclick="SignUp">Create user</button>
           </div>
        </div> 
    </div>
</asp:Content>
