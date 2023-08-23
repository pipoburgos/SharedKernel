The following code demonstrates basic usage of Shared Kernel Infastructure Fluent Validation.

```cs

services.AddFluentValidationValidators(typeof(InfrastructureAssembly).Assembly,
    ServiceLifetime.Transient, "en);

```