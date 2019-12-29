using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace TestBot.Modules
{
    //https://secure.runescape.com/m=hiscore_oldschool/hiscorepersonal?user1=arrow
    public class SkillData
    {
        public float rank { get; private set; }
        public float level { get; private set; }
        public float xp { get; private set; }

        public SkillData(string rank, string level, string xp)
        {
            this.rank = float.Parse(rank,System.Globalization.CultureInfo.InvariantCulture);
            this.level = float.Parse(level, System.Globalization.CultureInfo.InvariantCulture);
            this.xp = float.Parse(xp, System.Globalization.CultureInfo.InvariantCulture);
        }
    }


    public class MiniData
    {
        public float rank { get; private set; }
        public float score { get; private set; }

        public MiniData(string rank, string score)
        {
            this.rank = float.Parse(rank, System.Globalization.CultureInfo.InvariantCulture);
            this.score = float.Parse(score, System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    public class BossData
    {
        public float rank { get; private set; }
        public float score { get; private set; }
        public float kph { get; private set; }
        public float ehb { get; private set; }

        public BossData(string rank, string score, string kph)
        {
            this.rank = float.Parse(rank, System.Globalization.CultureInfo.InvariantCulture);
            this.score = float.Parse(score, System.Globalization.CultureInfo.InvariantCulture);
            this.kph = float.Parse(kph, System.Globalization.CultureInfo.InvariantCulture);
        }

        public void CalculateEHB(string score)
        {
            this.score = float.Parse(score, System.Globalization.CultureInfo.InvariantCulture);
            ehb = (this.score <= 0 || this.kph <= 0) ? 0.0f : this.score / this.kph;
        }
    }


    public class RunescapeHS
    {
        private string HSData;

        public RunescapeHS(string playerName)
        {
            WebClient client = new WebClient();
            string rs_url = "http://services.runescape.com/m=hiscore_oldschool/index_lite.ws?player=";
            rs_url += playerName;
            string hsData = string.Empty;
            try
            {
                HSData = client.DownloadString(rs_url);
            }
            catch (WebException)
            {
                HSData = string.Empty;
            }
        }

        public Dictionary<string, BossData> GetBossData()
        {
            string text = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/Data/boss_data.json");
            Dictionary<string, BossData> bossData = JsonConvert.DeserializeObject<Dictionary<string, BossData>>(text);

            return bossData;
        }

        public List<string> GetBossTable()
        {
            List<string> table = new List<string>();
            return table;
        }

        public List<string> GetBossDataTable()
        {
            Dictionary<string, BossData> bossData = GetBossData();
            string[] dataLines = HSData.Split('\n');
            int bossIndex = 36;
            int bossCount = 0;

            List<List<string>> tableContents = new List<List<string>>();
            foreach (string boss in bossData.Keys)
            {
                string[] data = dataLines[bossIndex].Split(',');
                bossData[boss].CalculateEHB(data[1]);
                bossIndex++;

                List<string> bossContents = new List<string>
                {
                    boss,
                    bossData[boss].score.ToString(),
                    bossData[boss].kph.ToString(),
                    bossData[boss].ehb.ToString("n2")
                };
                tableContents.Add(bossContents);

                Console.WriteLine(boss);
                Console.WriteLine("\t" + bossData[boss].rank);
                Console.WriteLine("\t" + bossData[boss].score);
                Console.WriteLine("\t" + bossData[boss].kph);
                Console.WriteLine("\t" + bossData[boss].ehb);
                bossCount++;
            }

            string[] columnNames = { "Boss", "Kills", "KPH", "EHB"};
            int[] columnWidths = { 28, 9, 5, 9 };

            Console.WriteLine("!!!!!!!!!!!!!!!!!!");

            DiscordTable table = new DiscordTable(
                columnNames, columnWidths, bossCount, tableContents
            );
            Console.WriteLine("~~~~~~~~~~");
            Console.WriteLine("DISCORD TABLE\n" + table.Table);

            List<string> splitTable = DiscordMessage.SplitTable(table.Table);
            return DiscordMessage.ToCodeBlock(splitTable);
        }

        private string GetCentered(int length, string name)
        {
            // length: the number of characters name should be centered on
            // name: the string to center in the given length
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

/*
//player class - from Arrow
public Discord.Embed GetBossEmbedded(string name)
{
    if (bossNames.Contains(name))
    {
        MiniData boss = bossData[name];
        int index = bossNames.IndexOf(name);
        var builder = new Discord.EmbedBuilder();
        string kills = killsPerHour[index];
        string ehb = float.Parse(kills) > 0.0f && boss.score > 0.0f ? (boss.score / float.Parse(kills)).ToString() : "0";
        builder.WithTitle(name);
        builder.ThumbnailUrl = "https://oldschool.runescape.wiki/images/thumb/9/9b/Zulrah_%28tanzanite%2C_Christmas%29.png/250px-Zulrah_%28tanzanite%2C_Christmas%29.png?e8bc2";
        builder.AddField(GetField("Rank", boss.rank.ToString()));
        builder.AddField(GetField("Kills", boss.score.ToString()));
        builder.AddField(GetField("KPH", kills));
        builder.AddField(GetField("EHB", ehb));

        return builder.Build();
    }
    return null;
}

private Discord.EmbedFieldBuilder GetField(string name, string value)
{
    var field = new Discord.EmbedFieldBuilder();
    field.Name = name;
    field.Value = value;
    field.IsInline = true;

    return field;
}
//response
var embed = player.GetBossEmbedded("Zulrah");
await ReplyAsync(null, false, embed);

    //await ReplyAsync(null   - message, false - text to speech?, embed  - embedslot );

*/