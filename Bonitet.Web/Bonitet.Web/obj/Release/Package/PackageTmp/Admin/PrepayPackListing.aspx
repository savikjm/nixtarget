<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="PrepayPackListing.aspx.cs" Inherits="Bonitet.Web.Admin.PrepayPackListing" %>
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
                <h1>Пакети</h1>

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
                            <%--<th>Тип Пакет</th>--%>
                            <th>ЕМБС</th>
                            <th>Лимит</th>
                            <th>Искористени</th>
                            <%--<th>Пост-пејд</th>--%>
                            <th></th>
                        </tr>
                        </thead>
                        <tbody class="text-left">
                            <asp:Repeater ID="r_PrepayPacks" runat="server">
                                <ItemTemplate>
                                    <tr id="tr_id_<%# Container.ItemIndex + 1 %>" class="tr-class-<%# Container.ItemIndex + 1 %>">
                                        <td><%# Eval("Username") %></td>
                                        <%--<td><%# Eval("PackTypeName") %></td>--%>
                                        <td><%# Eval("EMBS") %></td>
                                        <td><%# Eval("Total") %></td>
                                        <td><%# Eval("Used") %></td>
                                        <%--<td><%# ((bool)Eval("IsPostPaid") == true ? "Да" : "Не")  %></td>--%>
                                        <td>
                                            <a href="/Admin/UserDetails.aspx?id=<%# Eval("UserID") %>&packid=<%# Eval("ID") %>">Детали</a><%-- |
                                            <a href="/Admin/PrepayPackEdit.aspx?userid=<%# Eval("UserID") %>&packid=<%# Eval("ID") %>">Промени</a>--%>
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



    <script src="../js/bootstrap-table.js"></script>
</asp:Content>

