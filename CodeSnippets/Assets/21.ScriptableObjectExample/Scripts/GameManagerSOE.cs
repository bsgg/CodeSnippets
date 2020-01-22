using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnippetsCode.ScriptableObjectExample
{
    public class GameManagerSOE : MonoBehaviour
    {
        public static GameManagerSOE instance = null;

        [Header("Data")]
        public GameBalanceExample gameBalanceData;

        public CharacterStats randomBoss;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        private void Start()
        {
            // Generate a random boss
            randomBoss.Name = "Very Scary...Onion?";
            randomBoss.PowerAttack = Random.Range(gameBalanceData.BossMinPowerAttackRange, gameBalanceData.BossMaxPowerAttackRange);
            randomBoss.Health = Random.Range(gameBalanceData.BossMinHealthRange, gameBalanceData.BossMaxHealthRange);

            randomBoss.StrongAgainst = (SkillType)(Random.Range(0, (int)SkillType.TOTALSKILLS));
        }
    }
}
