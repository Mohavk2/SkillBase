using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.Common
{
    internal class TreeNode<T>
    {
        public TreeNode(){}
        public TreeNode(T item)
        {
            Item = item;
        }

        public T? Item { get; set; }
        public List<TreeNode<T>> Children { get; set; } = new();
        public bool IsRoot { get; set; }
    }
}