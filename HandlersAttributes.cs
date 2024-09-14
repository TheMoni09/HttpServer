using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class GetHandlerAttribute : Attribute
    {
        public string Path { get; }
        public GetHandlerAttribute(string path)
        {
            Path = path;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PostHandlerAttribute : Attribute
    {
        public string Path { get; }
        public PostHandlerAttribute(string path)
        {
            Path = path;
        }
    }

}
