using System;
using UnityEngine;

namespace Survivors.Managers
{
    public class TimeManager : MonoBehaviour
    {
        public static event Action OnUpdate;
        
        public static bool Paused;

        private void Update()
        {
            if (Paused)
                return;
            
            OnUpdate?.Invoke();
        }
    }
}