using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using UtilityBot.dto;

namespace UtilityBot.dto
{
    public class PermissionAndGuildDTO
    {
        public ulong GuildID { get; set; }
        public PermissionDTO Permissions { get; set; }
    }
}
