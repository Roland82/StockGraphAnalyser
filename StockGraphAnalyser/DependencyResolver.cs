
namespace StockGraphAnalyser
{
    using System.Linq;
    using Ninject;
    using System.Reflection;

    public class DependencyResolver
    {
        public static void Register(IKernel kernel) {     
            var assembly = Assembly.GetAssembly(typeof (DependencyResolver));
            var interfaces = assembly.GetTypes().Where(x => x.IsInterface);
            var classes = assembly.GetTypes().Where(x => x.IsClass);
            foreach (var classType in classes)
            {
                var assignableInterface = interfaces.FirstOrDefault(x => x.IsAssignableFrom(classType));
                if (assignableInterface != null)
                {              
                    kernel.Bind(new[] { assignableInterface }).To(classType);
                }
            }
        }
    }
}
