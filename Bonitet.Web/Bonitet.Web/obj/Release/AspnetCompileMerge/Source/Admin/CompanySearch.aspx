<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="CompanySearch.aspx.cs" Inherits="Bonitet.Web.Admin.CompanySearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="main-content">
        <div style="margin-bottom: 70px;"></div>
        <div class="container">
            <div class="row">
                <h1>Пребарување компании по ЕМБС</h1>


                <div class="napomena">
                    <div class="panel">
                        <div class="panel-heading">
                            <h3 class="panel-title">Пребарувајте компании по ЕМБС</h3>
                        </div>
                        <div class="panel-body">
                            <div class="btn-group col-xs-12">
                                <asp:Label runat="server" ID="error_msg"></asp:Label>
                            </div>
                            <div class="col-md-6 col-md-offset-3">
                                <div class="searching">
                                    <div class="input-group">
                                      
                                        <!-- /btn-group -->
                                        <input  runat="server" id="search_keyword" type="text" class="form-control" placeholder="ЕМБС"/>
                                        <span class="input-group-btn">
                                            <asp:Button OnClick="search_submit_Click" runat="server" ID="search_submit" CssClass="btn btn-default" Text="Пребарај" />
                                        </span>
                                        
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
