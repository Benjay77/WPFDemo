using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_UC_TextBoxTreeView
{
    public class DeptNode : NotifyPropertyChanged
    {
        public DeptNode()
        {
            this.Nodes = new List<DeptNode>();
            this.ParentDeptNo = null;
        }
        public int Layer { get; set; }
        public string DeptName { get; set; }
        public string DeptNo { get; set; }
        public string ParentDeptNo { get; set; }
        public string Header { get; set; }
        public string DeptID { get; set; }
        public List<DeptNode> Nodes { get; set; }

        bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    ChangeProperty("IsExpanded");
                }
            }
        }


        bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value != _isChecked)
                {
                    _isChecked = value;
                    ChangeProperty("IsChecked");
                }
            }
        }
    }
}
