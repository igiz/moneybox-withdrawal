using FakeItEasy;
using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.UnitTests
{
    public class BaseTest
    {
        protected IAccountRepository AccountRepository { get; }

        protected INotificationService NotificationService { get; }

        public BaseTest()
        {
            AccountRepository = A.Fake<IAccountRepository>();
            NotificationService = A.Fake<INotificationService>();
        }

        protected User MockUser(string name, string email)
        {
            return new User()
            {
                Id = Guid.NewGuid(),
                Email = email,
                Name = name
            };
        }

        protected Account MockAccount(User user, decimal balance = 0, decimal withdrawn = 0)
        {
            return new Account()
            {
                Id = Guid.NewGuid(),
                User = user,
                Balance = balance,
                PaidIn = 0,
                Withdrawn = withdrawn
            };
        }
    }
}
