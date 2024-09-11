using SharedKernel.Domain.Validators;

namespace SharedKernel.Application.Validator;

/// <summary> . </summary>
public interface IClassValidatorService
{
    /// <summary> . </summary>
    void ValidateDataAnnotations<T>(IEnumerable<T> classes) where T : class;

    /// <summary> . </summary>
    Result<Unit> ValidateDataAnnotationsResult<T>(IEnumerable<T> classes) where T : class;

    /// <summary> . </summary>
    void ValidateValidatableObjects<T>(IEnumerable<T> validatableObjects) where T : class, IValidatableObject;

    /// <summary> . </summary>
    Result<Unit> ValidateValidatableObjectsResult<T>(IEnumerable<T> validatableObjects)
        where T : class, IValidatableObject;
}
