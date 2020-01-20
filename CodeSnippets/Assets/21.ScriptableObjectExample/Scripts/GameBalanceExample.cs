using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnippetsCode.ScriptableObjectExample
{
    public enum SkillType { Fire, Water, Air, Electricity, Venon, TOTALSKILLS };

    [System.Serializable]
    public class CharacterStats
    {
        public string Name;
        public float Health;
        public float PowerAttack;
        public SkillType StrongAgainst;
        public Color ColorHero;
    }

    [CreateAssetMenu(fileName = "GameBalance", menuName = "CodeSnippets/GameBalance", order = 1)]
    public class GameBalanceExample : ScriptableObject
    {       

        [Header("Hero Stats")]
        public int MinimunHeroesForBattle = 2;

        // List of heroes
        public List<CharacterStats> HeroList;

        [Header("Boss Stats")]
        public int BossMinHealthRange = 100;
        public int BossMaxHealthRange = 500;
        public int BossMinPowerAttackRange = 2;
        public int BossMaxPowerAttackRange = 10; 

       
    }
}
