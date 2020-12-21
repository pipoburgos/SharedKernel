using SharedKernel.Domain.Aggregates;

namespace SharedKernel.Domain.Entities.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    public class Language : AggregateRootAuditableLogicalRemove<string>
    {
        #region Constructors

        private Language() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; private set; }

        #endregion
    }
}
