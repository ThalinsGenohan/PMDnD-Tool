using System;
using System.Collections.Generic;
using System.Text;

namespace Thalins.PMDnD
{
    static class Battle
    {
        public static List<KeyValuePair<CharacterSheet, int>> Initiative = new List<KeyValuePair<CharacterSheet, int>>();

        public static void Start(List<CharacterSheet> characters)
        {
            foreach (var character in characters)
            {
                Initiative.Add(new KeyValuePair<CharacterSheet, int>(character, character.Speed.Total));
            }
            Initiative.Sort((x, y) => y.Value.CompareTo(x.Value));

            foreach (var character in Initiative)
            {
                Console.WriteLine(character.Key.Name + " - " + character.Value);
            }
        }
    }
}
