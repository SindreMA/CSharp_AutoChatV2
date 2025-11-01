using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using Discord;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using static CSharp_AutoChatV2.Modules.CommandPermissionHandler;

namespace CSharp_AutoChatV2.Modules
{
    public class CommandPermissionHandler
    {
        static public List<PermissionClass> Permissions = new List<PermissionClass>();

        public class PermissionClass
        {
            public ulong GuildID { get; set; }
            public ulong PermissionID { get; set; }
        }
        static public bool IsAllowedToUseCommand(SocketCommandContext Context)
        {
            LoadList();
            GuildPermission Permission = GetPermission(0);

            if (Permissions.Exists(x => x.GuildID == Context.Guild.Id))
            {
                var perm = Permissions.Single(x => x.GuildID == Context.Guild.Id);
                Permission = GetPermission(perm.PermissionID);
            }
            if (UserPermissionCheck(Context.Guild.Users.Single(x => x.Id == Context.User.Id).GuildPermissions, Permission))
            {
                return true;
            }
            return false;
        }

        private static void LoadList()
        {
            try
            {
                string json = File.ReadAllText("Configs/Permissions.json");
                Permissions = JsonConvert.DeserializeObject<List<PermissionClass>>(json);
            }
            catch (Exception)
            {
            }
        }

        static public string PermissionNeed(ulong GuildID)
        {
            GuildPermission Permission = GetPermission(0);

            if (Permissions.Exists(x => x.GuildID == GuildID))
            {
                var perm = Permissions.Single(x => x.GuildID == GuildID);
                Permission = GetPermission(perm.PermissionID);
            }
            return Permission.ToString();

        }

