# Temel imaj olarak .NET SDK kullan
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# �al��ma dizinini ayarla
WORKDIR /app

# Proje dosyas�n� kopyala ve ba��ml�l�klar� y�kle
COPY *.csproj ./
RUN dotnet restore

# Uygulamay� derle
COPY . ./
RUN dotnet publish -c Release -o /out

# �al��t�rma i�in temel imaj olarak .NET Runtime kullans
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# �al��ma dizinini ayarlan
WORKDIR /app

# Yay�nlanan dosyalar� kopyalad
COPY --from=build /out .

# Uygulamay� �al��t�rs
ENTRYPOINT ["dotnet", "RabbitMqSmsConsumer.dll"]
