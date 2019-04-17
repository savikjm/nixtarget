<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="CompanyDetails.aspx.cs" Inherits="Bonitet.Web.Authenticated.CompanyDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/scripts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="main-content">
    <div style="margin-bottom: 70px;"></div>

    <div class="container">
        <h1>Информации за Kомпанија</h1>

        <div class="row">
            <input runat="server" type="hidden" id="c_CurCompanyID" class="cur_company_id" />
            <div class="col-md-10 col-md-offset-1">
                <div class="my_info">
                    <div class="panel">
                        <div class="panel-heading">
                            <a href="/Authenticated/CompanyListing.aspx" class="btn btn-back pull-left"><span class="hidden-xs">Назад</span> <i class="fa fa-chevron-left visible-xs"></i></a>
                            <h3 class="panel-title">Информации за компанијата</h3>
                        </div>
                        <div class="panel-body">
                            
                                <div class=" col-md-8 col-md-offset-2">
                                    <label for="username">ЕМБС</label>
                                    <%--<input type="text" id="username" name="username" readonly="readonly" value="6337376"/>--%>
                                    <asp:TextBox ReadOnly="true" CssClass="form-control cur_embs" runat="server" ID="c_embs" />
                                </div>
                                <div class=" col-md-8 col-md-offset-2">
                                    <label for="password">Назив</label>
<%--                                    <input type="text" id="password" name="password" readonly="readonly" value="Друштво за обработка на стакло,производство,трговија и услуги СТАКЛО ФЛОТ АЈ ДООЕЛ експорт-импорт Тетово" />--%>
                                    <asp:TextBox ReadOnly="true" CssClass="form-control" runat="server" ID="c_naziv" />
                                </div>
                                <div class="col-md-8 col-md-offset-2" style="margin-top: 20px;">
                                    <%--<div class="col-xs-12" style="padding: 0;">
                                        <a href="#" data-toggle="modal" data-target="#basicModal" class="btn info-btn">Нарачај бонитетен извештај</a>
                                    </div>
                                    <div class="col-xs-12" style="padding: 0;">
                                        <a data-toggle="modal" data-target="#basicModal3" class="btn info-btn">Провери блокада на сметка</a>
                                    </div>
                                    <div class="col-xs-12" style="padding: 0;">
                                        <a data-toggle="modal" data-target="#basicModal2" class="btn info-btn">Нарачај финансиски преглед</a>
                                    </div>--%>
                                    <div class="panel-body">
                                        <div class="btn-group custom_group" role="group">
                                            <div class="full_width">
                                            <button runat="server" id="crm_report" type="button" class="btn info-btn" data-toggle="modal" onclick="ResetFieldsCRMPDF()" data-target="#crm_modal">Преземи PDF</button>
                                            </div>
                                            <div class="full_width">
                                                <button type="button" runat="server" id="email_for_crm_report" onclick="ResetFieldsBonitet()" class="btn info-btn" data-toggle="modal" data-target="#bonitet_modal_pobaraj">Нарачај бонитетен извештај</button>
                                            </div>
                                        </div>
                                        <div class="btn-group custom_group" role="group">
                                            <div class="full_width">
                                                <button runat="server" id="short_report" type="button" class="btn info-btn" data-toggle="modal" onclick="ResetFieldsPDF()" data-target="#short_modal">Превземи финансиски преглед</button>
                                            </div>
                                            <div class="full_width">
                                                <button runat="server" id="email_for_report" type="button" onclick="SendEmail(2); ResetFieldsShort();" class="btn info-btn" data-toggle="modal" data-target="#short_modal_pobaraj">Нарачај финансиски преглед</button>
                                            </div>
                                        </div>
                                        <div class="btn-group custom_group" role="group">
                                            <div class="full_width">
                                                <button runat="server" id="short_blokada" type="button" class="btn info-btn" data-toggle="modal" onclick="ResetFieldsBlokada()" data-target="#blokada_modal">Провери блокада</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                          
                        </div>
                    </div>

                </div>
            </div>


        </div>



    </div>

