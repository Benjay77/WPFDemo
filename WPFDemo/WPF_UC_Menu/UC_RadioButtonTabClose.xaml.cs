using RCApp_Win.Util;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Linq;
using System.Windows.Media.Imaging;
using System;
using System.Collections.Generic;

namespace RCApp_Win.View.Common
{
    /// <summary>
    /// UC_RadioButtonTabClose.xaml 的交互逻辑
    /// </summary>
    public partial class UC_RadioButtonTabClose : RadioButton
    {
        #region 基本变量

        /// <summary>
        /// 父级TabPanel
        /// </summary>
        private TabPanel parent;

        /// <summary>
        /// 约定的宽度
        /// </summary>
        private double conventionWidth = 100;

        /// <summary>
        /// 自定义的closed事件
        /// </summary>
        public static readonly RoutedEvent OnClosedEvent = EventManager.RegisterRoutedEvent("OnClosed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UC_RadioButtonTabClose));
        public event RoutedEventHandler OnClosed
        {
            add { AddHandler(OnClosedEvent, value); }
            remove { RemoveHandler(OnClosedEvent, value); }
        }

        /// <summary>
        /// 自定义的click事件
        /// </summary>
        public static readonly RoutedEvent OnClickEvent = EventManager.RegisterRoutedEvent("OnClickNew", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UC_RadioButtonTabClose));
        public event RoutedEventHandler OnClickNew
        {
            add { AddHandler(OnClickEvent, value); }
            remove { RemoveHandler(OnClickEvent, value); }
        }

        /// <summary>
        /// 当前标签关闭后要选中的标签
        /// </summary>
        public UC_RadioButtonTabClose uC_ShouldChecked;

        #endregion

        public UC_RadioButtonTabClose()
        {
            InitializeComponent();
            this.Loaded += UC_RadioButtonTabClose_Loaded;
            this.Checked += UC_RadioButtonTabClose_Checked;
        }

        #region 控件触发事件
        /// <summary>
        /// loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC_RadioButtonTabClose_Loaded(object sender, RoutedEventArgs e)
        {
            parent = VisualTreeHelper.GetParent(this) as TabPanel;
            if (parent!=null)
            {
                Load();
            }
        }

        /// <summary>
        /// 标签选中触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC_RadioButtonTabClose_Checked(object sender, RoutedEventArgs e)
        {
            //关闭图片样式替换
            Image image = VisualHelperTreeExtension.GetChildByName<Image>(this, "image");
            if (image!=null)
            {
                image.Source = new BitmapImage(new Uri("pack://application:,,,/Themes/images/Common/NewMenuIcon/PressClosed.png"));
            }
            //点击事件
            RoutedEventArgs args = new RoutedEventArgs(OnClickEvent, this);
            this.RaiseEvent(args);
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 父级TabPanel尺寸发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Parent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AdjustWidth();
            e.Handled = true;
            //调整自身大小
            //保持约定宽度item的临界个数
            //int criticalCount = (int)((parent.Width - 5) / conventionWidth);
            //if (parent.Children.Count <= criticalCount)
            //{
            //    //小于等于临界个数 等于约定宽度
            //    this.Width = conventionWidth;
            //}
            //else
            //{
            //    //大于临界个数 等于平均宽度
            //    double perWidth = (parent.Width - 5) / parent.Children.Count;
            //    this.Width = perWidth;
            //}
        }
        #endregion

        #region 自定义方法

        /// <summary>
        /// Load
        /// </summary>
        private void Load()
        {
            //约定的宽度
            double.TryParse(parent.Tag.ToString(), out conventionWidth);

            //注册父级TabPanel尺寸发生变化事件
            parent.SizeChanged += Parent_SizeChanged;

            //自适应
            AdjustWidth();
        }

        /// <summary>
        /// 关闭方法
        /// </summary>
        public void Close()
        {
            if (parent == null)
                return;
            #region 标签关闭要显示的标签处理
            int index = parent.Children.IndexOf(this);//关闭标签的索引
            int currentIndex = -1;//当前标签索引
            if (index != 0 && index == parent.Children.Count-1)//标签为最后一个则前一个被选中且不止一个标签
            {                
                foreach (UC_RadioButtonTabClose uc in parent.Children)
                {
                    currentIndex++;
                    if (currentIndex == index-1)
                    {
                        uC_ShouldChecked = uc;
                    }
                }
            }
            else//不是则后一个被选中
            {
                foreach (UC_RadioButtonTabClose uc in parent.Children)
                {
                    currentIndex++;
                    if (currentIndex == index + 1)
                    {
                        uC_ShouldChecked = uc;
                    }
                }
            }
            #endregion

            //移除自身
            parent.Children.Remove(this);

            //移除事件
            parent.SizeChanged -= Parent_SizeChanged;

            //调整剩余项大小
            AdjustWidth();

            //关闭事件
            RoutedEventArgs args = new RoutedEventArgs(OnClosedEvent, this);
            this.RaiseEvent(args);
        }

        /// <summary>
        /// 调整宽度
        /// </summary>
        private void AdjustWidth()
        {
            //临界数量
            int criticalCount = (int)((parent.Width - 5) / conventionWidth);
            if (parent.Children.Count <= criticalCount)
            {
                //小于等于临界个数 等于约定宽度
                this.Width = conventionWidth;
            }
            else
            {
                //大于临界个数 每项都设成平均宽度
                double perWidth = (parent.Width - 5) / parent.Children.Count;
                foreach (UC_RadioButtonTabClose uc_RadioButtonTabClose in parent.Children)
                {
                    uc_RadioButtonTabClose.Width = perWidth;
                }
            }
        }
        #endregion
    }
}
