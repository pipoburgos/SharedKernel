global using SharedKernel.Domain.Aggregates;
global using SharedKernel.Domain.Entities;
global using SharedKernel.Domain.Events;
global using SharedKernel.Domain.Repositories;
global using SharedKernel.Domain.Specifications.Common;
global using SharedKernel.Domain.ValueObjects;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Linq.Expressions;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2"),
           InternalsVisibleTo("BankAccounts.Application"),
           InternalsVisibleTo("BankAccounts.Infrastructure"),
           InternalsVisibleTo("BankAccounts.Domain.Tests"),
           InternalsVisibleTo("BankAccounts.UseCases.Tests"),
           InternalsVisibleTo("BankAccounts.Integration.Tests"),
           InternalsVisibleTo("BankAccounts.Acceptance.Tests")]
