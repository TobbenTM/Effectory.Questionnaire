FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Effectory.Questionnaire.API/Effectory.Questionnaire.API.csproj", "Effectory.Questionnaire.API/"]
RUN dotnet restore "src/Effectory.Questionnaire.API/Effectory.Questionnaire.API.csproj"
COPY . .
WORKDIR "/src/Effectory.Questionnaire.API"
RUN dotnet build "Effectory.Questionnaire.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Effectory.Questionnaire.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Effectory.Questionnaire.API.dll"]
