﻿namespace KnightFrank.Antares.Domain
{
    using System;
    using System.Linq;
    using System.Reflection;

    using FluentValidation;

    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;

    using MediatR;

    using Ninject.Modules;
    using Ninject.Extensions.Conventions;

    public class DomainModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(
                x =>
                x.FromThisAssembly()
                 .SelectAllClasses()
                 .InheritedFrom(typeof(IRequestHandler<,>))
                 .BindAllInterfaces()
                 .Configure(y => y.WhenInjectedInto(typeof(ValidatorCommandHandler<,>))));

            this.Bind(typeof(IRequestHandler<,>)).To(typeof(ValidatorCommandHandler<,>));

            this.Bind(typeof(IGenericRepository<>)).To(typeof(GenericRepository<>));

            this.Bind(typeof(IReadGenericRepository<>)).To(typeof(ReadGenericRepository<>));

            AssemblyScanner.FindValidatorsInAssembly(Assembly.GetExecutingAssembly())
                           .ForEach(assemblyScanResult =>
                           {
                               Type domainValidatorType = GetDomainValidatorType(assemblyScanResult);

                               this.Kernel.Bind(domainValidatorType ?? assemblyScanResult.InterfaceType).To(assemblyScanResult.ValidatorType);
                           });
        }

        private static Type GetDomainValidatorType(AssemblyScanner.AssemblyScanResult assemblyScanResult)
        {
            return assemblyScanResult.ValidatorType.GetInterfaces()
                    .Where(y => y.IsGenericType)
                    .FirstOrDefault(y => y.GetGenericTypeDefinition() == typeof(IDomainValidator<>)
                                        && ((TypeInfo)assemblyScanResult.ValidatorType).ImplementedInterfaces.Contains(assemblyScanResult.InterfaceType)
                );
        }
    }
}
