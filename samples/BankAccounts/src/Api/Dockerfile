#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["samples/BankAccounts/src/Api/BankAccounts.Api.csproj", "samples/BankAccounts/src/Api/"]
COPY ["src/Api/SharedKernel.Api.csproj", "src/Api/"]
COPY ["src/Infrastructure/SharedKernel.Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Application/SharedKernel.Application.csproj", "src/Application/"]
COPY ["src/Domain/SharedKernel.Domain.csproj", "src/Domain/"]
COPY ["samples/BankAccounts/src/Infrastructure/BankAccounts.Infrastructure.csproj", "samples/BankAccounts/src/Infrastructure/"]
COPY ["src/Infrastructure.Dapper/SharedKernel.Infrastructure.Dapper.csproj", "src/Infrastructure.Dapper/"]
COPY ["src/Infrastructure.EntityFrameworkCore.SqlServer/SharedKernel.Infrastructure.EntityFrameworkCore.SqlServer.csproj", "src/Infrastructure.EntityFrameworkCore.SqlServer/"]
COPY ["src/Infrastructure.EntityFrameworkCore/SharedKernel.Infrastructure.EntityFrameworkCore.csproj", "src/Infrastructure.EntityFrameworkCore/"]
COPY ["src/Infrastructure.FluentValidation/SharedKernel.Infrastructure.FluentValidation.csproj", "src/Infrastructure.FluentValidation/"]
COPY ["src/Infrastructure.MailKit/SharedKernel.Infrastructure.MailKit.csproj", "src/Infrastructure.MailKit/"]
COPY ["src/Infrastructure.NetJson/SharedKernel.Infrastructure.NetJson.csproj", "src/Infrastructure.NetJson/"]
COPY ["src/Infrastructure.Polly/SharedKernel.Infrastructure.Polly.csproj", "src/Infrastructure.Polly/"]
COPY ["src/Infrastructure.Redis/SharedKernel.Infrastructure.Redis.csproj", "src/Infrastructure.Redis/"]
COPY ["samples/BankAccounts/src/Application/BankAccounts.Application.csproj", "samples/BankAccounts/src/Application/"]
COPY ["samples/BankAccounts/src/Domain/BankAccounts.Domain.csproj", "samples/BankAccounts/src/Domain/"]
RUN dotnet restore "samples/BankAccounts/src/Api/BankAccounts.Api.csproj"
COPY . .
WORKDIR "/src/samples/BankAccounts/src/Api"
RUN dotnet build "BankAccounts.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "BankAccounts.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankAccounts.Api.dll"]