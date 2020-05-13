using System;
using System.Collections.Generic;
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
using Visifire.Charts;

namespace VisifireShow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<string> strListx = new List<string>() { "苹果", "樱桃", "菠萝", "香蕉", "榴莲", "葡萄", "桃子", "猕猴桃" };
        private List<string> strListy_Workload = new List<string>() { "13", "75", "60", "38", "97", "22", "39", "999" };
        //private List<string> strListy_Workload = new List<string>() { "0", "0", "0", "0", "0", "0", "0", "0" };
        private List<string> strListy_Target= new List<string>() { "75", "90", "65", "41", "100", "30", "58", "9990" };

        private List<DateTime> LsTime = new List<DateTime>()
            { 
               new DateTime(2012,1,1),
               new DateTime(2012,2,1),
               new DateTime(2012,3,1),
               new DateTime(2012,4,1),
               new DateTime(2012,5,1),
               new DateTime(2012,6,1),
               new DateTime(2012,7,1),
               new DateTime(2012,8,1),
               new DateTime(2012,9,1),
               new DateTime(2012,10,1),
               new DateTime(2012,11,1),
               new DateTime(2012,12,1),
            };
        private List<string> cherry = new List<string>() { "33", "75", "60", "98", "67", "88", "39", "45", "13", "22", "45", "80" };
        private List<string> pineapple = new List<string>() { "13", "34", "38", "12", "45", "76", "36", "80", "97", "22", "76", "39" };
        private void ButColumn_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("柱状图");
            Simon.Children.Clear();
            CreateChartColumn("11月份水果销量", strListx, strListy_Workload);
        }

        private void ButPie_Click(object sender, RoutedEventArgs e)
        {
            Simon.Children.Clear();
            CreateChartPie("11月份水果销量", strListx, strListy_Workload);
        }
        private void ButSpline_Click(object sender, RoutedEventArgs e)
        {
            Simon.Children.Clear();
            CreateChartSpline("2013年樱桃、菠萝销量", LsTime, cherry, pineapple);
        }
        #region 柱状图
        public void CreateChartColumn(string name, List<string> valuex, List<string> valuey)
        {
            //创建一个图标
            Chart chart_WorkloadAndTarget = new Chart();
            chart_WorkloadAndTarget.Theme = "Theme2";

            //悬浮不提示
            chart_WorkloadAndTarget.ToolTipEnabled = false;

            //关闭动画
            chart_WorkloadAndTarget.AnimationEnabled = false;

            //设置图标的宽度和高度
            chart_WorkloadAndTarget.Width = 580;
            chart_WorkloadAndTarget.Height = 350;
            chart_WorkloadAndTarget.Margin = new Thickness(100, 5, 10, 5);
            //柱状图宽度
            chart_WorkloadAndTarget.DataPointWidth = 8;

            //是否启用打印和保持图片
            chart_WorkloadAndTarget.ToolBarEnabled = false;

            //设置图标的属性
            chart_WorkloadAndTarget.ScrollingEnabled = false;//是否启用或禁用滚动
            chart_WorkloadAndTarget.View3D = false;//3D效果显示

            //去掉X轴和图表格 
            //Axis xaxis = new Axis();
            //xaxis.Enabled = false;
            //chart_WorkloadAndTarget.AxesX.Add(xaxis);
            //ChartGrid xgrid = new ChartGrid();
            //xgrid.Enabled = false;
            //xaxis.Grids.Add(xgrid);

            //去掉Y轴和图表格
            Axis yaxis = new Axis();
            yaxis.Enabled = false;
            chart_WorkloadAndTarget.AxesY.Add(yaxis);
            ChartGrid ygrid = new ChartGrid();
            ygrid.Enabled = false;
            yaxis.Grids.Add(ygrid);

            // 创建新的数据线。               
            DataSeries dataSeries_Workload = new DataSeries();
            DataSeries dataSeries_Target = new DataSeries();

            //不显示图例说明
            dataSeries_Workload.ShowInLegend = false;
            dataSeries_Target.ShowInLegend = false;

            //柱状图顶端显示工作量/目标量
            //dataSeries_Target.LabelEnabled = true;
            //dataSeries_Target.LabelStyle = LabelStyles.OutSide;

            // 设置数据线的格式
            dataSeries_Workload.RenderAs = RenderAs.StackedColumn;//柱状Stacked
            dataSeries_Target.RenderAs = RenderAs.StackedColumn;//柱状Stacked

            // 设置数据点              
            DataPoint dataPoint_Workload;
            DataPoint dataPoint_Target;
            for (int i = 0; i < strListx.Count; i++)
            {
                // 创建数据点的实例。                   
                dataPoint_Workload = new DataPoint();
                dataPoint_Target = new DataPoint();

                // 设置X轴点                    
                dataPoint_Workload.AxisXLabel = strListx[i];
                dataPoint_Target.AxisXLabel = strListx[i];

                //设置Y轴点                   
                //KeyValuePair<int, int> kvp_WorkloadAndTarget = lst_WorkloadAndTarget[i];
                //if (kvp_WorkloadAndTarget.Key == 0 && kvp_WorkloadAndTarget.Value != 0)
                //{
                //    dataPoint_Workload.YValue = 0;
                //    dataPoint_Target.YValue = 100;
                //}
                //else if (kvp_WorkloadAndTarget.Key != 0 && kvp_WorkloadAndTarget.Value == 0)
                //{
                //    dataPoint_Workload.YValue = 100;
                //    dataPoint_Target.YValue = 0;
                //}
                //else if (kvp_WorkloadAndTarget.Key != 0 && kvp_WorkloadAndTarget.Value != 0)
                //{
                //    dataPoint_Workload.YValue = (kvp_WorkloadAndTarget.Key / kvp_WorkloadAndTarget.Value) * 100;
                //    dataPoint_Target.YValue = 100 * (1 - kvp_WorkloadAndTarget.Key / kvp_WorkloadAndTarget.Value);
                //}
                dataPoint_Workload.YValue = (double.Parse(strListy_Workload[i]) / (double.Parse(strListy_Target[i])) * 100)/100;
                dataPoint_Target.YValue = (100 - (double.Parse(strListy_Workload[i]) / (double.Parse(strListy_Target[i])) * 100))/100;

                //数据点颜色
                dataPoint_Workload.Color = (SolidColorBrush)new BrushConverter().ConvertFromString("#17D0B2");
                dataPoint_Target.Color = (SolidColorBrush)new BrushConverter().ConvertFromString("#ECECEC");

                //如果所有数据为0 则目标量是100% 颜色透明
                //if (kvp_WorkloadAndTarget.Key == 0 && kvp_WorkloadAndTarget.Value == 0)
                //{
                //    dataPoint_Workload.YValue = 0;
                //    dataPoint_Target.YValue = 100;
                //    dataPoint_Target.Color = new SolidColorBrush(Colors.Transparent);
                //}


                //添加点击事件        
                dataPoint_Workload.MouseLeftButtonDown += DataPoint_MouseLeftButtonDown;
                dataPoint_Target.MouseLeftButtonDown += DataPoint_MouseLeftButtonDown;

                //柱状图顶端显示工作量/目标量
                dataPoint_Target.LabelEnabled = true;
                dataPoint_Target.LabelStyle = LabelStyles.OutSide;
                dataPoint_Target.LabelFontSize = 9;
                dataPoint_Target.LabelText = strListy_Workload[i] + "/" + strListy_Target[i].ToString();//kvp_WorkloadAndTarget.Key.ToString() + "/" + kvp_WorkloadAndTarget.Value.ToString();

                

                //添加数据点                   
                dataSeries_Workload.DataPoints.Add(dataPoint_Workload);
                dataSeries_Target.DataPoints.Add(dataPoint_Target);
            }

            // 添加数据线到数据序列。                
            chart_WorkloadAndTarget.Series.Add(dataSeries_Workload);
            chart_WorkloadAndTarget.Series.Add(dataSeries_Target);

            //不显示阴影
            chart_WorkloadAndTarget.ShadowEnabled = false;

            //将生产的图表增加到Grid，然后通过Grid添加到上层Grid.           
            Grid gr = new Grid();
            gr.Children.Add(chart_WorkloadAndTarget);           
            Simon.Children.Add(gr);
        }
        #endregion

        #region 饼状图
        public void CreateChartPie(string name, List<string> valuex, List<string> valuey)
        {
            //创建一个图标
            Chart chart = new Chart();

            //设置图标的宽度和高度
            chart.Width = 580;
            chart.Height = 380;
            chart.Margin = new Thickness(100, 5, 10, 5);
            //是否启用打印和保持图片
            chart.ToolBarEnabled = false;

            //设置图标的属性
            chart.ScrollingEnabled = false;//是否启用或禁用滚动
            chart.View3D = true;//3D效果显示

            //创建一个标题的对象
            Title title = new Title();

            //设置标题的名称
            title.Text = name;
            title.Padding = new Thickness(0, 10, 5, 0);

            //向图标添加标题
            chart.Titles.Add(title);

            //Axis yAxis = new Axis();
            ////设置图标中Y轴的最小值永远为0           
            //yAxis.AxisMinimum = 0;
            ////设置图表中Y轴的后缀          
            //yAxis.Suffix = "斤";
            //chart.AxesY.Add(yAxis);

            // 创建一个新的数据线。               
            DataSeries dataSeries = new DataSeries();

            // 设置数据线的格式
            dataSeries.RenderAs = RenderAs.Pie;//柱状Stacked


            // 设置数据点              
            DataPoint dataPoint;
            for (int i = 0; i < valuex.Count; i++)
            {
                // 创建一个数据点的实例。                   
                dataPoint = new DataPoint();
                // 设置X轴点                    
                dataPoint.AxisXLabel = valuex[i];

                dataPoint.LegendText = "##" + valuex[i];
                //设置Y轴点                   
                dataPoint.YValue = double.Parse(valuey[i]);
                //添加一个点击事件        
                dataPoint.MouseLeftButtonDown += new MouseButtonEventHandler(DataPoint_MouseLeftButtonDown);
                //添加数据点                   
                dataSeries.DataPoints.Add(dataPoint);
            }

            // 添加数据线到数据序列。                
            chart.Series.Add(dataSeries);

            //将生产的图表增加到Grid，然后通过Grid添加到上层Grid.           
            Grid gr = new Grid();
            gr.Children.Add(chart);
            Simon.Children.Add(gr);
        }
        #endregion

        #region 折线图
        public void CreateChartSpline(string name, List<DateTime> lsTime, List<string> cherry, List<string> pineapple)
        {
            //创建一个图标
            Chart chart = new Chart();

            //设置图标的宽度和高度
            chart.Width = 580;
            chart.Height = 380;
            chart.Margin = new Thickness(100, 5, 10, 5);
            //是否启用打印和保持图片
            chart.ToolBarEnabled = false;

            //设置图标的属性
            chart.ScrollingEnabled = false;//是否启用或禁用滚动
            chart.View3D = true;//3D效果显示

            //创建一个标题的对象
            Title title = new Title();

            //设置标题的名称
            title.Text = name;
            title.Padding = new Thickness(0, 10, 5, 0);

            //向图标添加标题
            chart.Titles.Add(title);

            //初始化一个新的Axis
            Axis xaxis = new Axis();
            //设置Axis的属性
            //图表的X轴坐标按什么来分类，如时分秒
            xaxis.IntervalType = IntervalTypes.Months;
            //图表的X轴坐标间隔如2,3,20等，单位为xAxis.IntervalType设置的时分秒。
            xaxis.Interval = 1;
            //设置X轴的时间显示格式为7-10 11：20           
            xaxis.ValueFormatString = "MM月";
            //给图标添加Axis            
            chart.AxesX.Add(xaxis);

            Axis yAxis = new Axis();
            //设置图标中Y轴的最小值永远为0           
            yAxis.AxisMinimum = 0;
            //设置图表中Y轴的后缀          
            yAxis.Suffix = "斤";
            chart.AxesY.Add(yAxis);


            // 创建一个新的数据线。               
            DataSeries dataSeries = new DataSeries();
            // 设置数据线的格式。               
            dataSeries.LegendText = "樱桃";

            dataSeries.RenderAs = RenderAs.Spline;//折线图

            dataSeries.XValueType = ChartValueTypes.DateTime;
            // 设置数据点              
            DataPoint dataPoint;
            for (int i = 0; i < lsTime.Count; i++)
            {
                // 创建一个数据点的实例。                   
                dataPoint = new DataPoint();
                // 设置X轴点                    
                dataPoint.XValue = lsTime[i];
                //设置Y轴点                   
                dataPoint.YValue = double.Parse(cherry[i]);
                dataPoint.MarkerSize = 8;
                //dataPoint.Tag = tableName.Split('(')[0];
                //设置数据点颜色                  
                // dataPoint.Color = new SolidColorBrush(Colors.LightGray);                   
                dataPoint.MouseLeftButtonDown += new MouseButtonEventHandler(DataPoint_MouseLeftButtonDown);
                //添加数据点                   
                dataSeries.DataPoints.Add(dataPoint);
            }

            // 添加数据线到数据序列。                
            chart.Series.Add(dataSeries);


            // 创建一个新的数据线。               
            DataSeries dataSeriesPineapple = new DataSeries();
            // 设置数据线的格式。         

            dataSeriesPineapple.LegendText = "菠萝";

            dataSeriesPineapple.RenderAs = RenderAs.Spline;//折线图

            dataSeriesPineapple.XValueType = ChartValueTypes.DateTime;
            // 设置数据点              

            DataPoint dataPoint2;
            for (int i = 0; i < lsTime.Count; i++)
            {
                // 创建一个数据点的实例。                   
                dataPoint2 = new DataPoint();
                // 设置X轴点                    
                dataPoint2.XValue = lsTime[i];
                //设置Y轴点                   
                dataPoint2.YValue = double.Parse(pineapple[i]);
                dataPoint2.MarkerSize = 8;
                //dataPoint2.Tag = tableName.Split('(')[0];
                //设置数据点颜色                  
                // dataPoint.Color = new SolidColorBrush(Colors.LightGray);                   
                dataPoint2.MouseLeftButtonDown += new MouseButtonEventHandler(DataPoint_MouseLeftButtonDown);
                //添加数据点                   
                dataSeriesPineapple.DataPoints.Add(dataPoint2);
            }
            // 添加数据线到数据序列。                
            chart.Series.Add(dataSeriesPineapple);

            //将生产的图表增加到Grid，然后通过Grid添加到上层Grid.           
            Grid gr = new Grid();
            gr.Children.Add(chart);
            
            Simon.Children.Add(gr);
        }
        #endregion

        #region 点击事件
        //点击事件
        void DataPoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //DataPoint dp = sender as DataPoint;
            //MessageBox.Show(dp.YValue.ToString());
        }
        #endregion
        
    }
}
