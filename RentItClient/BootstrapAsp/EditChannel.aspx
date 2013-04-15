<%@ Page Title="" Language="C#" MasterPageFile="~/BootstrapASP.Master" AutoEventWireup="true" CodeBehind="EditChannel.aspx.cs" Inherits="BootstrapAsp.EditChannel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <h1>Edit properties for your channel</h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="span12">
        <div class="span8">
            <div class="control-group">
                <div style="text-align:center">
                    <label class="control-label" for="inputEmail" style="text-align:center"><h3>Reggae radio</h3></label>
                </div>
            </div>
        </div>
    </div>
    <div class="span3">
        <div class="control-group">
            <label class="control-label" for="input">Genres</label>
               <div class="controls">
                   <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>
               </div>
            <div class="controls">
                    <button type="submit" runat="server" class="btn btn-danger" id="Button1" onclick="SignUp">Delete</button>
            </div>
            
        </div>
    </div>
    <div class="span1">
    </div>
    <div class="span3">
        <div class="control-group">
            <label class="control-label" for="input">Avaliable genres</label>
            <div class="control-group">
                <div class="controls">
                       <asp:ListBox ID="ListBox2" runat="server"></asp:ListBox>
                </div>
            
                <div class="controls">
                    <button type="submit" runat="server" class="btn btn-success" id="btn_createuser" onclick="SignUp">Add</button>
                </div>
            </div>
        </div>
    </div>
    <div class="span8">

    </div>
</asp:Content>
