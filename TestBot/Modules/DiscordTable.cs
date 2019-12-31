using System;
using System.Collections.Generic;

namespace RavenBot.Modules
{
    public class DiscordTable
    {
        private readonly int DISCORD_MAX_CHAR_LIMIT = 2000;

        public string Table { get; private set; }

        public DiscordTable(string[] tableColumns, int[] columnCharWidths, int rows, List<List<string>> contents)
        {
            if (tableColumns.Length != columnCharWidths.Length)
            {
                Console.WriteLine("ERROR: DiscordTable: The lengths of the lists must be the same.");
                return;
            }

            string table = "";

            // Create the table contents
            for (int row = 0; row < rows; row++)
            {
                table += "|";
                for (int column = 0; column < columnCharWidths.Length; column++)
                {
                    int whiteSpace = columnCharWidths[column] - (contents[row][column].Length + 1);
                    string space = new string(' ', whiteSpace);
                    table += column == 0 ? " " + contents[row][column] + space + "|" : space + contents[row][column] + " |";
                }
                table += "\n";
            }

            // Create the table header
            string border = new string('-', 1 + columnCharWidths.Length + Sum(columnCharWidths));
            string header = border + "\n|";
            for (int i = 0; i < tableColumns.Length; i++)
            {
                header += GetCentered(columnCharWidths[i], tableColumns[i]);
            }
            header += "\n" + border + "\n";
            Table = header + table + border + "\n";
        }

        private int Sum(int[] array)
        {
            int sum = 0;
            foreach (int v in array)
            {
                sum += v;
            }

            return sum;
        }

        private string GetCentered(int length, string name)
        {
            // length: the number of characters name should be centered on
            // name: the string to center in the given length
            string padding = "";
            int nameLength = name.Length;
            float center = length / 2.0f - (nameLength / 2.0f);

            for (int i = 0; i < center; i++)
            {
                padding += " ";
            }

            return padding + name + padding + "|";
        }

        public List<string> Split()
        {
            List<string> tableParts = new List<string>();
            if (Table.Length > DISCORD_MAX_CHAR_LIMIT)
            {
                string[] tableLines = Table.Split('\n');
                int lineLength = tableLines[0].Length;

                int limit = (DISCORD_MAX_CHAR_LIMIT / lineLength) - 1;
                int linesAddedCounter = 0;
                string tableLineEntry = "";

                for (int i = 0; i < tableLines.Length; i++)
                {
                    if (linesAddedCounter == limit || i == tableLines.Length - 1)
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
                tableParts.Add("```" + Table + "```");
            }

            return tableParts;
        }
        /*
        private void ParseHSData(string hsData)
        {
            GetBossData();

            string[] dataLines = hsData.Split('\n');
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
        }*/

        /*public List<string> GetBossDataTable()
        {
            List<string> l = new List<string>();
            return l;
        }*/

        /*public List<string> GetBossDataTable2()
        {
            string table = "";
            int bossFieldLength = 28;  //must be even
            int numKillsLength = 9;    //must be odd
            int kphsLength = 5;        //must be odd
            int ehbLength = 9;         //must be odd
            int numDividers = 5;       //number of |

            string horizBorder = "";
            for(int i = 0; i < bossFieldLength + numKillsLength + kphsLength + ehbLength + numDividers; i++)
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

                // Efficient Hours Bossed (EHB), (+1 for the leading white space)
                float ehb = 0.0f;
                float kills = float.Parse(killsPerHour[bossCounter], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                if (kills > 0.0f && entry.score > 0.0f)
                {
                    ehb = entry.score / kills;
                }

                string theEHB = ehb.ToString("n2");
                whiteSpace = ehbLength - (theEHB.Length + 1);
                for (int i = 0; i < whiteSpace; i++)
                {
                    table += " ";
                }
                table += theEHB + " |";
                bossCounter++;
                table += "\n";
            }

            List<string> tableParts = new List<string>();
            if (table.Length > DISCORD_MAX_CHAR_LIMIT)
            {
                string[] tableLines = table.Split('\n');
                int lineLength = tableLines[0].Length;

                int limit = (DISCORD_MAX_CHAR_LIMIT / lineLength) - 1;
                int linesAddedCounter = 0;
                string tableLineEntry = "";

                for (int i = 0; i < tableLines.Length; i++)
                {
                    if (linesAddedCounter == limit || i == tableLines.Length - 1)
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
        }*/
    }
}
