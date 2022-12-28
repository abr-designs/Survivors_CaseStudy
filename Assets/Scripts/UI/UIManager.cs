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
            OnLevelUp(0);
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

        private void SetTimeText(in int seconds)
        {
            if (seconds == 0)
            {
                timeText.text = "00:00";
                return;
            }
            
            TimeSpan time = TimeSpan.FromSeconds(seconds);

            timeText.text = time.ToString(@"mm\:ss");
        }
        
        private void OnLevelUp(int newLevel)
        {
            levelText.text = $"level {newLevel.ToString()}";
            OnProgressToNextLevel(0);
            
            //TODO Pause the game
            levelUpWindow.SetActive(true);
            var toDisplay = itemOptions.GetRandomElements(3);
            for (var i = 0; i < toDisplay.Length; i++)
            {
                var item = toDisplay[i];
                itemButtonElements[i].Init(item, () =>
                {
                    OnItemSelected(item);
                });
            }
        }

        private static WeaponManager _weaponManager;
        private static PassiveManager _passiveManager;
        private void OnItemSelected(ItemBaseScriptableObject item)
        {
            switch (item)
            {
                case WeaponProfileScriptableObject weapon:
                    if (_weaponManager == null) _weaponManager = FindObjectOfType<WeaponManager>();
                    _weaponManager.AddNewAttack(weapon.type);
                    break;
                case PassiveItemProfileScriptableObject passive:
                    if (_passiveManager == null) _passiveManager = FindObjectOfType<PassiveManager>();
                    _passiveManager.AddNewPassive(passive.type);
                    break;
            }
        }

        private void OnProgressToNextLevel(float value)
        {
            xpSlider.value = value;
        }
    }
}