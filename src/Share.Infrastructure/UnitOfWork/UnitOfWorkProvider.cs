using Autofac;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Share.Infrastructure.UnitOfWork
{
    public class UnitOfWorkProvider : IUnitOfWorkProvider
    {
        private UnitOfWorkBuilder UnitOfWorkOptions { get; set; }

        private IComponentContext ComponentContext { get; set; }

        public UnitOfWorkProvider(IComponentContext componentContext, UnitOfWorkBuilder unitOfWorkOptions)
        {
            if (componentContext == null)
            {
                throw new ArgumentNullException(nameof(componentContext));
            }
            if (unitOfWorkOptions == null)
            {
                throw new ArgumentNullException(nameof(unitOfWorkOptions));
            }

            this.UnitOfWorkOptions = unitOfWorkOptions;
            this.ComponentContext = componentContext;
        }

        public IUnitOfWork CreateUnitOfWork(string name)
        {
            string creatorName = Consts.UNIT_OF_WORK_CREATOR_PREFIX + name;
            if (this.ComponentContext.IsRegisteredWithName<IUnitOfWorkCreator>(creatorName))
            {
                return this.ComponentContext.ResolveNamed<IUnitOfWorkCreator>(creatorName).CreateUnitOfWork();
            }
            else
            {
                throw new Exception("No UnitOfWork creator found");
            }
        }
    }
}
