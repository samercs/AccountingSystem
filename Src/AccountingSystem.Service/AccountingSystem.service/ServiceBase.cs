using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountingSystem.Data;

namespace AccountingSystem.Service
{
    public abstract class ServiceBase
    {
        private readonly IDataContextFactory _dataContextFactory;

        protected ServiceBase(IDataContextFactory dataContextFactory)
        {
            _dataContextFactory = dataContextFactory;
        }

        public IDataContext DataContext()
        {
            return _dataContextFactory.GetContext();
        }
    }
}
