using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class ColorChange : MonoBehaviour
{
    public GameObject whiteCD, whiteOD, redCD, redOD, blueCD, blueOD, greenCD, greenOD, pinkCD, pinkOD, cyanCD, cyanOD, yellowCD, yellowOD;
    public GameObject greyChest, redChest, blueChest, greenChest, pinkChest, cyanChest, yellowChest;
    public static GameObject memory1, memory2, memory3, memory4;
    [Space]
    public GameObject[] memoriesArray = { memory1, memory2, memory3, memory4 };
    public GameObject star;
    public Animator animator, transition;
    public Text tutorial1, scoreText;
    public GameObject tutorial2;
    int pointer = 0, sceneIndex;
    static int score;
    string sceneName;
    bool collEnter = false, isReached = false, whiteProp, greyProp, redProp, blueProp, greenProp, pinkProp, cyanProp, yellowProp;
    public bool lvFinished;
    public float waitTime = 1f;

    Vector2 currentPosition, greyChestPos, redChestPos, blueChestPos, greenChestPos, pinkChestPos, cyanChestPos, yellowChestPos;
    Color32[] PCA = new Color32[5]; //MEMORI WARNA, Previous Color Array
    public static Color32 currentColor, grey = new Color32(107, 121, 119, 255);
    Color32 white = new Color32(255, 255, 255, 255), red = new Color32(239, 84, 84, 255), blue = new Color32(45, 137, 210, 255),
        green = new Color32(64, 176, 107, 255), pink = new Color32(243, 153, 30, 255) , cyan = new Color32(195, 100, 194, 255),
        yellow = new Color32(255, 240, 65, 255);
   

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        sceneName = SceneManager.GetActiveScene().name;
        PCA[0] = white;

        PropChecker();
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("nama_tim", ButtonController.player_name);
        form.AddField("score", score.ToString());

        UnityWebRequest www = UnityWebRequest.Post("https://irgl.petra.ac.id/main/api_cek_tim", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error + "ini error");
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }

    void Update()
    {
        currentColor = PCA[pointer];
        currentPosition = gameObject.transform.position;

        DoorOpening(); //SELALU CEK PINTU MANA YG BUKA & TUTUP
        //ColorMemory();

        if (Input.GetKeyDown("f") && pointer < 4 && collEnter) //NAMBAH WARNA KE PCA
        {
            PropertiesAlteration();
            collEnter = false;
            SoundManager.PlaySound("open");
            if (sceneName == "Level01")
            {
                tutorial2.SetActive(true);
                tutorial1.text = "";
            }
            if (sceneName == "Level02")
            {
                tutorial1.text = "press r to remove color";
            }
        }

        if (Input.GetKeyDown("r") && pointer > 0) //REMOVE WARNA DARI PCA
        {
            RemovingColor();
            SoundManager.PlaySound("close");
        }

        if (Input.GetKeyDown("backspace") && sceneName != "0MainMenu") //RELOAD SCENE
        {
            score -= 1;
            StartCoroutine(Upload());
            SceneManager.LoadScene(sceneIndex);
        }
        if (sceneName != "0MainMenu")
        {
            scoreText.text = score.ToString();
            //print("score : " + score);
        }

        if (!(currentColor.Equals(grey)))
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 3f;
            gameObject.GetComponent<SpriteRenderer>().flipY = false;
        } 
        else
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = -3f;
            gameObject.GetComponent<SpriteRenderer>().flipY = true;
        }
    }

    void DoorOpening()
    {
        if (whiteProp) //PINTU PUTIH ===============
        {
            if (currentColor.Equals(white))
            {
                whiteCD.SetActive(false);
                whiteOD.SetActive(true);
            }
            else
            {
                whiteCD.SetActive(true);
                whiteOD.SetActive(false);
            }
        }
        if (redProp) //PINTU MERAH ==================
        {
            if (currentColor.Equals(red))
            {
                redCD.SetActive(false);
                redOD.SetActive(true);
            }
            else
            {
                redCD.SetActive(true);
                redOD.SetActive(false);
            }
        }
        if (blueProp) //PINTU BIRU ==============
        {
            if (currentColor.Equals(blue))
            {
                blueCD.SetActive(false);
                blueOD.SetActive(true);
            }
            else
            {
                blueCD.SetActive(true);
                blueOD.SetActive(false);
            }
        }
        if (greenProp) //PINTU HIJAU ==============
        {
            if (currentColor.Equals(green))
            {
                greenCD.SetActive(false);
                greenOD.SetActive(true);
            }
            else
            {
                greenCD.SetActive(true);
                greenOD.SetActive(false);
            }
        }
        if (cyanProp) //PINTU CYAN ==============
        {
            if (currentColor.Equals(cyan))
            {
                cyanCD.SetActive(false);
                cyanOD.SetActive(true);
            }
            else
            {
                cyanCD.SetActive(true);
                cyanOD.SetActive(false);
            }
        }
        if (pinkProp) //PINTU PINK ==============
        {
            if (currentColor.Equals(pink))
            {
                pinkCD.SetActive(false);
                pinkOD.SetActive(true);
            }
            else
            {
                pinkCD.SetActive(true);
                pinkOD.SetActive(false);
            }
        }
        if (yellowProp) //PINTU HIJAU ==============
        {
            if (currentColor.Equals(yellow))
            {
                yellowCD.SetActive(false);
                yellowOD.SetActive(true);
            }
            else
            {
                yellowCD.SetActive(true);
                yellowOD.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chest"))
        {
            collEnter = true;
            if (sceneName == "Level01")
            {
                tutorial1.text = "Press F to pick the chest color";
            }
        }
        if (collision.gameObject.CompareTag("Star") && !isReached)
        {
            isReached = true;
            animator.SetBool("isFinished", true);
            lvFinished = true;
            if (sceneName != "0MainMenu")
            {
                score += 5;
                StartCoroutine(Upload());
            }
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }
    void PropertiesAlteration ()
    {
        if (currentPosition.x >= greyChestPos.x - 2f && currentPosition.x <= greyChestPos.x + 2f && greyProp &&
            currentPosition.y >= greyChestPos.y - 1f && currentPosition.y <= greyChestPos.y + 1f && !currentColor.Equals(grey))
        {
            AddingColor(grey);
        }
        else if (currentPosition.x >= redChestPos.x - 2f && currentPosition.x <= redChestPos.x + 2f && redProp &&
            currentPosition.y >= redChestPos.y - 1f && currentPosition.y <= redChestPos.y + 1f && !currentColor.Equals(red))
        {
            AddingColor(red);
        }
        else if (currentPosition.x >= blueChestPos.x - 2f && currentPosition.x <= blueChestPos.x + 2f && blueProp &&
                 currentPosition.y >= blueChestPos.y - 2f && currentPosition.y <= blueChestPos.y + 2f && !currentColor.Equals(blue))
        {
            AddingColor(blue);
        }
        else if (currentPosition.x >= greenChestPos.x - 2f && currentPosition.x <= greenChestPos.x + 2f && greenProp &&
                 currentPosition.y >= greenChestPos.y - 2f && currentPosition.y <= greenChestPos.y + 2f && !currentColor.Equals(green))
        {
            AddingColor(green);
        }
        else if (currentPosition.x >= cyanChestPos.x - 2f && currentPosition.x <= cyanChestPos.x + 2f && cyanProp &&
                 currentPosition.y >= cyanChestPos.y - 2f && currentPosition.y <= cyanChestPos.y + 2f && !currentColor.Equals(cyan))
        {
            AddingColor(cyan);
        }
        else if (currentPosition.x >= pinkChestPos.x - 2f && currentPosition.x <= pinkChestPos.x + 2f && pinkProp &&
                 currentPosition.y >= pinkChestPos.y - 2f && currentPosition.y <= pinkChestPos.y + 2f && !currentColor.Equals(pink))
        {
            AddingColor(pink);
        }
        else if (currentPosition.x >= yellowChestPos.x - 2f && currentPosition.x <= yellowChestPos.x + 2f && yellowProp &&
                 currentPosition.y >= yellowChestPos.y - 2f && currentPosition.y <= yellowChestPos.y + 2f && !currentColor.Equals(yellow))
        {
            AddingColor(yellow);
        }
    }

    void AddingColor (Color32 color)
    {
        pointer++;
        PCA[pointer] = color;
        gameObject.GetComponent<Renderer>().material.color = PCA[pointer];

        memoriesArray[pointer - 1].GetComponent<Image>().color = PCA[pointer];
        memoriesArray[pointer - 1].SetActive(true);
    }

    void RemovingColor ()
    {
        pointer--;
        gameObject.GetComponent<Renderer>().material.color = PCA[pointer];

        memoriesArray[pointer].SetActive(false);
    }

    void PropChecker ()
    {
        if (sceneName == "Level01")
        {
            redProp = true; redChestPos = redChest.transform.position;
        }
        else if (sceneName == "Level02")
        {
            whiteProp = true;
            redProp = true; redChestPos = redChest.transform.position;
        }
        else if (sceneName == "Level03")
        {
            redProp = true; redChestPos = redChest.transform.position;
            blueProp = true; blueChestPos = blueChest.transform.position;
        }
        else if (sceneName == "Level04")
        {
            whiteProp = true;
            redProp = true; redChestPos = redChest.transform.position;
            blueProp = true; blueChestPos = blueChest.transform.position;
            greenProp = true; greenChestPos = greenChest.transform.position;
        }
        else if (sceneName == "Level05")
        {
            redProp = true; redChestPos = redChest.transform.position;
            blueProp = true; blueChestPos = blueChest.transform.position;
            greenProp = true; greenChestPos = greenChest.transform.position;
            greyProp = true; greyChestPos = greyChest.transform.position;
        }
        else if (sceneName == "Level06")
        {
            redProp = true; redChestPos = redChest.transform.position;
            blueProp = true; blueChestPos = blueChest.transform.position;
            greenProp = true; greenChestPos = greenChest.transform.position;
        }
        else if (sceneName == "Level07")
        {
            redProp = true; redChestPos = redChest.transform.position;
            blueProp = true; blueChestPos = blueChest.transform.position;
            greenProp = true; greenChestPos = greenChest.transform.position;
            cyanProp = true; cyanChestPos = cyanChest.transform.position;
            pinkProp = true; pinkChestPos = pinkChest.transform.position;
            yellowProp = true; yellowChestPos = yellowChest.transform.position;
            greyProp = true; greyChestPos = greyChest.transform.position;
        }
        else if (sceneName == "Level08")
        {
            redProp = true; redChestPos = redChest.transform.position;
            blueProp = true; blueChestPos = blueChest.transform.position;
            greenProp = true; greenChestPos = greenChest.transform.position;
            cyanProp = true; cyanChestPos = cyanChest.transform.position;
            pinkProp = true; pinkChestPos = pinkChest.transform.position;
        }
    }
    IEnumerator LoadLevel(int buildIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(buildIndex);
    }
}
