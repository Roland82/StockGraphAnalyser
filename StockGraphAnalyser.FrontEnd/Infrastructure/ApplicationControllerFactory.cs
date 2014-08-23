

namespace StockGraphAnalyser.FrontEnd.Infrastructure
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.SessionState;
    using Ninject;

    public class ApplicationControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel kernel;

        private ApplicationControllerFactory(IKernel kernel) {
            this.kernel = kernel;
        }

        public static IControllerFactory Create(IKernel kernel) {
            return new ApplicationControllerFactory(kernel);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, System.Type controllerType) {
            return controllerType == null ? null : (IController)this.kernel.Get(controllerType);
        }
    }
}