using System;
using System.Collections;
using System.Collections.Generic;
using Survivors.Base.Managers;
using UnityEngine;

namespace Survivors.Managers
{
    public class XpManager : ManagerBase
    {
        public static event Action<float> OnProgressToNextLevel;
        public static event Action<int> OnLevelUp;

        private static XpManager _instance;
        
        private int currentLevel;
        private int _currentXp;

        public XpManager()
        {
            _instance = this;
        }

        public static void AddXp(in int xp)
        {
            _instance.TryAddXp(xp);
        }
        private void TryAddXp(in int xp)
        {
            _currentXp += xp;
            
            var currentXpReq = GetXpRequired(currentLevel);
            var nextXpReq = GetXpRequired(currentLevel + 1);

            if (_currentXp >= nextXpReq)
            {
                currentLevel++;

                OnLevelUp?.Invoke(currentLevel);
                
                currentXpReq = GetXpRequired(currentLevel);
                nextXpReq = GetXpRequired(currentLevel + 1);
            }

            var currentOffset = _currentXp - currentXpReq;
            var nextOffset = nextXpReq - currentXpReq;

            OnProgressToNextLevel?.Invoke(currentOffset / (float)nextOffset);
        }
        
        
        //Based on: https://www.reddit.com/r/VampireSurvivors/comments/svtmvx/xp_level_up_bar_how_many_gems_are_needed_for_each/
        private static int GetXpRequired(in int level)
        {
            /*
                Looks like the formula/algorithm to calculate the XP need for reach the next level is:
                if(next_level < 20) -> (next_level*10)-5

                if(next_level > 20 < 40) -> (next_level*13)-6

                if(next_level > 40) -> (next_level*16)-8

                Exceptions:

                if(next_level == 20) -> (next_level*10)-5+600

                if(next_level == 40) -> (next_level*13)-6+2400
*/
            if (level == 0)
                return 0;
            if(level == 20)
                return (level * 10) - 5 + 600;
            if (level == 40)
                return (level * 13) - 6 + 2400;
            
            if (level < 20)
            {
                return (level * 10) - 5;
            }
            if (level > 20 && level < 40)
            {
                return (level * 13) - 6;
            }
            if (level > 40)
            {
                return (level * 16) - 8;
            }

            throw new Exception();
        }
    }
}
