# Notes

## Create empty Visual Studio solution
```sh
dotnet new sln --name InternetRelayChat.Message
```

## Create C# console projects (.NET 7.0)
```sh
dotnet new console --language C# --framework net7.0 --name InternetRelayChat.Message
dotnet new console --language C# --framework net7.0 --name InternetRelayChat.Message.Examples
```

## Create C# testing project (.NET 7.0)
```sh
dotnet new xunit --language C# --framework net7.0 --name InternetRelayChat.Message.Tests
```

### Add projects to solution
```sh
dotnet sln add ./InternetRelayChat.Message/InternetRelayChat.Message.csproj
dotnet sln add ./InternetRelayChat.Message.Examples/InternetRelayChat.Message.Examples.csproj
dotnet sln add ./InternetRelayChat.Message.Tests/InternetRelayChat.Message.Tests.csproj
```

# Add reference to main project in example & testing projects
```sh
dotnet add ./InternetRelayChat.Message.Examples/InternetRelayChat.Message.Examples.csproj reference ./InternetRelayChat.Message/InternetRelayChat.Message.csproj
dotnet add ./InternetRelayChat.Message.Tests/InternetRelayChat.Message.Tests.csproj reference ./InternetRelayChat.Message/InternetRelayChat.Message.csproj
```
