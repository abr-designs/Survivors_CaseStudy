using System.Collections;
using System.Collections.Generic;
using Survivors.Factories;
using UnityEngine;

namespace Survivors.Utilities
{
    public static class DamageAnimator
    {
        private static bool _setup;
        private static MonoBehaviour _monoBehaviour;

        private static HashSet<SpriteRenderer> _animatingRenderers;

        /*public static void DamageAnimation(SpriteRenderer spriteRenderer,
            in int flashes, 
            in float flashTime)
        {
            IEnumerator DamageAnimationCoroutine(int flashes, Color startColor, WaitForSeconds waitForSeconds)
            {
                for (int i = 0; i < flashes; i++)
                {
                    var color = startColor;
                    color.a = 0f;

                    spriteRenderer.color = color;

                    yield return waitForSeconds;

                    spriteRenderer.color = startColor;
                }
            }
            
            if (_setup == false)
            {
                _monoBehaviour = Object.FindObjectOfType<FactoryManager>();
                _setup = true;
            }
            
            var startColor = spriteRenderer.color;
            var waitForSeconds = new WaitForSeconds(flashTime);

            _monoBehaviour.StartCoroutine(DamageAnimationCoroutine(flashes, startColor, waitForSeconds));
        }*/
        
        public static void Play(SpriteRenderer spriteRenderer, 
            in float flashTime)
        {
            IEnumerator DamageAnimationCoroutine(Color startColor, WaitForSeconds waitForSeconds)
            {
                _animatingRenderers.Add(spriteRenderer);
                var color = startColor;
                color.a = 1f;

                spriteRenderer.color = color;

                yield return waitForSeconds;
                
                if(spriteRenderer == null)
                    yield break;

                spriteRenderer.color = startColor;
                _animatingRenderers.Remove(spriteRenderer);
            }
            
            if (_setup == false)
            {
                _animatingRenderers = new HashSet<SpriteRenderer>();
                _monoBehaviour = Object.FindObjectOfType<FactoryManager>();
                _setup = true;
            }

            if (_animatingRenderers.Contains(spriteRenderer))
                return;
            
            var startColor = spriteRenderer.color;
            var waitForSeconds = new WaitForSeconds(flashTime);

            _monoBehaviour.StartCoroutine(DamageAnimationCoroutine(startColor, waitForSeconds));
        }
    }
}