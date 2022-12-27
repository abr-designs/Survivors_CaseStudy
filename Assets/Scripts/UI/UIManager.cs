using System;
using Survivors.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Survivors.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private Slider xpSlider;
        [SerializeField]
        private TMP_Text levelText;
        [SerializeField]
        private TMP_Text timeText;

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
        }

        private void OnProgressToNextLevel(float value)
        {
            xpSlider.value = value;
        }
    }
}