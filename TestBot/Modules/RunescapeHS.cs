﻿using System;
using System.Net;
using System.Collections.Generic;


namespace TestBot.Modules
{
    public class SkillData
    {
        public float rank { get; private set; }
        public float level { get; private set; }
        public float xp { get; private set; }

        public SkillData(string rank, string level, string xp)
        {
            this.rank = float.Parse(rank,System.Globalization.CultureInfo.InvariantCulture);
            this.level = float.Parse(level, System.Globalization.CultureInfo.InvariantCulture); ;
            this.xp = float.Parse(xp, System.Globalization.CultureInfo.InvariantCulture); ;
        }
    }


    public class MiniData
    {
        public float rank { get; private set; }
        public float score { get; private set; }

        public MiniData(string rank, string score)
        {
            this.rank = float.Parse(rank, System.Globalization.CultureInfo.InvariantCulture); ;
            this.score = float.Parse(score, System.Globalization.CultureInfo.InvariantCulture); ;
        }
    }


    public class RunescapeHS
    {
        public readonly List<string> skills = new List<string> {
            "Overall", "Attack", "Defence", "Strength", "Hitpoints", "Ranged",
            "Prayer", "Magic", "Cooking", "Woodcutting", "Fletching", "Fishing",
            "Firemaking", "Crafting", "Smithing", "Mining", "Herblore",
            "Agility", "Thieving", "Slayer", "Farming", "Runecraft", "Hunter",
            "Construction"
        };
        public readonly List<string> minigames = new List<string> {
            "Unknown Minigame", "Bounty Hunter (Hunter)",
            "Bounty Hunter (Rogue)", "Clue Scrolls (all)", "UNKNOWN",
            "Clue Scrolls (easy)", "Clue Scrolls (medium)",
            "Clue Scrolls (hard)", "Clue Scrolls (elite)",
            "Clue Scrolls (master)", "LMS (rank)"
        };
        public readonly List<string> bossNames = new List<string>
        {
            "Abyssal Sire", "Alchemical Hydra", "Barrows Chests", "Bryophyta",
            "Chambers of Xeric", "Chambers of Xeric CM",
            "Chaos Elemental", "Chaos Fanatic", "Commander Zilyana",
            "Corporeal Beast", "Crazy Archaelogist", "Dagannoth Prime",
            "Dagannoth Rex", "Dagannoth Supreme", "Deranged Archaelogist",
            "General Graardor", "Giant Mole", "Grotesque Guardians", "Hespori",
            "Kalphite Queen", "King Black Dragon", "Kraken", "Kree'Arra",
            "K'ril Tsutsaroth", "Unknown 1", "Thermonuclear Smoke Devil",
            "Tzkal-zuk", "Unknown 2", "Skotizo", "Unknown 3", "Vet'ion",
            "Theatre of Blood", "Unknown 4", "Unknown 5", "TzTok-Jad",
            "Venenatis", "Unknown 6", "Vorkath", "Wintertodt", "Zalcano",
            "Zulrah"
        };

        private Dictionary<string, SkillData> skillData = new Dictionary<string, SkillData>();
        private Dictionary<string, MiniData> miniData = new Dictionary<string, MiniData>();
        private Dictionary<string, MiniData> bossData = new Dictionary<string, MiniData>();
        private int DISCORD_MAX_CHAR_LIMIT = 2000;

        private readonly List<string> killsPerHour = new List<string>
        {
            "0", "27", "0", "0", "3.5", "1.8", "48", "100", "25", "14", "0", "96", "96", "96", "0", "30", "90",
            "30", "0", "35", "100", "80", "28", "36", "45", "100", "10", "7.5", "0", "0", "0", "3", "110",
            "0.8", "2", "44", "30", "32", "0", "0", "35"
        };

        public RunescapeHS(string playerName)
        {
            WebClient client = new WebClient();
            string rs_url = "http://services.runescape.com/m=hiscore_oldschool/index_lite.ws?player=";
            rs_url += playerName;
            string hsData = string.Empty;
            try
            {
                hsData = client.DownloadString(rs_url);
                ParseHSData(hsData);

                /*
                string[] lines = hsData.Split('\n');
                //Boss stuff of interest starts at line 35
                string msg = "";
                int index = 35;
                foreach (string boss in bossNames)
                {
                    //kill kph ehb
                    string[] counts = lines[index].Split(',');
                    msg += boss + ", kills= " + counts[0] + " kph= " + counts[1] + "\n";
                    index++;
                }*/
            }
            catch (WebException)
            {
                hsData = string.Empty;
            }
        }

