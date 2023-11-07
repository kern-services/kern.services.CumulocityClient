#!/bin/bash

dotnet build -c Release
dotnet test -c Release
dotnet pack ./src/kern.services.CumulocityClient/kern.services.CumulocityClient.csproj -o . -c Release --no-build

