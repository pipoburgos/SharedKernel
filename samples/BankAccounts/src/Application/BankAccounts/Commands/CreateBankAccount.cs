using SharedKernel.Application.RailwayOrientedProgramming;

namespace BankAccounts.Application.BankAccounts.Commands
{
    /// <summary> Create a bank account. </summary>
    public class CreateBankAccount : ICommandRequest<ApplicationResult<ApplicationUnit>>
    {
        /// <summary> Constructor. </summary>
        public CreateBankAccount(Guid ownerId, string name, DateTime birthdate, string? surname,
            Guid movementId, decimal amount)
        {
            OwnerId = ownerId;
            Name = name;
            Birthdate = birthdate;
            Surname = surname;
            MovementId = movementId;
            Amount = amount;
        }

        /// <summary> Bank account identifier. </summary>
        public Guid Id { get; private set; }

        /// <summary> Owner identifier. </summary>
        public Guid OwnerId { get; }

        /// <summary> Owner name. </summary>
        public string Name { get; }

        /// <summary> Owner surname. </summary>
        public string? Surname { get; }

        /// <summary> Owner birthdate. </summary>
        public DateTime Birthdate { get; }

        /// <summary> Movement identifier. </summary>
        public Guid MovementId { get; }

        /// <summary> Initial amount. </summary>
        public decimal Amount { get; }

        /// <summary> Adds bank account identifier </summary>
        /// <param name="id"></param>
        public void AddId(Guid id) => Id = id;

        ///// <summary>  </summary>
        //public override string GetUniqueName()
        //{
        //    return "bankAccounts.create";
        //}

        ///// <summary>  </summary>
        //public override Request FromPrimitives(Dictionary<string, string> body, string id, string occurredOn)
        //{
        //    var command = new CreateBankAccount(Guid.Parse(body[nameof(OwnerId)]), body[nameof(Name)],
        //        ConvertToDateTime(body, nameof(Birthdate)), body[nameof(Surname)], Guid.Parse(body[nameof(MovementId)]),
        //        decimal.Parse(body[nameof(Amount)]));
        //    command.AddId(Guid.Parse(body[nameof(Id)]));
        //    return command;
        //}
    }
}
