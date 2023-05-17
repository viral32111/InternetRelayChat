# Notes

## Create empty Visual Studio solution
```sh
dotnet new sln --name InternetRelayChat
```

## Create C# console projects (.NET 7.0)
```sh
dotnet new console --language C# --framework net7.0 --name InternetRelayChat
dotnet new console --language C# --framework net7.0 --name InternetRelayChat.Examples
```

## Create C# testing project (.NET 7.0)
```sh
dotnet new xunit --language C# --framework net7.0 --name InternetRelayChat.Tests
```

## Add projects to solution
```sh
dotnet sln add ./InternetRelayChat/InternetRelayChat.csproj
dotnet sln add ./InternetRelayChat.Examples/InternetRelayChat.Examples.csproj
dotnet sln add ./InternetRelayChat.Tests/InternetRelayChat.Tests.csproj
```

## Add reference to main project in example & testing projects
```sh
dotnet add ./InternetRelayChat.Examples/InternetRelayChat.Examples.csproj reference ./InternetRelayChat/InternetRelayChat.csproj
dotnet add ./InternetRelayChat.Tests/InternetRelayChat.Tests.csproj reference ./InternetRelayChat/InternetRelayChat.csproj
```
