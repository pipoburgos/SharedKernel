FROM mcr.microsoft.com/dotnet/sdk AS build-env
WORKDIR /app

# COPY ./SharedKernel.sln ./
# 
# # Copy csproj from distinct layers
# COPY ./src/Domain/SharedKernel.Domain.csproj  ./src/Domain/SharedKernel.Domain.csproj
# COPY ./src/Application/SharedKernel.Application.csproj  ./src/Application/SharedKernel.Application.csproj
# COPY ./src/Infrastructure/SharedKernel.Infrastructure.csproj  ./src/Infrastructure/SharedKernel.Infrastructure.csproj
# COPY ./src/Api.Gateway/SharedKernel.Api.Gateway.csproj  ./src/Api.Gateway/SharedKernel.Api.Gateway.csproj
# COPY ./src/Api/SharedKernel.Api.csproj  ./src/Api/SharedKernel.Api.csproj
# 
# COPY ./test/Domain/SharedKernel.Domain.Tests.csproj  ./test/Domain/SharedKernel.Domain.Tests.csproj
# COPY ./test/Application/SharedKernel.Application.Tests.csproj  ./test/Application/SharedKernel.Application.Tests.csproj
# COPY ./test/Infrastructure/SharedKernel.Integration.Tests.csproj ./test/Infrastructure/SharedKernel.Integration.Tests.csproj
# 
# # restore all packages of all projects
# RUN dotnet restore



# Copy everything else and build
COPY . ./

RUN dotnet test "./test/Domain/SharedKernel.Domain.Tests.csproj" -c Release --framework net5.0
RUN dotnet test "./test/Application/SharedKernel.Application.Tests.csproj" -c Release --framework net5.0
RUN dotnet test "./test/Infrastructure/SharedKernel.Integration.Tests.csproj" -c Release --framework net5.0