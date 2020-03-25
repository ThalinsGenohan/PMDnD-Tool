using System;
using System.Collections.Generic;
using System.Text;

namespace Thalins.PMDnD
{
    internal class CharacterSheet
    {
        public struct Stat
        {
            public int Base { get => _base; set => Update(value, _boost, _buff); }
            public int Boost { get => _boost; set => Update(_base, value, _buff); }
            public int Buff { get => _buff; set => Update(_base, _boost, value); }
            public int Total { get; private set; }

            private int _base;
            private int _boost;
            private int _buff;

            private void Update(int baseStat, int boostStat, int buffStat)
            {
                _base = baseStat;
                _boost = boostStat;
                _buff = buffStat;
                Total = _base + _boost + _buff;
            }
        }

        public struct Proficiency
        {
            public string Name;
            public int Level { get => _level; set => Update(value, _buff); }
            public int Buff { get => _buff; set => Update(_level, value); }
            public int Total { get; private set; }
            public int Exp;
            public int MaxExp;

            private int _level;
            private int _buff;

            public Proficiency(string name, int level, int buff, int exp = 0)
            {
                Name = name;
                _level = level;
                _buff = buff;
                Total = level + buff;
                Exp = exp;
                MaxExp = level * 5;
            }

            private void Update(int level, int buff)
            {
                _level = level;
                _buff = buff;
                Total = _level + _buff;
            }
        }

        public struct Skill
        {
            public string Name;
            public string Type;
            public int Accuracy;
            public int Damage;
            public string Effect;

            public Skill(string name, string type, int accuracy, int damage, string effect)
            {
                Name = name;
                Type = type;
                Accuracy = accuracy;
                Damage = damage;
                Effect = effect;
            }
        }

        public string Name;
        public string Player;
        public string Gender;
        public string Species;
        public string Class;
        public string Type1;
        public string Type2;
        public string Ability;

        public int HP;
        public int MaxHP;
        public int Level;
        public int ActionsBase;
        public int ActionsBuff;
        public float ActionsStatus;
        public int Actions;
        public int Money;

        public Stat Strength;
        public Stat Special;
        public Stat Speed;
        public Stat Vitality;

        public Stat Accuracy;
        public Stat Evasion;
        public Stat Defense;

        public Dictionary<string, int> Statuses = new Dictionary<string, int>();
        public Dictionary<string, int> Inventory = new Dictionary<string, int>();

        public Dictionary<string, Proficiency> Proficiencies = new Dictionary<string, Proficiency>();

        public Dictionary<string, Skill> Moves = new Dictionary<string, Skill>();
        public Dictionary<string, Skill> Spells = new Dictionary<string, Skill>();

