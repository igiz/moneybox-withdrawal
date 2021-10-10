using FakeItEasy;
using Moneybox.App.Features;
using System;
using Xunit;

namespace Moneybox.App.UnitTests.Features
{
    public class WithdrawMoneyTests : BaseTest
    {
        private readonly WithdrawMoney _testSubject;

        public WithdrawMoneyTests()
        {
            _testSubject = new WithdrawMoney(AccountRepository, NotificationService);
        }

        [Fact]
        public void Withdraw_CannotWithdrawIfAccountIsEmpty_Test()
        {
            var user = MockUser("User", "user@user.com");
            var account = MockAccount(user);

            A.CallTo(() => AccountRepository.GetAccountById(account.Id))
                .Returns(account);

            var exception = Assert.Throws<InvalidOperationException>(() => _testSubject
            .Execute(account.Id, 1));
            Assert.Equal("Insufficient funds to make a withdrawal", exception.Message);
        }

        [Fact]
        public void Withdraw_CannotWithdrawIfInsufficientFunds_Test()
        {
            var user = MockUser("User", "user@user.com");
            var account = MockAccount(user, 200);

            A.CallTo(() => AccountRepository.GetAccountById(account.Id))
                .Returns(account);

            var exception = Assert.Throws<InvalidOperationException>(() => _testSubject
            .Execute(account.Id, 201m));
            Assert.Equal("Insufficient funds to make a withdrawal", exception.Message);
        }

        [Fact]
        public void Withdraw_CanWithdrawNotifyFundsLow_Test()
        {
            var user = MockUser("User", "user@user.com");
            var account = MockAccount(user, 300);

            A.CallTo(() => AccountRepository.GetAccountById(account.Id))
                .Returns(account);

            _testSubject.Execute(account.Id, 200m);

            A.CallTo(() => NotificationService.NotifyFundsLow(user.Email))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => AccountRepository.Update(account))
                .MustHaveHappenedOnceExactly();

            Assert.Equal(-200m, account.Withdrawn);
            Assert.Equal(100m, account.Balance);
        }
    }
}
