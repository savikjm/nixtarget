<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="PasswordReset.aspx.cs" Inherits="Bonitet.Web.PasswordReset" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/scripts.js"></script>
    <script>

        $(document).ready(function () {
            ResetResetPasswordFields();
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-centered">
        <div class="container-fluid">
            <div class="panel panel-default">
                <div class="page-header">
                    <h1>еБонитети.мк - за брзи и квалитетни деловни одлуки!</h1>
                    <img src="img/Eboniteti%20logo.png" />
                </div>
                <div class="panel-body">
                    <div>
                        <p id="reset_msg">За промена на вашата лозинка кликнете <a href="#" onclick="ResetResetPasswordFields();ResetPassword();return false;">тука</a>. Ќе ви биде испратен меил со вашата нова лозинка.</p>
                        <p style="display:none;" id="reset_error_msg">Проблем при ресетирање на вашата лозинка.</p>
                        <p style="display:none;" id="reset_success_msg">Вашата лозинка е ресетирана! Проверете ја вашата Е-пошта.</p>
                        <p style="display:none;" id="reset_expired_msg">Неважечки линк! Ако сакате да ја ресетирате вашата лозинка кликнете <a href="/Default.aspx">тука</a>.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
