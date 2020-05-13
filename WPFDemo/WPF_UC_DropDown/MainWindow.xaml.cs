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

namespace WPF_UC_DropDown
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// 
    /// Description     
    /// 自定义控件Demo
    /// 
    /// Editor         Edtion           Date           Memo
    /// Benjay         1.0              2017.08.15     新建文本下拉框包含关键字控件
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
        }

        #region 控件触发事件
        /// <summary>
        /// gr_Test鼠标点击触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gr_Test_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!uc_DropDown.IsMouseOver)
                {
                    uc_DropDown.popup_UpDown.Visibility = System.Windows.Visibility.Collapsed;
                    uc_DropDown.gr_Down.Visibility = System.Windows.Visibility.Collapsed;;
                }
            }
        }
        #endregion
    }
}
