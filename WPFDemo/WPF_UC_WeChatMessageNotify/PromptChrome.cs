using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF_UC_WeChatMessageNotify
{
    internal class PromptChrome : Control {
        static PromptChrome()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PromptChrome), new FrameworkPropertyMetadata(typeof(PromptChrome)));
        }     

        /// <summary>
        /// 小圆点的大小及位置设置
        /// </summary>
        /// <param name="arrangeBounds"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            //大小
            this.Width = 25;
            this.Height = 25;

            //位置
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            //相对位置
            TranslateTransform tt = new TranslateTransform();
            tt.X = -5;
            tt.Y = 5;
            this.RenderTransform = tt;

            return base.ArrangeOverride(arrangeBounds);
        }

        
    }
}
