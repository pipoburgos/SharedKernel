global using SharedKernel.Application.Cqrs.Commands;
global using SharedKernel.Application.Cqrs.Commands.Handlers;
global using SharedKernel.Application.Cqrs.Queries;
global using SharedKernel.Application.Events;
global using SharedKernel.Application.System;
global using System;
global using System.Threading;
global using System.Threading.Tasks;

using System.Runtime.CompilerServices;
[assembly:
    InternalsVisibleTo("DynamicProxyGenAssembly2"),
    InternalsVisibleTo("BankAccounts.Infrastructure"),
    InternalsVisibleTo("BankAccounts.UseCases.Tests"),
    InternalsVisibleTo("BankAccounts.Acceptance.Tests")]