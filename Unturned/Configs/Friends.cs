using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hag.Configs
{
    class Friends
    {
        public Dictionary<ulong, string> FriendsList = new Dictionary<ulong, string>();
        public bool SteamFriends = true;
        public bool GroupAsFriends = true;
    }
}
