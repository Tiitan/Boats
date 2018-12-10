

using System.Collections.Generic;

namespace Framework
{
    public static class NameGenerator
    {
        private static readonly string[] Names = {
            "Alpha", "Bravo", "Charlie", "Delta", "Echo", "Foxtrot", "Golf", "Hotel",
            "India", "Juliett", "Kilo", "Lima", "Mike", "November", "Oscar", "Papa",
            "Quebec", "Romeo", "Sierra", "Tango", "Uniform", "Victor", "Whiskey",
            "X-ray", "Yankee", "Zulu"
        };

        public static string NumberToName(int number)
        {
            string n2 = number / Names.Length > 0 ? (number / Names.Length + 1).ToString() : string.Empty;
            return Names[number % Names.Length] + n2;
        }
    }
}