        public Dictionary<string, SkillData> GetSkillData()
        {
            return skillData;
        }

        public Dictionary<string, MiniData> GetBossData()
        {
            return bossData;
        }

        public Dictionary<string, MiniData> GetMiniGameData()
        {
            return miniData;
        }

        private void ParseHSData(string hsData)
        {
            string[] dataLines = hsData.Split('\n');
            //Boss stuff of interest starts at line 35
            string msg = "";

            int dataIndex = 0;
            foreach(string skill in skills)
            {
                string[] data = dataLines[dataIndex].Split(',');
                skillData[skill] = new SkillData(data[0], data[1], data[2]);
                dataIndex++;
            }

            foreach(string mini in minigames)
            {
                string[] data = dataLines[dataIndex].Split(',');
                miniData[mini] = new MiniData(data[0], data[1]);
                dataIndex++;
            }

            foreach(string boss in bossNames)
            {
                string[] data = dataLines[dataIndex].Split(',');
                bossData[boss] = new MiniData(data[0], data[1]);
                dataIndex++;
            }
        }

        public List<string> GetBossDataTable()
        {
            string table = "";
            int bossFieldLength = 28;  //must be even
            int numKillsLength = 9;    //must be odd
            int kphsLength = 5;        //must be odd
            int ehbLength = 9;         //must be odd

            string horizBorder = "";
            for(int i = 0; i < bossFieldLength + numKillsLength + kphsLength + ehbLength; i++)
            {
                horizBorder += '-';
            }
            table += horizBorder + "\n|";
            table += GetCentered(bossFieldLength, "Boss");
            table += GetCentered(numKillsLength, "Kills");
            table += GetCentered(kphsLength, "KPH");
            table += GetCentered(ehbLength, "EHB") + "\n";
            table += horizBorder + "\n";

            int bossCounter = 0;
            foreach (string boss in bossNames)
            {
                // Boss column (+ 1 for the leading white space)
                int whiteSpace = bossFieldLength - (boss.Length + 1);
                table += "| " + boss;
                for(int i = 0; i < whiteSpace; i++)
                {
                    table += " ";
                }
                table += "|";

                // Kills column (+1 for the trailing white space)
                MiniData entry = bossData[boss];
                whiteSpace = numKillsLength - (entry.score.ToString().Length + 1);
                for(int i = 0; i < whiteSpace; i++)
                {
                    table += " ";
                }
                table += entry.score.ToString() + " |";

                // Kills per hour (KPH), (+1 for the leading white space)
                whiteSpace = kphsLength - (killsPerHour[bossCounter].Length + 1);
                for(int i = 0; i < whiteSpace; i++)
                {
                    table += " ";
                }
                table += killsPerHour[bossCounter] + " |";

                // EHB, (+1 for the leading white space)
                float ehb = 0.0f;
                float kills = float.Parse(killsPerHour[bossCounter], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                if (kills > 0.0f && entry.score > 0.0f)
                {
                    ehb = entry.score / kills;
                }

                whiteSpace = ehbLength - (ehb.ToString().Length + 1);
                for (int i = 0; i < whiteSpace; i++)
                {
                    table += " ";
                }
                table += ehb.ToString() + " |";
                bossCounter++;
                table += "\n";
            }

            List<string> tableParts = new List<string>();
            if (table.Length > DISCORD_MAX_CHAR_LIMIT)
            {
                string[] tableLines = table.Split('\n');
                Console.Write(table);
                int lineLength = tableLines[0].Length;

                int limit = (DISCORD_MAX_CHAR_LIMIT / lineLength) - 1;
                int linesAddedCounter = 0;
                string tableLineEntry = "";
                for (int i = 0; i < tableLines.Length; i++)
                {
                    if (linesAddedCounter == DISCORD_MAX_CHAR_LIMIT)
                    {
                        linesAddedCounter = 0;
                        tableParts.Add("```" + tableLineEntry + "```");
                        tableLineEntry = "";
                    }

                    tableLineEntry += tableLines[i] + "\n";
                    linesAddedCounter++;
                }

            }
            else
            {
                tableParts.Add("```" + table + "```");
            }

            return tableParts;
        }

        private string GetCentered(int length, string name)
        {
            string padding = "";
            int nameLength = name.Length;
            float center = length / 2.0f - (nameLength / 2.0f);

            for(int i = 0; i < center; i++)
            {
                padding += " ";
            }

            return padding + name + padding + "|";
        }
    }
}