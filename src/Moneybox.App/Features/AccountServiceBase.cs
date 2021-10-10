using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public abstract class AccountSercviceBase
    {
        protected IAccountRepository AccountRepository { get; }
        protected INotificationService NotificationService { get; }
        protected abstract string InsufficientFundsMessage { get; }

        protected AccountSercviceBase(IAccountRepository accountRepository, INotificationService notificationService)
        {
            AccountRepository = accountRepository;
            NotificationService = notificationService;
        }

        protected void Withdraw(Account account, decimal ammount)
        {
            var fromBalance = account.Balance - ammount;

            if (fromBalance < 0m)
            {
                throw new InvalidOperationException(InsufficientFundsMessage);
            }

            if (fromBalance < 500m)
            {
                NotificationService.NotifyFundsLow(account.User.Email);
            }

            account.Balance = account.Balance - ammount;
            account.Withdrawn = account.Withdrawn - ammount;
        }

        protected void Add(Account account, decimal ammount)
        {
            var paidIn = account.PaidIn + ammount;

            if (paidIn > Account.PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }

            if (Account.PayInLimit - paidIn < 500m)
            {
                NotificationService.NotifyApproachingPayInLimit(account.User.Email);
            }

            account.Balance = account.Balance + ammount;
            account.PaidIn = account.PaidIn + ammount;
        }
    }
}
