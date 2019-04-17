<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Bonitet.Web.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                    <p>Пристап до информациите имаат само регистрирани корисници.</p>
                    <p>Ве молиме најавете се со Вашето корисничко име и лозинка.</p>
                    <p>Доколку не сте корисник, повеќе информации можете да</p>
                    <p>добиете на тел. +389 2 3117 100 или на <a href="mailto:info@targetgroup.mk">info@targetgroup.mk</a></p>
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
                    <div class="btn-group">
                        <a href="#" data-toggle="modal" data-target="#forgot_password" onclick="ResetLostPasswordFields()">Заборавена лозинка?</a>
                    </div>
                </div>
                <div class="panel-body">
                    <div class="btn-group">
                        <a target="_blank" href="/img/Општи%20услови%20-%20Eбонитети%20мк.pdf"><b>Општи услови</b></a>
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
                <asp:Panel runat="server" ID="p_disabled" Visible="false">
                    <div>Вашиот профил не е активен!</div>
                </asp:Panel>
                <div class="panel-body">
                    <h3>Проверете ја финансиската состојба за Вашите деловни партнери или конкуренцијата.</h3>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="forgot_password" tabindex="-1" role="dialog" aria-labelledby="pdf_modal_label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">Заборавена лозинка</h4>
                </div>
                <div class="modal-body">
                    <img style="display:none;" width="25" height="25" class="loader_password" src="../img/loader.gif" />
                    <p id="msg_txt">Внесете е-пошта со која сте регистрирани на нашиот портал и ќе ви биде пратен мејл за ресетирање на лозинката.</p>
                    <p id="msg_success">Успешно праќање. Ве молиме проверете ја вашата е-пошта.</p>
                    <div class="panel-body">
                        <div class="btn-group ">
                            <input type="text" id="user_email" placeholder="Вашата е-пошта" class="form-control" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" id="email_submit_btn" onclick="LostPassword();" class="btn btn-default">Испрати</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Затвори</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
