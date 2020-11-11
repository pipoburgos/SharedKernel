using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Entities.Globalization
{
    public class Language : AggregateRootAuditableLogicalRemove<string>
    {
        #region Constructors

        private Language() { }

        public static Language Create(string id, string name)
        {
            return new Language
            {
                Id = id,
                Name = name
            };
        }

        #endregion

        #region Properties

        public string Name { get; private set; }

        #endregion
    }
}
