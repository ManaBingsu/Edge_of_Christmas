using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGoIngameScene : MonoBehaviour
{
    [SerializeField]
    private Image fadeOutBackground;

    Coroutine startCoroutine;

    public void ClickStart()
    {
        if (startCoroutine == null)
            startCoroutine = StartCoroutine(LoadInGameScene());
    }

    IEnumerator LoadInGameScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync("HSScene");
        op.allowSceneActivation = false;

        fadeOutBackground.gameObject.SetActive(true);
        Color startColor = new Color(0, 0, 0, 0);
        Color endColor = new Color(0, 0, 0, 1);

        fadeOutBackground.color = startColor;

        float progress = 0f;
        while(progress < 1f)
        {
            progress += Time.deltaTime;

            fadeOutBackground.color = Color.Lerp(startColor, endColor, progress);

           yield return null;
        }
      
        while(!op.isDone)
        {
            if(op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
                yield break;
            }
            yield return null;
        }
    }
}
