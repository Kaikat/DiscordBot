using System;
using System.Collections.Generic;

namespace RavenBot.Modules
{
    public sealed class BossCollection
    {
        private BossCollection()
        {
        }

        public static BossCollection Instance { get { return NestedBossCollection.instance; } }

        public readonly List<Boss> Bosses = new List<Boss>
        {
            new Boss("Abyssal Sire", 30), new Boss("Alchemical Hydra", 27),
            new Boss("Barrows Chests", 0), new Boss("Bryophyta", 0),
            new Boss("Chambers of Xeric", 3.5f), new Boss("Chambers of Xeric CM", 1.8f),
            new Boss("Chaos Elemental", 48), new Boss("Chaos Fanatic", 100),
            new Boss("Commander Zilyana", 25), new Boss("Corporeal Beast", 14),
            new Boss("Crazy Archaelogist", 0), new Boss("Dagannoth Prime", 96),
            new Boss("Dagannoth Rex", 96), new Boss("Dagannoth Supreme", 96),
            new Boss("Deranged Archaelogist", 0), new Boss("General Graardor", 30),
            new Boss("Giant Mole", 90), new Boss("Grotesque Guardians", 30),
            new Boss("Hespori", 0), new Boss("Kalphite Queen", 35),
            new Boss("King Black Dragon", 100), new Boss("Kraken", 80),
            new Boss("Kree'Arra", 28), new Boss("K'ril Tsutsaroth", 36),
            new Boss("Thermonuclear Smoke Devil", 110), new Boss("Obor", 0),
            new Boss("Sarachnis", 45), new Boss("Scorpia", 100),
            new Boss("Skotizo", 0), new Boss("The Gauntlet", 10),
            new Boss("The Corrupted Gauntlet", 7.5f), new Boss("Theatre of Blood", 3),
            new Boss("Thermonuclear Smoke Devil", 110), new Boss("TzKal-Zuk", 0.8f),
            new Boss("TzTok-Jad", 2), new Boss("Venenatis", 44),
            new Boss("Vet'ion", 30), new Boss("Vorkath", 32),
            new Boss("Wintertodt", 0), new Boss("Zalcano", 0),
            new Boss("Zulrah", 35)

        };

        private class NestedBossCollection
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static NestedBossCollection()
            {
            }
            internal static readonly BossCollection instance = new BossCollection();
        }
    }

    public class Boss
    {
        public string Name { get; private set; }
        public float KPH { get; private set; }

        public Boss(string name, float kph)
        {
            Name = name;
            KPH = kph;
        }
    }

    public class BossKill
    {
        public Boss Boss { get; private set; }
        public int Rank { get; private set; }
        public int Kills { get; private set; }

        public BossKill(Boss boss, string rank, string kills)
        {
            Boss = boss;
            Rank = Int32.Parse(rank);
            Kills = kills == "-1" ? 0: Int32.Parse(kills);
        }

        public float GetEHB()
        {
            if (Kills == 0 || Boss.KPH <= 0.0f)
            {
                return 0;
            }
            return Kills / Boss.KPH;
        }
    }
}
