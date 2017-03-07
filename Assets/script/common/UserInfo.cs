using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.script.common
{
    class UserInfo
    {
        static public bool isLogined = false;
        static public string token = null;
        static public string email = "";
        static public int joinedRoom = -1;
    }
}
