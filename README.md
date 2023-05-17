# Internet Relay Chat

[![CI](https://github.com/viral32111/InternetRelayChat.Message/actions/workflows/ci.yml/badge.svg)](https://github.com/viral32111/InternetRelayChat.Message/actions/workflows/ci.yml)
[![CodeQL](https://github.com/viral32111/InternetRelayChat.Message/actions/workflows/codeql.yml/badge.svg)](https://github.com/viral32111/InternetRelayChat.Message/actions/workflows/codeql.yml)

This is a barebones client-side implementation of the [Internet Relay Chat (IRC) messaging protocol](https://datatracker.ietf.org/doc/html/rfc1459.html) for my [Twitch Bot](https://github.com/viral32111/TwitchBot).

**NOTE: This is currently under development, so functionality is not guaranteed!**

## Background

I made this for use in my [Twitch Bot](https://github.com/viral32111/TwitchBot) as the [Twitch Chat API](https://dev.twitch.tv/docs/irc/) operates over IRC.

This [was originally hardcoded into the bot](https://github.com/viral32111/TwitchBot/tree/961fc729a8fc151686eb3e7c2c371768c9a81f7f/Source/InternetRelayChat). However, I separated it into its own project in January of 2023 to ease development.

I also wanted a project to try out automated unit testing and test-driven development in .NET for the first time.

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
