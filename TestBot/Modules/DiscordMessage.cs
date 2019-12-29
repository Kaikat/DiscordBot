using System;
using System.Collections.Generic;

namespace TestBot.Modules
{
    public static class DiscordMessage
    {
        static int DISCORD_MAX_CHAR_LIMIT = 2000;

        public static string ToCodeBlock(string message)
        {
            return "```" + message + "```";
        }

        public static List<string> ToCodeBlock(List<string> messages)
        {
            List<string> codified = new List<string>();
            foreach(string message in messages)
            {
                codified.Add("```" + message + "```");
            }
            return codified;
        }

        public static List<string> SplitTable(string table)
        {
            if(table.Length <= DISCORD_MAX_CHAR_LIMIT)
            {
                return new List<string> { table };
            }

            List<string> splitMessage = new List<string>();
            string[] lines = table.Split("\n");

            int numLines = lines.Length;
            int lineLength = lines[0].Length;

            int linesPerChunk = (DISCORD_MAX_CHAR_LIMIT / lineLength) - 1;
            int linesAdded = 0;
            string tableChunk = "";

            for (int i = 0; i < lines.Length; i++)
            {
                if (linesAdded == linesPerChunk || i == lines.Length - 1)
                {
                    linesAdded = 0;
                    splitMessage.Add(tableChunk);
                    tableChunk = "";
                }

                tableChunk += lines[i] + "\n";
                linesAdded++;
            }

            return splitMessage;
        }
    }
}
