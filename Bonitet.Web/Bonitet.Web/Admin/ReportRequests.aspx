<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="ReportRequests.aspx.cs" Inherits="Bonitet.Web.Admin.ReportRequests" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap-table.css" rel="stylesheet" />
    <link href="../css/bootstrap-select.min.css" rel="stylesheet" />
    <script src="../js/scripts.js"></script>

    <script src="../js/bootstrap-select.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="main-content">
        <div style="margin-bottom: 70px;"></div>
        <div class="container">
            <div class="row">
                <h1>Барања за извештаи</h1>

                <div class="napomena">
                    <div class="panel">
                        <div class="panel-heading">
                            <h3 class="panel-title">Филтрирај ги резултатите</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-md-6 col-md-offset-3">
                                <div class="searching" style="margin:0">
                                    <span style="margin-right: 10px;">Филтрирај: </span>
                                    <asp:DropDownList runat="server" name="search_selector" id="search_selector" class="selectpicker" AutoPostBack="true" OnSelectedIndexChanged="search_selector_SelectedIndexChanged">
                                        <asp:ListItem Value="1">All</asp:ListItem>
                                        <asp:ListItem Value="2">Pending Data</asp:ListItem>
                                        <asp:ListItem Value="3">Available</asp:ListItem>
                                        <asp:ListItem Value="4">Downloaded</asp:ListItem>
                                        <asp:ListItem Value="5">No Data</asp:ListItem>
                                    </asp:DropDownList>
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
                            <th class="created_on" style="width:165px;">Датум</th>
                            <th class="embs">ЕМБС</th>
                            <th class="company_name">Име на компанија</th>
                            <th class="username">Клиент</th>
                            <th class="year">Година</th>
                            <th class="report_type">Тип</th>
                            <th class="status_text">Статус</th>
                            <th class="action">Акција</th>
                        </tr>
                        </thead>
                        <tbody class="text-left">
                            <asp:Repeater ID="r_ReportRequests" runat="server">
                                <ItemTemplate>
                                    <tr id="tr_id_<%# Container.ItemIndex + 1 %>" class="tr-class-<%# Container.ItemIndex + 1 %> item-id<%# Eval("ID")  %>">
                                        <td class="created_on" style="width:165px;"><%# Eval("CreatedOn") %></td>
                                        <td class="embs"><%# Eval("EMBS") %></td>
                                        <td class="company_name"><%# Eval("CompanyName") %></td>
                                        <td class="username"><%# Eval("Username") %></td>
                                        <td class="year"><%# Eval("Year") %></td>
                                        <td class="report_type"><%# Eval("ReportTypeString") %></td>
                                        <td class="status_text"><%# Eval("StatusText") %></td>
                                        <td class="action">
                                            <%# (Eval("StatusText").ToString() == "Pending Data" ?(Convert.ToBoolean(Eval("NoData")) == false ? "<a style=\"color:gray;\" onclick=\"FillDataForRequestMail('" + Eval("EMBS") + "', " + Eval("UserID") + ", " + Eval("ID") +")\" data-toggle=\"modal\" data-target=\"#report_request_nodata\">Откажи (нема податоци)</a>" : "") : "")%>
                                            <%# (Convert.ToBoolean(Eval("SendMail")) == true ? (Convert.ToBoolean(Eval("MailSent")) == false ? "<a style=\"color:green;\" onclick=\"FillDataForRequestMail('" + Eval("EMBS") + "', " + Eval("UserID") + ", " + Eval("ID") +")\" data-toggle=\"modal\" data-target=\"#report_request\">Испрати мејл за достапност на извештај</a>" : "<a style=\"color:blue;\" onclick=\"FillDataForRequestMail('" + Eval("EMBS") + "', " + Eval("UserID") + ", " + Eval("ID") +")\" data-toggle=\"modal\" data-target=\"#report_request\">Испратено. Испрати маил за достапност на извештај</a>") : "")%>
                                            <%# (Convert.ToInt32(Eval("ReportType")) == 1 && Convert.ToBoolean(Eval("Paid")) == false  ? "<a style=\"color:green;\" onclick=\"FillDataForRequestMail('" + Eval("EMBS") + "', " + Eval("UserID") + ", " + Eval("ID") +")\" data-toggle=\"modal\" data-target=\"#charge_user\">Испрати мејл за успешна нарачка.</a>" : "")%>
                                            <a style="color:red;" onclick="FillDataForRequestMail('<%# Eval("EMBS")  %>',<%# Eval("UserID")  %>, <%# Eval("ID") %>)" data-toggle="modal" data-target="#delete_request">Избриши од листа</a>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                    <div runat="server" id="pagination_wrapper" class="custom_pagination">
                        <asp:LinkButton ID="PrevPageBtn" CssClass="arrow_left" runat="server" OnClick="PrevPageBtn_Click"><<</asp:LinkButton>
                        <p runat="server" id="cur_page">Page 1</p>
                        <asp:LinkButton ID="NextPageBtn" CssClass="arrow_right" runat="server" OnClick="NextPageBtn_Click">>></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>

    </section>

    <div class="modal fade" id="charge_user" tabindex="-1" role="dialog" aria-labelledby="report_request_label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">Дали сакате да испратите мејл до клиентот?</h4>
                </div>
                <div class="modal-body">
                    <p>Со овој мејл испраќате порака до клиентот дека нарачката на Бонитетен извештај е успешна и дека потрошил 1 (еден) извештај од својот пакет.</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="SendChargeMail()" data-dismiss="modal">Да</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Не</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="report_request" tabindex="-1" role="dialog" aria-labelledby="report_request_label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">Дали сакате да испратите мејл до клиентот?</h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="SendRequestMail()" data-dismiss="modal">Да</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Не</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="report_request_nodata" tabindex="-1" role="dialog" aria-labelledby="report_request_label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">Дали сакате да испратите мејл до клиентот?</h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="SendRequestMailNoData()" data-dismiss="modal">Да</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Не</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="delete_request" tabindex="-1" role="dialog" aria-labelledby="report_request_label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">Дали сакате да го избришете ова барање?</h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="DeleteFromList()" data-dismiss="modal">Да</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Не</button>
                </div>
            </div>
        </div>
    </div>

    
    <script src="../js/bootstrap-table.js"></script>
</asp:Content>
