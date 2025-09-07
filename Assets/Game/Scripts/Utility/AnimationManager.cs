using System.Collections;
using TMPro;
using UnityEngine;

public static class AnimationManager
{
    public static IEnumerator AnimateCoinText(TMP_Text text, int current, int target, string prefix = "", string postfix = "", float duration = 1f)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            text.text = prefix + Mathf.RoundToInt(Mathf.Lerp(current, target, t)) + postfix;

            yield return null;
        }

        text.text = prefix + target + postfix;
    }
}
