using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnippetsCode.ScriptableObjectExample
{
    [CreateAssetMenu(fileName = "GameBalance", menuName = "CodeSnippets/GameBalance", order = 1)]
    public class GameBalanceExample : ScriptableObject
    {
        [Header("Hero Stats")]
        public int MinimunHeroesForBattle = 3;
        public int MaxNumberHeroes = 10;
        public float MaximunHealthHeroes = 100;
        public float PowerAttack = 5;

        [Header("Boss Stats")]
        public int BossHealth = 500;
        public int BossPowerAttack = 10;

        [Header("UI")]
        public Color SelectedColorCharacter;
        public Color UnSelectedColorCharacter;
    }
}
