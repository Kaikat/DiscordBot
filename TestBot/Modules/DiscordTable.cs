using System;
using System.Collections.Generic;

namespace RavenBot.Modules
{
    public class DiscordTable
    {
        private readonly int DISCORD_MAX_CHAR_LIMIT = 2000;

        public string Table { get; private set; }
        private readonly int[] columnWidths;

        public DiscordTable(string[] tableColumns, int[] columnCharWidths, int rows, List<List<string>> contents)
        {
            columnWidths = columnCharWidths;

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

        public void AddTotalsRow(string[] contents)
        {
            string border = new string('-', 1 + columnWidths.Length + Sum(columnWidths));

            Table += "|";
            for (int column = 0; column < columnWidths.Length; column++)
            {
                int whiteSpace = columnWidths[column] - (contents[column].Length + 1);
                string space = new string(' ', whiteSpace);
                Table += column == 0 ? " " + contents[column] + space + "|" : space + contents[column] + " |";
            }
            Table += "\n" + border + "\n";
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
    }
}
