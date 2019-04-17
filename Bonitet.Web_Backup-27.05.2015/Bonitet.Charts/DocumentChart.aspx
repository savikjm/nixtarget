<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentChart.aspx.cs" Inherits="Bonitet.Charts.DocumentChart" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <asp:Chart ID="Chart1" runat="server" Height="458px" Width="689px">
            <series>
                <asp:Series ChartType="Pie" CustomProperties="PieLineColor=ActiveCaptionText, MinimumRelativePieSize=70, PieLabelStyle=Outside" Font="Calibri, 14px" IsValueShownAsLabel="True" IsVisibleInLegend="False" LabelBorderDashStyle="NotSet" Name="Series1">
                    <Points>
                        <asp:DataPoint Label="Test1 45%" YValues="45" />
                        <asp:DataPoint Label="Test2 40%" YValues="40" />
                        <asp:DataPoint Label="Test 3 10%" YValues="10" />
                        <asp:DataPoint Label="Test 4 4%" YValues="4" />
                        <asp:DataPoint AxisLabel="jkhjk" Label="Test 5 1%" YValues="1" />
                    </Points>
                    <SmartLabelStyle AllowOutsidePlotArea="Yes" CalloutStyle="None" IsMarkerOverlappingAllowed="True" MaxMovingDistance="60" />
                </asp:Series>
            </series>
            <chartareas>
                <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
            </chartareas>
        </asp:Chart>
        <asp:Chart ID="Chart2" runat="server">
            <Series>
                <asp:Series ChartType="Bar" Name="Series1">
                    <Points>
                        <asp:DataPoint IsVisibleInLegend="True" XValue="5" YValues="5" />
                    </Points>
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
    </form>
</body>
</html>
