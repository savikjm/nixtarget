<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="GenerateReport.aspx.cs" Inherits="Bonitet.Web.Admin.GenerateReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/scripts.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="main-content">
        <div style="margin-bottom: 70px;"></div>

        <div class="container">
            <div class="row">
                <h1>Информации за компанија</h1>
                <input runat="server" type="hidden" id="c_CurCompanyID" class="cur_company_id" />
                <div class="col-md-10 col-md-offset-1">
                    <div class="my_info">
                        <div class="panel">
                            <div class="panel-heading">
                                <h3 class="panel-title">Информации за компанија</h3>
                            </div>
                            <div class="panel-body">
                                    <div class=" col-md-8 col-md-offset-2">
                                        <label for="c_embs">ЕМБС: </label>           
                                        <asp:TextBox ReadOnly="true" CssClass="form-control cur_embs" runat="server" ID="c_embs" />
                                    </div>
                                    <div class=" col-md-8 col-md-offset-2">
                                        <label for="c_naziv">Назив: </label>                     
                                        <asp:TextBox ReadOnly="true" CssClass="form-control" runat="server" ID="c_naziv" />
                                    </div>
                                    <div class=" col-md-8 col-md-offset-2">
                                        <label for="client_embs">Клиент ЕМБС: </label>           
                                        <asp:TextBox CssClass="form-control client_embs" runat="server" ID="client_embs" />
                                    </div>
                                    <div class="col-md-8 col-md-offset-2" style="margin-top: 20px;">
                                        <div class="col-md-6" style="padding: 0;">
                                            <button runat="server" id="crm_report" type="button" class="btn custom_btn" data-toggle="modal" onclick="ResetFieldsCRMPDF()" data-target="#crm_modal">Преземи PDF</button>
                                        </div>
                                        <div class="col-md-6" style="padding: 0;">
                                            <a href="/Admin/CompanySearch.aspx" type="button" class="btn btn-default mybtn pull-right custom_btn">Назад</a>
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
</asp:Content>
