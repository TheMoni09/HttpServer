using System.Reflection;

namespace HttpServer
{
    public class Router
    {
        private Dictionary<string, MethodInfo> getRoutes = new Dictionary<string, MethodInfo>();
        private Dictionary<string, MethodInfo> postRoutes = new Dictionary<string, MethodInfo>();

        public Router(Type controllerType)
        {
            var methods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (var method in methods)
            {
                var getAttr = method.GetCustomAttribute<GetHandlerAttribute>();
                if (getAttr != null)
                {
                    getRoutes[getAttr.Path] = method;
                }

                var postAttr = method.GetCustomAttribute<PostHandlerAttribute>();
                if (postAttr != null)
                {
                    postRoutes[postAttr.Path] = method;
                }
            }
        }

        public HTTPResponse RouteRequest(string httpMethod, string path, HTTPRequest request, object controllerInstance)
        {
            MethodInfo? methodToInvoke = null;

            if (httpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                getRoutes.TryGetValue(path, out methodToInvoke);
            }
            else if (httpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                postRoutes.TryGetValue(path, out methodToInvoke);
            }

            if (methodToInvoke != null)
            {
                return (HTTPResponse)methodToInvoke.Invoke(controllerInstance, new object[] { request });
            }
            else
            {
                return new HTTPResponse(404, "Not Found", "<html><body><h1>404 Not Found</h1></body></html>");
            }
        }
    }
}
