using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Transactions;

namespace RepositoryIntegrationTests
{
    public class RollbackAttribute : Attribute, ITestAction
    {
        private TransactionScope transaction;

      
        public void BeforeTest(ITest test)
        {
            transaction = new TransactionScope();
        }

        public void AfterTest(ITest test)
        {
            transaction.Dispose();
        }

        public ActionTargets Targets
        {
            get { return ActionTargets.Test; }
        }
    }
}
