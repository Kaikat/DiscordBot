using System;
namespace TestBot.Modules
{
    public class DiscordTable
    {
        public DiscordTable()
        {
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
    }
}
