using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityBot.dto
{
    public class PermissionDTO
    {
        
        public bool ReadMessages { get; set; }
        public bool readMessageHistory { get; set; }
        public bool sendMessages { get; set; }
        public bool attachFiles { get; set; }
        public bool embedLinks { get; set; }
        public bool useExternalEmojis { get; set; }
        public bool addReactions { get; set; }
        public bool mentionEveryone { get; set; }
        public bool manageMessages { get; set; }
        public bool sendTTSMessages { get; set; }
        public bool managePermissions { get; set; }
        public bool createInstantInvite { get; set; }
        public bool manageChannel { get; set; }
        public bool manageWebhooks { get; set; }
    }
}
