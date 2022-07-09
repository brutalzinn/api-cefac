#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV TZ=America/Sao_Paulo
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CefacAPI/CefacAPI.csproj", "CefacAPI/"]
RUN dotnet restore "CefacAPI/CefacAPI.csproj"
COPY . .
WORKDIR "/src/CefacAPI"
RUN dotnet build "CefacAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CefacAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CefacAPI.dll"]