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
using System.Xml;
using System.Xml.Linq;

namespace WPF_UC_FirstHeader
{
    /// <summary>
    /// UC_FirstHeader.xaml 的交互逻辑
    /// </summary>
    public partial class UC_FirstHeader : UserControl
    {
        #region 基本变量
        // 设置按钮使能状态
        private bool isEnable = true;
        // 按钮的4种状态图片
        private ImageSource imageUp = null;
        private ImageSource imageHover = null;
        private ImageSource imageDown = null;
        private ImageSource imageDisable = null;
        // 按钮的文本属性
        private string text = "";
        private FontFamily textFamily;
        private double textSize;
        private Brush textColor;
        // 是否在当前按钮中按下
        private bool isClicking = false;
        // 点击事件
        public event EventHandler click;

        // 按钮可用
        public bool IsEnable
        {
            get { return isEnable; }
            set
            {
                isEnable = value;
                imageBtn.Source = isEnable ? imageUp : imageDisable;
            }
        }
        // 按钮弹起图片
        public ImageSource ImageUp
        {
            get { return imageUp; }
            set
            {
                imageUp = value;
                imageBtn.Source = imageUp;
            }
        }
        // 按钮划过图片
        public ImageSource ImageHover
        {
            get { return imageHover; }
            set { imageHover = value; }
        }
        // 按钮按下图片
        public ImageSource ImageDown
        {
            get { return imageDown; }
            set { imageDown = value; }
        }
        // 按钮禁用图片
        public ImageSource ImageDisable
        {
            get { return imageDisable; }
            set { imageDisable = value; }
        }
        // 按钮文本
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                labelBtn.Content = text;
            }
        }
        // 按钮字体
        public FontFamily TextFamily
        {
            get { return textFamily; }
            set
            {
                textFamily = value;
                labelBtn.FontFamily = textFamily;
            }
        }
        // 按钮字号
        public double TextSize
        {
            get { return textSize; }
            set
            {
                textSize = value;
                labelBtn.FontSize = textSize;
            }
        }
        // 文字颜色
        public Brush TextColor
        {
            get { return textColor; }
            set
            {
                textColor = value;
                labelBtn.Foreground = textColor;
            }
        }
        #endregion

        public UC_FirstHeader()
        {
            InitializeComponent();
        }

        #region 自定义方法
        /// <summary>
        /// linq操作XML获取图片按钮资源
        /// 用于
        /// </summary>
        public void GetButtonImageSource()
        {
            XElement xe_RootNode = XElement.Load(AppDomain.CurrentDomain.BaseDirectory + "ButtonImageSource.xml");
            if (xe_RootNode != null)
            {
                var source = from nodes in xe_RootNode.DescendantsAndSelf("ButtonImageSource").Elements("UC_FirstHeader")
                             select new
                             {
                                 Text = nodes.Attribute("Text").Value,
                                 SourceUnfocused = nodes.Attribute("SourceUnfocused").Value,
                                 SourceFocused = nodes.Attribute("SourceFocused").Value,
                             };
                foreach (var s in source)
                {
                    Text = s.Text;
                    //backImage = new BitmapImage(new Uri(System.Environment.CurrentDirectory + s.SourceUnfocused));
                    //EnablebackImage= new BitmapImage(new Uri(System.Environment.CurrentDirectory + s.SourceUnfocused));
                    //unEnablebackImage = (new BitmapImage(new Uri(System.Environment.CurrentDirectory + s.SourceFocused)));
                }
            }

        }

        #endregion

        #region 控件触发事件
        /// <summary>
        /// 鼠标进入触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            if (isEnable)
            {
                if (null != imageHover)
                {
                    imageBtn.Source = imageHover;
                }
            }
        }
        
        /// <summary>
        /// 鼠标按下触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isEnable)
            {
                isClicking = true;
                if (null != imageDown)
                {
                    imageBtn.Source = imageDown;
                }
            }
        }

        /// <summary>
        /// 鼠标弹起触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isEnable)
            {
                // 完成在控件上点击
                if (isClicking)
                {
                    isClicking = false;
                    imageBtn.Source = imageUp;
                    // 触发点击事件
                    if (null != click) click(this, null);
                }
            }
        }
        
        /// <summary>
        /// 鼠标离开触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isEnable)
            {
                isClicking = false;
                imageBtn.Source = imageUp;
            }
        }

        #endregion
    }
}