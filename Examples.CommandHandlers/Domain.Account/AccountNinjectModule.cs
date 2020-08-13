using MediatR;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace Domain.Account
{
    public class AccountNinjectModule : NinjectModule
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