</section>




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
                        Цената за нарачка на овој извештај изнесува  – 2.500,00 денари + ддв. (освен ако не е поинаку договорено со корисникот на апликацијата)
                        <br />
                        <br />
                        Ве молиме потврдете ја нарачката.
                    </p>
                    <p id="success_bontitet">Ви благодариме за извршената нарачка. Извештајот ќе го добиете на пријавената е-пошта најдоцна 1 (еден) работен ден по извршената нарачка.
                        <br />
                        За повеќе информации контактирајте не на 02/3117-100
                        <br />
                        Тимот на Ебонитети.мк
                    </p>
                    <p id="crm_pdf_error_message1">Почитувани,<br/>Вашиот лимит за нарачка на извештаи е достигнат или профилот е сеуште неактивен.<br/>Ве молиме контактирајте ја нашата грижа за корисници на телефон 02/3117-100 или пишете ни на <a href="mailto:boniteti@targetgroup.mk">boniteti@targetgroup.mk</a>.<br/><br/>Ви благодариме.<br/>Тимот на Ебонитети.мк</p>
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
                    <p style="display:none;" id="success_short_msg">Цената за нарачка на финансиски преглед изнесува 200,00 денари + ддв. (освен ако не е поинаку договорено со корисникот на апликацијата).<br /><br />
                        Вашето барање се обработува. Во најскоро време ќе добиете е-пошта со инструкции како да го превземете овој извештај.<br /><br />
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
                    <p id="short_pdf_info_bonitet">
                        Цената за превземање на овој извештај изнесува 200,00 денари + ддв, освен ако не е поинаку договорено со корисникот на платформата Ебонитети.мк.<br /><br />
                        Вашето барање се обработува, за кратко време извештејот ќе можете да го превземете во формат ПДФ.<br /><br />
                        Ви благодариме,<br />
                        Тимот на Ебонитети.мк
                    </p>
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
    <div class="modal fade" id="crm_modal" tabindex="-1" role="dialog" aria-labelledby="pdf_modal_label" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title crm_pdf_confirm">Дали сакате да го преземете документот?</h4>
                    <h4 class="modal-title crm_pdf_waiting">Ве молиме почекајте...</h4>
                    <h4 class="modal-title crm_pdf_error">Грешка!</h4>
                </div>
                <div class="modal-body">
                    <p id="crm_pdf_error_message">Почитувани,<br/>Вашиот лимит за нарачка на извештаи е достигнат или профилот е сеуште неактивен.<br/>Ве молиме контактирајте ја нашата грижа за корисници на телефон 02/3117-100 или пишете ни на <a href="mailto:boniteti@targetgroup.mk">boniteti@targetgroup.mk</a>.<br/><br/>Ви благодариме.<br/>Тимот на Ебонитети.мк</p>
                </div>
                <div class="modal-footer">
                    <img style="display:none;" width="25" height="25" class="loader_crm_pdf" src="../img/loader.gif" />
                    <button type="button" class="btn btn-default" id="generate_crm_pdf" onclick="GeneratePDF(1)">Да</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Откажи</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="blokada_modal" tabindex="-1" role="dialog" aria-labelledby="blokada_modal" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title blokada_confirm">Дали сакате да го преземете документот?</h4>
                    <h4 class="modal-title blokada_waiting">Ве молиме почекајте...</h4>
                    <h4 class="modal-title blokada_error">Грешка!</h4>
                </div>
                <div class="modal-body">
                    <p id="blokada_info_bonitet">
                        Цената за проверка на блокада изнесува – 200,00 денари + ддв.
                    </p>
                    <p style="display:none;" id="blokada_success_bontitet">Ви благодариме за извршената нарачка. Вашиот извештај се генерира, Ве молиме почекајте.<br />
                        За повеќе информации контактирајте не на 02/3117-100<br />
                        Тимот на Ебонитети.мк
                    </p>
                    <p id="blokada_error_message">Почитувани,<br/>Вашиот лимит за проверка на блокада е достигнат или профилот е сеуште неактивен.<br/>Ве молиме контактирајте ја нашата грижа за корисници на телефон 02/3117-100 или пишете ни на <a href="mailto:boniteti@targetgroup.mk">boniteti@targetgroup.mk</a>.<br/><br/>Ви благодариме.<br/>Тимот на Ебонитети.мк</p>
                </div>
                <div class="modal-footer">
                    <img style="display:none;" width="25" height="25" class="loader_blokada_pdf" src="../img/loader.gif" />
                    <button type="button" class="btn btn-default" id="generate_blokada_pdf" onclick="GeneratePDF(3)">Да</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Откажи</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
