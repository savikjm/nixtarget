<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="CompanyListing.aspx.cs" Inherits="Bonitet.Web.Authenticated.CompanyListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap-table.css" rel="stylesheet" />
    <link href="../css/bootstrap-select.min.css" rel="stylesheet" />

    <script src="../js/bootstrap-select.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="main-content">
        <div style="margin-bottom: 70px;"></div>
        <div class="container">
            <h1>Преглед на Компании</h1>
            <div class="napomena">
                <div class="panel">
                    <div class="panel-heading">
                        <h3 class="panel-title">Пребарувајте компании по ЕМБС, Назив или Адреса на субјекот.</h3>
                    </div>
                    <div class="panel-body">
                        <p><strong>Напомена: </strong>При пребарување на субјекти користете македонска подршка (кирилица), во спротивно системот нема да Ви генерира резултати од пребарувањето. </p>
                    </div>
                </div>
            </div>

            <div class="col-md-8 col-md-offset-2">

                <div class="searching">
                    <div class="input-group">
                        <div class="input-group-btn">
                            <div class="btn-group">
                                <select runat="server" name="search_selector" id="search_selector" class="selectpicker">
                                    <option value="name">Назив</option>
                                    <option value="sediste">Седиште</option>
                                    <option value="embs">ЕМБС</option>
                                </select>
                            </div>
                        </div>
                        <!-- /btn-group -->
                        <input runat="server" id="search_keyword" type="text" class="form-control" placeholder="Пребарувај" />
                        <span class="input-group-btn">
                            <asp:Button OnClick="search_submit_Click" runat="server" ID="search_submit" CssClass="btn btn-search" Text="Пребарај" />
                        </span>
                    </div>
                </div>
            </div>
        </div>
        <div class="container-fluid">
            <div class="search-results">
                <div class="table-responsive">
                    <table class="table table-bordered table-hover" id="data">
                        <thead>
                            <tr>
                                <th>ЕМБС</th>
                                <th>Назив на субјект</th>
                                <th>Седиште</th>
                                <th>Статус на субјектот</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody class="text-left">
                            <asp:Repeater ID="r_Companies" runat="server">
                                <ItemTemplate>
                                    <tr id="tr_id_<%# Container.ItemIndex + 1 %>" class="tr-class-<%# Container.ItemIndex + 1 %>">
                                        <td><%# Eval("EMBS") %></td>
                                        <td><%# Eval("CelosenNazivNaSubjektot") %></td>
                                        <td><%# Eval("Sediste") %></td>
                                        <td><%# (Eval("DatumNaPrestanok") == null ? "Активен" : "Неактивен")%></td>
                                        <td><a href="/Authenticated/CompanyDetails.aspx?id=<%# Eval("PK_Subjekt") %>">Нарачај</a></td>
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

    </section>


    <script src="../js/bootstrap-table.js"></script>
    <script src="../js/companies_scripts.js"></script>

</asp:Content>
