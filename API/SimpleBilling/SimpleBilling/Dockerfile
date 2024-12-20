


#####################################################################Local Docker###############################
# Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Switch to root user for build steps
USER root

# Copy and restore the .csproj file
COPY ["SimpleBilling/SimpleBilling.csproj", "./"]
RUN dotnet restore "SimpleBilling.csproj"

# Copy the rest of the source code
COPY SimpleBilling ./SimpleBilling

# Build the application
WORKDIR /src/SimpleBilling
RUN dotnet build "SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/SimpleBilling
RUN dotnet publish "SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Switch back to app user for runtime
USER app
ENTRYPOINT ["dotnet", "SimpleBilling.dll"]

############################################AWS############################################################
## Base image for running the app
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
## Build image
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#
## Switch to root user for build steps
#USER root
#
## Copy and restore the .csproj file
#COPY ["SimpleBilling.csproj", "./"]
#RUN dotnet restore "./SimpleBilling.csproj"
#
## Copy the rest of the source code
#RUN ls -la
#COPY . ./SimpleBilling
#
#
## Build the application
#WORKDIR /src/SimpleBilling
#RUN dotnet build "SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## Publish the application
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src/SimpleBilling
#RUN dotnet publish "SimpleBilling.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## Final runtime image
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#
## Switch back to app user for runtime
#USER app
#ENTRYPOINT ["dotnet", "SimpleBilling.dll"]