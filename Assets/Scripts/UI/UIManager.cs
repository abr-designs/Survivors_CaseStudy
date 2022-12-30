using System;
using Survivors.Managers;
using Survivors.ScriptableObjets.Attacks.Items;
using Survivors.ScriptableObjets.Items;
using Survivors.UI.Elements;
using Survivors.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Survivors.UI
{
    public class UIManager : MonoBehaviour
    {
        public static event Action<ItemBaseScriptableObject> OnItemSelected;
        
        [SerializeField, Header("Gameplay UI")]
        private Slider xpSlider;
        [SerializeField]
        private TMP_Text levelText;
        [SerializeField]
        private TMP_Text timeText;

        [SerializeField, Header("Level Up")]
        private GameObject levelUpWindow;
        [SerializeField]
        private ItemBaseScriptableObject[] itemOptions;
        [SerializeField]
        private ItemSelectUIElement[] itemButtonElements;


        private float _updateDelayTimer;

        private void OnEnable()
        {
            XpManager.OnProgressToNextLevel += OnProgressToNextLevel;
            XpManager.OnLevelUp += OnLevelUp;
        }

        private void Start()
        {
            //OnLevelUp(0);
            xpSlider.value = 0f;
            SetTimeText(0);
            levelUpWindow.SetActive(false);
        }

        private void Update()
        {
            if (_updateDelayTimer < 1f)
            {
                _updateDelayTimer += Time.deltaTime;
                return;
            }

            SetTimeText((int)Time.timeSinceLevelLoad);
        }

        private void OnDisable()
        {
            XpManager.OnProgressToNextLevel -= OnProgressToNextLevel;
            XpManager.OnLevelUp -= OnLevelUp;
        }
        
        //============================================================================================================//

        private void SetTimeText(in int seconds)
        {
            if (seconds == 0)
            {
                timeText.text = "00:00";
                return;
            }
            
            var time = TimeSpan.FromSeconds(seconds);
            timeText.text = time.ToString(@"mm\:ss");
        }
        

        
        //Callbacks
        //============================================================================================================//
        
        private void OnLevelUp(int newLevel)
        {
            levelUpWindow.SetActive(true);

            levelText.text = $"level {newLevel.ToString()}";
            OnProgressToNextLevel(0);
            
            //TODO Pause the game
            var toDisplay = itemOptions.GetRandomElements(3);
            for (var i = 0; i < toDisplay.Length; i++)
            {
                var item = toDisplay[i];
                itemButtonElements[i].Init(item, () =>
                {
                    Time.timeScale = 1f;
                    OnItemSelected?.Invoke(item);
                    levelUpWindow.SetActive(false);
                });
            }

            Time.timeScale = 0f;
        }

        private void OnProgressToNextLevel(float value)
        {
            xpSlider.value = value;
        }
        
        //============================================================================================================//
    }
}