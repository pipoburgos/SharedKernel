using BankAccounts.Application.BankAccounts.Commands;
using BankAccounts.Application.Shared.UnitOfWork;
using BankAccounts.Domain.BankAccounts;
using BankAccounts.Domain.BankAccounts.Repository;
using FluentAssertions;
using NSubstitute;
using SharedKernel.Application.Events;
using SharedKernel.Application.System;
using SharedKernel.Domain.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BankAccounts.UseCases.Tests.BankAccounts
{
    public class CreateBankAccountHandlerTests
    {
        private readonly CreateBankAccountHandler _sut;
        private readonly IDateTime _dateTime = Substitute.For<IDateTime>();
        private readonly IBankAccountRepository _bankAccountRepository = Substitute.For<IBankAccountRepository>();
        private readonly IBankAccountUnitOfWork _bankAccountUnitOfWork = Substitute.For<IBankAccountUnitOfWork>();
        private readonly IEventBus _eventBus = Substitute.For<IEventBus>();

        public CreateBankAccountHandlerTests()
        {
            _sut = new CreateBankAccountHandler(_dateTime, _bankAccountRepository, _bankAccountUnitOfWork, _eventBus);
        }

        [Fact]
        public async Task CreateBankAccountOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new CreateBankAccount(id, Guid.NewGuid(), "Roberto", new DateTime(1980, 2, 24),
                "Fernández", Guid.NewGuid(), 34);

            _dateTime.UtcNow.Returns(new DateTime(2022, 2, 14));


            // Act
            var exec = async () => await _sut.Handle(command, CancellationToken.None);


            // Assert
            await exec.Should().NotThrowAsync();
            await _bankAccountRepository.Received(1).Add(Arg.Is<BankAccount>(ba => ba.Id == id && ba.Balance == 34), CancellationToken.None);
            await _bankAccountUnitOfWork.Received(1).SaveChangesAsync(CancellationToken.None);
            await _eventBus.Received(1).Publish(Arg.Any<IEnumerable<DomainEvent>>(), CancellationToken.None);
        }
    }
}
