<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="Bonitet.Web.Authenticated.EditUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap-table.css" rel="stylesheet" />
    <script src="../js/scripts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="main-content">
    <div style="margin-bottom: 70px;"></div>
    <div class="container">
        <h1>Промени Информации</h1>

        <div class="row">

            <div class="col-md-10 col-md-offset-1">
                <div class="my_info">
                    <div class="panel">
                        <div class="panel-heading">
                            <h3 class="panel-title">Информации за корисникот</h3>
                        </div>
                        <div class="panel-body">
                            <div class="form-group">
                               <label runat="server" id="error_text" class="control-label" >Грешка!</label>
                            </div>
                                <div class=" col-md-8 col-md-offset-2">
                                    <label for="username">Корисничко име</label>
                                   <%-- <input type="text" id="username" name="username"/>--%>
                                     <asp:TextBox CssClass="form-control cur_embs" runat="server" ID="c_username" ReadOnly="true"/>
                                </div>
                                <div class=" col-md-8 col-md-offset-2">
                                    <label for="password">Лозинка</label>
                                    <%--<input type="password" id="password" name="password"/>--%>
                                    <asp:TextBox CssClass="form-control" runat="server" ID="c_password" />
                                </div>
                                <div class=" col-md-8 col-md-offset-2">
                                    <label for="email">Е-пошта</label>
                                    <%--<input type="email" id="email" name="email"/>--%>
                                    <asp:TextBox CssClass="form-control" runat="server" ID="c_email" />
                                </div>
                                <div class="col-md-8 col-md-offset-2" style="margin-top: 20px;">
                                    <div class="col-md-6" style="padding: 0;">
                                        <%--<button type="submit" class="btn btn-default mybtn pull-left">Зачувај</button>--%>
                                        <asp:Button OnClick="save_user_Click" id="save_user" CssClass="btn custom_btn" runat="server" Text="Зачувај"/>
                                    </div>
                                    <div class="col-md-6" style="padding: 0;">
                                        <a id="Button1" href="/Admin/UserListing.aspx" class="btn btn-default mybtn pull-right custom_btn">Назад</a>
                                    </div>
                                </div>
                           
                        </div>
                    </div>

                </div>
            </div>


        </div>


    </div>
</section>



</asp:Content>
