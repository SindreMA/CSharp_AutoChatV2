using System;
using Discord;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using System.Threading.Tasks;
using UtilityBot.dto;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading;
using CSharp_AutoChatV2;
using CSharp_AutoChatV2.Modules;
using Discord.Rest;

namespace TemplateBot
{
    class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _service;
        public static List<string> ignoreafk = new List<string>();
        public static List<string> AutoclearList = new List<string>();
        public static List<string> AutoCreator = new List<string>();
        public static List<string> VoiceON = new List<string>();
        public static List<string> UseTopic = new List<string>();
        public static List<Catagory> CatagoryGuilds = new List<Catagory>();
        public static List<PermissionAndGuildDTO> SavedPermission = new List<PermissionAndGuildDTO>();

        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            _service.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            _client.MessageReceived += _client_MessageReceived;
            _client.UserVoiceStateUpdated += _client_UserVoiceStateUpdated;

            try
            {
                string json = File.ReadAllText("Configs/CatagoryGuilds.json");
                CatagoryGuilds = JsonConvert.DeserializeObject<List<Catagory>>(json);
            }
            catch (Exception)
            {
            }
            try
            {
                string json = File.ReadAllText("Configs/ignoreafk.json");
                ignoreafk = JsonConvert.DeserializeObject<List<string>>(json);
            }
            catch (Exception)
            {
            }
            try
            {
                string json = File.ReadAllText("Configs/AutoClarGuilds.json");
                AutoclearList = JsonConvert.DeserializeObject<List<string>>(json);
            }
            catch (Exception)
            {
            }
            try
            {
                string json = File.ReadAllText("Configs/AutoCreator.json");
                AutoCreator = JsonConvert.DeserializeObject<List<string>>(json);
            }
            catch (Exception)
            {
            }
            try
            {
                string json = File.ReadAllText("Configs/UsingTopic.json");
                UseTopic = JsonConvert.DeserializeObject<List<string>>(json);
            }
            catch (Exception)
            {
            }
            try
            {
                string json = File.ReadAllText("Configs/VoiceMsgState.json");
                VoiceON = JsonConvert.DeserializeObject<List<string>>(json);
            }
            catch (Exception)
            {
            }
            try
            {
                string json = File.ReadAllText("Configs/SavedPermission.json");
                SavedPermission = JsonConvert.DeserializeObject<List<PermissionAndGuildDTO>>(json);
            }
            catch (Exception)
            {
            }
            _client.SetGameAsync(".? for commands");
        }
        public class Catagory
        {
            public ulong GuildID { get; set; }
            public ulong CatagoryID { get; set; }
        }

        private async Task _client_UserVoiceStateUpdated(SocketUser arg1, SocketVoiceState arg2, SocketVoiceState arg3)
        {
            await Program.Log($"[VoiceStateUpdate] User {arg1.Username} ({arg1.Id}) triggered voice state update", ConsoleColor.Magenta);
            await Program.Log($"[VoiceStateUpdate] Old channel: {(arg2.VoiceChannel != null ? $"{arg2.VoiceChannel.Name} ({arg2.VoiceChannel.Id})" : "null")}", ConsoleColor.Magenta);
            await Program.Log($"[VoiceStateUpdate] New channel: {(arg3.VoiceChannel != null ? $"{arg3.VoiceChannel.Name} ({arg3.VoiceChannel.Id})" : "null")}", ConsoleColor.Magenta);

            bool Isdiffrent = false;
            if (arg2.VoiceChannel != null)
            {
                if (arg2.IsDeafened != arg3.IsDeafened)
                {
                    Isdiffrent = true;
                }
                if (arg2.IsMuted != arg3.IsMuted)
                {
                    Isdiffrent = true;
                }
                if (arg2.IsSelfDeafened != arg3.IsSelfDeafened)
                {
                    Isdiffrent = true;
                }
                if (arg2.IsSelfMuted != arg3.IsSelfMuted)
                {
                    Isdiffrent = true;
                }
                if (arg2.IsSuppressed != arg3.IsSuppressed)
                {
                    Isdiffrent = true;
                }
            }
            if (!Isdiffrent)
            {
                await Program.Log($"[VoiceStateUpdate] Voice state is a channel change (not just mute/deaf change)", ConsoleColor.Magenta);
                bool doesexist = false;
                SocketVoiceState State3 = arg3;
                OverwritePermissions AllowPerm = new OverwritePermissions();
                bool AutoCreatorIsOn = false;
                foreach (var item in AutoCreator)
                {
                    try
                    {
                        if (item == arg3.VoiceChannel.Guild.Id.ToString())
                        {
                            AutoCreatorIsOn = true;
                        }
                    }
                    catch (Exception)
                    {
                        if (item == arg2.VoiceChannel.Guild.Id.ToString())
                        {
                            AutoCreatorIsOn = true;
                        }
                    }
                }
                foreach (var item in SavedPermission)
                {
                    ulong GuildID = 0;
                    if (arg2.VoiceChannel != null)
                    {
                        GuildID = arg2.VoiceChannel.Guild.Id;
                    }
                    else if (arg3.VoiceChannel != null)
                    {
                        GuildID = arg3.VoiceChannel.Guild.Id;
                    }
                    if (item.GuildID == GuildID)
                    {
                        doesexist = true;
                        PermValue readMessages;
                        PermValue readMessageHistory;
                        PermValue sendMessages;
                        PermValue attachFiles;
                        PermValue embedLinks;
                        PermValue useExternalEmojis;
                        PermValue addReactions;
                        PermValue mentionEveryone;
                        PermValue manageMessages;
                        PermValue sendTTSMessages;
                        PermValue createInstantInvite;
#pragma warning disable CS0219 // Variable is assigned but its value is never used
                        PermValue managePermissions;
#pragma warning restore CS0219 // Variable is assigned but its value is never used
                        PermValue manageChannel;
                        PermValue manageWebhooks;
                        if (item.Permissions.ReadMessages)
                        {
                            readMessages = PermValue.Allow;
                        }
                        else
                        {
                            readMessages = PermValue.Deny;
                        }
                        if (item.Permissions.readMessageHistory)
                        {
                            readMessageHistory = PermValue.Allow;
                        }
                        else
                        {
                            readMessageHistory = PermValue.Deny;
                        }
                        if (item.Permissions.sendMessages)
                        {
                            sendMessages = PermValue.Allow;
                        }
                        else
                        {
                            sendMessages = PermValue.Deny;
                        }
                        if (item.Permissions.attachFiles)
                        {
                            attachFiles = PermValue.Allow;
                        }
                        else
                        {
                            attachFiles = PermValue.Deny;
                        }
                        if (item.Permissions.embedLinks)
                        {
                            embedLinks = PermValue.Allow;
                        }
                        else
                        {
                            embedLinks = PermValue.Deny;
                        }
                        if (item.Permissions.useExternalEmojis)
                        {
                            useExternalEmojis = PermValue.Allow;
                        }
                        else
                        {
                            useExternalEmojis = PermValue.Deny;
                        }
                        if (item.Permissions.addReactions)
                        {
                            addReactions = PermValue.Allow;
                        }
                        else
                        {
                            addReactions = PermValue.Deny;
                        }
                        if (item.Permissions.mentionEveryone)
                        {
                            mentionEveryone = PermValue.Allow;
                        }
                        else
                        {
                            mentionEveryone = PermValue.Deny;
                        }
                        if (item.Permissions.manageMessages)
                        {
                            manageMessages = PermValue.Allow;
                        }
                        else
                        {
                            manageMessages = PermValue.Deny;
                        }
                        if (item.Permissions.sendTTSMessages)
                        {
                            sendTTSMessages = PermValue.Allow;
                        }
                        else
                        {
                            sendTTSMessages = PermValue.Deny;
                        }
                        if (item.Permissions.createInstantInvite)
                        {
                            createInstantInvite = PermValue.Allow;
                        }
                        else
                        {
                            createInstantInvite = PermValue.Deny;
                        }
                        if (item.Permissions.managePermissions)
                        {
                            managePermissions = PermValue.Allow;
                        }
                        else
                        {
                            managePermissions = PermValue.Deny;
                        }
                        if (item.Permissions.manageChannel)
                        {
                            manageChannel = PermValue.Allow;
                        }
                        else
                        {
                            manageChannel = PermValue.Deny;
                        }
                        if (item.Permissions.manageWebhooks)
                        {
                            manageWebhooks = PermValue.Allow;
                        }
                        else
                        {
                            manageWebhooks = PermValue.Deny;
                        }
                        AllowPerm = new OverwritePermissions(
                        viewChannel: readMessages,
                        readMessageHistory: readMessageHistory,
                        sendMessages: sendMessages,
                        attachFiles: attachFiles,
                        embedLinks: embedLinks,
                        useExternalEmojis: useExternalEmojis,
                        addReactions: addReactions,
                        mentionEveryone: mentionEveryone,
                        manageMessages: manageMessages,
                        sendTTSMessages: sendTTSMessages,
                        createInstantInvite: createInstantInvite,
                        manageChannel: manageChannel,
                        manageWebhooks: manageWebhooks
                        );

                    }
                }
                if (doesexist)
                {
                }
                else
                {
                    AllowPerm = new OverwritePermissions(
                     viewChannel: PermValue.Allow,
                     readMessageHistory: PermValue.Allow,
                     sendMessages: PermValue.Allow,
                     attachFiles: PermValue.Allow,
                     embedLinks: PermValue.Allow,
                     useExternalEmojis: PermValue.Allow,
                     addReactions: PermValue.Allow,
                     mentionEveryone: PermValue.Inherit,
                     manageMessages: PermValue.Deny,
                     sendTTSMessages: PermValue.Inherit,
                     createInstantInvite: PermValue.Deny,
                     manageChannel: PermValue.Deny,
                     manageWebhooks: PermValue.Deny
                     );
                }
                if (arg2.VoiceChannel != arg3.VoiceChannel)
                {
                    await Program.Log($"[VoiceStateUpdate] User changed voice channels (LEFT old channel)", ConsoleColor.Magenta);
                    if (arg2.VoiceChannel != null)
                    {
                        var Server = arg2.VoiceChannel.Guild;
                        SocketVoiceState State = arg2;
                        await Program.Log($"[VoiceStateUpdate] Processing LEFT event for guild {Server.Name} ({Server.Id})", ConsoleColor.Magenta);
                        await Program.Log($"[VoiceStateUpdate] Voice channel after user left: {State.VoiceChannel.Name} has {State.VoiceChannel.Users.Count} users remaining", ConsoleColor.Magenta);

                        // Log who the remaining users are
                        if (State.VoiceChannel.ConnectedUsers.Count > 0)
                        {
                            await Program.Log($"[VoiceStateUpdate] Remaining users in {State.VoiceChannel.Name}:", ConsoleColor.Yellow);
                            foreach (var user in State.VoiceChannel.ConnectedUsers)
                            {
                                await Program.Log($"[VoiceStateUpdate]   - {user.Username} ({user.Id}) [IsBot: {user.IsBot}]", ConsoleColor.Yellow);
                            }
                        }

                        if (Server.Users.Contains(arg1))
                        {
                            if (Server.TextChannels.ToList().Exists(x => x.Name == Program.V_NameConverter(State.VoiceChannel.Name)))
                            {
                                var channel = Server.TextChannels.ToList().Find(x => x.Name == Program.V_NameConverter(State.VoiceChannel.Name));
                                await Program.Log($"[VoiceStateUpdate] Found associated text channel by name: {channel.Name} ({channel.Id})", ConsoleColor.Magenta);
                                if (VoiceON.Exists(x => x == Server.Id.ToString()))
                                {
                                    await channel.SendMessageAsync(arg1.Username + " left the channel!");
                                }
                                Discord.RequestOptions sd = new RequestOptions();
                                sd.AuditLogReason = "Test";

                                await channel.RemovePermissionOverwriteAsync(arg1, sd);
                                await Program.Log(arg1.Username + "(" + arg1.Id + ")" + " left channel " + channel.Name + "(" + channel.Id + ") - Permission have been removed!", ConsoleColor.Green);

                                await CheckMembers(channel);

                                // IMPORTANT: Get FRESH voice channel data from guild (State.VoiceChannel has stale cached data)
                                var currentVoiceChannel = Server.GetVoiceChannel(State.VoiceChannel.Id);
                                // Calculate actual user count excluding bots
                                var actualUserCount = currentVoiceChannel.ConnectedUsers.Where(u => !u.IsBot).Count();
                                await Program.Log($"[VoiceStateUpdate] Actual user count (excluding bots): {actualUserCount}", ConsoleColor.Yellow);

                                if (AutoclearList.Exists(x => x == Server.Id.ToString()))
                                {
                                    await Program.Log($"[AutoClear] Guild {Server.Id} has AutoClear enabled", ConsoleColor.Cyan);
                                    await Program.Log($"[AutoClear] Voice channel users count: {currentVoiceChannel.ConnectedUsers.Count} (actual: {actualUserCount})", ConsoleColor.Cyan);
                                    if (actualUserCount == 0)
                                    {
                                        await Program.Log($"[AutoClear] Voice channel is empty, recreating text channel", ConsoleColor.Yellow);
                                        var name = channel.Name;
                                        var posi = channel.Position;
                                        await channel.DeleteAsync();
                                        Thread.Sleep(200);
                                        await CreateTxtChan(name, currentVoiceChannel.Guild, false, posi, AllowPerm, null, false, currentVoiceChannel.Id);
                                    }
                                }
                                if (AutoCreator.Exists(x => x == Server.Id.ToString()))
                                {
                                    await Program.Log($"[AutoCreator] Guild {Server.Id} has AutoCreator enabled", ConsoleColor.Cyan);
                                    await Program.Log($"[AutoCreator] Voice channel users count: {currentVoiceChannel.ConnectedUsers.Count} (actual: {actualUserCount})", ConsoleColor.Cyan);
                                    if (actualUserCount == 0)
                                    {
                                        await Program.Log($"[AutoCreator] Voice channel is empty, deleting text channel permanently", ConsoleColor.Yellow);
                                        try
                                        {
                                            await Program.Log($"[AutoCreator] Attempting to delete text channel: {channel.Name} ({channel.Id})", ConsoleColor.Yellow);
                                            Thread.Sleep(150);
                                            await channel.DeleteAsync();
                                            await Program.Log(arg1.Username + "(" + arg1.Id + ")" + " left channel " + channel.Name + "(" + channel.Id + ") - Channel have been removed!", ConsoleColor.Green);
                                            Thread.Sleep(150);
                                        }
                                        catch (Exception ex)
                                        {
                                            await Program.Log($"[AutoCreator] ERROR deleting channel: {ex.Message}", ConsoleColor.Red);
                                        }
                                    }
                                    else
                                    {
                                        await Program.Log($"[AutoCreator] Voice channel still has users, keeping text channel", ConsoleColor.Yellow);
                                    }
                                }
                            }
                            else if (UseTopic.Exists(x => x == Server.Id.ToString()))
                            {
                                await Program.Log($"[VoiceStateUpdate] UseTopic mode enabled, searching by topic", ConsoleColor.Magenta);
                                var list = Server.TextChannels.ToList().FindAll(z => z.Topic != null);
                                if (list.Exists(x => x.Topic.Contains(arg2.VoiceChannel.Id.ToString())))
                                {
                                    var channel = list.Find(x => x.Topic.Contains(arg2.VoiceChannel.Id.ToString()));
                                    await Program.Log($"[VoiceStateUpdate] Found associated text channel by topic: {channel.Name} ({channel.Id})", ConsoleColor.Magenta);
                                    if (VoiceON.Exists(x => x == Server.Id.ToString()))
                                    {
                                        await channel.SendMessageAsync(arg1.Username + " left the channel!");
                                    }
                                    await channel.RemovePermissionOverwriteAsync(arg1, Discord.RequestOptions.Default);
                                    await Program.Log(arg1.Username + "(" + arg1.Id + ")" + " left channel " + channel.Name + "(" + channel.Id + ") - Permission have been removed!", ConsoleColor.Green);

                                    await CheckMembers(channel);

                                    // IMPORTANT: Get FRESH voice channel data from guild (State.VoiceChannel has stale cached data)
                                    var currentVoiceChannelTopic = Server.GetVoiceChannel(State.VoiceChannel.Id);
                                    // Calculate actual user count excluding bots
                                    var actualUserCountTopic = currentVoiceChannelTopic.ConnectedUsers.Where(u => !u.IsBot).Count();
                                    await Program.Log($"[VoiceStateUpdate] Actual user count (excluding bots): {actualUserCountTopic}", ConsoleColor.Yellow);

                                    if (AutoclearList.Exists(x => x == Server.Id.ToString()))
                                    {
                                        await Program.Log($"[AutoClear-Topic] Guild {Server.Id} has AutoClear enabled", ConsoleColor.Cyan);
                                        await Program.Log($"[AutoClear-Topic] Voice channel users count: {currentVoiceChannelTopic.ConnectedUsers.Count} (actual: {actualUserCountTopic})", ConsoleColor.Cyan);
                                        if (actualUserCountTopic == 0)
                                        {
                                            await Program.Log($"[AutoClear-Topic] Voice channel is empty, recreating text channel", ConsoleColor.Yellow);
                                            var name = channel.Name;
                                            var posi = channel.Position;
                                            await channel.DeleteAsync();
                                            Thread.Sleep(100);
                                            await CreateTxtChan(name, currentVoiceChannelTopic.Guild, false, posi, AllowPerm, null, false, currentVoiceChannelTopic.Id);
                                        }
                                    }
                                    if (AutoCreator.Exists(x => x == Server.Id.ToString()))
                                    {
                                        await Program.Log($"[AutoCreator-Topic] Guild {Server.Id} has AutoCreator enabled", ConsoleColor.Cyan);
                                        await Program.Log($"[AutoCreator-Topic] Voice channel users count: {currentVoiceChannelTopic.ConnectedUsers.Count} (actual: {actualUserCountTopic})", ConsoleColor.Cyan);
                                        if (actualUserCountTopic == 0)
                                        {
                                            await Program.Log($"[AutoCreator-Topic] Voice channel is empty, deleting text channel permanently", ConsoleColor.Yellow);
                                            try
                                            {
                                                await Program.Log($"[AutoCreator-Topic] Attempting to delete text channel: {channel.Name} ({channel.Id})", ConsoleColor.Yellow);
                                                await channel.DeleteAsync();
                                                await Program.Log(arg1.Username + "(" + arg1.Id + ")" + " left channel " + channel.Name + "(" + channel.Id + ") - Channel have been removed!", ConsoleColor.Green);
                                                Thread.Sleep(150);
                                            }
                                            catch (Exception ex)
                                            {
                                                await Program.Log($"[AutoCreator-Topic] ERROR deleting channel: {ex.Message}", ConsoleColor.Red);
                                            }
                                        }
                                        else
                                        {
                                            await Program.Log($"[AutoCreator-Topic] Voice channel still has users, keeping text channel", ConsoleColor.Yellow);
                                        }
                                    }
                                }
                                else
                                {
                                    await Program.Log($"[VoiceStateUpdate] No text channel found by topic for voice channel {arg2.VoiceChannel.Id}", ConsoleColor.Yellow);
                                }
                            }
                            else
                            {
                                await Program.Log($"[VoiceStateUpdate] No text channel found by name: {Program.V_NameConverter(State.VoiceChannel.Name)}", ConsoleColor.Yellow);
                            }
                            if (Server.TextChannels.ToList().Exists(x => (x.Name == "voice-only" || x.Name == "voice_only") && arg3.VoiceChannel == null))
                            {
                                var vonlychannel = Server.TextChannels.ToList().Find(x => (x.Name == "voice-only" || x.Name == "voice_only") && arg3.VoiceChannel == null);
                                await vonlychannel.RemovePermissionOverwriteAsync(arg1, Discord.RequestOptions.Default);
                                if (VoiceON.Exists(x => x == Server.Id.ToString()))
                                {
                                    await vonlychannel.SendMessageAsync(arg1.Username + " left the channel from " + arg2.VoiceChannel.Name + "!");
                                }

                            }
                        }
                    }
                }
                bool FoundChannel = false;
                if (arg3.VoiceChannel != null)
                {
                    await Program.Log($"[VoiceStateUpdate] User JOINED a new voice channel: {arg3.VoiceChannel.Name} ({arg3.VoiceChannel.Id})", ConsoleColor.Magenta);
                    if (((ignoreafk.Exists(x => x == arg3.VoiceChannel.Guild.Id.ToString())) && arg3.VoiceChannel.Id != arg3.VoiceChannel.Guild.AFKChannel.Id) || !ignoreafk.Exists(x => x == arg3.VoiceChannel.Guild.Id.ToString()))
                    {
                        var Server = _client.GetGuild(arg3.VoiceChannel.Guild.Id);
                        SocketVoiceState State = arg3;
                        await Program.Log($"[VoiceStateUpdate] Processing JOIN event for guild {Server.Name} ({Server.Id})", ConsoleColor.Magenta);
                        if (Server.Users.Contains(arg1))
                        {
                            if (Server.TextChannels.ToList().Exists(x => x.Name == Program.V_NameConverter(State.VoiceChannel.Name)))
                            {
                                var channel = Server.TextChannels.ToList().Find(x => x.Name == Program.V_NameConverter(State.VoiceChannel.Name));
                                FoundChannel = true;
                                if (VoiceON.Exists(x => x == Server.Id.ToString()))
                                {
                                    await channel.SendMessageAsync(arg1.Username + " joined the channel!");
                                }
                                await channel.AddPermissionOverwriteAsync(arg1, AllowPerm);
                                await Program.Log(arg1.Username + "(" + arg1.Id + ")" + " joined channel " + channel.Name + "(" + channel.Id + ") - Permission have been set!", ConsoleColor.Green);

                            }
                            else if (UseTopic.Exists(x => x == Server.Id.ToString()))
                            {
                                if (Server.TextChannels.ToList().Exists(x => x.Topic != null && x.Topic.Contains(arg3.VoiceChannel.Id.ToString())))
                                {
                                    var channel = Server.TextChannels.ToList().Find(x => x.Topic != null && x.Topic.Contains(arg3.VoiceChannel.Id.ToString()));
                                    FoundChannel = true;
                                    if (VoiceON.Exists(x => x == Server.Id.ToString()))
                                    {
                                        await channel.SendMessageAsync(arg1.Username + " joined the channel!");
                                    }
                                    await channel.AddPermissionOverwriteAsync(arg1, AllowPerm, Discord.RequestOptions.Default);
                                    await Program.Log(arg1.Username + "(" + arg1.Id + ")" + " joined channel " + channel.Name + "(" + channel.Id + ") - Permission have been set!", ConsoleColor.Green);

                                }
                            }
                        }
                        if (Server.TextChannels.ToList().Exists(x => (x.Name == "voice-only" || x.Name == "voice_only") && arg2.VoiceChannel == null))
                        {
                            var vonlychannel = Server.TextChannels.ToList().Find(x => (x.Name == "voice-only" || x.Name == "voice_only") && arg2.VoiceChannel == null);
                            await vonlychannel.AddPermissionOverwriteAsync(arg1, AllowPerm, Discord.RequestOptions.Default);
                            if (VoiceON.Exists(x => x == Server.Id.ToString()))
                            {
                                await vonlychannel.SendMessageAsync(arg1.Username + " joined the channel from " + arg3.VoiceChannel.Name + "!");
                            }

                        }
                        if (!FoundChannel && AutoCreatorIsOn)
                        {
                            await Program.Log($"[AutoCreator] No text channel found, creating new one", ConsoleColor.Cyan);
                            if (UseTopic.Exists(x => x == Server.Id.ToString()))
                            {
                                await Program.Log($"[AutoCreator] Creating text channel with topic mode", ConsoleColor.Cyan);
                                var channel = await CreateTxtChan(arg3.VoiceChannel.Name, arg3.VoiceChannel.Guild, false, arg3.VoiceChannel.Position + 20, AllowPerm, arg1, true, arg3.VoiceChannel.Id);
                                if (VoiceON.Exists(x => x == Server.Id.ToString()))
                                {
                                    await channel.SendMessageAsync(arg1.Username + " joined the channel!");
                                }
                            }
                            else
                            {
                                await Program.Log($"[AutoCreator] Creating text channel with name conversion mode", ConsoleColor.Cyan);
                                var channel = await CreateTxtChan(arg3.VoiceChannel.Name, arg3.VoiceChannel.Guild, true, arg3.VoiceChannel.Position + 20, AllowPerm, arg1, false, arg3.VoiceChannel.Id);
                                if (VoiceON.Exists(x => x == Server.Id.ToString()))
                                {
                                    await channel.SendMessageAsync(arg1.Username + " joined the channel!");
                                }
                            }
                        }
                    }
                    if (AutoCreator.Exists(x => x == arg3.VoiceChannel.Guild.Id.ToString()))
                    {
                        await Program.Log($"[AutoCreator-Cleanup] Starting cleanup for guild {arg3.VoiceChannel.Guild.Id}", ConsoleColor.Cyan);
                        var voiceChannels = arg3.VoiceChannel.Guild.VoiceChannels.ToList();
                        await Program.Log($"[AutoCreator-Cleanup] Total voice channels to check: {voiceChannels.Count}", ConsoleColor.Cyan);
                        foreach (var item in voiceChannels)
                        {
                            await Program.Log($"[AutoCreator-Cleanup] Checking voice channel: {item.Name} ({item.Id}) with {item.ConnectedUsers.Count} users", ConsoleColor.Cyan);

                            // Log who is in the voice channel
                            if (item.ConnectedUsers.Count > 0)
                            {
                                foreach (var user in item.ConnectedUsers)
                                {
                                    await Program.Log($"[AutoCreator-Cleanup]   User in channel: {user.Username} ({user.Id}) [IsBot: {user.IsBot}]", ConsoleColor.Cyan);
                                }
                            }

                            if (item.ConnectedUsers.Count == 0)
                            {
                                await Program.Log($"[AutoCreator-Cleanup] Found empty voice channel: {item.Name} ({item.Id})", ConsoleColor.Cyan);
                                var textChannels = arg3.VoiceChannel.Guild.TextChannels.ToList();
                                await Program.Log($"[AutoCreator-Cleanup] Searching through {textChannels.Count} text channels for topic containing {item.Id}", ConsoleColor.Cyan);
                                if (textChannels.Exists(x => x.Topic != null && x.Topic.Contains(item.Id.ToString())))
                                {
                                    var channel = textChannels.Find(x => x.Topic != null && x.Topic.Contains(item.Id.ToString()));
                                    try
                                    {
                                        await Program.Log($"[AutoCreator-Cleanup] Deleting text channel: {channel.Name} ({channel.Id}) with topic: {channel.Topic}", ConsoleColor.Yellow);
                                        await channel.DeleteAsync();
                                        await Program.Log($"[AutoCreator-Cleanup] Successfully deleted text channel {channel.Name} ({channel.Id})", ConsoleColor.Green);
                                    }
                                    catch (Exception ex)
                                    {
                                        await Program.Log($"[AutoCreator-Cleanup] ERROR deleting channel: {ex.Message}", ConsoleColor.Red);
                                        await Program.Log($"[AutoCreator-Cleanup] Stack trace: {ex.StackTrace}", ConsoleColor.Red);
                                    }
                                }
                                else
                                {
                                    await Program.Log($"[AutoCreator-Cleanup] No text channel found with topic containing {item.Id}", ConsoleColor.Yellow);
                                }
                            }
                        }
                        await Program.Log($"[AutoCreator-Cleanup] Cleanup completed for guild {arg3.VoiceChannel.Guild.Id}", ConsoleColor.Cyan);
                    }
                }
                else
                {
                    await Program.Log($"[VoiceStateUpdate] User disconnected from all voice channels", ConsoleColor.Magenta);
                }
            }
            else
            {
                await Program.Log($"[VoiceStateUpdate] Ignoring event - only mute/deaf state changed, not a channel change", ConsoleColor.Magenta);
            }
        }



        public async Task<RestTextChannel> CreateTxtChan(string Name, SocketGuild Guild, bool Convert, int VPosision, OverwritePermissions AllowPerm, SocketUser User, bool UsingTopic, ulong VChanID)
        {
            var name = Name;
            if (Convert)
            {
                name = Program.V_NameConverter(Name);
            }
            else
            {
                name = Program.NameConverter(Name);
            }
            var channel = await Guild.CreateTextChannelAsync(name);

            // IMPORTANT: Await all permission operations to ensure they complete
            await channel.AddPermissionOverwriteAsync(_client.CurrentUser, Discord.OverwritePermissions.AllowAll(channel));
            if (User != null)
            {
                await channel.AddPermissionOverwriteAsync(User, AllowPerm, Discord.RequestOptions.Default);
            }
            await channel.AddPermissionOverwriteAsync(Guild.EveryoneRole, Discord.OverwritePermissions.DenyAll(channel));

            if (UsingTopic)
            {
                await channel.ModifyAsync(x => x.Topic = VChanID.ToString());
            }

            // IMPORTANT: Await category assignment to ensure channel is moved
            if (CatagoryGuilds.Exists(x=> x.GuildID == Guild.Id))
            {
                var item = CatagoryGuilds.Single(x => x.GuildID == Guild.Id);
                if (Guild.CategoryChannels.ToList().Exists(x=> x.Id == item.CatagoryID))
                {
                    var category = Guild.CategoryChannels.FirstOrDefault(x => x.Id == item.CatagoryID);
                    await channel.ModifyAsync(x => { x.CategoryId = category.Id; });
                }

            }
            else
            {
                await channel.ModifyAsync(x => x.Position = VPosision);
            }

            await Program.Log(User.Username + "(" + User.Id + ")" + " joined channel " + channel.Name + "(" + channel.Id + ") - Permission have been set!", ConsoleColor.Green);

            return channel;
        }
        public async Task CheckMembers(SocketTextChannel channel)
        {
            foreach (var item in channel.Users.Where(x=> !x.IsBot && x.Id != _client.CurrentUser.Id))
            {
                if (item.VoiceChannel == null)
                {
                    if (channel.PermissionOverwrites.ToList().Exists(x => x.TargetId == item.Id))
                    {
                        try
                        {
                            await channel.RemovePermissionOverwriteAsync(item);
                        }
                        catch (Exception) { }
                    }
                }
            }
        }
        private async Task _client_MessageReceived(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;
            if (msg == null) return;
            var context = new SocketCommandContext(_client, msg);
            if (AutoCreator.Exists(x => x == context.Guild.Id.ToString()))
            {
                foreach (var item in context.Guild.VoiceChannels)
                {
                    if (item.ConnectedUsers.Count == 0)
                    {
                        if (context.Guild.TextChannels.ToList().Exists(x => x.Topic != null && x.Topic.Contains(item.Id.ToString())))
                        {
                            var channel = context.Guild.TextChannels.ToList().Find(x => x.Topic != null && x.Topic.Contains(item.Id.ToString()));
                            await channel.DeleteAsync();
                        }
                    }
                }
            }
            int argPost = 0;
            if (msg.HasCharPrefix('.', ref argPost))
            {
                if (CommandPermissionHandler.IsAllowedToUseCommand(context))
                {
                    var result = _service.ExecuteAsync(context, argPost, null);
                    if (!result.Result.IsSuccess && result.Result.Error != CommandError.UnknownCommand)
                    {
                        await context.Channel.SendMessageAsync(result.Result.ErrorReason);
                    }
                    await Program.Log("Invoked " + msg + " in " + context.Channel + " with " + result.Result, ConsoleColor.Magenta);
                }
                else
                {
                    await Program.Log("Invoked " + msg + " in " + context.Channel + " with Permission Error!", ConsoleColor.Magenta);
                    await context.Channel.SendMessageAsync($@"You need {CommandPermissionHandler.PermissionNeed(context.Guild.Id)} to use the commands on this bot!");
                }
            }
            else
            {
                await Program.Log(context.Channel + "-" + context.User.Username + " : " + msg, ConsoleColor.White);
            }


        }
    }
}
