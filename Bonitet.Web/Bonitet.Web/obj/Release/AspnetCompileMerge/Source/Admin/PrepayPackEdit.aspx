<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="PrepayPackEdit.aspx.cs" Inherits="Bonitet.Web.Admin.PrepayPackEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="main-content">
        <div style="margin-bottom: 70px;"></div>
        <div class="container">
            <div class="row">
                <input runat="server" type="hidden" id="c_CurCompanyID" class="cur_company_id" />
                <h1>Креирај пакет</h1>

                <div class="col-md-8 col-md-offset-2">
                    <div class="my_info">
                        <div class="panel">
                            <div class="panel-heading">
                                <a href="/Admin/UserListing.aspx" runat="server" id="Back_UserDetails" type="button" class="btn btn-back pull-left">Назад</a>
                                <h3 class="panel-title">Креирај пакет</h3>
                            </div>
                            <div class="panel-body">
                                <div class="form-group">
                                    <label runat="server" id="error_text" class="control-label" for="start_date">Грешка при креирање!</label>
                                </div>
                                    <div class=" col-md-8 col-md-offset-2">
                                        <div class="form-group date" id="start_date_container">
                                            <label class="control-label" for="start_date">Важи од: </label>
                                            <div class="btn-group input-group">
                                                <asp:TextBox CssClass="form-control " runat="server" ID="start_date" />
                                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class=" col-md-8 col-md-offset-2">
                                        <div class="form-group date" id="end_date_container">
                                            <label class="control-label" for="end_date">Важи до: </label>
                                            <div class="btn-group input-group">
                                                <asp:TextBox CssClass="form-control " runat="server" ID="end_date" />
                                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class=" col-md-8 col-md-offset-2">
                                        <label for="pack">Вк. број извештаи: </label>
                                        <asp:TextBox CssClass="form-control"  runat="server" ID="pack" />
                                    </div>
                                    <div class=" col-md-8 col-md-offset-2">
                                        <label for="comment">Коментар: </label>
                                        <asp:TextBox TextMode="multiline" Columns="50" Rows="5" CssClass="form-control" runat="server" ID="comment" />
                                    </div>
                                    <div class=" col-md-8 col-md-offset-2">
                                        <label for="pack">Пост-пејд: </label>
                                        <asp:CheckBox CssClass="form-control"  runat="server" ID="post_paid" />
                                    </div>
                                    <div class="col-md-8 col-md-offset-2" style="margin-top: 20px;">
                                        <div class="col-md-6" style="padding: 0;">
                                            <asp:Button runat="server" CssClass="btn custom_btn" ID="submit_buttn" OnClick="submit_buttn_Click" Text="Зачувај" />
                                        </div>
                                        <div class="col-md-6" style="padding: 0;">
                                            <button type="submit" class="btn mybtn pull-right custom_btn">Креирај</button>
                                        </div>
                                    </div>                          
                                </div>
                             </div>
                        </div>
                    </div>


            </div>
        </div>
    </section>




    <script src="../js/moment.js"></script>
    <script src="../js/collapse.js"></script>
    <script src="../js/bootstrap-datetimepicker.min.js"></script>
    <script src="../js/scripts.js"></script>
</asp:Content>

