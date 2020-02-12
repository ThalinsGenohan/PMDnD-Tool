using System;
using System.Collections.Generic;
using System.Text;

namespace Thalins.PMDnD
{
    class CharacterSheet
    {
        [Flags] public enum Section
        {
            ALL = MAIN_INFO | STATS | COMBAT_STATS | MISC_INFO,

            MAIN_INFO   = 0b111111,
            INFO        = 1 << 0,
            TYPES       = 1 << 1,
            ACTIONS     = 1 << 2,
            HEALTH      = 1 << 3,
            STATUS      = 1 << 4,
            POKE        = 1 << 5,

            STATS       = 0b1111 << 6,
            STRENGTH    = 1 << 6,
            SPECIAL     = 1 << 7,
            SPEED       = 1 << 8,
            VITALITY    = 1 << 9,

            COMBAT_STATS    = 0b111 << 10,
            ACCURACY        = 1 << 10,
            EVASION         = 1 << 11,
            DEFENSE         = 1 << 12,

            MISC_INFO       = 0b11111 << 13,
            INVENTORY       = 1 << 13,
            TYPE_EFFECT     = 1 << 14,
            PROFICIENCIES   = 1 << 15,
            MOVES           = 1 << 16,
            SPELLS          = 1 << 17
        }

        private string _sheetName;

        public struct Stat
        {
            private int _base;
            public int Base
            {
                get { return _boost; }
                private set { Update(value, _boost, _buff); }
            }
            private int _boost;
            public int Boost
            {
                get { return _boost; }
                set { Update(_base, value, _buff); }
            }
            private int _buff;
            public int Buff
            {
                get { return _buff; }
                set { Update(_base, _boost, value); }
            }
            public int Total { get; private set; }

            public Stat(int baseStat, int boostStat = 0, int buffStat = 0)
            {
                _base = baseStat;
                _boost = boostStat;
                _buff = buffStat;
                Total = _base + _boost + _buff;
            }

            private void Update(int baseStat, int boostStat, int buffStat)
            {
                _base = baseStat;
                _boost = boostStat;
                _buff = buffStat;
                Total = _base + _boost + _buff;
            }
        }

        public string Player { get; private set; }
        public string Name { get; private set; }
        public string Species { get; set; }
        public Stat Strength { get; private set; }
        public Stat Special { get; private set; }
        public Stat Speed { get; private set; }
        public Stat Vitality { get; private set; }

        CharacterSheet(string sheetName)
        {
            _sheetName = sheetName;
            

        }

        public void UpdateRead(Section updateSection = Section.ALL)
        {
            List<string> ranges = new List<string>();
            if ((updateSection & Section.INFO) != 0)
            {
                ranges.Add(_sheetName + "!" + Sheets.Ranges.Name);
                ranges.Add(Sheets.Ranges.Player);
                ranges.Add(Sheets.Ranges.Species);
                ranges.Add(Sheets.Ranges.Gender);
                ranges.Add(Sheets.Ranges.Level);
                ranges.Add(Sheets.Ranges.Ability);
            }
        }
    }
}
