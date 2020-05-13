using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Charts.Navigation;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_UC_CurveChart
{
    /// <summary>
    /// UC_CurveChart.xaml 的交互逻辑
    /// </summary>
    public partial class UC_CurveChart : UserControl
    {
        public UC_CurveChart()
        {
            InitializeComponent();
            Loaded += UC_CurveChart_Loaded;
        }

        void UC_CurveChart_Loaded(object sender, RoutedEventArgs e)
        {
            List<HouseData> houseDataList = LoadHouseData("..\\..\\Data.txt");

            DateTime[] dates = new DateTime[houseDataList.Count];
            int[] numberSalesCompletion = new int[houseDataList.Count];
            int[] numberRentCompletion = new int[houseDataList.Count];
            int[] numberSalesTarget = new int[houseDataList.Count];
            int[] numberRentTarget = new int[houseDataList.Count];
            int totalSalesCompletion = 0;
            int totalRentCompletion = 0;

            for (int i = 0; i < houseDataList.Count; ++i)
            {
                dates[i] = houseDataList[i].date;
                numberSalesCompletion[i] = houseDataList[i].i_SalesCompletion;
                numberRentCompletion[i] = houseDataList[i].i_RentCompletion;
                numberSalesTarget[i] = houseDataList[i].i_SalesTarget;
                numberRentTarget[i] = houseDataList[i].i_RentTarget;
                totalSalesCompletion = totalSalesCompletion + houseDataList[i].i_SalesCompletion;
                totalRentCompletion = totalRentCompletion + houseDataList[i].i_RentCompletion;
            }

            var datesDataSource = new EnumerableDataSource<DateTime>(dates);
            datesDataSource.SetXMapping(x => dateAxis.ConvertToDouble(x));

            var numberSalesDataSource = new EnumerableDataSource<int>(numberSalesCompletion);
            numberSalesDataSource.SetYMapping(y => y);
            numberSalesDataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                Y => String.Format("出售数量：{0}", Y));

            var numberRentDataSource = new EnumerableDataSource<int>(numberRentCompletion);
            numberRentDataSource.SetYMapping(y => y);
            numberRentDataSource.AddMapping(ShapeElementPointMarker.ToolTipTextProperty,
                Y => String.Format("出租数量：{0}", Y));

            header.Content = string.Format(@"房源总量：" + (totalSalesCompletion + totalRentCompletion).ToString() + "套（出售：" + totalSalesCompletion + "套，出租：" + totalRentCompletion + "套）");

            CompositeDataSource compositeDataSource1 = new
              CompositeDataSource(datesDataSource, numberSalesDataSource);
            CompositeDataSource compositeDataSource2 = new
              CompositeDataSource(datesDataSource, numberRentDataSource);

           plotter.AddLineGraph(compositeDataSource1,
              new Pen(Brushes.Blue, 2),
              new CircleElementPointMarker { Size = 10.0, Fill = Brushes.Red },
              new PenDescription("完成量"));

            plotter.AddLineGraph(compositeDataSource2,
              new Pen(Brushes.LimeGreen, 2),
              new CircleElementPointMarker
              {
                  Size = 10.0,
                  //Pen = new Pen(Brushes.Black, 2.0),
                  Fill = Brushes.Orange
              },
              new PenDescription("目标量"));

            CursorCoordinateGraph cursorCoordinateGraph = new CursorCoordinateGraph();
            cursorCoordinateGraph.IsHorizontalDateTimeAxis = true;//横轴显示时间

            plotter.Children.Add(cursorCoordinateGraph);

            plotter.Viewport.FitToView();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static List<HouseData> LoadHouseData(string fileName)
        {
            var result = new List<HouseData>();
            FileStream fs = new FileStream(fileName, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                string[] pieces = line.Split(':');
                DateTime d = DateTime.Parse(pieces[0]);
                string num_Completion = pieces[1];
                string num_Target = pieces[2];
                string[] completion = num_Completion.Split(',');
                string[] target = num_Target.Split(',');
                int i_SalesCompletion = int.Parse(completion[0]);
                int i_RentCompletion = int.Parse(completion[1]);
                int i_SalesTarget = int.Parse(target[0]);
                int i_RentTarget = int.Parse(target[1]);
                HouseData hd = new HouseData(d, i_SalesCompletion, i_RentCompletion, i_SalesTarget, i_RentTarget);
                result.Add(hd);
            }
            sr.Close();
            fs.Close();
            return result;
        }

        private void plotter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ChartPlotter chart = sender as ChartPlotter;
            Point p = e.GetPosition(this).ViewportToData(chart.Transform);
            string x = p.X.ToString();
            string y = p.Y.ToString();
        }
    }

    /// <summary>
    /// 房源数据
    /// </summary>
    public class HouseData
    {
        public DateTime date;//时间戳
        public int i_SalesCompletion;//出售完成量
        public int i_RentCompletion;//出租完成量
        public int i_SalesTarget;//出售目标量
        public int i_RentTarget;//出租目标量

        public HouseData(DateTime date, int i_SalesCompletion, int i_RentCompletion, int i_SalesTarget, int i_RentTarget)
        {
            this.date = date;
            this.i_SalesCompletion = i_SalesCompletion;
            this.i_RentCompletion = i_RentCompletion;
            this.i_SalesTarget = i_SalesTarget;
            this.i_RentTarget = i_RentTarget;
        }

    }
}
