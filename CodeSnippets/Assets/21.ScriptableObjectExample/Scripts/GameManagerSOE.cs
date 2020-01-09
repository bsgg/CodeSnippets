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

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }            
    }
}
