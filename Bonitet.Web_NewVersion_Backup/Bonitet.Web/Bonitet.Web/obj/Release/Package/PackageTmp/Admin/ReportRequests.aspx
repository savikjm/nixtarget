<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="ReportRequests.aspx.cs" Inherits="Bonitet.Web.Admin.ReportRequests" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap-table.css" rel="stylesheet" />
    <link href="../css/bootstrap-select.min.css" rel="stylesheet" />
    <script src="../js/scripts.js"></script>

    <script src="../js/bootstrap-select.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-centered">
        <div class="container-fluid">
            <div class="panel panel-default">
                <div class="page-header">
                    <h1>Барања за извештаи</h1>
                </div>
                <div class="col-md-12" style="text-align:left;">
                    <p>Филтрирај ги резултатите</p>
                </div>
                <div class="input-group search_container short">
                    <asp:DropDownList runat="server" name="search_selector" id="search_selector" class="selectpicker" AutoPostBack="true" OnSelectedIndexChanged="search_selector_SelectedIndexChanged">
                        <asp:ListItem Value="1">All</asp:ListItem>
                        <asp:ListItem Value="2">Pending Data</asp:ListItem>
                        <asp:ListItem Value="3">Available</asp:ListItem>
                        <asp:ListItem Value="4">Downloaded</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <table data-height="430" data-toggle="table" class="table_width" id="companies_table" data-toolbar="#transform-buttons">
                    <thead>
                        <tr>
                            <th class="created_on">Датум</th>
                            <th class="embs">ЕМБС</th>
                            <th class="company_name">Име на компанија</th>
                            <th class="username">Клиент</th>
                            <th class="year">Година</th>
                            <th class="report_type">Тип</th>
                            <th class="status_text">Статус</th>
                            <th class="action">Акција</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="r_ReportRequests" runat="server">
                            <ItemTemplate>
                                <tr id="tr_id_<%# Container.ItemIndex + 1 %>" class="tr-class-<%# Container.ItemIndex + 1 %> item-id<%# Eval("ID")  %>">
                                    <td class="created_on"><%# Eval("CreatedOn") %></td>
                                    <td class="embs"><%# Eval("EMBS") %></td>
                                    <td class="company_name"><%# Eval("CompanyName") %></td>
                                    <td class="username"><%# Eval("Username") %></td>
                                    <td class="year"><%# Eval("Year") %></td>
                                    <td class="report_type"><%# Eval("ReportTypeString") %></td>
                                    <td class="status_text"><%# Eval("StatusText") %></td>
                                    <td class="action">
                                        <%# (Convert.ToBoolean(Eval("SendMail")) == true ? "<a onclick=\"FillDataForRequestMail('" + Eval("EMBS") + "', " + Eval("UserID") +")\" data-toggle=\"modal\" data-target=\"#report_request\">Испрати мејл</a>" : "")%>
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
    <script src="../js/bootstrap-table.js"></script>
</asp:Content>
