# CSharp AutoChat V2

A Discord bot built with Discord.Net for automated chat management.

## Prerequisites

- .NET 8.0 SDK (for local development)
- Docker (for containerized deployment)
- A Discord bot token

## Getting Started

### 1. Configure Your Bot Token

Copy the example token file and add your Discord bot token:

```bash
cp Configs/Token.json.example Configs/Token.json
```

Edit `Configs/Token.json` and replace `YOUR_DISCORD_BOT_TOKEN_HERE` with your actual Discord bot token.

### 2. Running with Docker (Recommended)

Build the Docker image:

```bash
docker build -t csharp-autochat .
```

Run the container:

```bash
docker run -d \
  --name csharp-autochat-bot \
  --restart unless-stopped \
  -v $(pwd)/Configs:/app/Configs \
  csharp-autochat
```

To view logs:

```bash
docker logs -f csharp-autochat-bot
```

To stop the bot:

```bash
docker stop csharp-autochat-bot
```

To remove the container:

```bash
docker rm csharp-autochat-bot
```

### 3. Running Locally

Restore dependencies:

```bash
dotnet restore
```

Build the project:

```bash
dotnet build
```

Run the bot:

```bash
dotnet run
```

## Additional Docker Commands

### Rebuild and restart the container

```bash
docker stop csharp-autochat-bot
docker rm csharp-autochat-bot
docker build -t csharp-autochat .
docker run -d \
  --name csharp-autochat-bot \
  --restart unless-stopped \
  -v $(pwd)/Configs:/app/Configs \
  csharp-autochat
```

### View real-time logs

```bash
docker logs -f csharp-autochat-bot
```

## Configuration

All configuration files are stored in the `Configs/` directory:

- `Configs/Token.json` - Your Discord bot token (required)
- `Configs/AutoClarGuilds.json` - Auto-clear guild settings
- `Configs/AutoCreator.json` - Auto-creator settings
- `Configs/CatagoryGuilds.json` - Category guild configurations
- `Configs/ignoreafk.json` - AFK ignore list
- `Configs/SavedPermission.json` - Saved permission configurations
- `Configs/UsingTopic.json` - Topic usage settings
- `Configs/VoiceMsgState.json` - Voice message state
- `Configs/Permissions.json` - Permission settings (optional)

## Project Structure

- `Program.cs` - Main entry point
- `CommandHandler.cs` - Command handling logic
- `Modules/` - Bot command modules
- `Dto/` - Data transfer objects
- `Configs/` - Bot configuration files

## Technologies

- .NET 8.0
- Discord.Net 3.16.0
- Docker

## Notes

- The bot uses `.` as the command prefix
- Token.json is excluded from git for security
- The Docker container will restart automatically unless stopped manually