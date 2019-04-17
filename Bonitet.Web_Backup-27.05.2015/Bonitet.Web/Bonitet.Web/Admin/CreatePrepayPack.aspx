<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="CreatePrepayPack.aspx.cs" Inherits="Bonitet.Web.Admin.CreatePrepayPack" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-centered">
        <div class="container-fluid col-centered">
            <div class="panel panel-default col-centered">
                <input runat="server" type="hidden" id="c_CurCompanyID" class="cur_company_id" />
                <div class="page-header">
                    <a href="/Admin/UserListing.aspx" runat="server" id="Back_UserDetails" type="button" class="btn btn-default pull-left">Назад</a>
                    <h1>Креирај пакет</h1>
                </div>
                <div class="form-group">
                    <label runat="server" id="error_text" class="control-label" for="start_date">Грешка при креирање!</label>
                </div>
                <div class="container-fluid col-xs-3 col-centered">
                    <div class="form-group date" id="start_date_container">
                        <label class="control-label" for="start_date">Важи од: </label>
                        <div class="btn-group input-group">
                            <asp:TextBox CssClass="form-control " runat="server" ID="start_date" />
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                    <div class="form-group date" id="end_date_container">
                        <label class="control-label" for="end_date">Важи до: </label>
                        <div class="btn-group input-group">
                            <asp:TextBox CssClass="form-control " runat="server" ID="end_date" />
                            <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                            </span>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label" for="pack">Вк. број извештаи: </label>
                        <div class="">
                            <asp:TextBox CssClass="form-control"  runat="server" ID="pack" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label" for="comment">Коментар: </label>
                        <div class="btn-group ">
                            <asp:TextBox TextMode="multiline" Columns="50" Rows="5" CssClass="form-control" runat="server" ID="comment" />
                        </div>
                    </div>
                    <div class="panel-body">
                        <div class="btn-group" role="group">
                            <button type="submit" class="btn btn-default">Креирај</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../js/moment.js"></script>
    <script src="../js/collapse.js"></script>
    <script src="../js/bootstrap-datetimepicker.min.js"></script>
    <script src="../js/scripts.js"></script>
</asp:Content>
