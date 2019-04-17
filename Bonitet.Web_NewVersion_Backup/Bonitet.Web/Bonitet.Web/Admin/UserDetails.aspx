<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="Bonitet.Web.Admin.UserDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap-table.css" rel="stylesheet" />
    <script src="../js/scripts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-centered">
        <div class="container-fluid">
            <div class="panel panel-default">
                <div class="page-header">
                    <a href="/Admin/UserListing.aspx" type="button" class="btn btn-default pull-left">Назад</a>
                    <h1>Информации за клиент</h1>
                </div>
                <div class="panel-body">
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-6 form-horizontal">
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="c_username">Корисничко име: </label>
                            <div class="btn-group col-sm-6">
                                <asp:TextBox ReadOnly="true" CssClass="form-control cur_embs" runat="server" ID="c_username" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="c_password">Лозинка: </label>
                            <div class="btn-group col-sm-6">
                                <asp:TextBox ReadOnly="true" CssClass="form-control" runat="server" ID="c_password" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="c_email">Е-пошта: </label>
                            <div class="btn-group col-sm-6">
                                <asp:TextBox ReadOnly="true" CssClass="form-control" runat="server" ID="c_email" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="c_embs">ЕМБС: </label>
                            <div class="btn-group col-sm-6">
                                <asp:TextBox ReadOnly="true" CssClass="form-control" runat="server" ID="c_embs" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="btn-group col-sm-offset-4 col-sm-6" >
                                <a runat="server" id="edit_user" class="btn btn-default" >Промени информации за клиент</a>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="btn-group col-sm-offset-4 col-sm-6" >
                                <a runat="server" id="c_kratko_izvestaj" href="CreatePrepayPack.aspx" type="button" class="btn btn-default" >Креирај пакет за краток извештај</a>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="btn-group col-sm-offset-4 col-sm-6" >
                                <a runat="server" id="c_blokada" href="CreatePrepayPack.aspx" type="button" class="btn btn-default" >Креирај пакет за блокада</a>
                            </div>
                        </div>
                    </div>
                </div>
                <table data-height="430" data-toggle="table" id="companies_table" data-toolbar="#transform-buttons">
                    <thead>
                        <tr>
                            <th>Важи од</th>
                            <th>Важи до</th>
                            <th>Тип Пакет</th>
                            <th>Вк. Број извештаи</th>
                            <th>Искористени</th>
                            <th>Коментар</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="r_PrepayPacks" runat="server">
                            <ItemTemplate>
                                <tr id="tr_id_<%# Container.ItemIndex + 1 %>" class="tr-class-<%# Container.ItemIndex + 1 %>">
                                    <td><%# Eval("DateStart") %></td>
                                    <td><%# Eval("DateEnd") %></td>
                                    <td><%# Eval("PackTypeName") %></td>
                                    <td><%# Eval("Pack") %></td>
                                    <td><%# Eval("Used") %></td>
                                    <td><%# Eval("Comment") %></td>
                                    <td>
                                        <%# (Convert.ToBoolean(Eval("Active")) == true ? "<a onclick=\"SetCurPackID('" + Eval("ID") + "')\" data-toggle=\"modal\" data-target=\"#pdf_modal1\">Деактивирај</a>" : "<a onclick=\"SetCurPackID('" + Eval("ID") + "')\" data-toggle=\"modal\" data-target=\"#pdf_modal2\">Активирај</a>") %> | 
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
    <div class="modal fade" id="pdf_modal1" tabindex="-1" role="dialog" aria-labelledby="pdf_modal_label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">Дали сакате да го деактивирате пакетот?</h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="DisablePack()" data-dismiss="modal">Да</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Не</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="pdf_modal2" tabindex="-1" role="dialog" aria-labelledby="pdf_modal_label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">Дали сакате да го активирате пакетот?</h4>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="EnablePack()" data-dismiss="modal">Да</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Не</button>
                </div>
            </div>
        </div>
    </div>
    <script src="../js/bootstrap-table.js"></script>
</asp:Content>
