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

namespace WPF_UC_WeChatMessageNotify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// Description     
    /// 自定义控件Demo
    /// 
    /// Editor         Edtion           Date           Memo
    /// Benjay         1.0              2017.12.19     新建自定义消息提醒控件
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 基本变量
        int promptCount;//消息数量
        #endregion
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            AdornerLayer layer = AdornerLayer.GetAdornerLayer(container);
            layer.Add(new PromptAdorner(button));
        }

        #region 控件触发事件
        /// <summary>
        /// +按钮点击触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            promptCount += 211;
            button.PromptCount = promptCount.ToString();
        }

        /// <summary>
        /// -按钮点击触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReduce_Click(object sender, RoutedEventArgs e)
        {
            promptCount -= 211;
            button.PromptCount = promptCount.ToString();
        }
        #endregion
    }
}
