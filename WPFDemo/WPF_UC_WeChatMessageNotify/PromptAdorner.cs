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
    internal class PromptAdorner : Adorner {

		protected override int VisualChildrenCount {
			get { return 1; }
		}

        public PromptAdorner(UIElement adornedElement)
			: base(adornedElement) {

            _chrome = new PromptChrome();
            _chrome.DataContext = adornedElement;
            this.AddVisualChild(_chrome);
		}


		protected override Visual GetVisualChild(int index) {
            return _chrome;
		}

		protected override Size ArrangeOverride(Size arrangeBounds) {
            _chrome.Arrange(new Rect(arrangeBounds));
			return arrangeBounds;
		}
        
		PromptChrome _chrome;
    }
}
