<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="EditAdmin.aspx.cs" Inherits="Bonitet.Web.Admin.EditAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<div class="col-centered">
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
    </div>--%>


    <section class="main-content">
        <div style="margin-bottom: 70px;"></div>

        <div class="container">
            <div class="row">
                <h1>Промени лозинка</h1>

                <div class="col-md-10 col-md-offset-1">
                    <div class="my_info">
                        <div class="panel">
                            <div class="panel-heading">
                                <h3 class="panel-title">Внесете нова лозинка</h3>
                            </div>
                            <div class="panel-body">
                                <div class="form-group">
                                    <label runat="server" id="error_text" class="control-label" >Грешка!</label>
                                </div>
                                    
                                    <div class=" col-md-8 col-md-offset-2">
                                        <label for="c_password">Лозинка: </label>             
                                        <asp:TextBox CssClass="form-control" runat="server" ID="c_password" />
                                    </div>
                                    
                                    <div class="col-md-8 col-md-offset-2" style="margin-top: 20px;">
                                        <div class="col-md-6" style="padding: 0;">
                                            <asp:Button OnClick="save_user_Click" id="save_user" CssClass="btn custom_btn" runat="server" Text="Зачувај"/>
                                        </div>
                                        <div class="col-md-6" style="padding: 0;">
                                            <a href="/Admin/Default.aspx" type="button" class="btn btn-default mybtn pull-right custom_btn">Назад</a>
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

