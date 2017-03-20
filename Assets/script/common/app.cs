using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.script.common
{
    class App
    {
        public static string serverPath = "http://127.0.0.1:8080/";
        public static string TurnState;
        public static string operateEmail;
        public static List<string> selectList = new List<string>();
        public static int selectLimit;
        public static bool isOfferState = false;
        public static int handIdx;
        public static int monsterStatus;
    }
}
