
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
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
using System.Drawing;
using System.Text.RegularExpressions;

namespace WPF_UC_DropDown
{
    /// <summary>
    /// UC_DropDown.xaml 的交互逻辑
    /// </summary>
    public partial class UC_DropDown : UserControl
    {
        #region 基本变量
        bool isDown = false;//是否展开
        //bool isWrong = false;//输入框输入是否错误
        public Dictionary<string, string> dic_Selected = new Dictionary<string, string>();//已选择的条件字典
        KeyValuePair<string, string> kvp_Old = new KeyValuePair<string, string>();//已选择的条件键/值对
        Dictionary<Label, string> dic_SelectedLabel = new Dictionary<Label, string>();//已选择的标签条件
        KeyValuePair<Label, string> kvp_OldLabel = new KeyValuePair<Label, string>();//已选择的标签条件键/值对
        SolidColorBrush scb_Focused = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 128, 0));//默认的Label选中颜色
        SolidColorBrush scb_UnFocused = (SolidColorBrush)new BrushConverter().ConvertFromString("#666666");//非选中的Label颜色
        #endregion

        public UC_DropDown()
        {
            InitializeComponent();
        }

        #region 控件触发事件
        /// <summary>
        /// listbox标签点击触发（暂不用）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void Lbl_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    Label lbl_Selected = sender as Label;
        //    kvp_Old = dic_Selected.FirstOrDefault(d => d.Key.ToString() == lbl_Selected.Tag.ToString());
        //    if (kvp_Old.Key != null)
        //    {
        //        txt_DropDown.Text = txt_DropDown.Text.Replace(txt_DropDown.Text.Substring(txt_DropDown.Text.IndexOf(kvp_Old.Key.ToString() + kvp_Old.Value),
        //                    kvp_Old.Key.ToString().Length + kvp_Old.Value.Length), "");
        //        dic_Selected.Remove(kvp_Old.Key);
        //    }
        //    txt_DropDown.Text += lbl_Selected.Tag.ToString() + lbl_Selected.Content;
        //    dic_Selected.Add(lbl_Selected.Tag.ToString(), lbl_Selected.Content.ToString());
        //}

        /// <summary>
        /// WrapPanel容器失去鼠标焦点触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Wp_LostFocus(object sender, RoutedEventArgs e)
        {
            WrapPanel wp = sender as WrapPanel;
            if (!wp.IsMouseOver)
            {
                //若出现失去焦点文本框无值 则删掉该条件
                List<TextBox> lst_TextBoxEmpty = GetChildObjects<TextBox>(wp, "").FindAll(t => string.IsNullOrEmpty(t.Text.Trim())).OrderBy(t => t.TabIndex).ToList();
                if (lst_TextBoxEmpty.Count == 2)
                {
                    kvp_Old = dic_Selected.FirstOrDefault(d => d.Key.ToString() == lst_TextBoxEmpty[0].Tag.ToString());
                    if (kvp_Old.Key != null)
                    {
                        txt_DropDown.Text = txt_DropDown.Text.Replace(txt_DropDown.Text.Substring(txt_DropDown.Text.IndexOf(kvp_Old.Key.ToString() + ":" + kvp_Old.Value),
                                   kvp_Old.Key.ToString().Length + 1 + kvp_Old.Value.Length), "");
                        dic_Selected.Remove(kvp_Old.Key);
                    }
                }
                else
                {
                    List<TextBox> lst_TextBox = GetChildObjects<TextBox>(wp, "").FindAll(t => !string.IsNullOrEmpty(t.Text.Trim())).OrderBy(t => t.TabIndex).ToList();
                    List<Label> lst_LabelUnit = GetChildObjects<Label>(wp, "").FindAll(l => l.Tag.ToString() == "Unit");//获取单位标签
                    string str_Unit = string.Empty;//单位
                    if (lst_LabelUnit.Count == 1)
                    {
                        str_Unit = lst_LabelUnit[0].Content.ToString();
                    }
                    if (lst_TextBox.Count == 1)
                    {
                        //容器内获取到1个文本框时只有内容不为空才是有效输入
                        if (!string.IsNullOrEmpty(lst_TextBox[0].Text.Trim()))
                        {
                            if (Regex.Match(lst_TextBox[0].Text.Trim(), "^([0-9]*[1-9][0-9]*)$").Success)
                            {
                                kvp_Old = dic_Selected.FirstOrDefault(d => d.Key.ToString() == lst_TextBox[0].Tag.ToString());
                                if (kvp_Old.Key != null)
                                {
                                    txt_DropDown.Text = txt_DropDown.Text.Replace(txt_DropDown.Text.Substring(txt_DropDown.Text.IndexOf(kvp_Old.Key.ToString() + ":" + kvp_Old.Value),
                                               kvp_Old.Key.ToString().Length + 1 + kvp_Old.Value.Length), "");
                                    dic_Selected.Remove(kvp_Old.Key);
                                    kvp_OldLabel = dic_SelectedLabel.FirstOrDefault(d => d.Value == lst_TextBox[0].Tag.ToString());
                                    if (kvp_OldLabel.Key != null)
                                    {
                                        kvp_OldLabel.Key.Foreground = scb_UnFocused;
                                        dic_SelectedLabel.Remove(kvp_OldLabel.Key);
                                    }
                                }
                                if (lst_TextBox[0].TabIndex == 0)
                                {

                                    dic_Selected.Add(lst_TextBox[0].Tag.ToString(), "大于等于" + lst_TextBox[0].Text.Trim() + str_Unit);
                                    txt_DropDown.Text += lst_TextBox[0].Tag + ":" + "大于等于" + lst_TextBox[0].Text.Trim() + str_Unit;
                                }
                                else
                                {
                                    dic_Selected.Add(lst_TextBox[0].Tag.ToString(), "小于等于" + lst_TextBox[0].Text.Trim() + str_Unit);
                                    txt_DropDown.Text += lst_TextBox[0].Tag + ":" + "小于等于" + lst_TextBox[0].Text.Trim() + str_Unit;
                                }
                                //txt_DropDown.ScrollToEnd();
                            }
                            else
                            {
                                MessageBox.Show("【" + lst_TextBox[0].Tag + "】输入框输入错误，请检查！");
                                FocusManager.SetFocusedElement(lst_TextBox[0], lst_TextBox[0]);
                                lst_TextBox[0].Focus();
                                lst_TextBox[0].SelectAll();
                                popup_UpDown.IsOpen = true;
                            }
                        }
                    }
                    else if (lst_TextBox.Count == 2)
                    {
                        //容器内获取到2个文本框时只有内容都不为空才是有效输入
                        if (!string.IsNullOrEmpty(lst_TextBox[0].Text.Trim()) && !string.IsNullOrEmpty(lst_TextBox[1].Text.Trim()))
                        {
                            if ((int.Parse(lst_TextBox[1].Text.Trim()) > int.Parse(lst_TextBox[0].Text.Trim())) && Regex.Match(lst_TextBox[0].Text.Trim(),
                            "^([0-9]*[1-9][0-9]*)$").Success && Regex.Match(lst_TextBox[1].Text.Trim(), "^([0-9]*[1-9][0-9]*)$").Success)
                            {
                                kvp_Old = dic_Selected.FirstOrDefault(d => d.Key.ToString() == lst_TextBox[0].Tag.ToString());
                                if (kvp_Old.Key != null)
                                {
                                    txt_DropDown.Text = txt_DropDown.Text.Replace(txt_DropDown.Text.Substring(txt_DropDown.Text.IndexOf(kvp_Old.Key.ToString() + ":" + kvp_Old.Value),
                                               kvp_Old.Key.ToString().Length + 1 + kvp_Old.Value.Length), "");
                                    dic_Selected.Remove(kvp_Old.Key);
                                    kvp_OldLabel = dic_SelectedLabel.FirstOrDefault(d => d.Value == lst_TextBox[0].Tag.ToString());
                                    if (kvp_OldLabel.Key != null)
                                    {
                                        kvp_OldLabel.Key.Foreground = scb_UnFocused;
                                        dic_SelectedLabel.Remove(kvp_OldLabel.Key);
                                    }
                                }
                                dic_Selected.Add(lst_TextBox[0].Tag.ToString(), lst_TextBox[0].Text.Trim() + "-" + lst_TextBox[1].Text.Trim() + str_Unit);
                                txt_DropDown.Text += lst_TextBox[0].Tag + ":" + lst_TextBox[0].Text.Trim() + "-" + lst_TextBox[1].Text.Trim() + str_Unit;
                                //txt_DropDown.ScrollToEnd();
                            }
                            else
                            {
                                MessageBox.Show("【" + lst_TextBox[0].Tag + "】输入框输入错误，请检查！");
                                FocusManager.SetFocusedElement(lst_TextBox[0], lst_TextBox[0]);
                                lst_TextBox[0].Focus();
                                lst_TextBox[0].SelectAll();
                                popup_UpDown.IsOpen = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// WrapPanel容器点击触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Wp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WrapPanel wp = sender as WrapPanel;
            List<Label> lst_label = GetChildObjects<Label>(wp, "").FindAll(a => ((a.IsMouseOver == true || a.Foreground.ToString() == scb_Focused.ToString())&&a.Tag.ToString()!="Unit"));
            if (lst_label.Count==1)
            {
                //非同一标签点击才是有效点击
                if (!dic_SelectedLabel.ContainsKey(lst_label[0]))
                {
                    lst_label[0].Foreground = scb_Focused;
                    kvp_Old = dic_Selected.FirstOrDefault(d => d.Key.ToString() == lst_label[0].Tag.ToString());
                    if (kvp_Old.Key != null)
                    {
                        txt_DropDown.Text = txt_DropDown.Text.Replace(txt_DropDown.Text.Substring(txt_DropDown.Text.IndexOf(kvp_Old.Key.ToString() + ":" + kvp_Old.Value),
                                    kvp_Old.Key.ToString().Length + 1 + kvp_Old.Value.Length), "");
                        dic_Selected.Remove(kvp_Old.Key);
                    }
                    if (lst_label[0].Content.ToString() != "不限")
                    {
                        txt_DropDown.Text += lst_label[0].Tag.ToString() + ":" + lst_label[0].Content;
                        dic_Selected.Add(lst_label[0].Tag.ToString(), lst_label[0].Content.ToString());
                        dic_SelectedLabel.Add(lst_label[0], lst_label[0].Tag.ToString());
                    }
                }
            }
            if (lst_label.Count == 2)
            {
                lst_label.ForEach(delegate (Label label)
                {
                    if (label.Foreground.ToString() == scb_Focused.ToString())
                    {
                        label.Foreground = scb_UnFocused;
                        if (dic_SelectedLabel.ContainsKey(label))
                        {
                            dic_SelectedLabel.Remove(label);
                        }
                    }
                    else
                    {
                        label.Foreground = scb_Focused;
                        kvp_Old = dic_Selected.FirstOrDefault(d => d.Key.ToString() == label.Tag.ToString());
                        if (kvp_Old.Key != null)
                        {
                            txt_DropDown.Text = txt_DropDown.Text.Replace(txt_DropDown.Text.Substring(txt_DropDown.Text.IndexOf(kvp_Old.Key.ToString() + ":" + kvp_Old.Value),
                                        kvp_Old.Key.ToString().Length + 1 + kvp_Old.Value.Length), "");
                            dic_Selected.Remove(kvp_Old.Key);
                        }
                        if (label.Content.ToString() != "不限")
                        {
                            txt_DropDown.Text += label.Tag.ToString() + ":" + label.Content;
                            dic_Selected.Add(label.Tag.ToString(), label.Content.ToString());
                            dic_SelectedLabel.Add(label, label.Tag.ToString());
                        }
                    }
                });
            }
        }


        /// <summary>
        /// txt_DropDown第一次鼠标点击触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Txt_DropDown_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            popup_UpDown.IsOpen = true;
            popup_UpDown.Visibility = Visibility.Visible;
            //ca_Up.Focus();
            if (isDown)
            {
                gr_Down.Visibility = Visibility.Visible;
            }

        }

        /// <summary>
        /// btn_UpDown点击触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_UpDown_Click(object sender, RoutedEventArgs e)
        {
            if (!isDown)
            {
                isDown = true;
                btn_UpDown.Content = "收起";
                gr_Down.Visibility = Visibility.Visible;
                scroll.Height = 458;
            }
            else
            {
                isDown = false;
                btn_UpDown.Content = "展开";
                gr_Down.Visibility = Visibility.Collapsed;
                scroll.Height = 285;
            }
        }
        #endregion

        #region 自定义方法
        /// <summary>
        /// 获取容器内子控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<T> GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Name == name || string.IsNullOrEmpty(name)))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(((T)child).Tag)))
                    {
                        childList.Add((T)child);
                    }
                }
                childList.AddRange(GetChildObjects<T>(child, ""));//指定集合的元素添加到List队尾  
            }
            return childList;
        }

        /// <summary>
        /// 清除控件内容
        /// </summary>
        public void Clear()
        {
            txt_DropDown.Clear();
            dic_Selected.Clear();
            if (dic_SelectedLabel.Count > 0)
            {
                foreach (Label lbl in dic_SelectedLabel.Keys)
                {
                    if (lbl.Content.ToString() != "不限")
                    {
                        lbl.Foreground = scb_UnFocused;
                    }
                }
                dic_SelectedLabel.Clear();
            }
            //清除内容 默认选择不限
            List<Label> lst_lblUnlimited = GetChildObjects<Label>(gr_UpDown, "").FindAll(t => t.Content.ToString() == "不限" && t.Foreground.ToString() == scb_UnFocused.ToString()).ToList();
            if (lst_lblUnlimited.Count > 0)
            {
                lst_lblUnlimited.ForEach(delegate (Label label)
                {
                    label.Foreground = scb_Focused;
                });
            }
            //清除所有文本框内容
            List<TextBox> lst_txt = GetChildObjects<TextBox>(gr_UpDown, "").FindAll(t => !string.IsNullOrEmpty(t.Text.Trim())).ToList();
            if (lst_txt.Count > 0)
            {
                lst_txt.ForEach(delegate (TextBox textBox)
                {
                    textBox.Clear();
                });
            }
        }
        #endregion

    }


}
