using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Infrastructure.UnitOfWork
{
    public interface IUnitOfWorkProvider
    {
        IUnitOfWork CreateUnitOfWork(string name);
    }
}
