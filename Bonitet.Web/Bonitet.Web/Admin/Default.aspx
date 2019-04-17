<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Bonitet.Web.Admin.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<div class="col-centered">
        <div class="container-fluid">
            <div class="panel panel-default">
                <div class="page-header">
                    <h1>еБонитети.мк - админ</h1>
                </div>
                <div class="panel-body">
                    <div class="btn-group ">
                        <asp:TextBox runat="server" ID="tb_username" placeholder="Корисничко име" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="btn-group">
                        <asp:TextBox runat="server" ID="tb_password" TextMode="Password" placeholder="Лозинка" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="btn-group" role="group" aria-label="...">
                        <asp:Button runat="server" ID="btn_submit" CssClass="btn btn-default" Text="Најави се" OnClick="btn_submit_Click" />
                    </div>
                </div>
                <asp:Panel runat="server" ID="p_message" Visible="false">
                    <div>Вашето корисничко име и/или лозинка се погрешни!</div>
                </asp:Panel>
            </div>
        </div>
    </div>--%>

    <section class="main-section">
        <div class="container">
            <div class="row">
                <div style="margin-top: 90px;"></div>
                <h1 class="text-center">е бонитети.мк - админ</h1>

                <div class="login col-sm-6 col-sm-offset-3">
                    <div class="margin-bottom">
                        <label for="tb_username">Корисничко име</label>
                        <asp:TextBox runat="server" ID="tb_username" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="margin-bottom">
                        <label for="tb_password">Лозинка</label>
                        <asp:TextBox runat="server" ID="tb_password" TextMode="Password" placeholder="Лозинка" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div>
                        <asp:Button runat="server" ID="btn_submit" CssClass="btn btn-default" Text="Најави се" OnClick="btn_submit_Click" />
                    </div>
                    <asp:Panel runat="server" ID="p_message" Visible="false">
                        <div>Вашето корисничко име и/или лозинка се погрешни!</div>
                    </asp:Panel>
                </div>


            </div>
        </div>
    </section>


</asp:Content>
