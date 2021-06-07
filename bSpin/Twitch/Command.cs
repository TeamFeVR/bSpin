using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatCore;
using ChatCore.Interfaces;
using UnityEngine;
using bSpin.CustomTypes;

namespace bSpin.Twitch
{
    static class Types
    {
        public enum Perms
        {
            Broadcaster,
            Moderator,
            VIP,
            Subscriber,
            Everyone
        }

        public struct Command
        {
            //ex: wadmin
            public string Trigger;
            //ex: wobble
            public string Sub;
            //if it has a sub, example being wobble vs wadmin wobble
            public bool HasSub;
            //ex: webcrawler
            public string Args;
            //ex: mod
            public List<Perms> Perms;
        }
        public static bool IsFirstWord(this string cmd, string word)
        {
            var cmd2 = cmd.Substring(1);
            var a = cmd2.Split(' ');
            return a[0].ToLower().Equals(word);
        }

        public static string ParseWadmin(this string command)
        {
            if (!command.IsFirstWord("wadmin"))
                return "";

            var cmd = command.Substring("!wadmin ".Length);
            var a = cmd.Split(' ');

            if (a[1].ToLower().Equals("preset"))
            {
                if (a[0].ToLower().Equals("add"))
                {
                    Plugin.wobbles.Add(new CustomTypes.WobbleProfile(
                        a[2],
                        new List<CustomTypes.Wobble>
                        {
                            new CustomTypes.Wobble(1, new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0))
                        }
                        ));
                }
                else if (a[0].ToLower().Equals("set"))
                {
                    Plugin.Log.Notice("Set");
                    for (int i = 0; i < Plugin.wobbles.Count; i++)
                    {
                        if (Plugin.wobbles[i].name.ToLower().Equals(a[2].ToLower()))
                        {
                            WobbleProfile tmp = Plugin.wobbles[i];
                            Wobble tmpw;
                            Vector3 input;
                            Wobble w = new Wobble();
                            Plugin.Log.Notice("yes");

                            if (tmp.Wobbles.Count() < Int32.Parse(a[3]))
                                tmp.Wobbles.Add(new Wobble(1, new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0)));
                            Plugin.Log.Notice($"{tmp.Wobbles.Count()} wobble movements");
                            switch (a[4].ToLower())
                            {
                                case "start_rotation":
                                    input = new Vector3(float.Parse(a[5]), float.Parse(a[6]), float.Parse(a[7]));
                                    tmpw = tmp.Wobbles.ElementAt(Int32.Parse(a[3]) - 1);
                                    w = new Wobble(tmpw.Length, input, tmpw.End_Rot, tmpw.Begin_Pos, tmpw.End_Pos, tmpw.Easing);
                                    break;
                                case "end_rotation":
                                    input = new Vector3(float.Parse(a[5]), float.Parse(a[6]), float.Parse(a[7]));
                                    tmpw = tmp.Wobbles.ElementAt(Int32.Parse(a[3]) - 1);
                                    w = new Wobble(tmpw.Length, tmpw.Begin_Rot, input, tmpw.Begin_Pos, tmpw.End_Pos, tmpw.Easing);
                                    break;
                                case "start_position":
                                    input = new Vector3(float.Parse(a[5]), float.Parse(a[6]), float.Parse(a[7]));
                                    tmpw = tmp.Wobbles.ElementAt(Int32.Parse(a[3]) - 1);
                                    w = new Wobble(tmpw.Length, tmpw.Begin_Rot, tmpw.End_Rot, input, tmpw.End_Pos, tmpw.Easing);
                                    break;
                                case "end_position":
                                    input = new Vector3(float.Parse(a[5]), float.Parse(a[6]), float.Parse(a[7]));
                                    tmpw = tmp.Wobbles.ElementAt(Int32.Parse(a[3]) - 1);
                                    w = new Wobble(tmpw.Length, tmpw.Begin_Rot, tmpw.End_Rot, tmpw.Begin_Pos, input, tmpw.Easing);
                                    break;
                                case "length":
                                    tmpw = tmp.Wobbles.ElementAt(Int32.Parse(a[3]) - 1);
                                    w = new Wobble(float.Parse(a[5]), tmpw.Begin_Rot, tmpw.End_Rot, tmpw.Begin_Pos, tmpw.End_Pos, tmpw.Easing);
                                    break;
                                case "easing":
                                    tmpw = tmp.Wobbles.ElementAt(Int32.Parse(a[3]) - 1);
                                    Enum.TryParse<EasingFunction.Ease>(a[5], out var easin);
                                    w = new Wobble(tmpw.Length, tmpw.Begin_Rot, tmpw.End_Rot, tmpw.Begin_Pos, tmpw.End_Pos, easin);
                                    break;
                            }
                            tmp.Wobbles.RemoveAt(Int32.Parse(a[3]) - 1);
                            tmp.Wobbles.Insert(Int32.Parse(a[3]) - 1, w);
                            Plugin.wobbles.RemoveAt(i);
                            Plugin.wobbles.Insert(i, tmp);
                            FileManager.SaveWobbleProfile(Plugin.wobbles[i]);
                        }
                    }
                }
                else if (a[0].ToLower().Equals("stats"))
                {
                    for (int i = 0; i < Plugin.wobbles.Count; i++)
                        if (Plugin.wobbles[i].name.ToLower().Equals(a[2].ToLower()))
                        {
                            WobbleProfile tmp = Plugin.wobbles[i];
                            Plugin.Log.Notice("yes");
                            string movements = tmp.Wobbles.Count().ToString() + $" movement{(tmp.Wobbles.Count().Equals(1) ? "," : "s,")}";
                            string length = "";
                            float seconds = 0;
                            foreach (var mvm in tmp.Wobbles)
                                seconds += mvm.Length;
                            length = seconds.ToString();
                            return $"Name: {tmp.name}, {movements} over the course of {length} seconds.";
                        }
                }
            }


            return "";
        }

        public static string FirstWord(this string all)
        {
            var cmd = all.Substring(1);
            var a = cmd.Split(' ');
            return a[0];
        }

        public static Command ToCommand(this IChatMessage msg)
        {
            string cmd = msg.Message.ToLower();

            cmd = cmd.Substring(cmd.IndexOf("!") + 1);
            string trigger = cmd.Substring(0, cmd.IndexOf(" "));
            bool sub = false;
            foreach(var comd in CommandHandler.Commands)
                if (comd.Trigger.ToLower().Equals(trigger))
                    sub = comd.HasSub;

            int splits = sub ? 2 : 3;
            var splitd = cmd.Split(' ');

            var rtn = new Command();
            rtn.Trigger = splitd[0];
            rtn.Sub = splitd[1];
            rtn.Args = splitd[2];
            return rtn;
        }


    }
}
