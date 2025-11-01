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
            string guildID = "";
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
                        PermValue managePermissions;
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
                    if (arg2.VoiceChannel != null)
                    {
                        var Server = arg2.VoiceChannel.Guild;
                        SocketVoiceState State = arg2;
                        if (Server.Users.Contains(arg1))
                        {
                            if (Server.TextChannels.ToList().Exists(x => x.Name == Program.V_NameConverter(State.VoiceChannel.Name)))
                            {
                                var channel = Server.TextChannels.ToList().Find(x => x.Name == Program.V_NameConverter(State.VoiceChannel.Name));
                                if (VoiceON.Exists(x => x == Server.Id.ToString()))
                                {
                                    await channel.SendMessageAsync(arg1.Username + " left the channel!");
                                }
                                Discord.RequestOptions sd = new RequestOptions();
                                sd.AuditLogReason = "Test";

                                await channel.RemovePermissionOverwriteAsync(arg1, sd);
                                await Program.Log(arg1.Username + "(" + arg1.Id + ")" + " left channel " + channel.Name + "(" + channel.Id + ") - Permission have been removed!", ConsoleColor.Green);

                                await CheckMembers(channel);
                                if (AutoclearList.Exists(x => x == Server.Id.ToString()))
                                {
                                    if (State.VoiceChannel.Users.Count == 0)
                                    {
                                        var name = channel.Name;
                                        var posi = channel.Position;
                                        await channel.DeleteAsync();
                                        Thread.Sleep(200);
                                        CreateTxtChan(name, State.VoiceChannel.Guild, false, posi, AllowPerm, null, false, State.VoiceChannel.Id);
                                    }
                                }
                                if (AutoCreator.Exists(x => x == Server.Id.ToString()))
                                {
                                    if (State.VoiceChannel.Users.Count == 0)
                                    {
                                        Thread.Sleep(150);
                                        await channel.DeleteAsync();
                                        await Program.Log(arg1.Username + "(" + arg1.Id + ")" + " left channel " + channel.Name + "(" + channel.Id + ") - Channel have been removed!", ConsoleColor.Green);

                                        Thread.Sleep(150);
                                    }
                                }
                            }
                            else if (UseTopic.Exists(x => x == Server.Id.ToString()))
                            {
                                var list = Server.TextChannels.ToList().FindAll(z => z.Topic != null);
                                if (list.Exists(x => x.Topic.Contains(arg2.VoiceChannel.Id.ToString())))
                                {
                                    var channel = list.Find(x => x.Topic.Contains(arg2.VoiceChannel.Id.ToString()));
                                    if (VoiceON.Exists(x => x == Server.Id.ToString()))
                                    {
                                        await channel.SendMessageAsync(arg1.Username + " left the channel!");
                                    }
                                    await channel.RemovePermissionOverwriteAsync(arg1, Discord.RequestOptions.Default);
                                    await Program.Log(arg1.Username + "(" + arg1.Id + ")" + " left channel " + channel.Name + "(" + channel.Id + ") - Permission have been removed!", ConsoleColor.Green);

                                    await CheckMembers(channel);
                                    if (AutoclearList.Exists(x => x == Server.Id.ToString()))
                                    {
                                        if (State.VoiceChannel.Users.Count == 0)
                                        {
                                            var name = channel.Name;
                                            var posi = channel.Position;
                                            await channel.DeleteAsync();
                                            Thread.Sleep(100);
                                            CreateTxtChan(name, State.VoiceChannel.Guild, false, posi, AllowPerm, null, false, State.VoiceChannel.Id);
                                        }
                                    }
                                    if (AutoCreator.Exists(x => x == Server.Id.ToString()))
                                    {
                                        if (State.VoiceChannel.Users.Count == 0)
                                        {
                                            await channel.DeleteAsync();
                                            await Program.Log(arg1.Username + "(" + arg1.Id + ")" + " left channel " + channel.Name + "(" + channel.Id + ") - Channel have been removed!", ConsoleColor.Green);

                                            Thread.Sleep(150);
                                        }
                                    }
                                }
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
                    if (((ignoreafk.Exists(x => x == arg3.VoiceChannel.Guild.Id.ToString())) && arg3.VoiceChannel.Id != arg3.VoiceChannel.Guild.AFKChannel.Id) || !ignoreafk.Exists(x => x == arg3.VoiceChannel.Guild.Id.ToString()))
                    {
                        var Server = _client.GetGuild(arg3.VoiceChannel.Guild.Id);
                        SocketVoiceState State = arg3;
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
                            if (UseTopic.Exists(x => x == Server.Id.ToString()))
                            {

                                var channel = CreateTxtChan(arg3.VoiceChannel.Name, arg3.VoiceChannel.Guild, false, arg3.VoiceChannel.Position + 20, AllowPerm, arg1, true, arg3.VoiceChannel.Id);
                                if (VoiceON.Exists(x => x == Server.Id.ToString()))
                                {
                                    await channel.SendMessageAsync(arg1.Username + " joined the channel!");
                                }
                            }
                            else
                            {
                                var channel = CreateTxtChan(arg3.VoiceChannel.Name, arg3.VoiceChannel.Guild, true, arg3.VoiceChannel.Position + 20, AllowPerm, arg1, false, arg3.VoiceChannel.Id);
                                if (VoiceON.Exists(x => x == Server.Id.ToString()))
                                {
                                    await channel.SendMessageAsync(arg1.Username + " joined the channel!");
                                }
                            }
                        }
                    }
                    if (AutoCreator.Exists(x => x == arg3.VoiceChannel.Guild.Id.ToString()))
                    {
                        foreach (var item in arg3.VoiceChannel.Guild.VoiceChannels)
                        {
                            if (item.Users.Count == 0)
                            {
                                if (arg3.VoiceChannel.Guild.TextChannels.ToList().Exists(x => x.Topic != null && x.Topic.Contains(item.Id.ToString())))
                                {
                                    var channel = arg3.VoiceChannel.Guild.TextChannels.ToList().Find(x => x.Topic != null && x.Topic.Contains(item.Id.ToString()));
                                    await channel.DeleteAsync();
                                    await Program.Log(arg1.Username + "(" + arg1.Id + ")" + " left channel " + channel.Name + "(" + channel.Id + ") - Channel have been removed!", ConsoleColor.Green);

                                }
                            }
                        }
                    }
                }
            }
        }
                
                   

        public RestTextChannel CreateTxtChan(string Name, SocketGuild Guild, bool Convert, int VPosision, OverwritePermissions AllowPerm, SocketUser User, bool UsingTopic, ulong VChanID)
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
            var channel = Guild.CreateTextChannelAsync(name).Result;
            channel.AddPermissionOverwriteAsync(_client.CurrentUser, Discord.OverwritePermissions.AllowAll(channel));
            if (User != null)
            {
                channel.AddPermissionOverwriteAsync(User, AllowPerm, Discord.RequestOptions.Default);
            }
            channel.AddPermissionOverwriteAsync(Guild.EveryoneRole, Discord.OverwritePermissions.DenyAll(channel));
            if (UsingTopic)
            {
                channel.ModifyAsync(x => x.Topic = VChanID.ToString());
            }
            if (CatagoryGuilds.Exists(x=> x.GuildID == Guild.Id))
            {
                var item = CatagoryGuilds.Single(x => x.GuildID == Guild.Id);
                if (Guild.CategoryChannels.ToList().Exists(x=> x.Id == item.CatagoryID))
                {
                    var category = Guild.CategoryChannels.FirstOrDefault(x => x.Id == item.CatagoryID);
                    channel.ModifyAsync(x => { x.CategoryId = category.Id; });
                }

            }
            else
            {
                channel.ModifyAsync(x => x.Position = VPosision);
            }
       
            Program.Log(User.Username + "(" + User.Id + ")" + " joined channel " + channel.Name + "(" + channel.Id + ") - Permission have been set!", ConsoleColor.Green);

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
                    if (item.Users.Count == 0)
                    {
                        if (context.Guild.TextChannels.ToList().Exists(x => x.Topic != null && x.Topic.Contains(item.Id.ToString())))
                        {
                            var channel = context.Guild.TextChannels.ToList().Find(x => x.Topic != null && x.Topic.Contains(item.Id.ToString()));
                            await channel.DeleteAsync();
                        }
                    }
                }
            }            int argPost = 0;
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
