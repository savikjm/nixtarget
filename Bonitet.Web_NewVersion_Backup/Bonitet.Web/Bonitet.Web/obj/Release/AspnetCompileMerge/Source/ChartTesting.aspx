<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChartTesting.aspx.cs" Inherits="Bonitet.Web.ChartTesting" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Chart ID="Chart1" runat="server" Height="250" Width="500" Enabled="False" Visible="False">
            <Series>
                <asp:Series Name="s1" BackGradientStyle="LeftRight" BackSecondaryColor="DodgerBlue" BorderColor="Transparent" ChartType="Bar" Color="Snow" CustomProperties="PointWidth=0.4" Font="Lucida Sans Unicode, 8.25pt"></asp:Series>
                <asp:Series BackSecondaryColor="White" BorderColor="Transparent" ChartArea="ChartArea1" ChartType="Bar" Color="White" Name="spacer" CustomProperties="PointWidth=2" >
                </asp:Series>
                <asp:Series BackGradientStyle="LeftRight" BackSecondaryColor="OrangeRed" BorderColor="Transparent" ChartArea="ChartArea1" ChartType="Bar" Color="Snow" CustomProperties="PointWidth=0.4" Font="Lucida Sans Unicode, 8.25pt" Name="s2">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1" BackColor="White" BackGradientStyle="TopBottom" BackSecondaryColor="White">
                    <AxisY IsLabelAutoFit="False" LineColor="157, 157, 157">
                        <MajorGrid LineColor="234, 234, 234" />
                        <MajorTickMark LineColor="157, 157, 157" />
                        <MinorTickMark Enabled="True" LineColor="200, 200, 200" />
                        <LabelStyle Font="Calibri, 8.25pt" ForeColor="89, 89, 89" Format="{0:0:0}" />
                    </AxisY>
                    <AxisX LineColor="157, 157, 157">
                        <MajorGrid LineWidth="0" />
                        <MajorTickMark LineColor="157, 157, 157" />
                        <MinorTickMark Enabled="True" LineColor="200, 200, 200" />
                        <LabelStyle Enabled="False" />
                    </AxisX>
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>

        <asp:Chart ID="Chart3" runat="server" Width="600px">
            <Series>
                <asp:Series ChartType="Pie" Name="Series1" CustomProperties="PieLabelStyle=Outside, CollectedThresholdUsePercent=False">
                    <Points>
                        <asp:DataPoint Label="Loooooooooooooooooooong label" XValue="70" YValues="70" />
                        <asp:DataPoint Label="Shoooooooooooooooort label" XValue="50" YValues="50" />
                        <asp:DataPoint Label="asdfasdfasdfasdf" XValue="10" YValues="10" />
                    </Points>
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>

        <asp:Chart ID="Chart2" runat="server">
            <Series>
            </Series>
            <ChartAreas>
            </ChartAreas>
        </asp:Chart>

        <asp:Chart ID="Chart4" runat="server" Width="704px">
            <Series>
                <asp:Series ChartType="Pie" Name="Series1" CustomProperties="PieLabelStyle=Outside, CollectedThresholdUsePercent=False">
                    <Points>
                        <asp:DataPoint Label="label 1" XValue="50" YValues="50" />
                        <asp:DataPoint Label="label 2" XValue="20" YValues="20" />
                        <asp:DataPoint Label="label 3" XValue="10" YValues="10" />
                        <asp:DataPoint Label="label 4" XValue="40" YValues="40" />
                    </Points>
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>

    </div>
    </form>
</body>
</html>
