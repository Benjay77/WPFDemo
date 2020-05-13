using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WPF_UC_Menu
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// 
    /// Description     
    /// 自定义控件Demo
    /// 
    /// Editor         Edtion           Date           Memo
    /// Benjay         1.0              2017.12.04     新建自定义菜单控件
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 基本变量
        public List<MenuItemViewModel> lst_MenuItem = new List<MenuItemViewModel>();//菜单数据源
        public ObservableCollection<MenuItemViewModel> MenuItems;//按层级绑定的菜单
        public MenuItem menuItemChecked = null;//选中的一级菜单
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            lst_MenuItem.Add(new MenuItemViewModel { Header = "房源", Code = "fy", ID = 1, ParentID = 0, MenuLevel = 1, MenuImageUrl = "", MenuDLL = "RCApp_Win.View.House", MenuWindowName = "",MenuWindowNameAdditional = "", IsMDIWindow = false, SubClassType = "",IsEnable = true });
            lst_MenuItem.Add(new MenuItemViewModel { Header = "客源", Code = "ky", ID = 2, ParentID = 0, MenuLevel = 1, MenuImageUrl = "", MenuDLL = "RCApp_Win.View.Customer", MenuWindowName = "", MenuWindowNameAdditional = "", IsMDIWindow = false, SubClassType="",IsEnable = true });
            lst_MenuItem.Add(new MenuItemViewModel { Header = "客源列表", Code = "ky-info", ID = 201, ParentID = 2, MenuLevel = 2, MenuImageUrl = "", MenuDLL = "RCApp_Win.View.Customer", MenuWindowName = "NewCustInfoPage", MenuWindowNameAdditional = "", IsMDIWindow = true, SubClassType = "", IsEnable = true });
            lst_MenuItem.Add(new MenuItemViewModel { Header = "约看管理", Code = "ky-abouttosee", ID = 202, ParentID = 2, MenuLevel = 2, MenuImageUrl = "", MenuDLL = "RCApp_Win.View.Customer", MenuWindowName = "AboutToSeePage", MenuWindowNameAdditional = "", IsMDIWindow = true, SubClassType = "", IsEnable = true });
            lst_MenuItem.Add(new MenuItemViewModel { Header = "约看管理子菜单1", Code = "ky-abouttosee-child1", ID = 20201, ParentID = 202, MenuLevel = 3, MenuImageUrl = "", MenuDLL = "RCApp_Win.View.Customer", MenuWindowName = "AboutToSeePageChild1", MenuWindowNameAdditional = "", IsMDIWindow = true, SubClassType = "", IsEnable = true });
            lst_MenuItem.Add(new MenuItemViewModel { Header = "约看管理子菜单2", Code = "ky-abouttosee-child2", ID = 20202, ParentID = 202, MenuLevel = 3, MenuImageUrl = "", MenuDLL = "RCApp_Win.View.Customer", MenuWindowName = "AboutToSeePageChild2", MenuWindowNameAdditional = "", IsMDIWindow = true, SubClassType = "", IsEnable = true });
            lst_MenuItem.Add(new MenuItemViewModel { Header = "约看管理子菜单3", Code = "ky-abouttosee-child3", ID = 20203, ParentID = 202, MenuLevel = 3, MenuImageUrl = "", MenuDLL = "RCApp_Win.View.Customer", MenuWindowName = "AboutToSeePageChild3", MenuWindowNameAdditional = "", IsMDIWindow = true, SubClassType = "", IsEnable = true });
            lst_MenuItem.Add(new MenuItemViewModel { Header = "带看管理", Code = "ky-look", ID = 203, ParentID = 2, MenuLevel = 2, MenuImageUrl = "", MenuDLL = "RCApp_Win.View.Customer", MenuWindowName = "AboutToSeePage", MenuWindowNameAdditional = "", IsMDIWindow = true, SubClassType = "", IsEnable = true });
            lst_MenuItem.Add(new MenuItemViewModel { Header = "工作台", Code = "mysy", ID = 3, ParentID = 0, MenuLevel = 1, MenuImageUrl = "", MenuDLL = "RCApp_Win.View.MyHomePage", MenuWindowName = "MyHomePageInfo", MenuWindowNameAdditional = "", IsMDIWindow = true, SubClassType = "", IsEnable = true });

            MenuItems = new ObservableCollection<MenuItemViewModel>
            {
                new MenuItemViewModel { Header = "房源",Code = "fy",ID = 1,ParentID=0,MenuLevel=1,MenuImageUrl="",MenuDLL="RCApp_Win.View.House",MenuWindowName="",MenuWindowNameAdditional = "",IsMDIWindow = false, SubClassType="",IsEnable = true },
                new MenuItemViewModel { Header = "客源",Code = "ky",ID = 2,ParentID=0,MenuLevel=1,MenuImageUrl="",MenuDLL="RCApp_Win.View.Customer",MenuWindowName="",MenuWindowNameAdditional = "",IsMDIWindow = false, SubClassType="",IsEnable = true,
                    MenuItems = new ObservableCollection<MenuItemViewModel>
                        {
                            new MenuItemViewModel { Header = "客源列表", Code = "ky-info",ID = 201,ParentID=2,MenuLevel=2,MenuImageUrl="",MenuDLL="RCApp_Win.View.Customer",MenuWindowName="NewCustInfoPage",MenuWindowNameAdditional = "",IsMDIWindow = true, SubClassType="",IsEnable = true},
                            new MenuItemViewModel { Header = "约看管理",Code = "ky-abouttosee",ID = 202,ParentID=2,MenuLevel=2,MenuImageUrl="",MenuDLL="RCApp_Win.View.Customer",MenuWindowName="AboutToSeePage",MenuWindowNameAdditional = "",IsMDIWindow = true, SubClassType="",IsEnable = true,
                                MenuItems = new ObservableCollection<MenuItemViewModel>
                                {
                                    new MenuItemViewModel { Header = "约看管理子菜单1",Code = "ky-abouttosee-child1",ID = 20201,ParentID=202,MenuLevel=3,MenuImageUrl="",MenuDLL="RCApp_Win.View.Customer",MenuWindowName="AboutToSeePageChild1",MenuWindowNameAdditional = "",IsMDIWindow = true, SubClassType="",IsEnable = true },
                                    new MenuItemViewModel { Header = "约看管理子菜单2",Code = "ky-abouttosee-child2",ID = 20202,ParentID=202,MenuLevel=3,MenuImageUrl="",MenuDLL="RCApp_Win.View.Customer",MenuWindowName="AboutToSeePageChild2",MenuWindowNameAdditional = "",IsMDIWindow = true, SubClassType="",IsEnable = true },
                                    new MenuItemViewModel { Header = "约看管理子菜单3",Code = "ky-abouttosee-child3",ID = 20203,ParentID=202,MenuLevel=3,MenuImageUrl="",MenuDLL="RCApp_Win.View.Customer",MenuWindowName="AboutToSeePageChild3",MenuWindowNameAdditional = "",IsMDIWindow = true, SubClassType="",IsEnable = true },
                                }
                            },
                            new MenuItemViewModel {  Header = "带看管理",Code = "ky-look",ID = 203,ParentID=2,MenuLevel=2,MenuImageUrl="",MenuDLL="RCApp_Win.View.Customer",MenuWindowName="AboutToSeePage",MenuWindowNameAdditional = "",IsMDIWindow = true, SubClassType="",IsEnable = true }
                        }
                },
                new MenuItemViewModel { Header = "工作台", Code = "mysy",ID = 3,ParentID=0,MenuLevel=1,MenuImageUrl="",MenuDLL="RCApp_Win.View.MyHomePage",MenuWindowName="MyHomePageInfo",MenuWindowNameAdditional = "",IsMDIWindow = true, SubClassType="",IsEnable = true }
            };

            if (MenuItems!=null)
            {
                List<MenuItemViewModel> list = MenuItems.ToList().FindAll(p => p.ParentID == 0);
                if (list!=null&&list.Count>0)
                {
                    BindMenu(0, null, list);
                }
                else
                {
                    MessageBox.Show("无菜单！");
                }
            }
        }

        #region 控件触发事件
        /// <summary>
        /// 菜单点击触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //根据当前点击的菜单执行打开窗口操作
            MenuItem menuItem = sender as MenuItem;
            MenuItemClick(menuItem);
            e.Handled = true;
        }
        #endregion

        #region 自定义方法
        /// <summary>
        /// 菜单绑定
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="item"></param>
        /// <param name="lst_CurrentMenuItems"></param>
        private void BindMenu(int parentId, MenuItem item, List<MenuItemViewModel> lst_CurrentMenuItems)
        {
            foreach (MenuItemViewModel menuItemView in lst_CurrentMenuItems)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Name = menuItemView.MenuWindowName;
                menuItem.Header = menuItemView.Header;
                menuItem.Tag = menuItemView;
                menuItem.Click += new RoutedEventHandler(MenuItem_Click);
                if (parentId == 0)
                {
                    menuItem = SetMenuitemImage(menuItemView, menuItem);
                    this.mainMenu.Items.Add(menuItem);
                }
                else
                {
                    //三级菜单重新设置样式
                    //if (menuItemView.MenuLevel == 3)
                    //{
                    //    menuItem.OverridesDefaultStyle = true;
                    //    if (menuItemView.MenuItems != null && menuItemView.MenuItems.Count > 0)
                    //    {
                    //        menuItem.Style = (Style)this.FindResource("ThirdMenuItemHeader");
                    //    }
                    //    else
                    //    {
                    //        menuItem.Style = (Style)this.FindResource("ThirdMenuItem");
                    //    }
                    //}
                    item.Items.Add(menuItem);
                }
                if (menuItemView.MenuItems != null && menuItemView.MenuItems.Count > 0)
                {
                    ScrollViewer scrollViewer = GetChildObjects<ScrollViewer>(menuItem, "").FirstOrDefault();
                    if (scrollViewer != null)
                    {
                        if (menuItemView.MenuItems.Count < 6)
                        {
                            scrollViewer.Height = scrollViewer.MinHeight * menuItemView.MenuItems.Count;
                        }
                        else
                        {
                            scrollViewer.Height = scrollViewer.MaxHeight;
                        }
                    }

                    BindMenu(menuItemView.ID, menuItem, menuItemView.MenuItems.ToList());
                }
            }
        }

        /// <summary>
        /// 设置一级菜单项内的图片
        /// </summary>
        /// <param name="menuItemView"></param>
        /// <param name="menuItem"></param>
        /// <returns></returns>
        private MenuItem SetMenuitemImage(MenuItemViewModel menuItemView, MenuItem menuItem)
        {
            try
            {
                if (menuItemView != null)
                {
                    Image image = new Image();
                    image.Height = 60;
                    image.Width = 60;
                    switch (menuItemView.ID)
                    {
                        case 1:
                            image.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/nav_02.png"));
                            break;
                        case 2:
                            image.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/nav_03.png"));
                            break;
                        case 3:
                            image.Source = new BitmapImage(new Uri(@"pack://application:,,,/images/nav_01.png"));
                            break;
                        default:
                            break;
                    }
                    menuItem.Header = image;
                }
            }
            catch (Exception ex)
            {
            }
            return menuItem;
        }

        /// <summary>
        /// 菜单点击处理方法
        /// </summary>
        /// <param name="menuItem"></param>
        private void MenuItemClick(MenuItem menuItem)
        {
            MenuItemViewModel menuItemViewModel = menuItem.Tag as MenuItemViewModel;
            if (menuItemViewModel != null)
            {
                if (menuItemViewModel.IsMDIWindow)
                {
                    //添加MDI窗体
                    MessageBox.Show("This is a MDI window!");
                }
                else
                {
                    if (menuItemViewModel.MenuLevel > 1)
                    {
                        //添加正常窗体
                        MessageBox.Show("This is a normal window!");
                    }
                }

                MenuItemClickStyle(menuItemViewModel);
            }
        }

        /// <summary>
        /// 菜单选中样式
        /// </summary>
        /// <param name="menuItem"></param>
        /// <param name="menuItemViewModel"></param>
        public void MenuItemClickStyle(MenuItemViewModel menuItemViewModel)
        {
            //如果有先前选中的一级菜单 取消选中
            if (menuItemChecked != null)
            {
                menuItemChecked.IsChecked = false;
            }
            //选中子菜单一级菜单呈现选中样式
            MenuItem menuItemTopLeve = null;
            try
            {
                switch (menuItemViewModel.MenuLevel)
                {
                    case 3:
                        menuItemTopLeve = GetChildObjects<MenuItem>(mainMenu, "").FindAll(m => ((MenuItemViewModel)m.Tag).ID == lst_MenuItem.FindAll(p => p.ID == lst_MenuItem.FindAll(a => a.ID == menuItemViewModel.ParentID).FirstOrDefault().ParentID).FirstOrDefault().ID).FirstOrDefault();
                        break;
                    case 2:
                        menuItemTopLeve = GetChildObjects<MenuItem>(mainMenu, "").FindAll(m => ((MenuItemViewModel)m.Tag).ID == lst_MenuItem.FindAll(p => p.ID == menuItemViewModel.ParentID).FirstOrDefault().ID).FirstOrDefault();
                        break;
                    default:
                        menuItemTopLeve = GetChildObjects<MenuItem>(mainMenu, "").FindAll(m => ((MenuItemViewModel)m.Tag).ID == menuItemViewModel.ID).FirstOrDefault();
                        break;
                }
                if (menuItemTopLeve != null && !string.IsNullOrEmpty(menuItemTopLeve.Header.ToString()))
                {
                    menuItemTopLeve.IsChecked = true;
                    menuItemChecked = menuItemTopLeve;
                }
            }
            catch (Exception)
            {
                menuItemChecked = null;
            }
        }

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
        #endregion
    }

    public class MenuItemViewModel
    {
        /// <summary>
        /// 菜单头
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 菜单编码（对应权限表编码）
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 菜单ID（唯一）
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 父级菜单ID
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// 菜单等级
        /// </summary>
        public int MenuLevel { get; set; }

        /// <summary> 
        /// 一级菜单头图片地址
        /// </summary>
        public string MenuImageUrl { get; set; }

        /// <summary>
        /// 菜单对应窗体的DLL
        /// </summary>
        public string MenuDLL { get; set; }
        
        /// <summary>
        /// 菜单对应的窗体名字
        /// </summary>
        public string MenuWindowName { get; set; }

        /// <summary>
        /// 菜单对应的额外的窗体名字（多个窗体共用一个菜单）
        /// </summary>
        public string MenuWindowNameAdditional { get; set; }

        /// <summary>
        /// 菜单对应窗体是否MDI窗体
        /// </summary>
        public bool IsMDIWindow { get; set; }

        /// <summary>
        /// 菜单对应窗体的报表类型
        /// </summary>
        public string SubClassType { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 菜单的子菜单集合
        /// </summary>
        public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }
    }
}
