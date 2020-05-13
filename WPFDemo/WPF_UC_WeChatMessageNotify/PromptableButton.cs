using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace WPF_UC_WeChatMessageNotify
{
    internal class PromptableButton : Button
    {
        /// <summary>
        /// 图片属性
        /// </summary>
        public ImageSource CoverImageSource {
            get { return (ImageSource)GetValue(CoverImageSourceProperty); }
            set { SetValue(CoverImageSourceProperty, value); }
        }

        public static readonly DependencyProperty CoverImageSourceProperty =
            DependencyProperty.Register("CoverImageSource", typeof(ImageSource), typeof(PromptableButton), new UIPropertyMetadata(null));

        /// <summary>
        /// 显示的消息数字属性
        /// </summary>
        public string PromptCount {
            get { return (string)GetValue(PromptCountProperty); }
            set { SetValue(PromptCountProperty, value); }
        }

        public static readonly DependencyProperty PromptCountProperty =
            DependencyProperty.Register("PromptCount", typeof(string), typeof(PromptableButton), 
            new FrameworkPropertyMetadata("0", new PropertyChangedCallback(PromptCountChangedCallBack), new CoerceValueCallback(CoercePromptCountCallback)));


        public PromptableButton()
        {
            
        }
        
        static PromptableButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PromptableButton), new FrameworkPropertyMetadata(typeof(PromptableButton)));
		}

        /// <summary>
        /// 消息数字显示
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object CoercePromptCountCallback(DependencyObject d, object value) {
            int promptCount = int.Parse(value.ToString());
            promptCount = Math.Max(0, promptCount);
            if (promptCount>99)//消息超过99 显示99+
            {
                return "99+";
            }
            return promptCount.ToString();
        }
        
        public static void PromptCountChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e) {

        }

    }
}
