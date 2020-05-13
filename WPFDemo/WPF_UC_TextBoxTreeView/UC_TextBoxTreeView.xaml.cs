using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace WPF_UC_TextBoxTreeView
{
    /// <summary>
    /// UC_TextBoxTreeView.xaml 的交互逻辑
    /// </summary>
    public partial class UC_TextBoxTreeView : UserControl
    {
        #region 基本变量
        List<DeptNode> lst_DeptNode = null;//数据源
        List<DeptNode> lst_TreeNodes = null;//绑定的树形list
        List<DeptNode> lst_SelectedNodes = new List<DeptNode>();//已勾选的节点
        #endregion

        public UC_TextBoxTreeView()
        {
            InitializeComponent();
            InitDepartment();
        }

        #region 控件触发事件
        /// <summary>
        /// 文本框点击触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_Tree_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            popup_Tree.IsOpen = true;
            Toggle.IsChecked = true;
        }

        /// <summary>
        /// 勾选点击触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (item != null)
            {
                item.Focus();
                if (tv_Tree.SelectedItem != null)
                {
                    DeptNode tree = (DeptNode)tv_Tree.SelectedItem;
                    if (lst_SelectedNodes.Find(s=>s.Layer == tree.Layer)==null)//同一层级节点不能多选
                    {
                        if (tree.IsChecked)
                        {
                            lst_SelectedNodes.Add(tree);
                        }
                        else
                        {
                            lst_SelectedNodes.Remove(tree);
                        }
                        SetChildrenChecked(tree, tree.IsChecked);
                        //SetParentChecked(tree, tree.IsChecked);//暂不勾选父节点
                    }
                    else
                    {
                        tree.IsChecked = false;
                        return;
                    }
                }
                e.Handled = true;
            }
            SetText();
        }
        #endregion

        #region 自定义方法
        /// <summary>
        /// 获取部门树形数据源
        /// </summary>
        private void InitDepartment()
        {
            //获取部门数据
            XDocument xd_Dept = XDocument.Load(@".\department.xml".Trim());
            lst_DeptNode = (from deptNode in xd_Dept.Descendants("DeptDetail").Where(d => d.Element("FlagDeleted").Value == "0" && d.Element("FlagTrashed").Value == "0")
                                    select new DeptNode
                                    {
                                        Layer = int.Parse(deptNode.Element("Layer").Value),
                                        DeptName = deptNode.Element("DeptName").Value,
                                        DeptNo = deptNode.Element("DeptNo").Value,
                                        ParentDeptNo = deptNode.Element("Layer").Value == "1" ? "" : deptNode.Element("Layer").Value == "2" ?
                                        deptNode.Element("DeptNo").Value.Substring(0, 2) : deptNode.Element("Layer").Value == "3" ?
                                        deptNode.Element("DeptNo").Value.Substring(0, 4) : deptNode.Element("DeptNo").Value.Substring(0, 6),
                                        Header = deptNode.Element("Header").Value,
                                        DeptID = deptNode.Element("DeptID").Value
                                    }).ToList();
            if (lst_DeptNode.Count > 0)
            {
                lst_TreeNodes = BindDeptNode(lst_DeptNode);
                tv_Tree.ItemsSource = lst_TreeNodes;
            }
        }

        /// <summary>
        /// 设置文本框文本
        /// </summary>
        private void SetText()
        {
            txt_Tree.Text = string.Empty;
            txt_Tree.Tag = "'";
            //List<DeptNode> lst_SelectedNodes = GetSelectedNodes(lst_TreeNodes);
            if (lst_SelectedNodes.Count>0)
            {
                lst_SelectedNodes = lst_SelectedNodes.OrderBy(s => s.Layer).ToList();
                txt_Tree.Text = lst_SelectedNodes.Take(1).ToList()[0].DeptName;//文本框只返回层级最高节点名字
                foreach (DeptNode node in lst_SelectedNodes)
                {
                    //txt_Tree.Text += node.DeptName+",";
                    txt_Tree.Tag += node.DeptID + "','";
                }
                //txt_Tree.Text = txt_Tree.Text.Substring(0, txt_Tree.Text.Length - 1);
                txt_Tree.Tag = txt_Tree.Tag.ToString().Substring(0, txt_Tree.Tag.ToString().Length - 2);
            }
        }

        /// <summary>
        /// 获取元素的父级
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);

            return source;
        }

        /// <summary>
        /// 勾选子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isChecked"></param>
        public void SetChildrenChecked(DeptNode node, bool isChecked)
        {
            foreach (DeptNode child in node.Nodes)
            {
                child.IsChecked = isChecked;
                if (isChecked)
                {
                    lst_SelectedNodes.Add(child);
                }
                else
                {
                    lst_SelectedNodes.Remove(child);
                }
                SetChildrenChecked(child,isChecked);
            }
        }

        /// <summary>
        /// 勾选父节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isChecked"></param>
        public void SetParentChecked(DeptNode node, bool isChecked)
        {
            if (!string.IsNullOrEmpty(node.ParentDeptNo))
            {
                DeptNode node_Parent = lst_DeptNode.Find(n => n.DeptNo == node.ParentDeptNo);
                if (node_Parent.Nodes.Find(n => n.IsChecked == true) != null)//若父节点的子节点有被勾选的 则勾选父节点
                {
                    node_Parent.IsChecked = true;
                    SetParentChecked(node_Parent, isChecked);
                }
                else
                {
                    node_Parent.IsChecked = false;
                    SetParentChecked(node_Parent, isChecked);
                }
            }
        }

        /// <summary>
        /// 私有方法，忽略层次关系的情况下，获取选中节点
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<DeptNode> GetSelectedNodes(List<DeptNode> list)
        {
            List<DeptNode> lst_Selected = new List<DeptNode>();
            foreach (var tree in list)
            {
                if (tree.IsChecked)
                {
                    lst_Selected.Add(tree);
                    foreach (var child in GetSelectedNodes(tree.Nodes))
                    {
                        lst_Selected.Add(child);
                    }
                }
            }
            return lst_Selected;
        }

        /// <summary>
        /// 绑定部门树
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private List<DeptNode> BindDeptNode(List<DeptNode> nodes)
        {
            List<DeptNode> outputList = new List<DeptNode>();
            if (nodes == null) return outputList;
            List<DeptNode> Layer1Nodes = nodes.Where(i => i.Layer == 1).OrderBy(i => i.DeptNo).ToList();
            List<DeptNode> Layer2Nodes = nodes.Where(i => i.Layer == 2).OrderBy(i => i.ParentDeptNo).ToList();
            List<DeptNode> Layer3Nodes = nodes.Where(i => i.Layer == 3).OrderBy(i => i.ParentDeptNo).ToList();
            List<DeptNode> Layer4Nodes = nodes.Where(i => i.Layer == 4).OrderBy(i => i.ParentDeptNo).ToList();

            foreach (var item in Layer1Nodes)
            {
                outputList.Add(item);
            }

            BindNewNode(Layer1Nodes, Layer2Nodes);

            BindNewNode(Layer2Nodes, Layer3Nodes);

            BindNewNode(Layer3Nodes, Layer4Nodes);
            
            return outputList;
        }

        /// <summary>
        /// 树的层级绑定
        /// </summary>
        /// <param name="lst_LayerSuperiorNodes"></param>
        /// <param name="lst_LayerSubordinateNodes"></param>
        private void BindNewNode(List<DeptNode> lst_LayerSuperiorNodes, List<DeptNode> lst_LayerSubordinateNodes)
        {
            List<DeptNode> lst_Result = null;
            foreach (var item in lst_LayerSuperiorNodes)
            {
                lst_Result = lst_LayerSubordinateNodes.FindAll(n => n.ParentDeptNo == item.DeptNo);
                if (lst_Result != null)
                {
                    item.Nodes.AddRange(lst_Result);
                }
            }
        }
        #endregion
    }
}
