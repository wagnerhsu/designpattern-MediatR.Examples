using MediatR;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace Domain.Order
{
    public class OrderNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x => x.FromThisAssembly()
                 .SelectAllClasses()
                 .InheritedFrom(typeof(IRequestHandler<,>))
                 .BindAllInterfaces());
        }
    }
}