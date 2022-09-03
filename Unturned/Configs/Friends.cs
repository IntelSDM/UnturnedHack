using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hag.Configs
{
    class Friends
    {
        public Dictionary<ulong, ulong> FriendsList = new Dictionary<ulong, ulong>();
        public bool SteamFriends = true;
        public bool GroupAsFriends = true;
    }
}
