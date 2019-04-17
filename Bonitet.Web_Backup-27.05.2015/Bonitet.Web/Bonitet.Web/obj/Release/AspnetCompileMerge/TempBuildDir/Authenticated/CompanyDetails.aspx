<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="CompanyDetails.aspx.cs" Inherits="Bonitet.Web.Authenticated.CompanyDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/scripts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-centered">
        <div class="container-fluid">
            <div class="panel panel-default">
                <input runat="server" type="hidden" id="c_CurCompanyID" class="cur_company_id" />
                <div class="page-header">
                    <a href="/Authenticated/CompanyListing.aspx" type="button" class="btn btn-default pull-left">Назад</a>
                    <h1>Информации за компанија</h1>
                </div>
                <div class="panel-body">
                    <p></p>
                </div>
                <div class="form-group">
                    <label class="control-label" for="p_username">ЕМБС: </label>
                    <div class="btn-group ">
                        <asp:TextBox ReadOnly="true" CssClass="form-control cur_embs" runat="server" ID="c_embs" />
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label" for="p_email">Назив: </label>
                    <div class="btn-group ">
                        <asp:TextBox ReadOnly="true" CssClass="form-control" runat="server" ID="c_naziv" />
                    </div>
                </div>
                <%--                <div class="form-group">
                    <label class="control-label" for="p_email">Датум на основање: </label>
                    <div class="btn-group ">
                        <asp:TextBox ReadOnly="true" CssClass="form-control" runat="server" ID="c_datum" />
                    </div>
                </div>--%>
                <div class="panel-body">
                    <div class="btn-group" role="group">
                        <button type="button" runat="server" id="report1" onclick="ResetFieldsBonitet()" class="btn btn-default" data-toggle="modal" data-target="#bonitet_modal_pobaraj">Нарачај бонитетен извештај</button>
                    </div>
                    <div class="btn-group" role="group">
                        <button runat="server" id="short_report" type="button" class="btn btn-default" data-toggle="modal" onclick="ResetFieldsPDF()" data-target="#short_modal">Преземи Краток PDF</button>
                        <button runat="server" id="email_for_report" type="button" onclick="SendEmail(2); ResetFieldsShort();" class="btn btn-default" data-toggle="modal" data-target="#short_modal_pobaraj">Нарачај финансиски преглед</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div class="modal fade" id="bonitet_modal_pobaraj" tabindex="-1" role="dialog" aria-labelledby="pdf_modal_label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title waiting_msg_bonitet">Ве молиме почекајте...</h4>
                    <h4 style="display:none;" class="modal-title success_msg_bonitet">Успешно праќање!</h4>
                    <h4 style="display:none;" class="modal-title error_msg_bonitet">Грешка!</h4>
                    <h4 style="display:none;" class="modal-title confirm_msg_bonitet">Потврди нарачка</h4>
                </div>
                <div class="modal-body">
                    <img style="display:none;" width="25" height="25" class="loader_bonitet" src="../img/loader.gif" />
                    <p id="info_bonitet">
                        Цената за нарачка на овој извештај изнесува 2.000 денари + ддв.
                        <br />
                        Ве молиме потврдете ја нарачката.
                    </p>
                    <p id="success_bontitet">Ви благодариме за извршената нарачка. Извештајот ќе го добиете на вашата е-пошта најдоцна 48 часа по извршената нарачката.<br />
                        Тимот на Ебонитети.мк
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" id="povtrdi_bonitet" onclick="SendEmail(1)">Потврди</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Затвори</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="short_modal_pobaraj" tabindex="-1" role="dialog" aria-labelledby="pdf_modal_label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title waiting_msg_short">Ве молиме почекајте...</h4>
                    <h4 style="display:none;" class="modal-title success_msg_short">Успешно праќање!</h4>
                    <h4 style="display:none;" class="modal-title error_msg_short">Грешка!</h4>
                </div>
                <div class="modal-body">
                    <img width="25" height="25" class="loader_short" src="../img/loader.gif" />
                    <p style="display:none;" id="success_short_msg">Вашето барање се обработува. Во најскоро време ќе добиете е-пошта со инструкции како да го превземете овој извештај.<br /><br />
                        Ви благодариме за нарачката.<br />  <br /> 
                        Тимот на Ебонитети.мк
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Затвори</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="short_modal" tabindex="-1" role="dialog" aria-labelledby="pdf_modal_label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title short_pdf_confirm">Дали сакате да го преземете документот?</h4>
                    <h4 class="modal-title short_pdf_waiting">Ве молиме почекајте...</h4>
                    <h4 class="modal-title short_pdf_error">Грешка!</h4>
                </div>
                <div class="modal-body">
                    <p id="short_pdf_error_message">Почитувани,<br/>Вашиот лимит за нарачка на скратени биланси е достигнат или профилот е сеуште неактивен.<br/>Ве молиме контактирајте ја нашата грижа за корисници на телефон 02/3117-100 или пишете ни на <a href="mailto:boniteti@targetgroup.mk">boniteti@targetgroup.mk</a>.<br/><br/>Ви благодариме.<br/>Тимот на Ебонитети.мк</p>
                </div>
                <div class="modal-footer">
                    <img style="display:none;" width="25" height="25" class="loader_short_pdf" src="../img/loader.gif" />
                    <button type="button" class="btn btn-default" id="generate_short_pdf" onclick="GeneratePDF(2)">Да</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Откажи</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
