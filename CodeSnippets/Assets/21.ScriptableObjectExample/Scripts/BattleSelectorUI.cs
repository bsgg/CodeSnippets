using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SnippetsCode.ScriptableObjectExample
{
    public class BattleSelectorUI : MonoBehaviour
    {
        [SerializeField] private List<CharacterSelectorUI> characterCardList;

        [SerializeField] private CharacterSelectorUI bossCharacterUI;

        [SerializeField] private Button StartBattleButton;
        [SerializeField] private Image StartBattleImageButton;

        [SerializeField] private Text stateTextBattle;

        private int selectedCharacters = 0;

        public CharacterStats randomBoss;


        [SerializeField] private Color selectedColorCharacter;
        [SerializeField] private Color unSelectedColorCharacter;

        [SerializeField] private Color battleReadyButton;
        [SerializeField] private Color battleNotReadyButton;


       // [SerializeField] private GameObject bossPanel;

        void Start()
        {
            bossCharacterUI.gameObject.SetActive(false);           

            selectedCharacters = 0;

            stateTextBattle.text = "BRING " + GameManagerSOE.instance.gameBalanceData.MinimunHeroesForBattle + " HEROES TO THE BATTLE";
            StartBattleImageButton.color = battleNotReadyButton;

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

            bossCharacterUI.description.text = "Choose wisely insignificant ant... this battle won't be easy";
        }


        public void OnSelectedCharacter(int index)
        {
            if ((index < 0) || (index >= characterCardList.Count)) return;

            bool tooManySelected = false;

            if (characterCardList[index].isSelected)
            {
                characterCardList[index].imageBackground.color = unSelectedColorCharacter;
                characterCardList[index].isSelected = false;
                selectedCharacters -= 1;
            }else
            {               

                if (selectedCharacters < GameManagerSOE.instance.gameBalanceData.MinimunHeroesForBattle)
                {
                    characterCardList[index].imageBackground.color = selectedColorCharacter;
                    characterCardList[index].isSelected = true;
                    selectedCharacters += 1;

                }else if (selectedCharacters == GameManagerSOE.instance.gameBalanceData.MinimunHeroesForBattle)
                {
                    tooManySelected = true;
                }
            }

            if (selectedCharacters < GameManagerSOE.instance.gameBalanceData.MinimunHeroesForBattle)
            {
                StartBattleImageButton.color = battleNotReadyButton;
                stateTextBattle.text = "BRING " + GameManagerSOE.instance.gameBalanceData.MinimunHeroesForBattle + " HEROES TO THE BATTLE";
            }
            else if (selectedCharacters == GameManagerSOE.instance.gameBalanceData.MinimunHeroesForBattle)
            {
                if (tooManySelected)
                {
                    stateTextBattle.text = "WASN'T I CLEAR? I SAID ONLY " + GameManagerSOE.instance.gameBalanceData.MinimunHeroesForBattle + " HEROES!!";
                    
                }else
                {
                    stateTextBattle.text = "READY FOR BATTLE";
                }
                StartBattleImageButton.color = battleReadyButton;
            }            
        }

        public void OnStartBattle()
        {
            //bossCharacterUI.description.text = "OK... You went throught the hard path.. back off or die!";

            bossCharacterUI.description.text = GameManagerSOE.instance.randomBoss.Name + "(" + GameManagerSOE.instance.randomBoss.Health + ")" + "\nPower Attack: " + GameManagerSOE.instance.randomBoss.PowerAttack + "\nSpell: " + GameManagerSOE.instance.randomBoss.StrongAgainst;

            // Show enemy
            bossCharacterUI.gameObject.SetActive(true);
        }
        
    }
}
