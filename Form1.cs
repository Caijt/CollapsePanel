using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CollapsePanelForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<MenuData> menuList = new List<MenuData>()
            {
                new MenuData(){ Id=1,ParentId=null,Name="IT中心",Path="it" },
                new MenuData(){ Id=2,ParentId=null,Name="人力资源",Path="hr" },
                new MenuData(){ Id=3,ParentId=null,Name="系统管理",Path="sys" },
                new MenuData(){ Id=4,ParentId=1,Name="IT资产",Path="asset" },
                new MenuData(){ Id=5,ParentId=4,Name="资产管理",Path="edit" },
                new MenuData(){ Id=6,ParentId=4,Name="资产领用",Path="use" },
                new MenuData(){ Id=7,ParentId=4,Name="资产交还",Path="return" },
                new MenuData(){ Id=8,ParentId=2,Name="部门管理",Path="dep" },
                new MenuData(){ Id=9,ParentId=2,Name="员工管理",Path="employee" },
                new MenuData(){ Id=10,ParentId=3,Name="用户管理",Path="user" },
            };
            this.collapsePanel1.Menus.AddRange(menuList);
            this.collapsePanel1.InitMenus();
        }

        private void CollapsePanel1_MenuDoubleClick(object sender, EventArgs e)
        {
            TreeView tree = sender as TreeView;
            if (tree == null)
                return;
            var node = tree.SelectedNode;
            if (node == null)
                return;
            //只对无子菜单的菜单弹出窗口
            if (node.Nodes.Count == 0)
            {
                MessageBox.Show($"当前菜单对应的路径是{node.Tag.ToString()}");
            }
        }
    }
}
