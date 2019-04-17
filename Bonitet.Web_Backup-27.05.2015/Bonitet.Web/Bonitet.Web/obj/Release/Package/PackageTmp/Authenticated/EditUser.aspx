<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="Bonitet.Web.Authenticated.EditUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap-table.css" rel="stylesheet" />
    <script src="../js/scripts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-centered">
        <div class="container-fluid">
            <div class="panel panel-default">
                <div class="page-header">
                    <h1>Промени инфомрации</h1>
                </div>
                <div class="panel-body">
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-6 form-horizontal">
                        <div class="form-group">
                            <label runat="server" id="error_text" class="control-label" >Грешка!</label>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="c_username">Username: </label>
                            <div class="btn-group col-sm-6">
                                <asp:TextBox CssClass="form-control cur_embs" runat="server" ID="c_username" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="c_password">Password: </label>
                            <div class="btn-group col-sm-6">
                                <asp:TextBox CssClass="form-control" runat="server" ID="c_password" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="c_email">Е-пошта: </label>
                            <div class="btn-group col-sm-6">
                                <asp:TextBox CssClass="form-control" runat="server" ID="c_email" />
                            </div>
                        </div>
                        <div class="form-group" >
                            <div class="btn-group" >
                                <asp:Button OnClick="save_user_Click" id="save_user" CssClass="btn btn-default" runat="server" Text="Зачувај"/>
                            </div>
                            <div class="btn-group" >
                                <a id="Button1" class="btn btn-default" href="/Admin/UserListing.aspx">Назад</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
