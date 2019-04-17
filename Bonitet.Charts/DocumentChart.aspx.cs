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

        public static string CreatePieChart(int PieType, double val1, double val2, double val3, double val4, double val5, double val6)
        {
            Chart c = new Chart();

            if (PieType > 2)
            {
                SetChartProperties(c, 800, 500);
            }
            else
            {
                SetChartProperties(c, 800, 380);
            }

            ChartArea ca = new ChartArea();

            SetChartAreaProperties(ca);
            SetPieChartProperties(ca);

            c.ChartAreas.Add(ca);

            var s1 = CreateSeriesByType(SeriesChartType.Pie, GradientStyle.None, 30, 144, 255, "s1");

            var total = val1 + val2 + val3 + val4 + val5 + val6;
            var p1 = (val1 / total) * 100;
            var p2 = (val2 / total) * 100;
            var p3 = (val3 / total) * 100;
            var p4 = (val4 / total) * 100;
            var p5 = (val5 / total) * 100;
            var p6 = (val6 / total) * 100;

            var sourceData = new SeriesData[6];

            switch (PieType)
            {
                case 1:
                    sourceData[0] = new SeriesData { Percent = p1, Value = val1, Name = "ТЕКОВНИ СРЕДСТВА" };
                    sourceData[1] = new SeriesData { Percent = p2, Value = val2, Name = "НЕТЕКОВНИ СРЕДСТВА" };
                    sourceData[2] = new SeriesData { Percent = p3, Value = val3, Name = "СРЕДСТВА (ИЛИ ГРУПИ ЗА ОТУЃУВАЊЕ ...)" };
                    sourceData[3] = new SeriesData { Percent = p4, Value = val4, Name = "ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ" };
                    sourceData[4] = new SeriesData { Percent = p5, Value = val5, Name = "АВР" };
                    break;
                case 2:
                    sourceData[0] = new SeriesData { Percent = p1, Value = val1, Name = "ОБВРСКИ" };
                    sourceData[1] = new SeriesData { Percent = p2, Value = val2, Name = "ГЛАВНИНА И РЕЗЕРВИ" };
                    sourceData[2] = new SeriesData { Percent = p3, Value = val3, Name = "ОБВРСКИ ПО ОСНОВ НА НЕТЕКОВНИ СРЕДСТВА..." };
                    sourceData[3] = new SeriesData { Percent = p4, Value = val4, Name = "ОДЛОЖЕНИ ДАНОЧНИ ОБВРСКИ" };
                    sourceData[4] = new SeriesData { Percent = p5, Value = val5, Name = "ПВР" };
                    break;
                case 11:
                    p1 = val1 / total;
                    p2 = val2 / total;
                    p3 = val3 / total;
                    p4 = val4 / total;
                    p5 = val5 / total;
                    sourceData[0] = new SeriesData { Percent = p1, Value = val1, Name = "Средства (или груп...)" };
                    sourceData[1] = new SeriesData { Percent = p2, Value = val2, Name = "Тековни средства" };
                    sourceData[2] = new SeriesData { Percent = p3, Value = val3, Name = "Претплатени трошоци" };
                    sourceData[3] = new SeriesData { Percent = p4, Value = val4, Name = "Средства" };
                    sourceData[4] = new SeriesData { Percent = p5, Value = val5, Name = "Одложени дан. Средства" };
                    break;
                case 12:
                    p1 = val1 / total;
                    p2 = val2 / total;
                    p3 = val3 / total;
                    p4 = val4 / total;
                    p5 = val5 / total;
                    p6 = val6 / total;
                    sourceData[0] = new SeriesData { Percent = p1, Value = val1, Name = "Одложени плаќања на..." };
                    sourceData[1] = new SeriesData { Percent = p2, Value = val2, Name = "Краткорочни обврски" };
                    sourceData[2] = new SeriesData { Percent = p3, Value = val3, Name = "Долгорочни обврски" };
                    sourceData[3] = new SeriesData { Percent = p4, Value = val4, Name = "Обврски по основ на нетековни средства..." };
                    sourceData[4] = new SeriesData { Percent = p5, Value = val5, Name = "Главнина и резерви" };
                    sourceData[5] = new SeriesData { Percent = p6, Value = val6, Name = "Одложени даночни обврски" };
                    break;
                case 13:
                    p1 = val1 / total;
                    p2 = val2 / total;
                    p3 = val3 / total;
                    p4 = val4 / total;
                    p5 = val5 / total;
                    sourceData[0] = new SeriesData { Percent = p1, Value = val1, Name = "Финансиски приходи" };
                    sourceData[1] = new SeriesData { Percent = p2, Value = val2, Name = "Приходи  од работењето" };
                    sourceData[2] = new SeriesData { Percent = p3, Value = val3, Name = "Друго" };
                    break;
                case 14:
                    p1 = val1 / total;
                    p2 = val2 / total;
                    p3 = val3 / total;
                    p4 = val4 / total;
                    p5 = val5 / total;
                    sourceData[0] = new SeriesData { Percent = p1, Value = val1, Name = "Финансиски расходи" };
                    sourceData[1] = new SeriesData { Percent = p2, Value = val2, Name = "Трошоци за вработени" };
                    sourceData[2] = new SeriesData { Percent = p3, Value = val3, Name = "Расходи од основна дејност" };
                    sourceData[3] = new SeriesData { Percent = p4, Value = val4, Name = "Друго" };
                    break;
            }
            int i = 0;
            foreach (var item in sourceData)
            {
                var p = SetDataPointForChart(i, item.Value, item.Percent, item.Name);
                s1.Points.Add(p);
                s1.YValueType = ChartValueType.Int64;
                i++;
            }

            Color[] myPalette = new Color[6];
            if (PieType > 2)
            {
                //#748C41
                //#799244
                //#91AF53
                //#CDDBB8
                //#9BBB59
                myPalette = new Color[6]{
                    Color.FromArgb(116, 140, 65),
                    Color.FromArgb(121, 146, 68),
                    Color.FromArgb(145, 175, 83),
                    Color.FromArgb(205, 219, 184),
                    Color.FromArgb(155, 187, 89),
                    Color.FromArgb(155, 187, 89),
                };
            }
            else {
                myPalette = new Color[6]{
                    Color.FromArgb(91, 155, 213),
                    Color.FromArgb(68, 114, 196),
                    Color.FromArgb(165, 165, 165),
                    Color.FromArgb(37, 94, 145),
                    Color.FromArgb(38, 68, 120),
                    Color.FromArgb(38, 68, 120),
                };
            }

            c.Palette = ChartColorPalette.None; 
            c.PaletteCustomColors = myPalette;


            c.Series.Add(s1);

            c.Series["s1"]["PieLabelStyle"] = "Outside";
            if (PieType > 2)
            {
                c.Series["s1"]["PieLabelStyle"] = "Disabled";
            }
            c.Series["s1"]["MinimumRelativePieSize"] = "50";
            c.Series["s1"]["PieLineColor"] = "ActiveCaptionText";
            c.Series["s1"]["PieStartAngle"] = "270";

            var filename = Guid.NewGuid().ToString();

            filename += ".png";

            c.SaveImage(AbsolutePath + filename, ChartImageFormat.Png);

            return filename;
        }

        public static string CreatePieChartNew(int PieType, Dictionary<string, List<double>> values, List<int[]> pie_chart_colors)
        {
            Chart c = new Chart();

            SetChartProperties(c, 800, 500);
           
            ChartArea ca = new ChartArea();

            SetChartAreaProperties(ca);
            SetPieChartProperties(ca);

            c.ChartAreas.Add(ca);

            var s1 = CreateSeriesByType(SeriesChartType.Pie, GradientStyle.None, 30, 144, 255, "s1");

            var sourceData = new SeriesData[values.Count()];

            var counter = 0;
            foreach (var item in values)
            {
                sourceData[counter] = new SeriesData { Percent = item.Value[1], Value = item.Value[0], Name = item.Key };
                counter++;
            }

            int i = 0;
            foreach (var item in sourceData)
            {
                var p = SetDataPointForChart(i, item.Value, item.Percent, item.Name);
                s1.Points.Add(p);
                s1.YValueType = ChartValueType.Int64;
                i++;
            }

            Color[] myPalette = new Color[pie_chart_colors.Count()];
            counter = 0;
            foreach (var item in pie_chart_colors)
            {
                myPalette[counter] = Color.FromArgb(item[0], item[1], item[2]);
                counter++;
            }

            c.Palette = ChartColorPalette.None;
            c.PaletteCustomColors = myPalette;


            c.Series.Add(s1);

            c.Series["s1"]["PieLabelStyle"] = "Outside";
            c.Series["s1"]["PieLabelStyle"] = "Disabled";
            c.Series["s1"]["MinimumRelativePieSize"] = "50";
            c.Series["s1"]["PieLineColor"] = "ActiveCaptionText";
            c.Series["s1"]["PieStartAngle"] = "270";

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