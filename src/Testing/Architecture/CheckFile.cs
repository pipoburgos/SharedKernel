namespace SharedKernel.Testing.Architecture;

public enum CheckFile
{
    ValidatorTests = 1,
    EndpointTests = 2,
    HandlerTests = 4,
    Validator = 8,
    Endpoint = 16,
    Handler = 32,
}