using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Infrastructure.UnitOfWork
{
    internal interface IUnitOfWorkCreator
    {
        IUnitOfWork CreateUnitOfWork();
    }
}