        public CharacterSheet(string name)
        {
            Console.WriteLine("Creating new character \"{0}\"...", name);
            Name = name;

            List<string> stringRanges = new List<string>
            {
                Name + "!" + Sheets.StandardRanges.Player,
                Name + "!" + Sheets.StandardRanges.Gender,
                Name + "!" + Sheets.StandardRanges.Species,
                Name + "!" + Sheets.StandardRanges.Class,
                Name + "!" + Sheets.StandardRanges.Type1,
                Name + "!" + Sheets.StandardRanges.Type2,
                Name + "!" + Sheets.StandardRanges.Ability
            };
            List<string> numberRanges = new List<string>
            {
                Name + "!" + Sheets.StandardRanges.HP,
                Name + "!" + Sheets.StandardRanges.MaxHP,
                Name + "!" + Sheets.StandardRanges.Level,
                Name + "!" + Sheets.StandardRanges.Actions.Base,
                Name + "!" + Sheets.StandardRanges.Actions.Buff,
                Name + "!" + Sheets.StandardRanges.Actions.Status,
                Name + "!" + Sheets.StandardRanges.Actions.Total,
                Name + "!" + Sheets.StandardRanges.Money,

                Name + "!" + Sheets.StandardRanges.Strength.Base,
                Name + "!" + Sheets.StandardRanges.Strength.Boost,
                Name + "!" + Sheets.StandardRanges.Strength.Buff,
                Name + "!" + Sheets.StandardRanges.Special.Base,
                Name + "!" + Sheets.StandardRanges.Special.Boost,
                Name + "!" + Sheets.StandardRanges.Special.Buff,
                Name + "!" + Sheets.StandardRanges.Speed.Base,
                Name + "!" + Sheets.StandardRanges.Speed.Boost,
                Name + "!" + Sheets.StandardRanges.Speed.Buff,
                Name + "!" + Sheets.StandardRanges.Vitality.Base,
                Name + "!" + Sheets.StandardRanges.Vitality.Boost,
                Name + "!" + Sheets.StandardRanges.Vitality.Buff,

                Name + "!" + Sheets.StandardRanges.Accuracy.Equip,
                Name + "!" + Sheets.StandardRanges.Accuracy.Buff,
                Name + "!" + Sheets.StandardRanges.Accuracy.Debuff,
                Name + "!" + Sheets.StandardRanges.Evasion.Equip,
                Name + "!" + Sheets.StandardRanges.Evasion.Buff,
                Name + "!" + Sheets.StandardRanges.Evasion.Debuff,
                Name + "!" + Sheets.StandardRanges.Defense.Base,
                Name + "!" + Sheets.StandardRanges.Defense.Equip,
                Name + "!" + Sheets.StandardRanges.Defense.Buff
            };

            List<string> ranges = new List<string>();
            stringRanges.ForEach((range) => ranges.Add(range));
            numberRanges.ForEach((range) => ranges.Add(range));

            var values = Sheets.GetFromSheet(ranges);

            Player  = values[stringRanges[0]];
            Gender  = values[stringRanges[1]];
            Species = values[stringRanges[2]];
            Class   = values[stringRanges[3]];
            Type1   = values[stringRanges[4]];
            Type2   = values[stringRanges[5]];
            Ability = values[stringRanges[6]];

            HP              = int.Parse(values[numberRanges[0]]);
            MaxHP           = int.Parse(values[numberRanges[1]]);
            Level           = int.Parse(values[numberRanges[2]]);
            ActionsBase     = int.Parse(values[numberRanges[3]]);
            ActionsBuff     = int.Parse(values[numberRanges[4]]);
            ActionsStatus   = float.Parse(values[numberRanges[5]]);
            Actions         = int.Parse(values[numberRanges[6]]);
            Money           = int.Parse(values[numberRanges[7]]);

            Strength.Base   = int.Parse(values[numberRanges[8]]);
            Strength.Boost  = int.Parse(values[numberRanges[9]]);
            Strength.Buff   = int.Parse(values[numberRanges[10]]);
            Special.Base    = int.Parse(values[numberRanges[11]]);
            Special.Boost   = int.Parse(values[numberRanges[12]]);
            Special.Buff    = int.Parse(values[numberRanges[13]]);
            Speed.Base      = int.Parse(values[numberRanges[14]]);
            Speed.Boost     = int.Parse(values[numberRanges[15]]);
            Speed.Buff      = int.Parse(values[numberRanges[16]]);
            Vitality.Base   = int.Parse(values[numberRanges[17]]);
            Vitality.Boost  = int.Parse(values[numberRanges[18]]);
            Vitality.Buff   = int.Parse(values[numberRanges[19]]);

            Accuracy.Base   = int.Parse(values[numberRanges[20]]);
            Accuracy.Boost  = int.Parse(values[numberRanges[21]]);
            Accuracy.Buff   = int.Parse(values[numberRanges[22]]);
            Evasion.Base    = int.Parse(values[numberRanges[23]]);
            Evasion.Boost   = int.Parse(values[numberRanges[24]]);
            Evasion.Buff    = int.Parse(values[numberRanges[25]]);
            Defense.Base    = int.Parse(values[numberRanges[26]]);
            Defense.Boost   = int.Parse(values[numberRanges[27]]);
            Defense.Buff    = int.Parse(values[numberRanges[28]]);
        }
    }
}
