using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollapsePanelForm
{
    public class MenuData
    {
        public int Id { get; set; }
        //可为空，当为空时，说明当前节点是根节点
        public int? ParentId { get; set; }
        //模块或菜单的名称
        public string Name { get; set; }
        //这个是用于构建菜单对应Form控件的路径的，可以利用反射实现打开匹配路径的Form控件
        public string Path { get; set; }
    }
}
