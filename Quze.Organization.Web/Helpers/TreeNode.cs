using Newtonsoft.Json;
using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.Organization.Web.Helpers
{
    [JsonConverter(typeof(TreeNodeConvertor<ServiceType>))]
    public class TreeNode<T>:IComparable<TreeNode<T>>
    {
        public TreeNode<T> ParentTreeNode { get; set; }
        public List<TreeNode<T>> ChildrenTreeNode { get; set; }
        public T Content { get; set; }
        public int Id { get; set; }

        public TreeNode(T content, int id)
        {
            this.Content = content;
            this.Id = id;
            ChildrenTreeNode = new List<TreeNode<T>>();
        }

        public TreeNode(int id)
        {
            this.Id = id;
        }


        public void AddChild(TreeNode<T> child)
        {
            child.ParentTreeNode = this;
            ChildrenTreeNode.Add(child);
        }

       
           public int CompareTo(TreeNode<T> other)
        {
            return Id - other.Id;
        }
    }
}
