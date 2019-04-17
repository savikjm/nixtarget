<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="Bonitet.Web.Admin.EditUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap-table.css" rel="stylesheet" />
    <script src="../js/scripts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--    <div class="col-centered">
        <div class="container-fluid">
            <div class="panel panel-default">
                <div class="page-header">
                    <h1>Информации за клиент</h1>
                </div>
                <div class="form-group">
                    <label runat="server" id="error_text" class="control-label" >Грешка!</label>
                </div>
                <div class="panel-body">
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-6 form-horizontal">
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="c_username">Корисничко име: </label>
                            <div class="btn-group col-sm-6">
                                <asp:TextBox CssClass="form-control cur_embs" runat="server" ID="c_username" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="c_password">Лозинка: </label>
                            <div class="btn-group col-sm-6">
                                <asp:TextBox CssClass="form-control" runat="server" ID="c_password" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4" or="c_email">Е-пошта: </label>
                            <div class="btn-group col-sm-6">
                                <asp:TextBox CssClass="form-control" runat="server" ID="c_email" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4" or="c_embs">ЕМБС: </label>
                            <div class="btn-group col-sm-6">
                                <asp:TextBox CssClass="form-control" runat="server" ID="c_embs" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="btn-group">
                                <asp:Button OnClick="save_user_Click" id="save_user" CssClass="btn btn-default" runat="server" Text="Зачувај"/>
                            </div>
                            <div class="btn-group">
                                <a href="/Admin/UserListing.aspx" type="button" class="btn btn-default pull-left">Назад</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>--%>


    <section class="main-content">
        <div style="margin-bottom: 70px;"></div>

        <div class="container">
            <div class="row">
                <h1>Информации за клиент</h1>

                <div class="col-md-10 col-md-offset-1">
                    <div class="my_info">
                        <div class="panel">
                            <div class="panel-heading">
                                <h3 class="panel-title">Промени информации за клиент</h3>
                            </div>
                            <div class="panel-body">
                                <div class="form-group">
                                    <label runat="server" id="error_text" class="control-label" >Грешка!</label>
                                </div>
                                    <div class=" col-md-8 col-md-offset-2">
                                        <label for="c_username">Корисничко име: </label>                
                                        <asp:TextBox CssClass="form-control cur_embs" runat="server" ID="c_username" />
                                    </div>
                                    <div class=" col-md-8 col-md-offset-2">
                                        <label for="c_password">Лозинка: </label>                        
                                        <asp:TextBox CssClass="form-control" runat="server" ID="c_password" />
                                    </div>
                                    <div class=" col-md-8 col-md-offset-2">
                                        <label for="c_email">Е-пошта: </label>
                                        <asp:TextBox CssClass="form-control" runat="server" ID="c_email" />
                                    </div>
                                    <div class=" col-md-8 col-md-offset-2">
                                        <label for="c_embs">ЕМБС: </label>                      
                                        <asp:TextBox CssClass="form-control" runat="server" ID="c_embs" />
                                    </div>
                                    <div class="col-md-8 col-md-offset-2" style="margin-top: 20px;">
                                        <div class="col-md-6" style="padding: 0;">
                                            <asp:Button OnClick="save_user_Click" id="save_user" CssClass="btn custom_btn" runat="server" Text="Зачувај"/>
                                        </div>
                                        <div class="col-md-6" style="padding: 0;">
                                            <a href="/Admin/UserListing.aspx" type="button" class="btn btn-default mybtn pull-right custom_btn">Назад</a>
                                        </div>
                                    </div>                          
                                </div>
                             </div>
                        </div>
                    </div>
            </div>
        </div>

    </section>




</asp:Content>
