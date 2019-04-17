<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="ReportsByDate.aspx.cs" Inherits="Bonitet.Web.Admin.ReportsByDate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="../css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="../css/bootstrap-table.css" rel="stylesheet" />
    <script src="../js/bootstrap-select.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="main-content">
        <div style="margin-bottom: 70px;"></div>
        <div class="container">
            <div class="row">
                <h1>Извештај во одреден временски период</h1>

                <div class="napomena">
                    <div class="panel" style="margin-bottom:50px;">
                        <div class="panel-heading">
                            <h3 class="panel-title">Филтрирај ги резултатите</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-md-12">
                                <div class="searching" style="margin: 0">
                                    <div class="input-group">
                                        <input type="hidden" runat="server" class="sDate" id="c_sDate" />
                                        <input type="hidden" runat="server" class="eDate" id="c_eDate" />
                                        <div class="btn-group float_left">
                                            <label for="pack">ЕМБС: </label>
                                            <asp:TextBox CssClass="form-control" runat="server" ID="c_embs" />
                                        </div>
                                        <div class="form-group date float_left" id="start_date_container1">
                                            <label class="control-label" for="start_date1">Од: </label>
                                            <div class="btn-group input-group">
                                                <asp:TextBox CssClass="form-control " runat="server" ID="start_date1" />
                                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="form-group date float_left" id="end_date_container1">
                                            <label class="control-label" for="end_date1">До: </label>
                                            <div class="btn-group input-group">
                                                <asp:TextBox CssClass="form-control " runat="server" ID="end_date1" />
                                                <span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="input-group-btn float_left">
                                            <div class="btn-group mySelect" style="margin-top:25px;">
                                                <asp:DropDownList runat="server" id="type_selector1" CssClass="selectpicker">
                                                    <asp:ListItem Value="-1">Сите</asp:ListItem>
                                                    <asp:ListItem Value="0">При-пејд</asp:ListItem>
                                                    <asp:ListItem Value="1">Пост-пејд</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <span class="form-group float_left custom">
                                            <asp:Button OnClick="search_submit_Click" runat="server" ID="search_submit" CssClass="btn btn-default" Text="Генерирај" />
                                        </span>
                                        <div class="form-group error_msg">
                                            <label runat="server" visible="false" id="error_text" class="control-label">Грешка</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </div>
        <div class="container-fluid">
            <div class="row">
                <div class="search-results">
                    <div class="table-responsive">
                        <table class="table table-bordered table-hover" id="data">
                            <thead>
                                <tr>
                                    <th class="embs">ЕМБС</th>
                                    <th class="username">Клиент</th>
                                    <th class="bonitet">Бонитетни извештаи</th>
                                    <th class="finansiski">Финансиски преглед</th>
                                    <th class="blokada">Блокади</th>
                                </tr>
                            </thead>
                            <tbody class="text-left">
                                <asp:Repeater ID="r_ReportByDate" runat="server">
                                    <ItemTemplate>
                                        <tr id="tr_id_<%# Container.ItemIndex + 1 %>" class="tr-class-<%# Container.ItemIndex + 1 %> item-id<%# Eval("ID")  %>">
                                            <td class="embs"><%# Eval("EMBS") %></td>
                                            <td class="username"><%# Eval("Username") %></td>
                                            <td class="bonitet <%# ((bool)Eval("BonitetiIsPostpaid") == true ? "post_paid" : "") %>"><%# Eval("BonitetiTotal") %></td>
                                            <td class="finansiski <%# ((bool)Eval("FinansiskiIsPostpaid") == true ? "post_paid" : "") %>"><%# Eval("FinansiskiTotal") %></td>
                                            <td class="blokada <%# ((bool)Eval("BlokadiIsPostpaid") == true ? "post_paid" : "") %>"><%# Eval("BlokadiTotal") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                        <div runat="server" id="pagination_wrapper" class="custom_pagination" style="text-align:center;">
                            <asp:LinkButton ID="PrevPageBtn" CssClass="arrow_left" runat="server" OnClick="PrevPageBtn_Click"><<</asp:LinkButton>
                            <p runat="server" id="cur_page">Page 1</p>
                            <asp:LinkButton ID="NextPageBtn" CssClass="arrow_right" runat="server" OnClick="NextPageBtn_Click">>></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <script src="../js/bootstrap-table.js"></script>
    <script src="../js/moment.js"></script>
    <script src="../js/collapse.js"></script>
    <script src="../js/bootstrap-datetimepicker.min.js"></script>
    <script src="../js/scripts.js"></script>
    <script>
        $(document).ready(function () {
            $('#start_date_container').datetimepicker({
                defaultDate: new Date(),
                minDate: new Date()
            });
            $('#end_date_container').datetimepicker({
                defaultDate: new Date(),
                minDate: new Date()
            });
        });
    </script>
</asp:Content>
