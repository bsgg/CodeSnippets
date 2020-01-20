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

        void Start()
        {            
            selectedCharacters = 0;

            stateTextBattle.text = "At least " + GameManagerSOE.instance.gameBalanceData.MinimunHeroesForBattle + " for battle";

            for (int i=0; i< GameManagerSOE.instance.gameBalanceData.HeroList.Count; i++)
            {
                characterCardList[i].description.text = GameManagerSOE.instance.gameBalanceData.HeroList[i].Name + "\nPower Attack: " + GameManagerSOE.instance.gameBalanceData.HeroList[i].PowerAttack + "\nStrong Against: " + GameManagerSOE.instance.gameBalanceData.HeroList[i].StrongAgainst;
                characterCardList[i].imageCharacter.color = GameManagerSOE.instance.gameBalanceData.HeroList[i].ColorHero;
                characterCardList[i].imageBackground.color = GameManagerSOE.instance.gameBalanceData.UnSelectedColorCharacter;
                characterCardList[i].isSelected = false;
            }
        }


        public void OnSelectedCharacter(int index)
        {
            if ((index < 0) || (index >= characterCardList.Count)) return;
            if (characterCardList[index].isSelected)
            {
                characterCardList[index].imageBackground.color = GameManagerSOE.instance.gameBalanceData.UnSelectedColorCharacter;
                characterCardList[index].isSelected = false;
                selectedCharacters -= 1;
            }else
            {
                characterCardList[index].imageBackground.color = GameManagerSOE.instance.gameBalanceData.SelectedColorCharacter;
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
