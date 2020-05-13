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

namespace WPF_UC_TabControlWithClose
{
    /// <summary>
    /// UCTabItemWithClose.xaml 的交互逻辑
    /// </summary>
    public partial class UC_TabItemWithClose : TabItem
    {
        #region 基本变量
        /// <summary>
        /// 父级TabControl
        /// </summary>
        private TabControl tab_Parent;
        /// <summary>
        /// 约定的宽度
        /// </summary>
        private double conventionWidth = 100;
        #endregion

        public UC_TabItemWithClose()
        {
            InitializeComponent();
        }
        

        #region 控件事件
        /// <summary>
        /// loaded
        /// </summary>
        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            //找到父级TabControl
            tab_Parent = FindParentTabControl(this);
            if (tab_Parent != null)
            {
                Load();
            } 
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            if (tab_Parent == null)
                return;

            //移除自身
            tab_Parent.Items.Remove(this);
            //移除事件
            tab_Parent.SizeChanged -= m_Parent_SizeChanged;

            //调整剩余项大小
            //保持约定宽度item的临界个数
            //int criticalCount = (int)((tab_Parent.ActualWidth - 5) / conventionWidth);
            ////平均宽度
            //double perWidth = (tab_Parent.ActualWidth - 5) / tab_Parent.Items.Count;
            //foreach (UC_TabItemWithClose item in tab_Parent.Items)
            //{
            //    if (tab_Parent.Items.Count <= criticalCount)
            //    {
            //        item.Width = conventionWidth;
            //    }
            //    else
            //    {
            //        item.Width = perWidth;
            //    }
            //}
            AdjustWidth();
        }

        /// <summary>
        /// 父级TabControl尺寸发生变化
        /// </summary>
        private void m_Parent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //调整自身大小
            //保持约定宽度item的临界个数
            int criticalCount = (int)((tab_Parent.ActualWidth - 5) / conventionWidth);
            if (tab_Parent.Items.Count <= criticalCount)
            {
                //小于等于临界个数 等于约定宽度
                this.Width = conventionWidth;
            }
            else
            {
                //大于临界个数 等于平均宽度
                double perWidth = (tab_Parent.ActualWidth - 5) / tab_Parent.Items.Count;
                this.Width = perWidth;
            }
        }
        #endregion

        #region 自定义方法

        /// <summary>
        /// Load
        /// </summary>
        private void Load()
        {
            //约定的宽度
            double.TryParse(tab_Parent.Tag.ToString(), out conventionWidth);
            //注册父级TabControl尺寸发生变化事件
            tab_Parent.SizeChanged += m_Parent_SizeChanged;

            //自适应
            //保持约定宽度item的临界个数
            //int criticalCount = (int)((tab_Parent.ActualWidth - 5) / conventionWidth);
            //if (tab_Parent.Items.Count <= criticalCount)
            //{
            //    //小于等于临界个数 等于约定宽度
            //    this.Width = conventionWidth;
            //}
            //else
            //{
            //    //大于临界个数 每项都设成平均宽度
            //    double perWidth = (tab_Parent.ActualWidth - 5) / tab_Parent.Items.Count;
            //    foreach (UC_TabItemWithClose item in tab_Parent.Items)
            //    {
            //        item.Width = perWidth;
            //    }
            //    this.Width = perWidth;
            //}
            AdjustWidth();
        }

        /// <summary>
        /// 调整宽度
        /// </summary>
        private void AdjustWidth()
        {
            int criticalCount = (int)((tab_Parent.ActualWidth - 5) / conventionWidth);
            if (tab_Parent.Items.Count <= criticalCount)
            {
                //小于等于临界个数 等于约定宽度
                this.Width = conventionWidth;
            }
            else
            {
                //大于临界个数 每项都设成平均宽度
                double perWidth = (tab_Parent.ActualWidth - 5) / tab_Parent.Items.Count;
                foreach (UC_TabItemWithClose item in tab_Parent.Items)
                {
                    item.Width = perWidth;
                }
            }
        }

        /// <summary>
        /// 递归找父级TabControl
        /// </summary>
        /// <param name="reference">依赖对象</param>
        /// <returns>TabControl</returns>
        private TabControl FindParentTabControl(DependencyObject reference)
        {
            DependencyObject dObj = VisualTreeHelper.GetParent(reference);
            if (dObj == null)
                return null;
            if (dObj.GetType() == typeof(TabControl))
                return dObj as TabControl;
            else
                return FindParentTabControl(dObj);
        }
        #endregion
    }
}
