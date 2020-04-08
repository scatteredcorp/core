FROM mcr.microsoft.com/dotnet/core/sdk:2.2

COPY . /BGC

WORKDIR /BGC/core/BGC

RUN dotnet build --configuration Release -o /app

ENTRYPOINT ["/usr/bin/dotnet", "/app/BGC.dll"]
CMD ["help"]
