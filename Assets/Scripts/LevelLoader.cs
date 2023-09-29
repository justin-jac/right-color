using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float waitTime = 1f;

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        print("start coroutine");
    }
    IEnumerator LoadLevel (int buildIndex)
    {
        print("ie numerator");
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(waitTime);
        print("test");
        SceneManager.LoadScene(buildIndex);
    }
}
