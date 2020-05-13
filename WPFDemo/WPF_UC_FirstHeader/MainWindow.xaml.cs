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

namespace WPF_UC_FirstHeader
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    ///  
    /// Description     
    /// 自定义控件Demo
    /// 
    /// Editor         Edtion           Date           Memo
    /// Benjay         1.0              2017.08.25     新建图片按钮控件
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            uc_FirstHeader.click += uc_FirstHeader_click;
        }

        void uc_FirstHeader_click(object sender, EventArgs e)
        {
            uc_FirstHeader.IsEnable = !uc_FirstHeader.IsEnable;
        }
    }
}
