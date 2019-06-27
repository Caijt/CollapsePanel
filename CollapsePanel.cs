using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CollapsePanelForm
{
    public partial class CollapsePanel : UserControl
    {
        /// <summary>
        /// 这是菜单列表数据，控件公开的属性必须定义以下这些特性，不然会出错，提示未标记为可序列化
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Localizable(true)]
        [MergableProperty(false)]
        public List<MenuData> Menus { get; set; }
        /// <summary>
        /// 菜单双击事件
        /// </summary>
        public event EventHandler MenuDoubleClick;
        /// <summary>
        /// 模块的按钮列表
        /// </summary>
        private List<Button> headerButtons;
        /// <summary>
        /// 模块下的菜单，每一个模块下面的菜单对应一个TreeView控件
        /// </summary>
        private List<TreeView> treeViews;
        /// <summary>
        /// 当前控件打开的模块索引值
        /// </summary>
        private int? openMenuIndex = null;
        /// <summary>
        /// 当模块处理打开状态时，模块名称后带的符号
        /// </summary>
        private string openArrow = " <<";
        /// <summary>
        /// 当模块处理关闭状态时，模块名称后带的符号
        /// </summary>
        private string hideArrow = " >>";
        public CollapsePanel()
        {
            InitializeComponent();
            headerButtons = new List<Button>();
            treeViews = new List<TreeView>();
            Menus = new List<MenuData>();
            this.InitMenus();
        }
        /// <summary>
        /// 根据Menus的数据初始化控件，就是动态增加Button跟TreeView控件
        /// </summary>
        public void InitMenus()
        {
            this.Controls.Clear();
            //过滤出所有ParentId为null的根节点，就是模块列表
            foreach (var menu in Menus.Where(a => a.ParentId == null))
            {
                Button headerButton = new Button();
                headerButton.Dock = DockStyle.Top;
                headerButton.Tag = menu.Name;
                headerButton.Text = menu.Name + hideArrow;
                headerButton.TabStop = false;
                headerButton.Click += headerButton_Click;
                headerButtons.Add(headerButton);

                this.Controls.Add(headerButton);
                //这个BringToFront置于顶层方法对于布局很重要
                headerButton.BringToFront();

                TreeView tree = new TreeView();
                //用一个递归方法构建出nodes节点
                tree.Nodes.AddRange(buildTreeNode(menu.Id, menu.Path.Substring(0, 1).ToUpper() + menu.Path.Substring(1)));
                tree.Visible = false;
                tree.Dock = DockStyle.Fill;
                tree.NodeMouseDoubleClick += Tree_DoubleClick;
                treeViews.Add(tree);
                this.Controls.Add(tree);
            }
        }

        private void Tree_DoubleClick(object sender, EventArgs e)
        {
            if (MenuDoubleClick != null)
            {
                MenuDoubleClick(sender, e);
            }
        }
        /// <summary>
        /// 模块按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headerButton_Click(object sender, EventArgs e)
        {
            var clickButton = sender as Button;
            //得出当前单击的模块按钮索引值
            var clickMenuIndex = headerButtons.IndexOf(clickButton);
            //如果当前单击的模块按钮索引值等于已经打开的模块索引值的话，那么当前模块要关闭，否则则打开
            if (openMenuIndex == clickMenuIndex)
            {
                clickButton.Text = clickButton.Tag.ToString() + hideArrow;
                this.treeViews[clickMenuIndex].Hide();
                openMenuIndex = null;
            }
            else
            {
                //关闭之前打开的模块按钮
                if (openMenuIndex.HasValue)
                {
                    this.treeViews[openMenuIndex.Value].Hide();
                    headerButtons[openMenuIndex.Value].Text = headerButtons[openMenuIndex.Value].Tag.ToString() + hideArrow;
                }
                clickButton.Text = clickButton.Tag.ToString() + openArrow;
                openMenuIndex = clickMenuIndex;
                this.treeViews[clickMenuIndex].Show();
            }
            //以下的操作也很重要，根据当前单击的模块按钮索引值，小于这个值的模块按钮移到上面，大于的移到下面
            int i = 0;
            foreach (var b in headerButtons)
            {
                if (i <= clickMenuIndex || openMenuIndex == null)
                {
                    b.Dock = DockStyle.Top;
                    b.BringToFront();

                }
                else
                {
                    b.Dock = DockStyle.Bottom;
                    b.SendToBack();
                }
                i++;
            }
            //最后对应的TreeView控件得置于顶层，这样布局就完美了
            this.treeViews[clickMenuIndex].BringToFront();
        }
        /// <summary>
        /// 递归根据节点的Id，构建出TreeNode数组，这个prefixPath是用来构建完美的Path路径的
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="prefixPath"></param>
        /// <returns></returns>
        private TreeNode[] buildTreeNode(int parentId, string prefixPath)
        {
            List<TreeNode> nodeList = new List<TreeNode>();
            Menus.ForEach(m =>
            {
                if (m.ParentId == parentId)
                {
                    //拼接当前节点完整路径，然后再传给递归方法
                    string path = prefixPath + "." + m.Path.Substring(0, 1).ToUpper() + m.Path.Substring(1);
                    TreeNode node = new TreeNode();
                    node.Text = m.Name;
                    node.Tag = path;
                    node.Nodes.AddRange(buildTreeNode(m.Id, path));
                    nodeList.Add(node);

                }
            });
            return nodeList.ToArray();
        }
    }
}
