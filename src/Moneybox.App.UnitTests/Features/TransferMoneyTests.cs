using FakeItEasy;
using Moneybox.App.Features;
using Xunit;

namespace Moneybox.App.UnitTests.Features
{
    public class TransferMoneyTests : BaseTest
    {
        private readonly TransferMoney _testSubject;

        public TransferMoneyTests()
        {
            _testSubject = new TransferMoney(AccountRepository, NotificationService);
        }

        [Fact]
        public void Transfer_CanTransfer_Test()
        {
            var user = MockUser("User", "user@user.com");
            var accountOne = MockAccount(user, 100m);
            var accountTwo = MockAccount(user);

            A.CallTo(() => AccountRepository.GetAccountById(accountOne.Id))
                .Returns(accountOne);

            A.CallTo(() => AccountRepository.GetAccountById(accountTwo.Id))
                .Returns(accountTwo);

            _testSubject.Execute(accountOne.Id, accountTwo.Id, 50m);

            A.CallTo(() => AccountRepository.Update(accountOne))
                .MustHaveHappenedOnceExactly();

            A.CallTo(() => AccountRepository.Update(accountTwo))
                .MustHaveHappenedOnceExactly();

            Assert.Equal(50m, accountOne.Balance);
            Assert.Equal(50m, accountTwo.Balance);

            Assert.Equal(-50m, accountOne.Withdrawn);
            Assert.Equal(50m, accountTwo.PaidIn);
        }
    }
}
