using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using System;

namespace Moneybox.App.Features
{
    public class TransferMoney : AccountSercviceBase
    {
        protected override string InsufficientFundsMessage => "Insufficient funds to make a transfer";

        public TransferMoney(IAccountRepository accountRepository, INotificationService notificationService) : base(accountRepository, notificationService)
        {
        }

        public void Execute(Guid fromAccountId, Guid toAccountId, decimal ammount)
        {
            var from = AccountRepository.GetAccountById(fromAccountId);
            var to = AccountRepository.GetAccountById(toAccountId);

            //Todo: This ofcourse needs to be wrapped in a proper transaction.
            Withdraw(from, ammount);
            Add(to, ammount);

            AccountRepository.Update(from);
            AccountRepository.Update(to);
        }
    }
}
