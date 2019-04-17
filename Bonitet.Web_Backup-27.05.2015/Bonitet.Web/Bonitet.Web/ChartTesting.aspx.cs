using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace Bonitet.Web
{
    public partial class ChartTesting : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {


            Chart2.AntiAliasing = AntiAliasingStyles.All;
            Chart2.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            Chart2.Width = 500; //SET HEIGHT
            Chart2.Height = 250; //SET WIDTH

            ChartArea ca = new ChartArea();

            ca.BackColor = Color.FromArgb(255, 255, 255);
            ca.BackSecondaryColor = Color.FromArgb(255, 255, 255);
            ca.BackGradientStyle = GradientStyle.TopBottom;

            ca.AxisY.IsMarksNextToAxis = true;
            ca.AxisY.Title = " ";
            ca.AxisY.LineColor = Color.FromArgb(157, 157, 157);
            ca.AxisY.MajorTickMark.Enabled = true;
            ca.AxisY.MinorTickMark.Enabled = true;
            ca.AxisY.MajorTickMark.LineColor = Color.FromArgb(157, 157, 157);
            ca.AxisY.MinorTickMark.LineColor = Color.FromArgb(200, 200, 200);
            ca.AxisY.LabelStyle.ForeColor = Color.FromArgb(89, 89, 89);
            ca.AxisY.LabelStyle.Format = "{0:0.0}";

            ca.AxisY.LabelStyle.IsEndLabelVisible = true;
            ca.AxisY.LabelStyle.Font = new Font("Calibri", 4, FontStyle.Regular);
            ca.AxisY.MajorGrid.LineColor = Color.FromArgb(234, 234, 234);

            ca.AxisX.IsMarksNextToAxis = true;
            ca.AxisX.LabelStyle.Enabled = false;
            ca.AxisX.LineColor = Color.FromArgb(157, 157, 157);
            ca.AxisX.MajorGrid.LineWidth = 0;
            ca.AxisX.MajorTickMark.Enabled = true;
            ca.AxisX.MinorTickMark.Enabled = true;
            ca.AxisX.MajorTickMark.LineColor = Color.FromArgb(157, 157, 157);
            ca.AxisX.MinorTickMark.LineColor = Color.FromArgb(200, 200, 200);

            Chart2.ChartAreas.Add(ca);


            Series ss1 = new Series();
            ss1.ChartType = SeriesChartType.Bar;
            ss1.Font = new Font("Lucida Sans Unicode", 6f);
            ss1.Color = Color.FromArgb(255, 250, 250);
            ss1.BorderColor = Color.Transparent;
            ss1.BackSecondaryColor = Color.FromArgb(30, 144, 255);
            ss1.BackGradientStyle = GradientStyle.LeftRight;
            ss1.Name = "ss1";
            ss1.Legend = "ss1";


            Series ss2 = new Series();
            ss2.ChartType = SeriesChartType.Bar;
            ss2.Font = new Font("Lucida Sans Unicode", 6f);
            ss2.Color = Color.FromArgb(255, 250, 250);
            ss2.BorderColor = Color.Transparent;
            ss2.BackSecondaryColor = Color.FromArgb(255, 69, 0);
            ss2.BackGradientStyle = GradientStyle.LeftRight;
            ss2.Name = "ss2";
            ss2.Legend = "ss2";


            Series spacer1 = new Series();
            spacer1.ChartType = SeriesChartType.Bar;
            spacer1.Font = new Font("Lucida Sans Unicode", 6f);
            spacer1.Color = Color.FromArgb(255, 255, 255);
            spacer1.BorderColor = Color.Transparent;
            spacer1.BackSecondaryColor = Color.FromArgb(255, 255, 255);
            spacer1.BackGradientStyle = GradientStyle.None;
            spacer1.Name = "spacer1";
            spacer1.Legend = "spacer1";

            var sourceData = new double[2];

            sourceData[0] = 3651831238;
            sourceData[1] = 3627955814;

            int i = 0;
            foreach (var dr in sourceData)
            {
                DataPoint p = new DataPoint();
                p.XValue = i;
                p.YValues = new Double[] { dr };
                p.Font = new Font("Calibri", 8, FontStyle.Regular);
                Chart1.Series["s1"].Points.Add(p);
                ss1.Points.Add(p);
                i++;
            }

            sourceData[0] = 4036025701;
            sourceData[1] = 4038748473;

            i = 0;
            foreach (var dr in sourceData)
            {
                DataPoint p = new DataPoint();
                p.XValue = i;
                p.YValues = new Double[] { dr };
                p.Font = new Font("Calibri", 8, FontStyle.Regular);
                Chart1.Series["s2"].Points.Add(p);
                Chart1.Series["s2"].YValueType = ChartValueType.Int64;
                ss2.Points.Add(p);
                i++;
            }

            Chart1.Series["s1"].Points.Add(new DataPoint());



            Chart2.Series.Add(ss1);
            //Chart2.Series.Add(spacer1);
            //Chart2.Series.Add(ss2);

            Chart2.Series["ss1"]["PointWidth"] = (0.4).ToString();
            //Chart2.Series["ss2"]["PointWidth"] = (0.4).ToString();
            //Chart2.Series["spacer1"]["PointWidth"] = (2).ToString();

            Chart2.Visible = false;

        }

        protected void Chart4_Load(object sender, EventArgs e)
        {

        }
    }
}