        static public GuildPermission GetPermission(ulong NR)
        {
            if (NR == 2)
            {
                return GuildPermission.KickMembers;
            }
            else if (NR == 1)
            {
                return GuildPermission.CreateInstantInvite;
            }
            else if (NR == 4)
            {
                return GuildPermission.BanMembers;
            }
            else if (NR == 8)
            {
                return GuildPermission.Administrator;
            }
            else if (NR == 16)
            {
                return GuildPermission.ManageChannels;
            }
            else if (NR == 32)
            {
                return GuildPermission.ManageGuild;
            }
            else if (NR == 64)
            {
                return GuildPermission.AddReactions;
            }
            else if (NR == 128)
            {
                return GuildPermission.ViewAuditLog;
            }
            else if (NR == 1024)
            {
                return GuildPermission.ViewChannel;
            }
            else if (NR == 2048)
            {
                return GuildPermission.SendMessages;
            }
            else if (NR == 4096)
            {
                return GuildPermission.SendTTSMessages;
            }
            else if (NR == 8192)
            {
                return GuildPermission.ManageMessages;
            }
            else if (NR == 16384)
            {
                return GuildPermission.EmbedLinks;
            }
            else if (NR == 32768)
            {
                return GuildPermission.AttachFiles;
            }
            else if (NR == 65536)
            {
                return GuildPermission.ReadMessageHistory;
            }
            else if (NR == 131072)
            {
                return GuildPermission.MentionEveryone;
            }
            else if (NR == 262144)
            {
                return GuildPermission.UseExternalEmojis;
            }
            else if (NR == 1048576)
            {
                return GuildPermission.Connect;
            }
            else if (NR == 2097152)
            {
                return GuildPermission.Speak;
            }
            else if (NR == 4194304)
            {
                return GuildPermission.MuteMembers;
            }
            else if (NR == 8388608)
            {
                return GuildPermission.DeafenMembers;
            }
            else if (NR == 16777216)
            {
                return GuildPermission.MoveMembers;
            }
            else if (NR == 33554432)
            {
                return GuildPermission.UseVAD;
            }
            else if (NR == 67108864)
            {
                return GuildPermission.ChangeNickname;
            }
            else if (NR == 134217728)
            {
                return GuildPermission.ManageNicknames;
            }
            else if (NR == 268435456)
            {
                return GuildPermission.ManageRoles;
            }
            else if (NR == 536870912)
            {
                return GuildPermission.ManageWebhooks;
            }
            else if (NR == 1073741824)
            {
                return GuildPermission.ManageEmojisAndStickers;
            }

            else
            {
                return GuildPermission.KickMembers;
            }

        }
        static public bool UserPermissionCheck(GuildPermissions userPermissions, GuildPermission permission)
        {
            if (permission == GuildPermission.AddReactions && userPermissions.AddReactions)
            {
                return true;
            }
            else if (permission == GuildPermission.CreateInstantInvite && userPermissions.CreateInstantInvite)
            {
                return true;
            }
            else if (permission == GuildPermission.KickMembers && userPermissions.KickMembers)
            {
                return true;
            }
            else if (permission == GuildPermission.BanMembers && userPermissions.BanMembers)
            {
                return true;
            }
            else if (permission == GuildPermission.Administrator && userPermissions.Administrator)
            {
                return true;
            }
            else if (permission == GuildPermission.ManageChannels && userPermissions.ManageChannels)
            {
                return true;
            }
            else if (permission == GuildPermission.ManageGuild && userPermissions.ManageGuild)
            {
                return true;
            }
            else if (permission == GuildPermission.ViewAuditLog && userPermissions.ViewAuditLog)
            {
                return true;
            }
            else if (permission == GuildPermission.ViewChannel && userPermissions.ViewChannel)
            {
                return true;
            }
            else if (permission == GuildPermission.SendMessages && userPermissions.SendMessages)
            {
                return true;
            }
            else if (permission == GuildPermission.SendTTSMessages && userPermissions.SendTTSMessages)
            {
                return true;
            }
            else if (permission == GuildPermission.ManageMessages && userPermissions.ManageMessages)
            {
                return true;
            }
            else if (permission == GuildPermission.EmbedLinks && userPermissions.EmbedLinks)
            {
                return true;
            }
            else if (permission == GuildPermission.AttachFiles && userPermissions.AttachFiles)
            {
                return true;
            }
            else if (permission == GuildPermission.ReadMessageHistory && userPermissions.ReadMessageHistory)
            {
                return true;
            }
            else if (permission == GuildPermission.UseExternalEmojis && userPermissions.UseExternalEmojis)
            {
                return true;
            }
            else if (permission == GuildPermission.MentionEveryone && userPermissions.MentionEveryone)
            {
                return true;
            }
            else if (permission == GuildPermission.Connect && userPermissions.Connect)
            {
                return true;
            }
            else if (permission == GuildPermission.Speak && userPermissions.Speak)
            {
                return true;
            }
            else if (permission == GuildPermission.MuteMembers && userPermissions.MuteMembers)
            {
                return true;
            }
            else if (permission == GuildPermission.DeafenMembers && userPermissions.DeafenMembers)
            {
                return true;
            }
            else if (permission == GuildPermission.MoveMembers && userPermissions.MoveMembers)
            {
                return true;
            }
            else if (permission == GuildPermission.UseVAD && userPermissions.UseVAD)
            {
                return true;
            }
            else if (permission == GuildPermission.ChangeNickname && userPermissions.ChangeNickname)
            {
                return true;
            }
            else if (permission == GuildPermission.ManageRoles && userPermissions.ManageRoles)
            {
                return true;
            }
            else if (permission == GuildPermission.ManageWebhooks && userPermissions.ManageWebhooks)
            {
                return true;
            }
            else if (permission == GuildPermission.ManageEmojisAndStickers && userPermissions.ManageEmojisAndStickers)
            {
                return true;
            }
            return false;
        }
    }
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("ShowCommandPermission")]
        public async Task ShowCommandPermission()
        {
            string perms = "CreateInstantInvite = 1\n" +
"KickMembers = 2\n" +
"BanMembers = 4\n" +
"Administrator = 8\n" +
"ManageChannels = 16\n" +
"ManageGuild = 32\n" +
"AddReactions = 64\n" +
"ViewAuditLog = 128\n" +
"ReadMessages = 1024\n" +
"SendMessages = 2048\n" +
"SendTTSMessages = 4096\n" +
"ManageMessages = 8192\n" +
"EmbedLinks = 16384\n" +
"AttachFiles = 32768\n" +
"ReadMessageHistory = 65536\n" +
"MentionEveryone = 131072\n" +
"UseExternalEmojis = 262144\n" +
"Connect = 1048576\n" +
"Speak = 2097152\n" +
"MuteMembers = 4194304\n" +
"DeafenMembers = 8388608\n" +
"MoveMembers = 16777216\n" +
"UseVAD = 33554432\n" +
"ChangeNickname = 67108864\n" +
"ManageNicknames = 134217728\n" +
"ManageRoles = 268435456\n" +
"ManageWebhooks = 536870912\n" +
"ManageEmojis = 1073741824";
            
            await Context.Channel.SendMessageAsync("Here are the permissions id's:\n```"+ perms + "```\n" + "Currently active permission is: " + CommandPermissionHandler.PermissionNeed(Context.Guild.Id));

        }
        [Command("SetCommandPermission")]
        public async Task SetCommandPermission(ulong Id)
        {
            
            if (CommandPermissionHandler.Permissions.Exists(x => x.GuildID == Context.Guild.Id))
            {
                var perm = CommandPermissionHandler.Permissions.Single(x => x.GuildID == Context.Guild.Id);
                perm.PermissionID = Id;
                await Context.Channel.SendMessageAsync($@"You now need the {CommandPermissionHandler.PermissionNeed(Context.Guild.Id)} to use commands!");
            }
            else
            {

                PermissionClass perm = new PermissionClass();
                perm.GuildID = Context.Guild.Id;
                perm.PermissionID = Id;
                CommandPermissionHandler.Permissions.Add(perm);
                await Context.Channel.SendMessageAsync($@"You now need the {CommandPermissionHandler.PermissionNeed(Context.Guild.Id)} to use commands!");
            }
            

        }
    }
}