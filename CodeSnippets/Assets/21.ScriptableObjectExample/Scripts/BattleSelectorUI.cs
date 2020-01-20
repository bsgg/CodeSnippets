using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SnippetsCode.ScriptableObjectExample
{
    public class BattleSelectorUI : MonoBehaviour
    {
        [SerializeField] private List<CharacterSelectorUI> characterCardList;

        [SerializeField] private Button StartBattleButton;

        [SerializeField] private Text stateTextBattle;

        private int selectedCharacters = 0;

        public CharacterStats randomBoss;


        [SerializeField] private Color selectedColorCharacter;
        [SerializeField] private Color unSelectedColorCharacter;

        void Start()
        {            
            selectedCharacters = 0;

            stateTextBattle.text = "At least " + GameManagerSOE.instance.gameBalanceData.MinimunHeroesForBattle + " for battle";

            int numberHeroes = GameManagerSOE.instance.gameBalanceData.HeroList.Count;
            for (int i=0; i< numberHeroes; i++)
            {
                CharacterStats heroStats = GameManagerSOE.instance.gameBalanceData.HeroList[i];
                string heroDesc = heroStats.Name;
                heroDesc += "\nPower Attack: ";
                heroDesc += heroStats.PowerAttack;
                heroDesc += "\nStrong Against: ";
                heroDesc += heroStats.StrongAgainst;

                characterCardList[i].description.text = heroDesc;

                characterCardList[i].imageCharacter.color = heroStats.ColorHero;

                characterCardList[i].imageBackground.color = unSelectedColorCharacter;
                characterCardList[i].isSelected = false;
            }
        }


        public void OnSelectedCharacter(int index)
        {
            if ((index < 0) || (index >= characterCardList.Count)) return;
            if (characterCardList[index].isSelected)
            {
                characterCardList[index].imageBackground.color = unSelectedColorCharacter;
                characterCardList[index].isSelected = false;
                selectedCharacters -= 1;
            }else
            {
                characterCardList[index].imageBackground.color = selectedColorCharacter;
                characterCardList[index].isSelected = true;
                selectedCharacters += 1;
            }

            if (selectedCharacters >= GameManagerSOE.instance.gameBalanceData.MinimunHeroesForBattle)
            {
                stateTextBattle.text = "Ready for battle";
            }else
            {
                stateTextBattle.text = "At least " + GameManagerSOE.instance.gameBalanceData.MinimunHeroesForBattle + " for battle";
            }
        }
        
    }
}
