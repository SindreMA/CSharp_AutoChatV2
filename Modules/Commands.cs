using CSharp_AutoChatV2;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UtilityBot.dto;

namespace TemplateBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Alias("?")]
        public async Task help()
        {
            var channel = Context.Channel;
            await channel.SendMessageAsync(
                "([]) = Optinal   [] = Required    () = Explaination \n\n"+
                "**.count **(Returns the amount of people in your voice channel)" + Environment.NewLine +
                "**.voicemsg [on/off]** (Turns off or on the join message[MODS])" + Environment.NewLine +
                "**.CreateV ([CategoryID])** (Creates all the a 'v _' text channel per voice channel)" + Environment.NewLine +
                "**.AddNewV ([CategoryID])** (Creates all the a 'v _' text channel that are missing)" + Environment.NewLine +
                "**.DeleteV** (Deletes all the text channels that start with 'v _' )" + Environment.NewLine +
                "**.VRefresh** (Refreshes all the user permission [Usefull if someone joins when the bot is offline])" + Environment.NewLine +
                "**.ShowConfig** (Shows you the current config for the v_ channels [if you have a config])" + Environment.NewLine +
                "**.ChangeConfig [Permission] [True/False]** (Lets you change/create a config for the v_ channels )" + Environment.NewLine +
                "**.RemoveConfig** (Removes your config and goes back to default)" + Environment.NewLine +
                "**.usetopic** (Toggle on and off on how to detect a voice channel(use this on to have custom names))" + Environment.NewLine +
                "**.ignoreafk** (Toggle on and off for if you want to ignore the AFK channel)" + Environment.NewLine +
                "**.AutoCreator** (Toggle on and off if you want the bot to autocreate the channels)" + Environment.NewLine +
                "**.Autoclear** (Toggle on and off if you want the bot to recreate the channel whenever its empty)" + Environment.NewLine +
                "**.SetCommandPermission [PermissionID]** (Sets the required permission to use the bot)" + Environment.NewLine +
                "**.ShowCommandPermission [PermissionID]** (Shows you the PermissionIDs)" + Environment.NewLine +
                "**.DefaultCategory [CategoryId]** (Sets a category where the channels will be autocreated in)" + Environment.NewLine +


                "**.help **(shows this message)");


        }
        [Command("removeconfig")]
        public async Task removeconfig()
        {
            try
            {

                foreach (var item in CommandHandler.SavedPermission)
                {
                    if (item.GuildID == Context.Guild.Id)
                    {
                        CommandHandler.SavedPermission.Remove(item);
                        await Context.Channel.SendMessageAsync("Config have been deleted, you are now using default settings!");
                        File.WriteAllText("Configs/SavedPermission.json", JsonConvert.SerializeObject(CommandHandler.SavedPermission));
                    }
                }
            }
            catch (Exception)
            {

            }


        }
        [Command("showconfig")]
        public async Task showconfig()
        {
            bool didfind = false;
            foreach (var item in CommandHandler.SavedPermission)
            {
                if (Context.Guild.Id == item.GuildID)
                {
                    didfind = true;
                    string perm = JsonConvert.SerializeObject(item.Permissions);
                    await Context.Channel.SendMessageAsync(
                        "Here are the permissions for the v_ channels(what is being set automatically)" + Environment.NewLine +
                        "To change it do the command .ChangeConfig [Permission] [True/False]" +
                        "" + Environment.NewLine +
                        perm.Replace(",\"", Environment.NewLine).Replace("{\"", "").Replace("}", "").Replace("\":", " = ")
                        );
                }
            }
            if (!didfind)
            {
                await Context.Channel.SendMessageAsync(
                    "You are using the default settings!" + Environment.NewLine +
                    "ReadMessages = true        " + Environment.NewLine +
                    "readMessageHistory = true  " + Environment.NewLine +
                    "sendMessages = true        " + Environment.NewLine +
                    "attachFiles = true         " + Environment.NewLine +
                    "embedLinks = true          " + Environment.NewLine +
                    "useExternalEmojis = true   " + Environment.NewLine +
                    "addReactions = true        " + Environment.NewLine +
                    "mentionEveryone = inherit     " + Environment.NewLine +
                    "manageMessages = false     " + Environment.NewLine +
                    "sendTTSMessages = inherit    " + Environment.NewLine +
                    "managePermissions = false  " + Environment.NewLine +
                    "createInstantInvite = false" + Environment.NewLine +
                    "manageChannel = false" + Environment.NewLine +
                    "manageWebhooks = false"
                    );
            }
        }
        [Command("ChangeConfig")]
        public async Task ChangeConfig(string perm, bool state)
        {


            PermissionDTO newPermissions = new PermissionDTO();
            newPermissions.ReadMessages = true;
            newPermissions.readMessageHistory = true;
            newPermissions.sendMessages = true;
            newPermissions.attachFiles = true;
            newPermissions.embedLinks = true;
            newPermissions.useExternalEmojis = true;
            newPermissions.addReactions = true;
            newPermissions.mentionEveryone = false;
            newPermissions.manageMessages = false;
            newPermissions.sendTTSMessages = false;
            newPermissions.createInstantInvite = false;
            newPermissions.managePermissions = false;
            newPermissions.manageChannel = false;
            newPermissions.manageWebhooks = false;

            foreach (var item in CommandHandler.SavedPermission)
            {
                if (item.GuildID == Context.Guild.Id)
                {
                    if (item.Permissions.ReadMessages)
                    {
                        newPermissions.ReadMessages = true;
                    }
                    else
                    {
                        newPermissions.ReadMessages = false;
                    }
                    if (item.Permissions.readMessageHistory)
                    {
                        newPermissions.readMessageHistory = true;
                    }
                    else
                    {
                        newPermissions.readMessageHistory = false;
                    }
                    if (item.Permissions.sendMessages)
                    {
                        newPermissions.sendMessages = true;
                    }
                    else
                    {
                        newPermissions.sendMessages = false;
                    }
                    if (item.Permissions.attachFiles)
                    {
                        newPermissions.attachFiles = true;
                    }
                    else
                    {
                        newPermissions.attachFiles = false;
                    }
                    if (item.Permissions.embedLinks)
                    {
                        newPermissions.embedLinks = true;
                    }
                    else
                    {
                        newPermissions.embedLinks = false;
                    }
                    if (item.Permissions.useExternalEmojis)
                    {
                        newPermissions.useExternalEmojis = true;
                    }
                    else
                    {
                        newPermissions.useExternalEmojis = false;
                    }
                    if (item.Permissions.addReactions)
                    {
                        newPermissions.addReactions = true;
                    }
                    else
                    {
                        newPermissions.addReactions = false;
                    }
                    if (item.Permissions.mentionEveryone)
                    {
                        newPermissions.mentionEveryone = true;
                    }
                    else
                    {
                        newPermissions.mentionEveryone = false;
                    }
                    if (item.Permissions.manageMessages)
                    {
                        newPermissions.manageMessages = true;
                    }
                    else
                    {
                        newPermissions.manageMessages = false;
                    }
                    if (item.Permissions.sendTTSMessages)
                    {
                        newPermissions.sendTTSMessages = true;
                    }
                    else
                    {
                        newPermissions.sendTTSMessages = false;
                    }
                    if (item.Permissions.createInstantInvite)
                    {
                        newPermissions.createInstantInvite = true;
                    }
                    else
                    {
                        newPermissions.createInstantInvite = false;
                    }
                    if (item.Permissions.managePermissions)
                    {
                        newPermissions.managePermissions = true;
                    }
                    else
                    {
                        newPermissions.managePermissions = false;
                    }
                    if (item.Permissions.manageChannel)
                    {
                        newPermissions.manageChannel = true;
                    }
                    else
                    {
                        newPermissions.manageChannel = false;
                    }
                    if (item.Permissions.manageWebhooks)
                    {
                        newPermissions.manageWebhooks = true;
                    }
                    else
                    {
                        newPermissions.manageWebhooks = false;
                    }
                }
            }



            if (perm.ToLower() == "readMessages".ToLower() && state)
            {
                newPermissions.ReadMessages = true;
            }
            else if (perm.ToLower() == "readMessages".ToLower() && !state)
            {
                newPermissions.ReadMessages = false;
            }
            ////////////////////NEW  SETTING/////////////////////
            else if (perm.ToLower() == "readMessageHistory".ToLower() && state)
            {
                newPermissions.readMessageHistory = true;
            }
            else if (perm.ToLower() == "readMessageHistory".ToLower() && !state)
            {
                newPermissions.readMessageHistory = false;
            }
            ////////////////////NEW  SETTING/////////////////////
            else if (perm.ToLower() == "sendMessages".ToLower() && state)
            {
                newPermissions.sendMessages = true;
            }
            else if (perm.ToLower() == "sendMessages".ToLower() && !state)
            {
                newPermissions.sendMessages = false;
            }
            ////////////////////NEW  SETTING/////////////////////
            else if (perm.ToLower() == "attachFiles".ToLower() && state)
            {
                newPermissions.attachFiles = true;
            }
            else if (perm.ToLower() == "attachFiles".ToLower() && !state)
            {
                newPermissions.attachFiles = false;
            }
            ////////////////////NEW  SETTING/////////////////////
            else if (perm.ToLower() == "embedLinks".ToLower() && state)
            {
                newPermissions.embedLinks = true;
            }
            else if (perm.ToLower() == "embedLinks".ToLower() && !state)
            {
                newPermissions.embedLinks = false;
            }
            ////////////////////NEW  SETTING/////////////////////
            else if (perm.ToLower() == "useExternalEmojis".ToLower() && state)
            {
                newPermissions.useExternalEmojis = true;
            }
            else if (perm.ToLower() == "useExternalEmojis".ToLower() && !state)
            {
                newPermissions.useExternalEmojis = false;
            }
            ////////////////////NEW  SETTING/////////////////////
            else if (perm.ToLower() == "addReactions".ToLower() && state)
            {
                newPermissions.addReactions = true;
            }
            else if (perm.ToLower() == "addReactions".ToLower() && !state)
            {
                newPermissions.addReactions = false;
            }
            ////////////////////NEW  SETTING/////////////////////
            else if (perm.ToLower() == "mentionEveryone".ToLower() && state)
            {
                newPermissions.mentionEveryone = true;
            }
            else if (perm.ToLower() == "mentionEveryone".ToLower() && !state)
            {
                newPermissions.mentionEveryone = false;
            }
            ////////////////////NEW  SETTING/////////////////////
            else if (perm.ToLower() == "manageMessages".ToLower() && state)
            {
                newPermissions.manageMessages = true;
            }
            else if (perm.ToLower() == "manageMessages".ToLower() && !state)
            {
                newPermissions.manageMessages = false;
            }
            ////////////////////NEW  SETTING/////////////////////
            else if (perm.ToLower() == "sendTTSMessages".ToLower() && state)
            {
                newPermissions.sendTTSMessages = true;
            }
            else if (perm.ToLower() == "sendTTSMessages".ToLower() && !state)
            {
                newPermissions.sendTTSMessages = false;
            }
            ////////////////////NEW  SETTING/////////////////////
            else if (perm.ToLower() == "createInstantInvite".ToLower() && state)
            {
                newPermissions.createInstantInvite = true;
            }
            else if (perm.ToLower() == "createInstantInvite".ToLower() && !state)
            {
                newPermissions.createInstantInvite = false;
            }
            ////////////////////NEW  SETTING/////////////////////
            else if (perm.ToLower() == "managePermissions".ToLower() && state)
            {
                newPermissions.managePermissions = true;
            }
            else if (perm.ToLower() == "managePermissions".ToLower() && !state)
            {
                newPermissions.managePermissions = false;
            }
            ////////////////////NEW  SETTING/////////////////////
            else if (perm.ToLower() == "manageChannel".ToLower() && state)
            {
                newPermissions.manageChannel = true;
            }
            else if (perm.ToLower() == "manageChannel".ToLower() && !state)
            {
                newPermissions.manageChannel = false;
            }
            ////////////////////NEW  SETTING/////////////////////
            else if (perm.ToLower() == "manageWebhooks".ToLower() && state)
            {
                newPermissions.manageWebhooks = true;
            }
            else if (perm.ToLower() == "manageWebhooks".ToLower() && !state)
            {
                newPermissions.manageWebhooks = false;
            }
            foreach (var item in CommandHandler.SavedPermission)
            {
                if (item.GuildID == Context.Guild.Id)
                {
                    CommandHandler.SavedPermission.Remove(item);
                }
            }
            PermissionAndGuildDTO permandguild = new PermissionAndGuildDTO();
            permandguild.GuildID = Context.Guild.Id;
            permandguild.Permissions = newPermissions;
            CommandHandler.SavedPermission.Add(permandguild);
            File.WriteAllText("Configs/SavedPermission.json", JsonConvert.SerializeObject(CommandHandler.SavedPermission));
            await Context.Channel.SendMessageAsync("Changes have been saved!");



        }
        [Command("Autoclear")]
        public async Task Autoclear()
        {

            bool AutoChatIsOn = false;
            bool AutoCreator = false;
            if (CommandHandler.AutoclearList != null)
            {
                foreach (var item in CommandHandler.AutoCreator)
                {
                    if (item == Context.Guild.Id.ToString())
                    {
                        AutoCreator = true;
                    }
                }

            }

            if (AutoCreator)
            {
                await Context.Channel.SendMessageAsync("AutoCreator and AutoClear cant be used at the same time, Please turn off auto creation by using .autocreator");
            }

            else if (CommandHandler.AutoclearList != null)
            {
                foreach (var item in CommandHandler.AutoclearList)
                {
                    if (item == Context.Guild.Id.ToString())
                    {
                        AutoChatIsOn = true;
                    }
                }
                if (AutoChatIsOn)
                {
                    CommandHandler.AutoclearList.Remove((Context.Guild.Id.ToString()));
                    await Context.Channel.SendMessageAsync("Auto clearing is now off");
                    File.WriteAllText("Configs/AutoClarGuilds.json", JsonConvert.SerializeObject(CommandHandler.AutoclearList));
                }
                else if (!AutoChatIsOn)
                {
                    CommandHandler.AutoclearList.Add((Context.Guild.Id.ToString()));
                    await Context.Channel.SendMessageAsync("Auto clearing is now on");
                    File.WriteAllText("Configs/AutoClarGuilds.json", JsonConvert.SerializeObject(CommandHandler.AutoclearList));
                }
            }
            else
            {
                CommandHandler.AutoclearList.Add((Context.Guild.Id.ToString()));
                await Context.Channel.SendMessageAsync("Auto clearing is now on");
                File.WriteAllText("Configs/AutoClarGuilds.json", JsonConvert.SerializeObject(CommandHandler.AutoclearList));
            }
        }//Put the guild in a list that makes it clear chat on 0 ppl
        [Command("AutoCreator")]
        public async Task AutoCreator()
        {
            bool AutoCreator = false;
            bool AutoChatIsOn = false;
            if (CommandHandler.AutoclearList != null)
            {
                foreach (var item in CommandHandler.AutoclearList)
                {
                    if (item == Context.Guild.Id.ToString())
                    {
                        AutoChatIsOn = true;
                    }
                }

            }
            if (AutoChatIsOn)
            {
                await Context.Channel.SendMessageAsync("AutoCreator and AutoClear cant be used at the same time, Please turn off autoclearing by using .autoclear");
            }
            else if (CommandHandler.AutoclearList != null)
            {
                foreach (var item in CommandHandler.AutoCreator)
                {
                    if (item == Context.Guild.Id.ToString())
                    {
                        AutoCreator = true;
                    }
                }
                if (AutoCreator)
                {
                    CommandHandler.AutoCreator.Remove((Context.Guild.Id.ToString()));
                    await Context.Channel.SendMessageAsync("Auto creating is now off");
                    File.WriteAllText("Configs/AutoCreator.json", JsonConvert.SerializeObject(CommandHandler.AutoCreator));
                }
                else if (!AutoCreator)
                {
                    CommandHandler.AutoCreator.Add((Context.Guild.Id.ToString()));
                    await Context.Channel.SendMessageAsync("Auto creating is now on");
                    File.WriteAllText("Configs/AutoCreator.json", JsonConvert.SerializeObject(CommandHandler.AutoCreator));
                }
            }
            else
            {
                CommandHandler.AutoCreator.Add((Context.Guild.Id.ToString()));
                await Context.Channel.SendMessageAsync("Auto creating is now on");
                File.WriteAllText("Configs/AutoCreator.json", JsonConvert.SerializeObject(CommandHandler.AutoCreator));
            }
        }//Put the guild in a list that makes it auto create the channels and delete them when not in use
        [Command("usetopic")]
        public async Task usetopic()
        {
            if (CommandHandler.UseTopic.Exists(x => x == Context.Guild.Id.ToString()))
            {
                CommandHandler.UseTopic.Remove((Context.Guild.Id.ToString()));
                await Context.Channel.SendMessageAsync("Using topic is now off");
                File.WriteAllText("Configs/UsingTopic.json", JsonConvert.SerializeObject(CommandHandler.UseTopic));
            }
            else
            {
                CommandHandler.UseTopic.Add((Context.Guild.Id.ToString()));
                await Context.Channel.SendMessageAsync("Using topic is now on");
                File.WriteAllText("Configs/UsingTopic.json", JsonConvert.SerializeObject(CommandHandler.UseTopic));
            }

        }
        [Command("ignoreafk")]
        public async Task ignoreafk()
        {
            if (CommandHandler.ignoreafk.Exists(x => x == Context.Guild.Id.ToString()))
            {
                CommandHandler.ignoreafk.Remove((Context.Guild.Id.ToString()));
                await Context.Channel.SendMessageAsync("Using ignoreafk is now off");
                File.WriteAllText("Configs/ignoreafk.json", JsonConvert.SerializeObject(CommandHandler.ignoreafk));
            }
            else
            {
                CommandHandler.ignoreafk.Add((Context.Guild.Id.ToString()));
                await Context.Channel.SendMessageAsync("Using ignoreafk is now on");
                File.WriteAllText("Configs/ignoreafk.json", JsonConvert.SerializeObject(CommandHandler.ignoreafk));
            }

        }
        [Command("DeleteV")]
        public async Task DeleteV()
        {
            var d = Context.Guild.TextChannels;
            foreach (var TxtChannel in d)
            {
                foreach (var VoiceChannel in Context.Guild.VoiceChannels)
                {
                    try
                    {
                        if (TxtChannel.Topic != null && TxtChannel.Topic.Contains(VoiceChannel.Id.ToString()))
                        {
                            await TxtChannel.DeleteAsync();
                            Console.WriteLine("Deleting " + TxtChannel.Name);
                        }
                    }
                    catch (Exception)
                    {
                    }


                }
                if (TxtChannel.Name.StartsWith("v_"))
                {
                    await TxtChannel.DeleteAsync();
                    Console.WriteLine("Deleting " + TxtChannel.Name);
                }
                else
                {
                    Console.WriteLine("skipping " + TxtChannel.Name);
                }


            }



        }//rebuild v_
        [Command("CreateV")]
        public async Task CreateV([Optional] ulong CatogoryID)
        {
            
            var d = Context.Guild.VoiceChannels;
            foreach (var VoiceChannel in d)
            {
                try
                {
                    if (CommandHandler.UseTopic.Exists(x => x == Context.Guild.Id.ToString()))
                    {
                        try
                        {

                            var s = await Context.Guild.CreateTextChannelAsync(Program.NameConverter(VoiceChannel.Name));
                            await s.AddPermissionOverwriteAsync(Context.Client.CurrentUser, Discord.OverwritePermissions.AllowAll(s), Discord.RequestOptions.Default);
                            await s.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, Discord.OverwritePermissions.DenyAll(s), Discord.RequestOptions.Default);
                            await s.ModifyAsync(x => x.Position = VoiceChannel.Position + 20);
                            await s.ModifyAsync(x => x.Topic = VoiceChannel.Id.ToString());
                            if (CatogoryID != 0)
                            {
                                try
                                {
                                    var category = Context.Guild.CategoryChannels.FirstOrDefault(x => x.Id == CatogoryID);
                                    await s.ModifyAsync(x => { x.CategoryId = category.Id; });
                                }
                                catch (Exception)
                                {
                                }
                                
                            }

                        }
                        catch (Exception)
                        {
                            await Context.Channel.SendMessageAsync("Creation of " + VoiceChannel.Name + " failed!");
                        }
                    }
                    else
                    {
                        try
                        {

                            var s = await Context.Guild.CreateTextChannelAsync(Program.V_NameConverter(VoiceChannel.Name));
                            await s.ModifyAsync(x => x.Position = VoiceChannel.Position + 20);
                            await s.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, Discord.OverwritePermissions.DenyAll(s), Discord.RequestOptions.Default);
                            if (CatogoryID != 0)
                            {
                                try
                                {
                                    var category = Context.Guild.CategoryChannels.FirstOrDefault(x => x.Id == CatogoryID);
                                    await s.ModifyAsync(x => { x.CategoryId = category.Id; });
                                }
                                catch (Exception)
                                {
                                }

                            }
                        }
                        catch (Exception)
                        {
                            await Context.Channel.SendMessageAsync("Creation of v_" + VoiceChannel.Name + " failed!  (Does the voice channel have any special characters?)");
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        [Command("AddNewV")]
        public async Task AddNewV([Optional] ulong CatogoryID)
        {
            var textChannels = Context.Guild.TextChannels;
            foreach (var VoiceChannel in Context.Guild.VoiceChannels)
            {
                try
                {
                    if (CommandHandler.UseTopic.Exists(x => x == Context.Guild.Id.ToString()))
                    {
                        List<ulong> CreatedChannels = new List<ulong>();
                        foreach (var textchannel in textChannels)
                        {
                            if (textchannel.Topic != null)
                            {
                                try
                                {
                                    Context.Guild.GetVoiceChannel(ulong.Parse(textchannel.Topic));
                                    CreatedChannels.Add(VoiceChannel.Id);
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                        if (!CreatedChannels.Exists(x => x == VoiceChannel.Id))
                        {
                            try
                            {
                                var s = await Context.Guild.CreateTextChannelAsync(Program.NameConverter(VoiceChannel.Name));
                                await s.AddPermissionOverwriteAsync(Context.Client.CurrentUser, Discord.OverwritePermissions.AllowAll(s), Discord.RequestOptions.Default);
                                await s.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, Discord.OverwritePermissions.DenyAll(s), Discord.RequestOptions.Default);
                                await s.ModifyAsync(x => x.Position = VoiceChannel.Position + 20);
                                await s.ModifyAsync(x => x.Topic = VoiceChannel.Id.ToString());
                                if (CatogoryID != 0)
                                {
                                    try
                                    {
                                        var category = Context.Guild.CategoryChannels.FirstOrDefault(x => x.Id == CatogoryID);
                                        await s.ModifyAsync(x => { x.CategoryId = category.Id; });
                                    }
                                    catch (Exception)
                                    {
                                    }

                                }
                            }
                            catch (Exception)
                            {
                                await Context.Channel.SendMessageAsync("Creation of " + VoiceChannel.Name + " failed!");
                            }
                        }
                    }
                    else
                    {
                        List<ulong> CreatedChannels = new List<ulong>();
                        foreach (var textchannel in textChannels)
                        {
                            if (textchannel.Name == Program.V_NameConverter(VoiceChannel.Name))
                            {
                                    CreatedChannels.Add(VoiceChannel.Id);
                           
                            }
                        }
                        if (!CreatedChannels.Exists(x => x == VoiceChannel.Id))
                        {
                            try
                            {

                                var s = await Context.Guild.CreateTextChannelAsync(Program.V_NameConverter(VoiceChannel.Name));
                                await s.ModifyAsync(x => x.Position = VoiceChannel.Position + 20);
                                if (CatogoryID != 0)
                                {
                                    try
                                    {
                                        var category = Context.Guild.CategoryChannels.FirstOrDefault(x => x.Id == CatogoryID);
                                        await s.ModifyAsync(x => { x.CategoryId = category.Id; });
                                    }
                                    catch (Exception)
                                    {
                                    }

                                }
                                await s.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, Discord.OverwritePermissions.DenyAll(s), Discord.RequestOptions.Default);
                            }
                            catch (Exception)
                            {
                                await Context.Channel.SendMessageAsync("Creation of v_" + VoiceChannel.Name + " failed!  (Does the voice channel have any special characters?)");
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        [Command("VoiceMsg")]
        public async Task Voicemsg()
        {
            bool VoiceISON = false;
            if (CommandHandler.VoiceON != null)
            {
                foreach (var item in CommandHandler.VoiceON)
                {
                    if (item == Context.Guild.Id.ToString())
                    {
                        VoiceISON = true;
                    }
                }
                if (VoiceISON)
                {
                    CommandHandler.VoiceON.Remove((Context.Guild.Id.ToString()));
                    await Context.Channel.SendMessageAsync("Join/leave message is now off");
                    File.WriteAllText("Configs/VoiceMsgState.json", JsonConvert.SerializeObject(CommandHandler.VoiceON));
                }
                else if (!VoiceISON)
                {
                    CommandHandler.VoiceON.Add((Context.Guild.Id.ToString()));
                    await Context.Channel.SendMessageAsync("Join/leave message is now on");
                    File.WriteAllText("Configs/VoiceMsgState.json", JsonConvert.SerializeObject(CommandHandler.VoiceON));
                }
            }
            else
            {
                CommandHandler.VoiceON.Add((Context.Guild.Id.ToString()));
                await Context.Channel.SendMessageAsync("Join/leave message is now on");
                File.WriteAllText("Configs/VoiceMsgState.json", JsonConvert.SerializeObject(CommandHandler.VoiceON));
            }

        }
        [Command("vrefresh")]
        public async Task vrefresh()
        {
            await Context.Channel.SendMessageAsync("Starting refreshing all permission for all users!(this might take a few min)");
            await Context.Channel.TriggerTypingAsync();
            var bgw = new BackgroundWorker();
            bgw.DoWork += (_, __) =>
            {
                refresh(Context);
            };
            bgw.RunWorkerCompleted += (_, __) =>
            {
                Context.Channel.SendMessageAsync("Refreshing permission is done!");
            };
            bgw.RunWorkerAsync();
        }
        [Command("moveallto")]
        public async Task moveallto(string channel)
        {
            if (CommandHandler.AutoCreator.Exists(x => x == Context.Guild.Id.ToString()))
            {


                var AutoCreatoritem = CommandHandler.AutoCreator.Find(x => x == Context.Guild.Id.ToString());
                if (AutoCreatoritem != null)
                {
                    CommandHandler.AutoCreator.Remove(AutoCreatoritem);
                }


                var h = Context.Guild.GetUser(Context.User.Id);
                var d = Context.Guild.VoiceChannels;
                var users = await (Context.Guild as IGuild).GetUsersAsync();

                bool exist = false;

                IVoiceChannel j = null;

                foreach (var item in d)
                {
                    if (channel.ToLower().Replace(" ", "") == item.Name.ToLower().Replace(" ", ""))
                    {
                        exist = true;
                        j = item;
                    }
                }
                if (!exist)
                {
                    await Context.Channel.SendMessageAsync("Cant find channel!");
                }
                else
                {

                    foreach (var item in d)
                    {
                        if (item.Id == h.VoiceChannel.Id)
                        {
                            foreach (var user in users)
                            {
                                if (user.VoiceChannel != null)
                                {


                                    if (user.VoiceChannel.Id == item.Id)
                                    {
                                        await (user as IGuildUser)?.ModifyAsync(x =>
                                        {
                                            x.Channel = Optional.Create(j);
                                        });
                                    }
                                }
                            }
                        }

                    }




                }
                if (AutoCreatoritem != null)
                {
                    CommandHandler.AutoCreator.Add(AutoCreatoritem);
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("Please turn off autocreator to use this command!");
            }
        }
        [Command("count")]
        public async Task Count()
        {
            if ((Context.User as SocketGuildUser).VoiceChannel != null)
            {


                var h = Context.Guild.GetUser(Context.User.Id);
                var users = (h.VoiceChannel as SocketVoiceChannel).Users;
                int i = users.Count;

                await Context.Channel.SendMessageAsync("There are " + i + " people in " + h.VoiceChannel.Name);
            }
            else
            {
                await Context.Channel.SendMessageAsync("You must be in a voice channel to use this!");
            }
        }
        [Command("DefaultCategory")]
        public async Task DefaultCatogory([Optional]ulong ID)
        {
            var car = Context.Guild.CategoryChannels.ToList().Exists(x => x.Id == ID && x.Guild.Id == Context.Guild.Id);
            if (ID == 0)
            {
                if (CommandHandler.CatagoryGuilds.Exists(x => x.GuildID == Context.Guild.Id))
                {
                    var item = CommandHandler.CatagoryGuilds.Single(x => x.GuildID == Context.Guild.Id);
                    CommandHandler.CatagoryGuilds.Remove(item);
                    await Context.Channel.SendMessageAsync("Default category removed!");
                }
                else
                {
                    await Context.Channel.SendMessageAsync("There is no category set as default, use the command with a categoryID to create one.");
                }
            }
            else if (car)
            {
                var Category = Context.Guild.CategoryChannels.ToList().Single(x => x.Id == ID && x.Guild.Id == Context.Guild.Id);

                if (CommandHandler.CatagoryGuilds.Exists(x => x.GuildID == Context.Guild.Id))
                {
                    var item = CommandHandler.CatagoryGuilds.Single(x => x.GuildID == Context.Guild.Id);
                    item.CatagoryID = ID;

                }
                else
                {
                    CommandHandler.Catagory sds = new CommandHandler.Catagory();
                    sds.CatagoryID = ID;
                    sds.GuildID = Context.Guild.Id;

                    CommandHandler.CatagoryGuilds.Add(sds);
                }

        
                await Context.Channel.SendMessageAsync("Default category have been set/changed!");
                File.WriteAllText("Configs/CatagoryGuilds.json", JsonConvert.SerializeObject(CommandHandler.CatagoryGuilds));

            }
            else
            {
                await Context.Channel.SendMessageAsync("Cant find Category!, make sure the ID is correct!");
                    
            }
            
                
        }
        [Command("info")]
        public async Task Info()
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            await ReplyAsync(
                $"{Format.Bold("Info")}\n" +
                $"- Author: SindreMA#9630\n" +
                $"- Library: Discord.Net ({DiscordConfig.Version})\n" +
                $"- Runtime: {RuntimeInformation.FrameworkDescription} {RuntimeInformation.OSArchitecture}\n" +
                $"- Uptime: {GetUptime()}\n\n" +

                $"{Format.Bold("Stats")}\n" +
                $"- Heap Size: {GetHeapSize()} MB\n" +
                $"- Guilds: {(Context.Client as DiscordSocketClient).Guilds.Count}\n" +
                $"- Channels: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Channels.Count)}\n" +
                $"- Users: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Users.Count)}"
            );
        }
        [Command("setgame")]
        public async Task SetGame([Remainder]string text)
        {
            if (Context.User.Id == 170605899189190656)
            {
                await Context.Client.SetGameAsync(text);
            }

        }
        private static string GetUptime()
           => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
        private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();
        public async void refresh(SocketCommandContext e)
        {


            var AllowPerm = new OverwritePermissions(
              viewChannel: PermValue.Allow,
              readMessageHistory: PermValue.Allow,
              sendMessages: PermValue.Allow,
              attachFiles: PermValue.Allow,
              embedLinks: PermValue.Allow,
              useExternalEmojis: PermValue.Allow,
              addReactions: PermValue.Inherit,
              mentionEveryone: PermValue.Deny,
              manageMessages: PermValue.Deny,
              sendTTSMessages: PermValue.Deny,
              createInstantInvite: PermValue.Deny,
              manageChannel: PermValue.Deny,
              manageWebhooks: PermValue.Deny
              );
            var users = await (e.Guild as IGuild).GetUsersAsync();
            var textChannels = await (e.Guild as IGuild).GetTextChannelsAsync();


            foreach (var user in users)
            {
                Console.WriteLine("Starting " + user.Username);
                if (user.VoiceChannel != null)
                {
                    Console.WriteLine("User is in voice");

                    foreach (var channel in textChannels)

                    {

                        if (channel.Name.StartsWith("v_"))
                        {

                            Console.WriteLine("checking " + channel.Name);
                            try
                            {

                                if (Program.V_NameConverter(user.VoiceChannel.Name) == channel.Name)
                                {
                                    await channel.AddPermissionOverwriteAsync(user, AllowPerm);
                                }
                                else
                                {
                                    await channel.RemovePermissionOverwriteAsync(user);
                                }

                            }
                            catch (Exception)
                            {
                                Console.WriteLine("fail check");
                            }
                        }
                    }
                }
                else if (user.VoiceChannel == null)
                {
                    Console.WriteLine("User is not in voice");
                    foreach (var channel in textChannels)
                    {
                        if (channel.Name.StartsWith("v_"))
                        {
                            Console.WriteLine("checking " + channel.Name);
                            try
                            {
                                await channel.RemovePermissionOverwriteAsync(user);
                            }
                            catch (Exception)
                            {
                            }

                        }
                    }

                }
            }

            await e.Channel.SendMessageAsync("Permission to V_Channels have been refreshed!");


        }

    }
}
