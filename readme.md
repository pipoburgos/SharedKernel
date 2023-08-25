# SHARED KERNEL

This is a shared kernel implementation [based on hgraca post](https://herbertograca.com/2017/11/16/explicit-architecture-01-ddd-hexagonal-onion-clean-cqrs-how-i-put-it-all-together/) with Domain Driven Design, Hexagonal, Onion and Clean Architectures.

[Domain Driven Design basics in spanish.](https://github.com/jatubio/5minutos_laravel/wiki/Resumen-sobre-DDD.-Domain-Driven-Design)

## 𝗦𝘁𝗮𝘁𝘀

![github stats](https://github-readme-stats.vercel.app/api?username=pipoburgos&show_icons=true&theme=dracula)

### Project content:
  - CQRS with Command and Query bus. (InMemory).
  - Event sourcing with Events and Event Bus interface. (InMemoryEventBus, RedisEventBus and RabbitMqEventBus implementations)
  - Aggregates, Entities and Value Objects.
  - Specification pattern.
  - Unit of Work pattern.
  - Auditable Service.
  - Repository pattern with EFCore, Mongo, Elasticsearch, Redis and FileSystem implementations.
  - HealthChecks (Sql Server, Mongo, RabbitMq, Redis, Cpu, Ram, etc).
  - IEntityValidator with FluentValidation library.
  - Report render for SQL Server Reporting Services SSRS.
  - Active Directory Service integration.
  - Open Api 3 specification.
  - Identity Server 4 integration.
  - Use Prometheus metrics middleware.
    
    

### Target Frameworks compatibility:
- Application Core Layer: net40, net45, net451, net452, net46, net461, net462, net47, net471, net472, net48, netstandard2.0, netstandard2.1, net6.0 and net7.0

- Infrastructure Layer: net462, net47, net471, net472, net48, netstandard2.0, netstandard2.1, net6.0 and net7.0

- User Interface Layer: net6.0 and net7.0


### References

[See how to configure user interface layer (Startup.cs)](src/Api/readme.md)

[See how to register a module](src/Infrastructure/readme.md)

[See ShopDDD project sample usage](https://github.com/alvarosinmarca/ShopDDD)
