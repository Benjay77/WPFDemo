using JP.HCZZ.WPFApp.IService.Models.QW.Setting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace JP.HCZZP.WPFApp.QW.Modules.ControlLibrary.UserControls.Common
{
    /// <summary>
    /// UC_FilterCombobox.xaml 的交互逻辑
    /// </summary>
    public partial class UC_FilterCombobox : UserControl
    {
        public UC_FilterCombobox()
        {
            InitializeComponent();
        }

        #region Properties
        private ObservableCollection<SimpleResourceModel> dictionaryModels = new ObservableCollection<SimpleResourceModel>();
        /// <summary>
        /// 存储数据的临时集合
        /// </summary>
        public ObservableCollection<SimpleResourceModel> DictionaryModels
        {
            get => dictionaryModels;
            set => dictionaryModels = value;
        }

        public static readonly DependencyProperty DictionaryModelSelectedProperty = DependencyProperty.Register("DictionaryModelSelected"
            , typeof(SimpleResourceModel), typeof(UC_FilterCombobox));

        [Description("选择的类型")]
        [Category("Common Properties")]
        public SimpleResourceModel DictionaryModelSelected
        {
            get
            {
                return (SimpleResourceModel)GetValue(DictionaryModelSelectedProperty);
            }
            set
            {
                SetValue(DictionaryModelSelectedProperty, value);
            }
        }

        public static readonly DependencyProperty DataSourceListProperty = DependencyProperty.Register("DataSourceList"
            , typeof(ObservableCollection<SimpleResourceModel>), typeof(UC_FilterCombobox));

        [Description("数据源")]
        [Category("Common Properties")]
        public ObservableCollection<SimpleResourceModel> DataSourceList
        {
            get
            {
                return (ObservableCollection<SimpleResourceModel>)GetValue(DataSourceListProperty);
            }
            set
            {
                SetValue(DataSourceListProperty, value);
            }
        }

        public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ListBox));
        /// <summary>
        /// 自定义列表选择变化事件
        /// </summary>
        public event RoutedEventHandler SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 文本框回车触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_TextPopup_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender != null && !string.IsNullOrEmpty(txt_TextPopup.Text.Trim()))
                {                   
                    if (DataSourceList != null && DataSourceList.Count > 0)
                    {
                        if (DictionaryModels == null || DictionaryModels.Count == 0 || (DictionaryModels.Count > 0 && DictionaryModels.Count != DataSourceList.Count))
                        {
                            DictionaryModels.Clear();
                            DataSourceList.ToList().ForEach(m =>
                            {
                                DictionaryModels.Add(m);
                            });
                        }
                        bool isFirstCharIsLetter = false;//是否首字符为字母
                        foreach (char item in txt_TextPopup.Text.Trim().ToCharArray())
                        {
                            if (item >= 'A' && item <= 'z')//如果字符为字母 则以警员name拼音首字母匹配
                            {
                                isFirstCharIsLetter = true;                                
                                int charIndex = txt_TextPopup.Text.Trim().ToCharArray().ToList().IndexOf(item);//当前字母是第几个
                                DictionaryModels.ToList().ForEach(b =>
                                {
                                    if (!string.IsNullOrEmpty(b.Code) && !GetFirstLetterOfChineseString(b.Code.Substring(charIndex, 1)).ToString().ToLower().Equals(item.ToString().Trim().ToLower()) && DictionaryModels.Contains(b)||string.IsNullOrEmpty(b.Code))
                                    {
                                        DictionaryModels.Remove(b);
                                    }
                                });
                            }
                            else//非字母直接跳出
                            {
                                break;
                            }
                        }
                        if (!isFirstCharIsLetter)
                        {
                            DictionaryModels = new ObservableCollection<SimpleResourceModel>(DictionaryModels.ToList().FindAll(a => !string.IsNullOrEmpty(a.Code) && a.Code.Contains(txt_TextPopup.Text.Trim())));
                        }
                        if (DictionaryModels.Count == 0)
                        {
                            NoResultBorder.Visibility = Visibility.Visible;
                            listBox_TextPopup.Visibility = Visibility.Collapsed;
                            //DictionaryModelSelected = new SimpleResourceModel();
                            //处理警员选择变化
                            //RoutedEventArgs args = new RoutedEventArgs(SelectionChangedEvent, listBox_TextPopup);
                            //this.RaiseEvent(args);
                        }
                        else
                        {
                            chk_Togglet.IsChecked = true;
                            NoResultBorder.Visibility = Visibility.Collapsed;
                            listBox_TextPopup.Visibility = Visibility.Visible;
                            listBox_TextPopup.ItemsSource = DictionaryModels;
                        }
                        //处理警员选择变化
                        RoutedEventArgs args = new RoutedEventArgs(SelectionChangedEvent, listBox_TextPopup);
                        this.RaiseEvent(args);
                    }
                }
                else if (string.IsNullOrEmpty(txt_TextPopup.Text.Trim()))
                {
                    CleanOrClearTextBox();
                }
            }
            else if (e.Key == Key.Back ||e.Key == Key.Delete)
            {
                CleanOrClearTextBox();
            }
        }

        /// <summary>
        /// 回退或者清除文本框文本
        /// </summary>
        private void CleanOrClearTextBox()
        {
            //DictionaryModelSelected = new SimpleResourceModel();
            NoResultBorder.Visibility = Visibility.Collapsed;
            listBox_TextPopup.Visibility = Visibility.Visible;
            if (DataSourceList.Count != listBox_TextPopup.Items.Count)
            {
                DictionaryModels.Clear();
                listBox_TextPopup.ItemsSource = DataSourceList;
            }
            if (DictionaryModelSelected != null && !string.IsNullOrEmpty(DictionaryModelSelected.ID))
            {
                DictionaryModelSelected = null;
            }
            //处理警员选择变化
            RoutedEventArgs args = new RoutedEventArgs(SelectionChangedEvent, listBox_TextPopup);
            this.RaiseEvent(args);
        }

        /// <summary>
        /// 下拉箭头鼠标进入触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chk_Togglet_MouseEnter(object sender, MouseEventArgs e)
        {
            if (DataSourceList != null && DataSourceList.Count > 0)
            {
                chk_Togglet.IsChecked = true;
                NoResultBorder.Visibility = Visibility.Collapsed;
                listBox_TextPopup.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 下拉箭头点击触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chk_Togglet_Checked(object sender, RoutedEventArgs e)
        {
            //CheckBox checkBox = sender as CheckBox;
            //if ((bool)checkBox.IsChecked)
            //{
            //    NoResultBorder.Visibility = Visibility.Collapsed;
            //    listBox_TextPopup.Visibility = Visibility.Visible;
            //}
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
                chk_Togglet.IsChecked = false;
            }
        }

        /// <summary>
        /// 下拉列表选择触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_TextPopup_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox_TextPopup.SelectedItem != null)
            {
                if (DictionaryModels == null || DictionaryModels.Count == 0)
                {
                    DataSourceList.ToList().ForEach(m =>
                    {
                        DictionaryModels.Add(m);
                    });
                }
                txt_TextPopup.Text = DictionaryModelSelected.Code;
                //处理警员选择变化
                RoutedEventArgs args = new RoutedEventArgs(SelectionChangedEvent, listBox_TextPopup);
                this.RaiseEvent(args);
            }
        }

        #region 控制Popup在界面滚动时错位问题
        /// <summary>
        /// 下拉框关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popup_TextPopup_Closed(object sender, EventArgs e)
        {
            List<ScrollViewer> scrollViewers = FindVisualParent<ScrollViewer>(popup_TextPopup);
            if (scrollViewers?.Count > 0)
            {
                scrollViewers.ForEach(m => m.ScrollChanged -= Scrollviewer_ScrollChanged);
                scrollViewers.Clear();
            }
        }

        /// <summary>
        /// 控件外滚动条滚动时，关闭Popup，避免位置错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scrollviewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            //如果是listbox的滚动条滚动则不关闭下拉框
            if (e.OriginalSource == FindVisualChild<ScrollViewer>(listBox_TextPopup))
            {
                return;
            }
            e.Handled = true;
            if (popup_TextPopup != null)
            {
                if (popup_TextPopup.IsOpen)
                {
                    popup_TextPopup.IsOpen = false;
                }
            }
        }

        /// <summary>
        /// 下拉框打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popup_TextPopup_Opened(object sender, EventArgs e)
        {
            List<ScrollViewer>  scrollViewers = FindVisualParent<ScrollViewer>(popup_TextPopup);
            if (scrollViewers?.Count > 0)
            {
                scrollViewers.ForEach(m =>
                {
                    m.ScrollChanged += Scrollviewer_ScrollChanged;
                });
            }
        }

        private void Scrollview_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            e.Handled = true;
        }
        #endregion

        /// <summary>
        /// 查找可视化树子级指定类型控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null) return childItem;
                }
            }
            return null;
        }

        /// <summary>
        /// 查找可视化树父级指定类型控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<T> FindVisualParent<T>(DependencyObject obj) where T : DependencyObject
        {
            try
            {
                List<T> TList = new List<T> { };
                DependencyObject parent = VisualTreeHelper.GetParent(obj);
                if (parent != null)
                {
                    if (parent is T model)
                    {
                        TList.Add(model);
                    }
                    TList.AddRange(FindVisualParent<T>(parent));
                }
                return TList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取拼音首字母
        /// </summary>
        /// <param name="CnChar"></param>
        /// <returns></returns>
        public static string GetFirstLetterOfChineseString(string CnChar)
        {
            long iCnChar;
            byte[] ZW = System.Text.Encoding.Default.GetBytes(CnChar);

            //如果是字母，则直接返回
            if (ZW.Length == 1)
            {
                return CnChar.ToUpper();
            }
            else
            {
                int i1 = (short)(ZW[0]);
                int i2 = (short)(ZW[1]);
                iCnChar = i1 * 256 + i2;
            }
            if ((iCnChar >= 45217) && (iCnChar <= 45252))
            {
                return "A";
            }
            else if ((iCnChar >= 45253) && (iCnChar <= 45760))
            {
                return "B";
            }
            else if ((iCnChar >= 45761) && (iCnChar <= 46317))
            {
                return "C";
            }
            else if ((iCnChar >= 46318) && (iCnChar <= 46825))
            {
                return "D";
            }
            else if ((iCnChar >= 46826) && (iCnChar <= 47009))
            {
                return "E";
            }
            else if ((iCnChar >= 47010) && (iCnChar <= 47296))
            {
                return "F";
            }
            else if ((iCnChar >= 47297) && (iCnChar <= 47613))
            {
                return "G";
            }
            else if ((iCnChar >= 47614) && (iCnChar <= 48118))
            {
                return "H";
            }
            else if ((iCnChar >= 48119) && (iCnChar <= 49061))
            {
                return "J";
            }
            else if ((iCnChar >= 49062) && (iCnChar <= 49323))
            {
                return "K";
            }
            else if ((iCnChar >= 49324) && (iCnChar <= 49895))
            {
                return "L";
            }
            else if ((iCnChar >= 49896) && (iCnChar <= 50370))
            {
                return "M";
            }

            else if ((iCnChar >= 50371) && (iCnChar <= 50613))
            {
                return "N";
            }
            else if ((iCnChar >= 50614) && (iCnChar <= 50621))
            {
                return "O";
            }
            else if ((iCnChar >= 50622) && (iCnChar <= 50905))
            {
                return "P";
            }
            else if ((iCnChar >= 50906) && (iCnChar <= 51386))
            {
                return "Q";
            }
            else if ((iCnChar >= 51387) && (iCnChar <= 51445))
            {
                return "R";
            }
            else if ((iCnChar >= 51446) && (iCnChar <= 52217))
            {
                return "S";
            }
            else if ((iCnChar >= 52218) && (iCnChar <= 52697))
            {
                return "T";
            }
            else if ((iCnChar >= 52698) && (iCnChar <= 52979))
            {
                return "W";
            }
            else if ((iCnChar >= 52980) && (iCnChar <= 53640))
            {
                return "X";
            }
            else if ((iCnChar >= 53689) && (iCnChar <= 54480))
            {
                return "Y";
            }
            else if ((iCnChar >= 54481) && (iCnChar <= 55289))
            {
                return "Z";
            }
            else return ("?");
        }
        #endregion


    }
}
