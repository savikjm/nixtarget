<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="UserListing.aspx.cs" Inherits="Bonitet.Web.Admin.UserListing" %>
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
                <h1>Преглед на Компании</h1>

                <div class="napomena">
                    <div class="panel">
                        <div class="panel-heading">
                            <h3 class="panel-title">Пребарувајте клиенти по корисничко име и ЕМБС.</h3>
                        </div>
                        <div class="panel-body">
                            <p><strong>Напомена: </strong>При пребарување на субјекти користете македонска подршка (кирилица), во спротивно системот нема да Ви генерира резултати од пребарувањето. </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-6 col-md-offset-3">
                <div class="searching">
                    <div class="input-group">
                        <div class="input-group-btn">
                            <div class="btn-group mySelect">
                                <select runat="server" name="search_selector" id="search_selector" class="selectpicker" >
						            <option value="name">Корисничко име</option>
						            <option value="embs">ЕМБС</option>
					            </select>
                            </div>
                        </div>
                        <!-- /btn-group -->
                        <input  runat="server" id="search_keyword" type="text" class="form-control" placeholder="Пребарувај"/>
                        <span class="input-group-btn">
                            <asp:Button OnClick="search_submit_Click" runat="server" ID="search_submit" CssClass="btn btn-default" Text="Пребарај" />
                        </span>
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
                            <th>Корисничко име</th>
                            <th>ЕМБС</th>
                            <th>Е-пошта</th>
                            <th></th>
                        </tr>
                        </thead>
                        <tbody class="text-left">
                            <asp:Repeater ID="r_Users" runat="server">
                                <ItemTemplate>
                                    <tr id="tr_id_<%# Container.ItemIndex + 1 %>" class="tr-class-<%# Container.ItemIndex + 1 %>">
                                        <td><%# Eval("Username") %></td>
                                        <td><%# Eval("EMBS") %></td>
                                        <td><%# Eval("Email") %></td>
                                        <td>
                                            <a href="/Admin/UserDetails.aspx?id=<%# Eval("ID") %>">Детали</a> |
                                            <%# (Convert.ToBoolean(Eval("IsActive")) == true ? "<a onclick=\"SetCurUserID('" + Eval("ID") + "')\" data-toggle=\"modal\" data-target=\"#user_modal1\">Деактивирај</a>" : "<a onclick=\"SetCurUserID('" + Eval("ID") + "')\" data-toggle=\"modal\" data-target=\"#user_modal2\">Активирај</a>") %>
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







    <div class="modal fade" id="user_modal1" tabindex="-1" role="dialog" aria-labelledby="pdf_modal_label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">Дали сакате да го деактивирате профилот на клиентот?</h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="DisableUser()" data-dismiss="modal">Да</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Не</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="user_modal2" tabindex="-1" role="dialog" aria-labelledby="pdf_modal_label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">Дали сакате да го активирате профилот на клиентот?</h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="EnableUser()" data-dismiss="modal">Да</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Не</button>
                </div>
            </div>
        </div>
    </div>
    <script src="../js/bootstrap-table.js"></script>
</asp:Content>
