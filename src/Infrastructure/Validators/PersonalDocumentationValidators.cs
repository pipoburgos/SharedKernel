using FluentValidation;

namespace SharedKernel.Infrastructure.Validators
{
    /// <summary> CIF, NIF, NIE and DNI validators </summary>
    public static class PersonalDocumentationValidators
    {
        /// <summary>
        /// CIF validator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> Cif<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value => Domain.ValueObjects.PersonalDocumentation.Cif.Create(value).IsValid());
        }

        /// <summary>
        /// DNI validator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> Dni<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value => Domain.ValueObjects.PersonalDocumentation.Dni.Create(value).IsValid());
        }

        /// <summary>
        /// NIE validator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> Nie<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value => Domain.ValueObjects.PersonalDocumentation.Nie.Create(value).IsValid());
        }

        /// <summary>
        /// NIF or CIF validator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> NifOrCif<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value => Domain.ValueObjects.PersonalDocumentation.NifOrCif.Create(value).IsValid());
        }

        /// <summary>
        /// NIF validator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> Nif<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(value => Domain.ValueObjects.PersonalDocumentation.Nif.Create(value).IsValid());
        }
    }
}
