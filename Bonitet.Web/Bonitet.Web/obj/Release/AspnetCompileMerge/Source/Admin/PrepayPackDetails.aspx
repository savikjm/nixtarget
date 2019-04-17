<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="PrepayPackDetails.aspx.cs" Inherits="Bonitet.Web.Admin.PrepayPackDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap-table.css" rel="stylesheet" />
    <script src="../js/scripts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="main-content">
        <div style="margin-bottom: 70px;"></div>
        <div class="container-fluid">
            <div class="row">
                <h1>Информации за пакет</h1>
                <a runat="server" id="Back_UserDetails" href="#" type="button" class="btn" style="margin-bottom: 20px!important;">Назад</a>
            <div class="search-results">
                <div class="table-responsive">
                    <table class="table table-bordered table-hover" id="data">
                        <thead>
                        <tr>
                            <th>Датум на креирање</th>
                            <th>ЕМБС</th>
                            <th>Компанија</th>
                            <th>Преземено</th>
                            <th>Пост-пејд</th>
                        </tr>
                        </thead>
                        <tbody class="text-left">
                            <asp:Repeater ID="r_PrepayPacks" runat="server">
                                <ItemTemplate>
                                    <tr id="tr_id_<%# Container.ItemIndex + 1 %>" class="tr-class-<%# Container.ItemIndex + 1 %>">
                                        <td><%# Eval("DateCreated") %></td>
                                        <td><%# Eval("EMBS") %></td>
                                        <td><%# Eval("CompanyName") %></td>
                                        <td><%# Eval("Downloads") %></td>
                                        <td><%# ((bool)Eval("IsPostPaid") == false ? "Не" : "Да") %></td>
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
