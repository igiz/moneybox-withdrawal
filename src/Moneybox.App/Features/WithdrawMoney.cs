using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class WithdrawMoney : AccountSercviceBase
    {
        protected override string InsufficientFundsMessage => "Insufficient funds to make a withdrawal";

        public WithdrawMoney(IAccountRepository accountRepository, INotificationService notificationService) : base(accountRepository, notificationService)
        {
        }

        public void Execute(Guid fromAccountId, decimal ammount)
        {
            var from = AccountRepository.GetAccountById(fromAccountId);
            Withdraw(from, ammount);
            AccountRepository.Update(from);
        }
    }
}
