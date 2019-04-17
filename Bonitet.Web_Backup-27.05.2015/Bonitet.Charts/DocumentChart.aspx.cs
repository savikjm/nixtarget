using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace Bonitet.Charts
{
    public partial class DocumentChart : System.Web.UI.Page
    {
        public static string webRootPath = System.Web.HttpContext.Current.Server.MapPath("~");

        public static string AbsolutePath = Path.GetFullPath(Path.Combine(webRootPath, "..\\Bonitet.Web\\App_Data\\chart_images\\"));


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public static void SetChartProperties(Chart c, int width, int height)
        {
            c.AntiAliasing = AntiAliasingStyles.All;
            c.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            c.Width = width; //SET HEIGHT
            c.Height = height; //SET WIDTH
        }

        public static void SetChartAreaProperties(ChartArea ca)
        {
            ca.BackColor = Color.FromArgb(255, 255, 255);
            ca.BackSecondaryColor = Color.FromArgb(255, 255, 255);
            ca.BackGradientStyle = GradientStyle.TopBottom;
        }

        public static void SetBarChartProperties(ChartArea ca)
        {
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
        }

        public static void SetPieChartProperties(ChartArea ca)
        {
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
        }

        public static Series CreateSeriesByType(SeriesChartType type, GradientStyle style, int r, int g, int b, string name)
        {
            Series s1 = new Series();
            s1.ChartType = type;
            s1.Font = new Font("Lucida Sans Unicode", 9f);
            s1.Color = Color.FromArgb(255, 250, 250);
            s1.BorderColor = Color.Transparent;
            s1.BackSecondaryColor = Color.FromArgb(r, g, b);
            s1.BackGradientStyle = style;
            s1.Name = name;
            s1.Legend = name;
            return s1;
        }

        public static DataPoint SetDataPointForChart(int i, double val, double percent, string name)
        {
            DataPoint p = new DataPoint();
            p.XValue = i;
            p.YValues = new Double[] { val };
            p.Font = new Font("Calibri", 8, FontStyle.Regular);
            if(percent != 0.0)
                p.Label = name + "; " + percent + " %";
            return p;
        }

        public static string CreateBarChart(double trosoci, double trosoci_lastyear, double prihodi, double prihodi_lastyear)
        {
            Chart c = new Chart();

            SetChartProperties(c, 500, 250);

            ChartArea ca = new ChartArea();

            SetChartAreaProperties(ca);
            SetBarChartProperties(ca);

            c.ChartAreas.Add(ca);

            var s1 = CreateSeriesByType(SeriesChartType.Bar, GradientStyle.LeftRight, 30, 144, 255, "s1");
            var spacer = CreateSeriesByType(SeriesChartType.Bar, GradientStyle.None, 255, 255, 255, "spacer");
            var s2 = CreateSeriesByType(SeriesChartType.Bar, GradientStyle.LeftRight, 255, 69, 0, "s2");


            var sourceData = new double[2];

            sourceData[0] = prihodi_lastyear;
            sourceData[1] = trosoci_lastyear;

            int i = 0;
            foreach (var dr in sourceData)
            {
                var p = SetDataPointForChart(i, dr, 0, "");
                s1.Points.Add(p);
                i++;
            }

            sourceData[0] = prihodi;
            sourceData[1] = trosoci; 

            i = 0;
            foreach (var dr in sourceData)
            {
                var p = SetDataPointForChart(i, dr, 0, "");
                s2.Points.Add(p);
                s2.YValueType = ChartValueType.Int64;
                i++;
            }

            c.Series.Add(s1);
            c.Series.Add(spacer);
            c.Series.Add(s2);


            c.Series["s1"]["PointWidth"] = "0.4";
            c.Series["s2"]["PointWidth"] = "0.4";
            c.Series["spacer"]["PointWidth"] = "2";

            var filename = Guid.NewGuid().ToString();

            filename += ".png";

            c.SaveImage(AbsolutePath + filename, ChartImageFormat.Png);

            return filename;
        }

        public static string CreatePieChart(int PieType, double val1, double val2, double val3, double val4, double val5)
        {
            Chart c = new Chart();

            SetChartProperties(c, 800, 380);

            ChartArea ca = new ChartArea();

            SetChartAreaProperties(ca);
            SetPieChartProperties(ca);

            c.ChartAreas.Add(ca);

            var s1 = CreateSeriesByType(SeriesChartType.Pie, GradientStyle.None, 30, 144, 255, "s1");

            var total = val1 + val2 + val3 + val4 + val5;
            var p1 = Convert.ToDouble(((val1 / total) * 100).ToString("0.00"));
            var p2 = Convert.ToDouble(((val2 / total) * 100).ToString("0.00"));
            var p3 = Convert.ToDouble(((val3 / total) * 100).ToString("0.00"));
            var p4 = Convert.ToDouble(((val4 / total) * 100).ToString("0.00"));
            var p5 = Convert.ToDouble(((val5 / total) * 100).ToString("0.00"));

            var sourceData = new SeriesData[5];

            if(PieType == 1)
            {
                sourceData[0] = new SeriesData { Percent = p1, Value = val1, Name = "ТЕКОВНИ СРЕДСТВА" };
                sourceData[1] = new SeriesData { Percent = p2, Value = val2, Name = "НЕТЕКОВНИ СРЕДСТВА" };
                sourceData[2] = new SeriesData { Percent = p3, Value = val3, Name = "СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ ...)" };
                sourceData[3] = new SeriesData { Percent = p4, Value = val4, Name = "ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ" };
                sourceData[4] = new SeriesData { Percent = p5, Value = val5, Name = "АВР" };
            }
            else
            {
                sourceData[0] = new SeriesData { Percent = p1, Value = val1, Name = "ОБВРСКИ" };
                sourceData[1] = new SeriesData { Percent = p2, Value = val2, Name = "ГЛАВНИНА И РЕЗЕРВИ" };
                sourceData[2] = new SeriesData { Percent = p3, Value = val3, Name = "ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА..." };
                sourceData[3] = new SeriesData { Percent = p4, Value = val4, Name = "ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ" };
                sourceData[4] = new SeriesData { Percent = p5, Value = val5, Name = "ПВР" };
            }
            int i = 0;
            foreach (var item in sourceData)
            {
                var p = SetDataPointForChart(i, item.Value, item.Percent, item.Name);
                s1.Points.Add(p);
                s1.YValueType = ChartValueType.Int64;
                i++;
            }

            Color[] myPalette = new Color[5]{
                Color.FromArgb(91, 155, 213),
                Color.FromArgb(68, 114, 196),
                Color.FromArgb(165, 165, 165),
                Color.FromArgb(37, 94, 145),
                Color.FromArgb(38, 68, 120),
            };

            c.Palette = ChartColorPalette.Excel;

            c.Series.Add(s1);

            c.Series["s1"]["PieLabelStyle"] = "Outside";
            c.Series["s1"]["MinimumRelativePieSize"] = "50";
            c.Series["s1"]["PieLineColor"] = "ActiveCaptionText";
            var filename = Guid.NewGuid().ToString();

            filename += ".png";

            c.SaveImage(AbsolutePath + filename, ChartImageFormat.Png);

            return filename;
        }
    }

    public struct SeriesData
    {
        public double Value { get; set; }
        public double Percent { get; set; }
        public string Name { get; set; }
    }
}