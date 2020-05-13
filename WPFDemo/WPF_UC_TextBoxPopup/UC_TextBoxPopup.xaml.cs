using System;
using System.Collections.Generic;
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

namespace RCApp_Win.View.Common
{
    /// <summary>
    /// UC_TextBoxPopup.xaml 的交互逻辑
    /// </summary>
    public partial class UC_TextBoxPopup : UserControl
    {
        #region 基本变量
        public delegate void MouseLeftButtonDownCallBack(TextBox textBox);
        public new event MouseLeftButtonDownCallBack OnMouseLeftButtonDown;
        #endregion

        public UC_TextBoxPopup()
        {
            InitializeComponent();
        }

        #region 控件触发事件
        /// <summary>
        /// 文本框点击触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_TextPopup_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (chk_Togglet.IsChecked == true || listBox_TextPopup.SelectedIndex!=-1)
            {
                chk_Togglet.IsChecked = false;
                listBox_TextPopup.SelectedIndex = -1;
            }
            txt_TextPopup.Foreground = new SolidColorBrush(Color.FromRgb(71, 207, 167));
            if (OnMouseLeftButtonDown!=null)
            {
                OnMouseLeftButtonDown(txt_TextPopup);
            }
        }

        /// <summary>
        /// 下拉箭头鼠标进入触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chk_Togglet_MouseEnter(object sender, MouseEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            checkBox.IsChecked = true;
        }

        /// <summary>
        /// 下拉箭头 下拉框 文本框 鼠标离开触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chk_Togglet_MouseLeave(object sender, MouseEventArgs e)
        {
            Control control = sender as Control;
            if (control is CheckBox)
            {
                if (!popup_TextPopup.IsMouseOver && !txt_TextPopup.IsMouseOver)
                {
                    chk_Togglet.IsChecked = false;
                }
            }
            else if (control is TextBox)
            {
                if (!popup_TextPopup.IsMouseOver && !chk_Togglet.IsMouseOver)
                {
                    chk_Togglet.IsChecked = false;
                }
            }
            else
            {
                if (listBox_TextPopup.SelectedIndex > -1)
                {
                    txt_TextPopup.Foreground = new SolidColorBrush(Color.FromRgb(71, 207, 167));
                }
                chk_Togglet.IsChecked = false;
            }
        }

        #endregion

        #region 自定义方法
        /// <summary>
        /// 清除控件内容
        /// </summary>
        public void Clear()
        {
            listBox_TextPopup.SelectedIndex = -1;
            txt_TextPopup.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#000000");
        }
        #endregion
    }
}
