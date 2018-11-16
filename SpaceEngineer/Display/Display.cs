#if DEBUG
using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using VRageMath;
using VRage.Game;
using VRage.Collections;
using Sandbox.ModAPI.Ingame;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Game.EntityComponents;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;

namespace SpaceEngineer.Display
{
    public sealed class Program : MyGridProgram
    {
#endif
        //============================
        // Copy from here
        //============================
        public List<string> Ores = new List<string>()
        {
            "Iron",
            "Nickel",
            "Cobalt",
            "Magnesium",
            "Silicon",
            "Silver",
            "Gold",
            "Platinum",
            "Uranium",
            "Ice",
            "Stone"
        };
        public Dictionary<string, Group> Groups = new Dictionary<string, Group>();

        public List<Screen> Screens = new List<Screen>();

        public System.Text.RegularExpressions.Regex rgCmd =
            new System.Text.RegularExpressions.Regex(@"\$([a-zA-Z]+)(\[(.+)\])?",
                System.Text.RegularExpressions.RegexOptions.Compiled |
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        public System.Text.RegularExpressions.Regex rgArgs =
            new System.Text.RegularExpressions.Regex(@"((\w+)=((\w+)|""((\w*\s*)+)""))|(\w+)",
                System.Text.RegularExpressions.RegexOptions.Compiled |
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        public class Group
        {
            public string Name;
            public List<Screen> Screens = new List<Screen>();
        }

        public class Screen
        {
            public IMyTextPanel TextPanel;
            public Page CurrentPage;
            public Dictionary<string, Page> Pages = new Dictionary<string, Page>();

            public Screen(IMyTextPanel o)
            {
                TextPanel = o;
                ParseScreen();
            }

            public void ParseScreen()
            {
                var page = new Page();
                var currentPageName = "0";
                var lines = TextPanel.CustomData.Split('\n');
                for (var i = 0; i < lines.Length; i++)
                {
                    if (lines[i].StartsWith("$page"))
                    {
                        Pages.Add(currentPageName, page);
                        currentPageName = lines[i].Split(' ')[1];
                        page = new Page();
                    }
                    else
                        page.Content += lines[i] + '\n';
                }
                Pages.Add(currentPageName, page);
                CurrentPage = Pages.First().Value;
            }
        }

        public class Page
        {
            public string Content = "";
        }

        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
            Reload();
        }

        void Main(string argument, UpdateType updateSource)
        {
            if (argument != null)
            {
                // reload
                if (argument.Equals("reload"))
                {
                    Reload();
                }
                else
                {
                    var args = argument.Split(' ');
                    for (var i = 0; i < args.Length; i++)
                    {
                        var s = args[i].Split('=');
                        if (s[0].Equals("group"))
                        {
                            var groupName = s[1];
                            Echo("ask group " + groupName);
                            i++;
                            var ss = args[i].Split('=');
                            if (ss[0].Equals("page"))
                            {
                                Group g = null;
                                if (Groups.TryGetValue(groupName, out g))
                                {
                                    Echo("ask page " + ss[1]);
                                    for (var j = 0; j < g.Screens.Count; j++)
                                    {
                                        Page p = null;
                                        if (g.Screens[j].Pages.TryGetValue(ss[1], out p))
                                        {
                                            if (p != null)
                                                g.Screens[j].CurrentPage = p;
                                        }
                                    }
                                }
                            }
                        }
                        if (s[0].Equals("screen"))
                        {
                            i++;
                            var ss = args[i].Split('=');
                            if (ss[0].Equals("page"))
                            {
                                for (var j = 0; j < Screens.Count; j++)
                                {
                                    var p = Screens[j].Pages[ss[1]];
                                    if (p != null)
                                        Screens[j].CurrentPage = p;
                                }
                            }
                        }
                    }
                }
            }
            UpdateScreens();
        }

        public void Reload()
        {
            Echo("Reloading...");
            // Clear
            Groups = new Dictionary<string, Group>();
            Screens = new List<Screen>();
            // Check Groups
            var blockGroups = new List<IMyBlockGroup>();
            GridTerminalSystem.GetBlockGroups(blockGroups);
            for (var i = 0; i < blockGroups.Count; i++)
            {
                var screens = new List<IMyTextPanel>();
                blockGroups[i].GetBlocksOfType(screens);
                var group = new Group();
                if (screens.Count > 0)
                {
                    group.Name = blockGroups[i].Name;
                    Groups[group.Name] = group;
                }
                for (var j = 0; j < screens.Count; j++)
                {
                    group.Screens.Add(new Screen(screens[j]));
                }
            }
            // check screens
            var lcds = new List<IMyTextPanel>();
            GridTerminalSystem.GetBlocksOfType(lcds);
            for (var i = 0; i < lcds.Count; i++)
            {
                if (!GroupsContains(lcds[i]))
                {
                    Screens.Add(new Screen(lcds[i]));
                }
            }
            // debug
            Echo("Groups : ");
            var l = Groups.Keys.ToList();
            for (var i = 0; i < l.Count; i++)
            {
                Echo(l[i]);
            }
            Echo("Screens : ");
            for (var i = 0; i < Screens.Count; i++)
            {
                Echo(Screens[i].TextPanel.CustomName);
            }
        }

        private void UpdateScreens()
        {
            var lcd = Screens.Concat(Groups.SelectMany(g => g.Value.Screens)).Where(s => s.CurrentPage != null)
                .ToList();
            for (var i = 0; i < lcd.Count; i++)
            {
                var s = "";
                Echo("LCD: " + lcd[i].TextPanel.CustomName);
                if (lcd[i].CurrentPage.Content == null) continue;
                var lines = lcd[i]?.CurrentPage.Content.Split('\n');

                for (var j = 0; j < lines.Length; j++)
                {
                    var ignoreLine = false;
                    // Parse commands
                    var cMatches = rgCmd.Matches(lines[j]);
                    foreach (System.Text.RegularExpressions.Match cMatch in cMatches)
                    {
                        var cGroup = cMatch.Groups;
                        var cmd = cGroup[1].Value;
                        //parse args
                        var aMatches = rgArgs.Matches(cGroup[2].Value);
                        var args = new Dictionary<string, string>();
                        foreach (System.Text.RegularExpressions.Match aMatch in aMatches)
                        {
                            var k = aMatch.Groups[2].Value;
                            var v = aMatch.Groups[4].Value;
                            if (v.Equals(""))
                            {
                                v = aMatch.Groups[5].Value;
                            }
                            if (v.Equals(""))
                            {
                                k = aMatch.Groups[7].Value;
                                v = "True";
                            }
                            args.Add(k, v);
                            Echo("kv: " + k + "=>" + v);
                        }
                        // test commands
                        if (cmd.Equals("count"))
                        {
                            string itemName;
                            if (!args.TryGetValue("item", out itemName))
                            {
                                lines[j] = lines[j].Replace(cGroup[0].Value, "#$count need 'item' argument#");
                            }
                            else
                            {
                                var v = CountItems(itemName);
                                if (args.ContainsKey("ignoreIfEmpty") && v == 0)
                                    ignoreLine = true;
                                lines[j] = lines[j].Replace(cGroup[0].Value, v + "");
                            }
                        }
                        else if (cmd.Equals("progress"))
                        {
                            var ent = new List<IMyTerminalBlock>();
                            string group;
                            string name;
                            if (args.TryGetValue("group", out group))
                            {
                                GridTerminalSystem.GetBlockGroupWithName(group).GetBlocks(ent);
                                ent = ent.OrderBy(e => e.CustomName).ToList();
                            }
                            else if (args.TryGetValue("name", out name))
                            {
                                ent.Add(GridTerminalSystem.GetBlockWithName(name));
                            }
                            else
                            {
                                lines[j] = lines[j].Replace(cGroup[0].Value,
                                    " #$percent need argument 'group' or 'name'# ");
                                continue;
                            }
                            // get options [length, text, bar, full, printName]
                            bool printName = Args(args, "printname", false);
                            var displayPercent = Args(args, "percent", false);
                            var displayBar = Args(args, "bar", true);
                            if (Args(args, "full", false))
                            {
                                displayBar = true;
                                displayPercent = true;
                            }
                            var length = Args(args, "length", 75);

                            var content = "";
                            // apply
                            foreach (var c in ent)
                            {
                                if (c == null) continue;
                                if (c is IMyProductionBlock)
                                {
                                    var p0 = PercentFilled(c);
                                    var p1 = PercentFilled(c, 1);
                                    var p0b = ProgressBar(p0, 100, displayPercent, length);
                                    var p1b = ProgressBar(p1, 100, displayPercent, length);

                                    if (displayBar && displayPercent)
                                    {
                                        content += (printName ? c.CustomName + '\n' : "") +
                                                   p0b + new string(' ', (75 + 5 - p0b.Length) * 2) + " => " + p1b +
                                                   "\n";
                                    }
                                    else if (displayBar)
                                    {
                                        content += (printName ? c.CustomName + '\n' : "") + p0b + " => " + p1b +
                                                   "\n";
                                    }
                                    else
                                    {
                                        content += (printName ? c.CustomName + '\n' : "") + p0 + " => " + p1 + "\n";
                                    }
                                }
                                else
                                {
                                    var p0 = PercentFilled(c);
                                    var p0b = ProgressBar(p0, 100, displayPercent, length);

                                    if (displayBar)
                                    {
                                        content += (printName ? c.CustomName + '\n' : "") + p0b + "\n";
                                    }
                                    else
                                    {
                                        content += (printName ? c.CustomName + '\n' : "") + p0 + "\n";
                                    }
                                }
                            }
                            lines[j] = lines[j].Replace(cGroup[0].Value, content.Substring(0, content.Length - 1));
                        }
                        else if (cmd.Equals("GlobalJumpDrive"))
                        {
                            var jds = new List<IMyJumpDrive>();
                            GridTerminalSystem.GetBlocksOfType(jds, z => true);
                            int load = (int) jds.Sum(z => z.CurrentStoredPower);
                            int max = (int) jds.Sum(z => z.MaxStoredPower);
                            lines[j] = lines[j].Replace(cGroup[0].Value, ProgressBar(load, max, true));
                        }
                        else if (cmd.Equals("pos"))
                        {
                            var cockpits = new List<IMyCockpit>();
                            GridTerminalSystem.GetBlocksOfType(cockpits);
                            s += cockpits[0].CubeGrid.GetPosition() + "\n";
                        }
                        else if (cmd.Equals("ores"))
                        {
                            string containerName;
                            string groupName;
                            var containers = new List<IMyTerminalBlock>();
                            if (args.TryGetValue("group", out groupName))
                            {
                                var list = new List<IMyTerminalBlock>();
                                GridTerminalSystem.GetBlockGroupWithName(groupName).GetBlocks(list);
                                containers.AddList(list);
                            }
                            else if (args.TryGetValue("container", out containerName))
                                containers.Add(GridTerminalSystem.GetBlockWithName(containerName));
                            else
                                GridTerminalSystem.GetBlocksOfType(containers);

                            var o = "";
                            var ignoreIfEmpty = Args(args, "ignoreIfEmpty", false);
                            foreach (var ore in Ores)
                            {
                                var v = CountItems(ore + " ore");
                                if (!(ignoreIfEmpty && v == 0))
                                    o += ore + ": " + FormatNumber(v / 1000f) + " K\n";
                            }
                            lines[j] = lines[j].Replace(cGroup[0].Value, o.Substring(0, o.Length - 1));
                        }
                        else if (cmd.Equals("ingots"))
                        {
                            string containerName;
                            string groupName;
                            var containers = new List<IMyTerminalBlock>();
                            if (args.TryGetValue("group", out groupName))
                            {
                                var list = new List<IMyTerminalBlock>();
                                GridTerminalSystem.GetBlockGroupWithName(groupName).GetBlocks(list);
                                containers.AddList(list);
                            }
                            else if (args.TryGetValue("container", out containerName))
                                containers.Add(GridTerminalSystem.GetBlockWithName(containerName));
                            else
                                GridTerminalSystem.GetBlocksOfType(containers);

                            var o = "";
                            var ignoreIfEmpty = Args(args, "ignoreIfEmpty", false);
                            foreach (var ore in Ores)
                            {
                                if (ore == "Ice") continue;
                                var v = CountItems(ore + " ingot");
                                if (!(ignoreIfEmpty && v == 0))
                                    o += ore + ": " + FormatNumber(v / 1000f) + " K\n";
                            }
                            lines[j] = lines[j].Replace(cGroup[0].Value, o.Substring(0, o.Length - 1));
                        }
                        else
                        {
                            //unknown command
                        }
                    }
                    if (!ignoreLine)
                        s += lines[j] + "\n";
                }
                lcd[i].TextPanel.WritePublicText(s);
            }
        }

        public float CountItems(string itemName, IMyEntity container = null)
        {
            long sum = 0;
            itemName = itemName.Replace("5.56", "5p56"); //patchmaaaan !
            var nameParts = new List<string>(itemName.Split('_'))
                .SelectMany(p => p.Split(' '))
                .Select(namePart => namePart.ToLower())
                .ToList();
            var containers = new List<IMyEntity>();
            if (container != null)
                containers.Add(container);
            else
                GridTerminalSystem.GetBlocksOfType(containers);
            for (var i = 0; i < containers.Count; i++)
            {
                for (int j = 0; j < containers[i].InventoryCount; j++)
                {
                    sum += containers[i].GetInventory(j).GetItems()
                        .Where(item =>
                            nameParts.All(namePart => item.GetDefinitionId().ToString().ToLower().Contains(namePart)))
                        .Sum(item => item.Amount.RawValue);
                }
            }
            return (float) Math.Round(sum / 1000000f, 2);
        }

        public bool GroupsContains(IMyTextPanel panel)
        {
            foreach (var pair in Groups)
            {
                for (var j = 0; j < pair.Value.Screens.Count; j++)
                {
                    if (pair.Value.Screens[j].TextPanel == panel)
                        return true;
                }
            }
            return false;
        }

        public string ProgressBar(float current, float max, bool detailed = false, int displaySize = 75)
        {
            var ratio = displaySize / max;
            var fcurrent = current * ratio;
            var s = "";
            for (var i = 0; i < displaySize; i++)
            {
                if (i < fcurrent)
                    s += "|";
                else
                    s += "'";
            }
            if (detailed)
                s += " " + current / max * 100;
            return s;
        }

        int PercentFilled(IMyEntity c, int invNum = 0)
        {
            if (c is IMyJumpDrive)
            {
                var j = (IMyJumpDrive) c;
                return (int) (j.CurrentStoredPower / j.MaxStoredPower * 100);
            }
            if (c is IMyGasTank)
            {
                var j = (IMyGasTank) c;
                return (int) (j.FilledRatio * 100f);
            }
            return (int) ((float) c.GetInventory(invNum).CurrentVolume / (float) c.GetInventory(invNum).MaxVolume *
                          100);
        }

        public static TSource Args<TSource>(Dictionary<string, string> args, string key,
            TSource defaultValue = default(TSource))
        {
            string sv;
            if (args.TryGetValue(key, out sv))
            {
                if (sv.ToLower().Equals("true"))
                {
                    return (TSource) Convert.ChangeType(true, typeof(TSource));
                }
                return (TSource) Convert.ChangeType(sv, typeof(TSource));
            }
            return defaultValue;
        }

        public string FormatNumber(float value)
        {
            return value.ToString("#,0.00", System.Globalization.CultureInfo.InvariantCulture);
        }


#if DEBUG
    }
}
#endif