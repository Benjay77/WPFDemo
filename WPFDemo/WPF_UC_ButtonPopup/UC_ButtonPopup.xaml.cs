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
    /// UC_ButtonPopup.xaml 的交互逻辑
    /// </summary>
    public partial class UC_ButtonPopup : UserControl
    {
        #region 基本变量
        /// <summary>
        /// 自定义的Click事件
        /// </summary>
        public static readonly RoutedEvent OnClickEvent = EventManager.RegisterRoutedEvent("OnClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UC_RadioButton));
        public event RoutedEventHandler OnClick
        {
            add { AddHandler(OnClickEvent, value); }
            remove { RemoveHandler(OnClickEvent, value); }
        }
        #endregion

        public UC_ButtonPopup()
        {
            InitializeComponent();
        }

        #region 控件触发事件
        /// <summary>
        /// 按钮点击触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ButtonPopup_Click(object sender, RoutedEventArgs e)
        {
            tg_ButtonPopup.IsChecked = true;
            popup_ButtonPopup.IsOpen = true;
        }

        /// <summary>
        /// 下拉框鼠标离开触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popup_ButtonPopup_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!btn_ButtonPopup.IsMouseOver&&!popup_ButtonPopup.IsMouseOver)
            {
                tg_ButtonPopup.IsChecked = false;
                popup_ButtonPopup.IsOpen = false;
            }
        }

        /// <summary>
        /// RadioButton点击触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Click(object sender, RoutedEventArgs e)
        {
            UC_RadioButton radioButton = sender as UC_RadioButton;
            RoutedEventArgs args = new RoutedEventArgs(OnClickEvent, radioButton);
            this.RaiseEvent(args);
        }
        #endregion

        #region 自定义方法
        /// <summary>
        /// 清除控件内容
        /// </summary>
        public void Clear()
        {
            listBox_ButtonPopup.SelectedIndex = -1;
        }
        #endregion

        
    }
}
