#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY NuGet.Config ./
COPY ["*.props", "../"]
COPY *.props ../
COPY ["host/Mre.Sb.PersonRegistration.HttpApi.Host/Mre.Sb.PersonRegistration.HttpApi.Host.csproj", "./Mre.Sb.PersonRegistration.HttpApi.Host/"]
COPY ["src/Mre.Sb.PersonRegistration.Application/Mre.Sb.PersonRegistration.Application.csproj", "./Mre.Sb.PersonRegistration.Application/"]
COPY ["src/Mre.Sb.PersonRegistration.Application.Contracts/Mre.Sb.PersonRegistration.Application.Contracts.csproj", "./Mre.Sb.PersonRegistration.Application.Contracts/"]
COPY ["src/Mre.Sb.PersonRegistration.Domain.Shared/Mre.Sb.PersonRegistration.Domain.Shared.csproj", "./Mre.Sb.PersonRegistration.Domain.Shared/"]
COPY ["src/Mre.Sb.PersonRegistration.Domain/Mre.Sb.PersonRegistration.Domain.csproj", "./Mre.Sb.PersonRegistration.Domain/"]
COPY ["src/Mre.Sb.Notification.HttpApi.Client/Mre.Sb.Notification.HttpApi.Client.csproj", "./Mre.Sb.Notification.HttpApi.Client/"]
COPY ["src/Mre.Sb.PersonRegistration.HttpApi/Mre.Sb.PersonRegistration.HttpApi.csproj", "./Mre.Sb.PersonRegistration.HttpApi/"]
COPY ["src/Mre.Sb.PersonRegistration.EntityFrameworkCore/Mre.Sb.PersonRegistration.EntityFrameworkCore.csproj", "./Mre.Sb.PersonRegistration.EntityFrameworkCore/"]
COPY ["host/Mre.Sb.PersonRegistration.Host.Shared/Mre.Sb.PersonRegistration.Host.Shared.csproj", "./Mre.Sb.PersonRegistration.Host.Shared/"]
RUN dotnet restore --configfile NuGet.Config "Mre.Sb.PersonRegistration.HttpApi.Host/Mre.Sb.PersonRegistration.HttpApi.Host.csproj"

COPY ["host/Mre.Sb.PersonRegistration.HttpApi.Host", "./Mre.Sb.PersonRegistration.HttpApi.Host/"]
COPY ["src/Mre.Sb.PersonRegistration.Application", "./Mre.Sb.PersonRegistration.Application/"]
COPY ["src/Mre.Sb.PersonRegistration.Application.Contracts", "./Mre.Sb.PersonRegistration.Application.Contracts/"]
COPY ["src/Mre.Sb.PersonRegistration.Domain.Shared", "./Mre.Sb.PersonRegistration.Domain.Shared/"]
COPY ["src/Mre.Sb.PersonRegistration.Domain", "./Mre.Sb.PersonRegistration.Domain/"]
COPY ["src/Mre.Sb.Notification.HttpApi.Client", "./Mre.Sb.Notification.HttpApi.Client/"]
COPY ["src/Mre.Sb.PersonRegistration.HttpApi", "./Mre.Sb.PersonRegistration.HttpApi/"]
COPY ["src/Mre.Sb.PersonRegistration.EntityFrameworkCore", "./Mre.Sb.PersonRegistration.EntityFrameworkCore/"]
COPY ["host/Mre.Sb.PersonRegistration.Host.Shared", "./Mre.Sb.PersonRegistration.Host.Shared/"]
RUN dotnet build "Mre.Sb.PersonRegistration.HttpApi.Host/Mre.Sb.PersonRegistration.HttpApi.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mre.Sb.PersonRegistration.HttpApi.Host/Mre.Sb.PersonRegistration.HttpApi.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mre.Sb.PersonRegistration.HttpApi.Host.dll"]