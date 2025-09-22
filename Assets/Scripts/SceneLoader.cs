using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader singletonInstance;

    private bool isLoadindInProgress;

    public static SceneLoader Instance { 
        
        get
        {
            if (singletonInstance == null)
            {
                var go = new GameObject("[SceneLoader]");

                singletonInstance = go.AddComponent<SceneLoader>();

                DontDestroyOnLoad(go);
            }

            return singletonInstance;
        }
    }

    public static void Load(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return;
        
        if (Instance.isLoadindInProgress) return;

        Instance.StartCoroutine(Instance.LoadRoutine(sceneName));
            
      
    }

    private IEnumerator LoadRoutine(string targetSceneName)
    {
        isLoadindInProgress = true;

        var loadOperation = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Single);

        loadOperation.allowSceneActivation = true;

        while (!loadOperation.isDone)
        {
            yield return null;
        }
        
        yield return null;

        isLoadindInProgress = false;    
    }

}
