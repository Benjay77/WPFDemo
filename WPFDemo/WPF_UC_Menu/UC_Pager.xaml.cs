using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq.Expressions;

namespace RCApp_Win.View.Common
{
    /// <summary>
    /// UC_Pager.xaml 的交互逻辑
    /// </summary>
    public partial class UC_Pager : UserControl
    {
        #region 基本变量
        public delegate void PageChangedEventHandler(int pageIndex);
        public event PageChangedEventHandler PageChangedEvent;
        public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register("PageIndex", typeof(int), typeof(UC_Pager));
        public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register("PageSize", typeof(int), typeof(UC_Pager));
        public static readonly DependencyProperty TotalPageProperty = DependencyProperty.Register("TotalPage", typeof(int), typeof(UC_Pager));
        public static readonly DependencyProperty TotalItemProperty = DependencyProperty.Register("TotalItem", typeof(int), typeof(UC_Pager));
        public int PageStart { get; set; }
        #endregion

        public UC_Pager()
        {
            PageStart = 0;
            InitializeComponent();
            Clear();
        }

        #region 属性
        public int PageIndex
        {
            get
            {
                return (int)GetValue(PageIndexProperty);
            }
            set { SetValue(PageIndexProperty, value);
                ButtonEnable();
            }
        }

        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        public int TotalPage
        {
            get { return (int)GetValue(TotalPageProperty); }
            set { SetValue(TotalPageProperty, value);
                if ((int)GetValue(TotalPageProperty) == 0)
                {
                    btn_PrevPage.IsEnabled = false;
                    btn_NextPage.IsEnabled = false;
                    btn_GotoPage.IsEnabled = false;
                }
                ButtonEnable();
            }
        }

        public int TotalItem
        {
            get { return (int)GetValue(TotalItemProperty); }
            set { SetValue(TotalItemProperty, value); }
        }
        #endregion

        #region 控件触发事件
        public void GotoPage_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex > PageStart && PageIndex <= TotalPage && PageIndex>=1)
            {
                ButtonEnable();
                if (PageChangedEvent != null)
                {
                    PageChangedEvent(PageIndex);
                }
            }
        }

        private void PrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex > PageStart && PageIndex >1 && PageIndex <= TotalPage)
            {
                PageIndex--;
                if (PageChangedEvent != null)
                {
                    PageChangedEvent(PageIndex);
                }
            }
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex < TotalPage)
            {
                PageIndex++;
                if (PageChangedEvent != null)
                {
                    PageChangedEvent(PageIndex);
                }
            }
        }

        #region 只允许输入数字

        private void txt_MaxNoInPage_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }
            //屏蔽非法按键
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Enter || (((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.Enter) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift))
            {
                if (e.Key == Key.Enter)
                {
                    txt_MaxNoInPage.Text = txt_MaxNoInPage.Text.Replace(" ", "");
                    if (!string.IsNullOrEmpty(txt_MaxNoInPage.Text.Trim()) && int.Parse(txt_MaxNoInPage.Text.Trim()) != PageSize && PageSize > 0)//每页显示数改变重新分页查询
                    {
                        PageSize = int.Parse(txt_MaxNoInPage.Text.Trim());
                        PageIndex = 1;//改变每页显示数 重新从第一页显示数据
                        if (PageChangedEvent != null)
                        {
                            PageChangedEvent(PageIndex);
                        }
                    }
                }
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //屏蔽中文输入和非法字符粘贴输入
            TextBox textBox = sender as TextBox;
            TextChange[] change = new TextChange[e.Changes.Count];
            e.Changes.CopyTo(change, 0);
            int offset = change[0].Offset;
            if (change[0].AddedLength > 0)
            {
                double num = 0;
                if (change[0].AddedLength > 8||textBox.Text.Trim() == "0" || !Double.TryParse(textBox.Text, out num))
                {
                    textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
                    textBox.Select(offset, 0);
                }
            }
        }

        #endregion
        #endregion

        #region 自定义方法
        /// <summary>
        /// 按钮可用判断
        /// </summary>
        private void ButtonEnable()
        {
            if ((int)GetValue(PageIndexProperty) == 1)
            {
                //if ((int)GetValue(TotalPageProperty)==0)
                //{
                //    btn_PrevPage.IsEnabled = false;
                //    btn_NextPage.IsEnabled = false;
                //    btn_GotoPage.IsEnabled = false;
                //}
                //else
                //{
                btn_PrevPage.IsEnabled = false;
                btn_NextPage.IsEnabled = true;
                btn_GotoPage.IsEnabled = true;
                //}
                if ((int)GetValue(TotalItemProperty) <= (int)GetValue(PageSizeProperty))
                {
                    btn_GotoPage.IsEnabled = false;
                    btn_NextPage.IsEnabled = false;
                }
                
            }
            else if ((int)GetValue(PageIndexProperty) == (int)GetValue(TotalPageProperty))
            {
                btn_PrevPage.IsEnabled = true;
                btn_NextPage.IsEnabled = false;
                btn_GotoPage.IsEnabled = true;
            }
            else if ((int)GetValue(PageIndexProperty) > 1 && (int)GetValue(PageIndexProperty) < (int)GetValue(TotalPageProperty))
            {
                btn_PrevPage.IsEnabled = true;
                btn_NextPage.IsEnabled = true;
                btn_GotoPage.IsEnabled = true;
            }
        }

        /// <summary>
        /// 清空控件内容
        /// </summary>
        public void Clear()
        {
            PageIndex = 1;
            PageSize = 50;
            //TotalPage = 0;
            //TotalItem = 0;
        }
        #endregion

    }

    
}
