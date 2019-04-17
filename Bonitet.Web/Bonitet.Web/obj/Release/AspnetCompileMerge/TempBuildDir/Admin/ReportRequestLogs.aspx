<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="ReportRequestLogs.aspx.cs" Inherits="Bonitet.Web.Admin.ReportRequestLogs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../css/bootstrap-table.css" rel="stylesheet" />
    <link href="../css/bootstrap-select.min.css" rel="stylesheet" />
    <script src="../js/scripts.js"></script>

    <script src="../js/bootstrap-select.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="main-content">
        <div style="margin-bottom: 70px;"></div>
        <div class="container">
            <div class="row">
                <h1>Барања за извештаи</h1>

                <div class="napomena">
                    <div class="panel">
                        <div class="panel-heading">
                            <h3 class="panel-title">Филтрирај ги резултатите</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-md-6 col-md-offset-3">
                                <div class="searching" style="margin:0">
                                    <span style="margin-right: 10px;">Филтрирај: </span>
                                    <asp:DropDownList runat="server" name="search_selector" id="search_selector" class="selectpicker" AutoPostBack="true" OnSelectedIndexChanged="search_selector_SelectedIndexChanged">
                                        <asp:ListItem Value="1">All</asp:ListItem>
                                        <asp:ListItem Value="2">Pending Data</asp:ListItem>
                                        <asp:ListItem Value="3">Available</asp:ListItem>
                                        <asp:ListItem Value="4">Downloaded</asp:ListItem>
                                        <asp:ListItem Value="5">No Data</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>                        

                        </div>
                    </div>
                </div>
            </div>
        </div>

        

        <div class="container-fluid">
            <div class="row">
            <div class="search-results">
                <div class="table-responsive">
                    <table class="table table-bordered table-hover" id="data">
                        <thead>
                        <tr>
                            <th class="created_on" style="width:165px;">Датум</th>
                            <th class="embs">ЕМБС</th>
                            <th class="company_name">Име на компанија</th>
                            <th class="username">Клиент</th>
                            <th class="year">Година</th>
                            <th class="report_type">Тип</th>
                            <th class="status_text">Статус</th>
                        </tr>
                        </thead>
                        <tbody class="text-left">
                            <asp:Repeater ID="r_ReportRequests" runat="server">
                                <ItemTemplate>
                                    <tr id="tr_id_<%# Container.ItemIndex + 1 %>" class="tr-class-<%# Container.ItemIndex + 1 %> item-id<%# Eval("ID")  %>">
                                        <td class="created_on" style="width:165px;"><%# Eval("CreatedOn") %></td>
                                        <td class="embs"><%# Eval("EMBS") %></td>
                                        <td class="company_name"><%# Eval("CompanyName") %></td>
                                        <td class="username"><%# Eval("Username") %></td>
                                        <td class="year"><%# Eval("Year") %></td>
                                        <td class="report_type"><%# Eval("ReportTypeString") %></td>
                                        <td class="status_text"><%# Eval("StatusText") %></td>
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
    </div>
    </section>
    
    <script src="../js/bootstrap-table.js"></script>
</asp:Content>
