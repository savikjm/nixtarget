<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="UserProfile.aspx.cs" Inherits="Bonitet.Web.Authenticated.UserProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap-table.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-centered">
        <div class="container-fluid">
            <div class="panel panel-default">
                <div class="page-header">
                    <h1>Moj Профил</h1>
                </div>
                <div class="panel-body">
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-6 form-horizontal">
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="p_username">Корисничко име: </label>
                            <div class="btn-group col-sm-6">
                                <asp:TextBox ReadOnly="true" CssClass="form-control" runat="server" ID="p_username" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4" for="p_email">Е-пошта: </label>
                            <div class="btn-group col-sm-6">
                                <asp:TextBox ReadOnly="true" CssClass="form-control" runat="server" ID="p_email" />
                            </div>
                        </div>
                        <div class="form-group" >
                            <div class="btn-group col-sm-offset-4 col-sm-6" >
                            <a runat="server" id="edit_user" class="btn btn-default" >Промени информации</a>
                            </div>
                        </div>
                    </div>
                </div>
                <h3>Преглед на извршени нарачки:</h3>
                <p>Сите ваши нарачки се архивираат и можете да ги преземате неограничен број пати.</p>
                <table data-height="430" data-toggle="table" id="companies_table" data-toolbar="#transform-buttons">
                    <thead>
                        <tr>
                            <th>ЕМБС</th>
                            <th>Назив на субјект</th>
                            <th>Датум на креирање на извештајот</th>
                            <th>Преземено (пати)</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="r_UserReports" runat="server">
                            <ItemTemplate>
                                <tr id="tr_id_<%# Container.ItemIndex + 1 %>" class="tr-class-<%# Container.ItemIndex + 1 %>">
                                    <td><%# Eval("EMBS") %></td>
                                    <td><%# Eval("CompanyName") %></td>
                                    <td><%# Eval("DateCreated") %></td>
                                    <td><%# Eval("Downloads") %></td>
                                    <td><a href="/Document.ashx?uid=<%# Eval("UID") %>">Преземи</a></td>
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
