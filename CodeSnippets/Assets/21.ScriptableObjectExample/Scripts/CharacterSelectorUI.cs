using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SnippetsCode.ScriptableObjectExample
{
    public class CharacterSelectorUI : MonoBehaviour
    {
        public Text description;
        public Image imageSelection;
        public Image imageCharacter;
        public Button charButton;

        public bool isSelected { get; set; }       
    }
}
