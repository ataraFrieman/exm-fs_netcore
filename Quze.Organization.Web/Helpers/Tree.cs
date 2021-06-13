using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Quze.Organization.Web.Helpers
{
    public class Tree<T>
    {
   
        public TreeNode<T> Root { get; set; }

        public Tree(TreeNode<T> root)
        {
            this.Root = root;
        }

        public void AddTreeNode(TreeNode<T> newNode, TreeNode<T> parentNode)
        {
            parentNode.AddChild(newNode);
        }


    }
}
