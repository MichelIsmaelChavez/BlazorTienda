#!/bin/bash

# Instalar .NET SDK
wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
apt-get update
apt-get install -y apt-transport-https
apt-get update
apt-get install -y dotnet-sdk-9.0

# Restaurar dependencias y publicar
dotnet restore
dotnet publish -c Release -o wwwroot

# Mover archivos necesarios
cp -r wwwroot/_framework wwwroot/
cp -r wwwroot/_blazor wwwroot/
cp wwwroot/index.html wwwroot/ 