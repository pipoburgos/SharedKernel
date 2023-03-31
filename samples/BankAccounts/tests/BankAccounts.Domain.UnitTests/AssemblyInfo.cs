global using FluentAssertions;
global using NSubstitute;
global using System;
global using Xunit;

using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2"),
           InternalsVisibleTo("BankAccounts.UseCases.Tests"),
           InternalsVisibleTo("BankAccounts.Integration.Tests"),
           InternalsVisibleTo("BankAccounts.Acceptance.Tests")]