#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["samples/BankAccounts/src/Api/BankAccounts.Api.csproj", "samples/BankAccounts/src/Api/"]
COPY ["src/Api/SharedKernel.Api.csproj", "src/Api/"]
COPY ["src/Infrastructure/SharedKernel.Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Application/SharedKernel.Application.csproj", "src/Application/"]
COPY ["src/Domain/SharedKernel.Domain.csproj", "src/Domain/"]
COPY ["samples/BankAccounts/src/Infrastructure/BankAccounts.Infrastructure.csproj", "samples/BankAccounts/src/Infrastructure/"]
COPY ["samples/BankAccounts/src/Application/BankAccounts.Application.csproj", "samples/BankAccounts/src/Application/"]
COPY ["samples/BankAccounts/src/Domain/BankAccounts.Domain.csproj", "samples/BankAccounts/src/Domain/"]
RUN dotnet restore "samples/BankAccounts/src/Api/BankAccounts.Api.csproj"
COPY . .
WORKDIR "/src/samples/BankAccounts/src/Api"
RUN dotnet build "BankAccounts.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankAccounts.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankAccounts.Api.dll"]