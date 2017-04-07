using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;

namespace Assets.script.common
{
    class App
    {
        public static string serverPath = "http://localhost:8080/";
        public static string TurnState;
        public static string operateEmail;
        public static List<string> selectList = new List<string>();
        public static int selectLimit;
        public static bool isOfferState = false;
        public static int handIdx;
        public static int monsterStatus;
        public static int gameResult;// 0 : win , 1 : lose

        static App()
        {
            try
            {
                StreamReader sr = new StreamReader("serverPath.conf", Encoding.UTF8);
                String line;
                if ((line = sr.ReadLine()) != null)
                {
                    serverPath = line;
                }
                sr.Close();
            } catch(Exception e)
            {
                Debug.Log(e.StackTrace);
            }
        }
    }
}
