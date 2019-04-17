<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="EditAdmin.aspx.cs" Inherits="Bonitet.Web.Admin.EditAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-centered">
        <div class="container-fluid">
            <div class="panel panel-default">
                <div class="page-header">
                    <h1>Промени лозинка</h1>
                </div>
                <div class="form-group">
                    <label runat="server" id="error_text" class="control-label" >Грешка!</label>
                </div>
                <div class="panel-body">
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-6 form-horizontal">
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="c_password">Лозинка: </label>
                            <div class="btn-group col-sm-6">
                                <asp:TextBox CssClass="form-control" runat="server" ID="c_password" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="btn-group" >
                                <asp:Button OnClick="save_user_Click" id="save_user" CssClass="btn btn-default" runat="server" Text="Зачувај"/>
                            </div>
                            <div class="btn-group" >
                                <a runat="server" href="/Admin/Default.aspx" class="btn btn-default" >Назад</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

