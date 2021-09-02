using System.Collections;
using UnityEngine;
namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentActiveFade = null;
        public void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }
        public Coroutine FadeOut(float duration)
        {
            return Fade(1f, duration);
        }

        public Coroutine FadeIn(float duration)
        {
            return Fade(0f, duration);
        }
        private IEnumerator FadeRoutine(float targetAlpha, float duration)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, Time.deltaTime / duration);
                yield return null;
            }
        }
        public Coroutine Fade(float targetAlpha, float duration)
        {
            if (currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }
            currentActiveFade = StartCoroutine(FadeRoutine(targetAlpha, duration));
            return currentActiveFade;
        }
    }
}
