using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadAsyncScreen : MonoBehaviour {

    public Slider loadingBar;
    private AsyncOperation async;
    public int levelToLoad = 1;

    void Start () {

        StartCoroutine(LoadLevelWithBar(levelToLoad));

	}
	
    IEnumerator LoadLevelWithBar(int level)
    {
        async = SceneManager.LoadSceneAsync(level);
        while (!async.isDone)
        {
            loadingBar.value = async.progress;
            yield return null;
        }
    }

}
