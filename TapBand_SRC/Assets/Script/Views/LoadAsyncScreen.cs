using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadAsyncScreen : MonoBehaviour {

    public Slider loadingBar;
    private AsyncOperation async;
    public int levelToLoad = 1;

    void Start () {

        StartCoroutine(LoadLevelWithBar(levelToLoad));

	}
	
    IEnumerator LoadLevelWithBar(int level)
    {
        async = Application.LoadLevelAsync(level);
        while (!async.isDone)
        {
            loadingBar.value = async.progress;
            yield return null;
        }
    }

}
