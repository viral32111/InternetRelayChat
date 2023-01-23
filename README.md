# InternetRelayChat.Message

This is a NuGet package for .NET Core 7.0 for constructing and parsing [Internet Relay Chat messages](https://datatracker.ietf.org/doc/html/rfc1459.html#section-2.3.1).

**NOTE: Currently under development, functionality is not guaranteed!**

## Background

This is made for use in my [Twitch Bot](https://github.com/viral32111/TwitchBot) as the [Twitch Chat API](https://dev.twitch.tv/docs/irc/) uses IRC. It was originally hardcoded straight into it, however to ease development I separated it in January of 2023.

I also wanted a decent project to try out automated unit testing & test-driven development in .NET for the first time.

## Usage

1. Add the latest version of the [`viral32111.InternetRelayChat.Message`](https://github.com/users/viral32111/packages/nuget/package/viral32111.InternetRelayChat.Message) NuGet package to your .NET Core project:

```sh
dotnet add package viral32111.InternetRelayChat.Message
```

2. Include the namespace at the beginning of your C# source file(s):

```csharp
using viral32111.InternetRelayChat.Message;
```

3. Use the package! See [DOCS.md](/DOCS.md) for documentation.

## License

Copyright (C) 2023 [viral32111](https://viral32111.com).

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as
published by the Free Software Foundation, either version 3 of the
License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program. If not, see https://www.gnu.org/licenses.
