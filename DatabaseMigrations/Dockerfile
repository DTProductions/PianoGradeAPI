FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

COPY . /build
WORKDIR /build
RUN dotnet restore
RUN dotnet publish -o output


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

COPY --from=build /build/output /app
WORKDIR /app
ENTRYPOINT [ "dotnet", "DatabaseMigrations.dll" ]