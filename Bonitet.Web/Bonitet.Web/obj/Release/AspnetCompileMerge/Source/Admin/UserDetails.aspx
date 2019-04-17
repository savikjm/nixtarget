<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="Bonitet.Web.Admin.UserDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap-table.css" rel="stylesheet" />
    <script src="../js/scripts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="main-content">
        <div style="margin-bottom: 70px;"></div>

        <div class="container">
            <div class="row">
                <h1>Информации за клиент</h1>

                <div class="col-md-10 col-md-offset-1">
                    <div class="my_info">
                        <div class="panel">
                            <div class="panel-heading">
                                <a href="/Admin/UserListing.aspx" type="button" class="btn btn-back pull-left">Назад</a>
                                <h3 class="panel-title">Информации за клиентот</h3>
                            </div>
                            <div class="panel-body">
                                    <div class=" col-md-8 col-md-offset-2">
                                        <label for="c_username">Корисничко име: </label>                
                                        <asp:TextBox ReadOnly="true" CssClass="form-control cur_embs" runat="server" ID="c_username" />
                                    </div>
                                    <div class=" col-md-8 col-md-offset-2">
                                        <label for="c_password">Лозинка: </label>                        
                                        <asp:TextBox ReadOnly="true" CssClass="form-control" runat="server" ID="c_password" />
                                    </div>
                                    <div class=" col-md-8 col-md-offset-2">
                                        <label for="c_email">Е-пошта: </label>
                                        <asp:TextBox ReadOnly="true" CssClass="form-control" runat="server" ID="c_email" />
                                    </div>
                                    <div class=" col-md-8 col-md-offset-2">
                                        <label for="c_embs">ЕМБС: </label>                      
                                        <asp:TextBox ReadOnly="true" CssClass="form-control" runat="server" ID="c_embs" />
                                    </div>
                                    <div class="col-md-8 col-md-offset-2" style="margin-top: 20px;">
                                        <div class="col-md-8 col-md-offset-2" style="padding: 0;">                                      
                                            <a runat="server" id="edit_user" class="btn big_btn" >Промени информации за клиент</a>
                                        </div>
                                        <div class="col-md-8 col-md-offset-2" style="padding: 0;">
                                            <a runat="server" id="c_boniteten_izvestaj" href="CreatePrepayPack.aspx" type="button" class="btn big_btn" >Креирај пакет за бонитетен извештај</a>
                                        </div>
                                        <div class="col-md-8 col-md-offset-2" style="padding: 0;">
                                            <a runat="server" id="c_kratko_izvestaj" href="CreatePrepayPack.aspx" type="button" class="btn big_btn" >Креирај пакет за краток извештај</a>
                                        </div>
                                        <div class="col-md-8 col-md-offset-2" style="padding: 0;">
                                            <a runat="server" id="c_blokada" href="CreatePrepayPack.aspx" type="button" class="btn big_btn" >Креирај пакет за блокада</a>
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
                                <th>Важи од</th>
                                <th>Важи до</th>
                                <th>Тип Пакет</th>
                                <th>Вк. Број извештаи</th>
                                <th>Искористени</th>
                                <th>Пост-пејд</th>
                                <th>Коментар</th>
                                <th></th>
                            </tr>
                            </thead>
                            <tbody class="text-left">
                                <asp:Repeater ID="r_PrepayPacks" runat="server">
                                    <ItemTemplate>
                                        <tr id="tr_id_<%# Container.ItemIndex + 1 %>" class="tr-class-<%# Container.ItemIndex + 1 %>">
                                            <td><%# Eval("DateStart") %></td>
                                            <td><%# Eval("DateEnd") %></td>
                                            <td><%# Eval("PackTypeName") %></td>
                                            <td><%# Eval("Pack") %></td>
                                            <td><%# Eval("Used") %></td>
                                            <td><%# (Convert.ToBoolean(Eval("IsPostPaid")) == false ? "Не" : "Да") %></td>
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
        </div>


    </section>


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
