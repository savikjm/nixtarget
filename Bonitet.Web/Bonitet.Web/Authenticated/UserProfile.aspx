<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="UserProfile.aspx.cs" Inherits="Bonitet.Web.Authenticated.UserProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap-table.css" rel="stylesheet" />
    <link href="../css/bootstrap-select.min.css" rel="stylesheet" />

    <script src="../js/bootstrap-select.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="main-content">
        <div style="margin-bottom: 70px;"></div>
        <div class="container">
            <h1>Мој Профил</h1>
            <div class="row">
                <div class="col-md-8 col-md-offset-2">
                    <div class="check">
                        <div class="panel">
                            <div class="panel-heading">
                                <h3 class="panel-title">Информации за корисникот</h3>
                            </div>
                            <div class="panel-body">

                                <div class="col-md-6">
                                    <label for="username">Корисничко име</label>
                                    <%--<input type="text" id="username" name="username" readonly="readonly" value="user88"/>--%>
                                    <asp:TextBox ReadOnly="true" CssClass="form-control" runat="server" ID="p_username" />
                                </div>
                                <div class="col-md-6">
                                    <label for="password">Е-пошта</label>
                                    <%--                                    <input type="email" id="password" name="password" readonly="readonly" value="user88@gmail.com"/>--%>
                                    <asp:TextBox ReadOnly="true" CssClass="form-control" runat="server" ID="p_email" />

                                </div>
                                <div class="margin-top">
                                    <div class="col-md-6">
                                        <a runat="server" id="edit_user" class="btn btn-default pull-left">Промени информации</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <section class="orders">
        <div class="container-fluid">
            <h3>Преглед на извршени нарачки:</h3>
            <h4>Сите ваши нарачки се архивираат и можете да ги преземате неограничен број пати.</h4>

            <div class="col-md-8 col-md-offset-2">
                <div class="searching">
                    <div class="input-group">
                        <div class="input-group-btn">
                            <div class="btn-group">
                                <select runat="server" name="search_selector" id="search_selector" class="selectpicker">
                                    <option value="name">Назив</option>
                                    <option value="embs">ЕМБС</option>
                                    <option value="tip_izvestaj">Тип извештај</option>
                                </select>
                            </div>
                        </div>
                        <input runat="server" id="search_keyword" type="text" class="form-control" placeholder="Пребарувај" />
                        <span class="input-group-btn">
                            <asp:Button runat="server" OnClick="search_submit_Click" ID="search_submit" CssClass="btn btn-search" Text="Пребарај" />
                        </span>
                    </div>
                </div>
            </div>

            <div class="table-responsive user_reports">
                <table class="table table-bordered table-hover" id="data">
                    <thead>
                        <tr>
                            <th><asp:LinkButton CssClass="down" CommandArgument="asc-EMBS" runat="server" OnClick="sort_embs_Click" ID="sort_embs">ЕМБС<span></span></asp:LinkButton></th>
                            <th><asp:LinkButton CssClass="down" CommandArgument="asc-CompanyName" runat="server" OnClick="sort_naziv_Click" ID="sort_naziv">Назив на субјект<span></span></asp:LinkButton></th>
                            <th><asp:LinkButton CssClass="down" CommandArgument="asc-PackTypeName" runat="server" OnClick="sort_tip_Click" ID="sort_tip">Тип извештај<span></span></asp:LinkButton></th>
                            <th><asp:LinkButton CssClass="down" CommandArgument="asc-DateCreated" runat="server" OnClick="sort_datum_Click" ID="sort_datum">Датум на креирање на извештајот<span></span></asp:LinkButton></th>
                            <th><asp:LinkButton CssClass="down" CommandArgument="asc-Downloads" runat="server" OnClick="sort_download_Click" ID="sort_download">Преземено (пати)<span></span></asp:LinkButton></th>
                            <th><i class="fa fa-download text-center"></i></th>
                        </tr>
                    </thead>
                    <tbody class="text-left">
                        <asp:Repeater ID="r_UserReports" runat="server">
                            <ItemTemplate>
                                <tr id="tr_id_<%# Container.ItemIndex + 1 %>" class="tr-class-<%# Container.ItemIndex + 1 %>">
                                    <td><%# Eval("EMBS") %></td>
                                    <td><%# Eval("CompanyName") %></td>
                                    <td><%# Eval("PackTypeName") %></td>
                                    <td><%# Eval("DateCreated") %></td>
                                    <td><%# Eval("Downloads") %></td>
                                    <td><a href="/Document.ashx?uid=<%# Eval("UID") %>">Превземи</a></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>

            </div>
            <div runat="server" id="pagination_wrapper" class="custom_pagination">
                <asp:LinkButton ID="PrevPageBtn" CssClass="arrow_left" runat="server" OnClick="PrevPageBtn_Click"><<</asp:LinkButton>
                <p runat="server" id="cur_page">Page 1</p>
                <asp:LinkButton ID="NextPageBtn" CssClass="arrow_right" runat="server" OnClick="NextPageBtn_Click">>></asp:LinkButton>
            </div>
        </div>
    </section>


    <script src="../js/bootstrap-table.js"></script>
</asp:Content>
