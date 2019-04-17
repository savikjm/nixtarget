<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="Pricelist.aspx.cs" Inherits="Bonitet.Web.Pricelist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="main-content">
    <div style="margin-bottom: 70px;"></div>
    <div class="container">
        <h1>Ценовник</h1>

        <div class="row">
            <div class="middle-row">

                <div class="col-md-8 col-md-offset-2">
                    <div class="panel sodrzina">
                        <div class="panel-heading">
                            <h3 class="panel-title">Годишната претплата:<b> 5.000 денaри + ддв.</b></h3>
                        </div>
                        <div class="panel-body">
                            <ul class="list-group">
                                 <li class="list-group-item">Претплатата овозможува инстантна нарачка на бонитетни извештаи, финансиски прегледи,  проверка на блокада на сметка, преглед на статус (активни/неактивни) и архивирање на нарачките.</li>
                                <li class="list-group-item">Со претплатата добивате до <strong>50% попуст</strong> на извештаите.</li>
                                <li class="list-group-item"><strong>Бонитетен извештај:</strong> 2.500 ден. + ддв / со претплата: <strong>1.500 ден. + ддв.</strong></li>
                                <li class="list-group-item"><strong>Финансиски преглед:</strong> 200 ден. + ддв / со претплата: <strong>100 ден. + ддв.</strong></li>
                                <li class="list-group-item"><strong>Проверка на блокада на сметка:</strong> 200 ден. + ддв / со претплата: <strong>100 ден. + ддв.</strong></li>
                            </ul>
                        </div>
                    </div>
                </div>

<%--                <div class="col-md-8 col-md-offset-2">
                    <div class="panel sodrzina">
                        <div class="panel-heading">
                            <h3 class="panel-title">Дополнителни пакети за финансиски преглед на компании:</h3>
                        </div>
                        <div class="panel-body">
                            <ul class="list-group">
                                <li class="list-group-item"><b>+10</b> финансиски прегледи: <b>1.500 ден. + ддв</b></li>
                                <li class="list-group-item"><b>+50</b> финансиски прегледи: <b>4.000 ден. + ддв</b></li>
                            </ul>
                        </div>
                    </div>
                </div>--%>

                <div class="col-md-8 col-md-offset-2">
                    <div class="panel dopolnitelno">
                        <div class="panel-heading">
                            <h3 class="panel-title">Дополнителни Информации</h3>
                        </div>
                        <div class="panel-body">
                            <p>За сите ваши прашања контактирајте ја нашата грижа за корисници на тел: <b>02/3117-100</b> или <a href="mailto:support@targetgroup.mk">support@targetgroup.mk</a>
                            </p>
                            <p>Тимот на Ебонитети</p>
                        </div>
                    </div>
                </div>


            </div>
        </div>

      
    </div>
</section>


</asp:Content>
