using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SnippetsCode.ScriptableObjectExample
{
    public class BattleSelectorUI : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] private List<CharacterSelectorUI> characterCardList;

        [Header("Boss")]
        [SerializeField] private GameObject bossPanel;
        [SerializeField] private CharacterSelectorUI bossCharacterUI;

        [Header("UI")]
        [SerializeField] private Text titleText;
        [SerializeField] private Button StartBattleButton;
        [SerializeField] private Image StartBattleImageButton;
        [SerializeField] private Color battleReadyButton;
        [SerializeField] private Color battleNotReadyButton;



        private int selectedCharacters = 0;

        public CharacterStats randomBoss;


     

        void Start()
        {
            bossPanel.gameObject.SetActive(false);           

            selectedCharacters = 0;

            titleText.text = "BRING " + GameManagerSOE.instance.gameBalanceData.MinimunHeroesForBattle + " HEROES TO THE BATTLE";
            StartBattleImageButton.color = battleNotReadyButton;

            int numberHeroes = GameManagerSOE.instance.gameBalanceData.HeroList.Count;
            for (int i=0; i< numberHeroes; i++)
            {
                CharacterStats heroStats = GameManagerSOE.instance.gameBalanceData.HeroList[i];
                string heroDesc = heroStats.Name;
                heroDesc += "\nAttack: ";
                heroDesc += heroStats.PowerAttack;
                heroDesc += "\nStrong: ";
                heroDesc += heroStats.StrongAgainst;

                characterCardList[i].description.text = heroDesc;

                characterCardList[i].imageCharacter.sprite = heroStats.Image;

                characterCardList[i].imageSelection.gameObject.SetActive(false);
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
                characterCardList[index].imageSelection.gameObject.SetActive(false);
                characterCardList[index].isSelected = false;
                selectedCharacters -= 1;
            }else
            {               

                if (selectedCharacters < GameManagerSOE.instance.gameBalanceData.MinimunHeroesForBattle)
                {
                    characterCardList[index].imageSelection.gameObject.SetActive(true);
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
                titleText.text = "BRING " + GameManagerSOE.instance.gameBalanceData.MinimunHeroesForBattle + " HEROES TO THE BATTLE";
            }
            else if (selectedCharacters == GameManagerSOE.instance.gameBalanceData.MinimunHeroesForBattle)
            {
                if (tooManySelected)
                {
                    titleText.text = "WASN'T I CLEAR? I SAID ONLY " + GameManagerSOE.instance.gameBalanceData.MinimunHeroesForBattle + " HEROES!!";
                    
                }else
                {
                    titleText.text = "READY FOR BATTLE";
                }
                StartBattleImageButton.color = battleReadyButton;
            }            
        }

        public void OnStartBattle()
        {
            bossCharacterUI.description.text = GameManagerSOE.instance.randomBoss.Name + "(" + GameManagerSOE.instance.randomBoss.Health + ")" + "\nPower Attack: " + GameManagerSOE.instance.randomBoss.PowerAttack + "\nSpell: " + GameManagerSOE.instance.randomBoss.StrongAgainst;

            // Show enemy
            bossPanel.gameObject.SetActive(true);
        }
        
    }
}
