using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Animator removing;
    public GameObject tutorial;
    public TMP_InputField inputField;
    public static string player_name;
    public GameObject textValidator;
    string sceneName;
    public float waitTime = 1f;
    public bool valid = false;
    private Text textValidate;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "0MainMenu")
        {
            Time.timeScale = 0f;
        }
        textValidate = textValidator.GetComponent<Text>();
    }

    IEnumerator checkTeam()
    {
        WWWForm form = new WWWForm();
        form.AddField("nama_tim", player_name);
        // form.AddField("point", 1000);
        string url = "https://irgl.petra.ac.id/main/api_cek_tim";
        WWW w = new WWW(url, form);
        yield return w;

        if (w.error != null)
        {
            Debug.Log("submit gagal");
            Debug.Log(w.error);
        }
        else
        {
            if (w.isDone)
            {
                // Debug.Log(w.text);

                if (w.text == "berhasil")
                {
                    Debug.Log("nama ada");
                    valid = true;
                    textValidate.color = Color.green;
                    textValidate.text = "selamat datang " + player_name;
                }
                else
                {
                    textValidate.color = Color.red;
                    textValidate.text = "nama tim tidak tersedia";
                }
            }
        }

        w.Dispose();
    }
    public void PlayButton ()
    {
        player_name = inputField.text;
        StartCoroutine(checkTeam());
        if (valid)
        {
            Time.timeScale = 1f;
            if (sceneName == "0MainMenu")
            {
                StartCoroutine(RemoveMenu());
            }
            SoundManager.PlaySound("button");
        }
    }
    public void QuitButton ()
    {
        Application.Quit();
        SoundManager.PlaySound("button");
    }
    public void HomeButton()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator RemoveMenu ()
    {
        removing.SetBool("isClicked", true);
        yield return new WaitForSeconds(waitTime);
        tutorial.SetActive(true);
    }
}
