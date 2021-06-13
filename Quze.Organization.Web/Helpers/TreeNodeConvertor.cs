using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.Organization.Web.Helpers
{
    public class TreeNodeConvertor<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            // we can serialize everything that is a TreeNode
            return typeof(TreeNode<T>).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // we currently support only writing of JSON
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // we serialize a node by just serializing the _children dictionary
            var node = value as TreeNode<T>;

            serializer.Serialize(writer, new
            {
                Content = node.Content,
                ChildrenTreeNode = node.ChildrenTreeNode,
            });
        }

    }
}
