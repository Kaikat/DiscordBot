using System;
using System.Net;
using System.Collections.Generic;

namespace RavenBot.Modules
{
    public class Player
    {
        public string Name { get; private set; }
        public bool Valid { get; private set; }
        private string HSData;

        public Player(string name)
        {
            Name = name;
            WebClient client = new WebClient();
            string rs_url = "http://services.runescape.com/m=hiscore_oldschool/index_lite.ws?player=";
            rs_url += name;
            string hsData = string.Empty;
            try
            {
                HSData = client.DownloadString(rs_url);
            }
            catch (WebException)
            {
                HSData = string.Empty;
            }

            Valid = HSData != string.Empty;
        }

        public List<BossKill> GetBossKills()
        {
            List<BossKill> bossKills = new List<BossKill>();
            string[] dataLines = HSData.Split('\n');
            int bossIndex = 35;

            BossCollection bossCollection = BossCollection.Instance;
            
            for(int i = 0; i < bossCollection.Bosses.Count; i++)
            {
                if(dataLines[bossIndex] == string.Empty)
                {
                    Console.WriteLine("ERROR: The data from RS must have been updated; the bossIndex is no longer correct");
                }
                string[] data = dataLines[bossIndex].Split(',');
                BossKill kill = new BossKill(bossCollection.Bosses[i], data[0], data[1]);
                bossKills.Add(kill);
                bossIndex++;
            }

            return bossKills;
        }

        public List<string> GetBossDataTableParts()
        {
            List<BossKill> kills = this.GetBossKills();
            List<List<string>> tableContents = new List<List<string>>();
            foreach (BossKill kill in kills)
            {
                tableContents.Add(new List<string>
                {
                    kill.Boss.Name,
                    kill.Kills.ToString(),
                    kill.Boss.KPH.ToString(),
                    kill.GetEHB().ToString("n2")
                });
            }

            /*tableContents.Add(new List<string>
            {
                "TOTALS", totals[0].ToString(), "", totals[1].ToString("n2")
            });*/

            string[] columnNames = { "Boss", "Kills", "KPH", "EHB" };
            int[] columnWidths = { 28, 9, 5, 9 };

            // + 1 to rows for the row of totals
            DiscordTable table = new DiscordTable(
               columnNames, columnWidths, kills.Count, tableContents
            );
            float[] totals = GetTotals(kills);
            table.AddTotalsRow(new string[] { "Totals", totals[0].ToString(), "", totals[1].ToString("n2") });
            List<string> splitTable = DiscordMessage.SplitTable(table.Table);
            return DiscordMessage.ToCodeBlock(splitTable);
        }

        private float[] GetTotals(List<BossKill> kills)
        {
            float totalKills = 0;
            float totalEHB = 0;
            foreach(BossKill kill in kills)
            {
                totalKills += kill.Kills;
                totalEHB += kill.GetEHB();
            }

            return new float[] { totalKills, totalEHB };
        }
    }
}
