# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and restore first
COPY PlaidApi.sln ./
COPY PlaidApi/PlaidApi.csproj PlaidApi/
RUN dotnet restore PlaidApi/PlaidApi.csproj

# Copy the rest and publish
COPY PlaidApi/ PlaidApi/
RUN dotnet publish PlaidApi/PlaidApi.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
# The API will listen on 8080; Render will map it publicly
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PlaidApi.dll"]
