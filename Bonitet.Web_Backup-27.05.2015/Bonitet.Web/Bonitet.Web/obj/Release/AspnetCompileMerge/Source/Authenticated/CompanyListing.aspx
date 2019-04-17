<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.Master" AutoEventWireup="true" CodeBehind="CompanyListing.aspx.cs" Inherits="Bonitet.Web.Authenticated.CompanyListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap-table.css" rel="stylesheet" />
    <link href="../css/bootstrap-select.min.css" rel="stylesheet" />

    <script src="../js/bootstrap-select.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-centered">
        <div class="container-fluid">
            <div class="panel panel-default">
                <div class="page-header">
                    <h1>ПРЕГЛЕД НА КОМПАНИИ</h1>
                </div>
                <div class="col-md-4">
                </div>
                <div class="col-md-4">
                    <p>Пребарувајте компании по ЕМБС, Назив или Адреса на субјекот.</p>
                    <p><u>Напомена</u>: Пребарувањето на "Назив" или "Седиште" на субјект да го правите со македонска поддрша (кирилица), во спротивно системот нема да Ви генерира резултати од пребарувањето.</p>
                </div>
                <div class="input-group search_container">
					<select runat="server" name="search_selector" id="search_selector" class="selectpicker" >
						<option value="name">Назив</option>
						<option value="sediste">Седиште</option>
						<option value="embs">ЕМБС</option>
					</select>
                    <input runat="server" id="search_keyword" type="text" class="form-control" placeholder="Пребарувај" />
                    <span class="input-group-btn">
                        <asp:Button OnClick="search_submit_Click" runat="server" ID="search_submit" CssClass="btn btn-default" Text="Пребарај" />
                    </span>
                </div>
                <table data-height="400" data-toggle="table" id="companies_table" data-toolbar="#transform-buttons">
                    <thead>
                        <tr>
                            <th>ЕМБС</th>
                            <th>Назив на субјект</th>
                            <th>Седиште</th>
                            <th>Статус на субјектот</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="r_Companies" runat="server">
                            <ItemTemplate>
                                <tr id="tr_id_<%# Container.ItemIndex + 1 %>" class="tr-class-<%# Container.ItemIndex + 1 %>">
                                    <td><%# Eval("EMBS") %></td>
                                    <td><%# Eval("CelosenNazivNaSubjektot") %></td>
                                    <td><%# Eval("Sediste") %></td>
                                    <td><%# Eval("Status") %></td>
                                    <td><a href="/Authenticated/CompanyDetails.aspx?id=<%# Eval("PK_Subjekt") %>">Нарачај</a></td>
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
    <script src="../js/companies_scripts.js"></script>

</asp:Content>
