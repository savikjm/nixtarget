<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="PrepayPackListing.aspx.cs" Inherits="Bonitet.Web.Admin.PrepayPackListing" %>
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
                    <h1>Пакети</h1>
                </div>
                <div class="col-md-4">
                </div>
                <div class="col-md-4">
                    <p>Пребарувајте клиенти по корисничко име и ЕМБС.</p>
                    <p><u>Напомена</u>: Пребарувањето на "корисничко име" или "ЕМБС" на клиентот да го правите со македонска поддрша (кирилица), во спротивно системот нема да Ви генерира резултати од пребарувањето.</p>
                </div>
                <div class="input-group search_container">
					<select runat="server" name="search_selector" id="search_selector" class="selectpicker" >
						<option value="name">Корисничко име</option>
						<option value="embs">ЕМБС</option>
					</select>
                    <input runat="server" id="search_keyword" type="text" class="form-control" placeholder="Пребарувај" />
                    <span class="input-group-btn">
                        <asp:Button OnClick="search_submit_Click" runat="server" ID="search_submit" CssClass="btn btn-default" Text="Пребарај" />
                    </span>
                </div>
                <table data-height="430" data-toggle="table" id="companies_table" data-toolbar="#transform-buttons">
                    <thead>
                        <tr>
                            <th>Корисничко име</th>
                            <th>ЕМБС</th>
                            <th>Лимит</th>
                            <th>Искористени</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="r_PrepayPacks" runat="server">
                            <ItemTemplate>
                                <tr id="tr_id_<%# Container.ItemIndex + 1 %>" class="tr-class-<%# Container.ItemIndex + 1 %>">
                                    <td><%# Eval("Username") %></td>
                                    <td><%# Eval("EMBS") %></td>
                                    <td><%# Eval("Total") %></td>
                                    <td><%# Eval("Used") %></td>
                                    <td>
                                        <a href="/Admin/PrepayPackDetails.aspx?userid=<%# Eval("UserID") %>&packid=<%# Eval("ID") %>">Детали</a> |
                                        <a href="/Admin/PrepayPackEdit.aspx?userid=<%# Eval("UserID") %>&packid=<%# Eval("ID") %>">Промени</a>
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
    <script src="../js/bootstrap-table.js"></script>
</asp:Content>

