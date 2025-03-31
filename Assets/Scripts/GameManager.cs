using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject Pota;
    public GameObject BuyukPota;

    private bool oncooldown = false;

    
    public int scoresinarow;
    public int hitcounter;
    public int misscounter;
    public int currentscore;
    
    public bool again = false;

    public float currentTime = 60f;
    public TextMeshProUGUI timerText;
    public bool gameRunning = true;
    public Canvas UI;
    public GameObject ESC;


    public GameObject normalBall;
    public GameObject bouncyBall;
    public GameObject heavyBall;

    public GameObject normalBall1;
    public GameObject bouncyBall1;
    public GameObject heavyBall1;

    public FPSController FPcontroller;
    public GameObject Player;
    public Sunshine Sunshine;

    public GameObject LoseScreen;
    public TextMeshProUGUI FinalPointsText;
    public GameObject PlusPoints;
    public GameObject MinusPoints;
    public TextMeshProUGUI PlusPointsText;
    public TextMeshProUGUI MinusPointsText;

    public GameObject ScoreTable;

    public float abilitytimer = 10f;
    public Image abilityfill;
    public Image abilityhighlight;

    private bool secondstage = false;
    public static bool firsttime = true;
    


    public GameObject Panel;
    public GameObject SettingsPanel;

    public GameObject TwiceSpeedText;
    public GameObject StreakText;
    

    public enum BallType
    {
        Normal,
        Bouncy,
        Heavy
    }

    public GameObject GenerateBall(Vector3 spawnPos)
    {
        GameObject ballPrefab = null;
        BallType type = BallType.Normal;

        if (again)
        {
       
            if (FPcontroller.currentBall != null)
            {
                type = FPcontroller.currentBall.GetComponent<Ball>().ballType;
            }
            ballPrefab = GetBallPrefab(type);
            if (ballPrefab != null)
            {
                GameObject spawnedBall = Instantiate(ballPrefab, spawnPos, Quaternion.identity);
                Ball ballScript = spawnedBall.GetComponent<Ball>();
                ballScript.ballType = type;
                spawnedBall.GetComponent<Collider>().enabled = false;
                spawnedBall.GetComponent<Rigidbody>().detectCollisions = false;
                spawnedBall.GetComponent<Rigidbody>().isKinematic = true;
                FPcontroller.currentBall = spawnedBall;
                FPcontroller.hasball = true;
                FPcontroller.SetBallTransparency(spawnedBall, 0.4f);
                return spawnedBall;
            }
        }
        else
        {
           
            if (hitcounter >= 3)
            {
                ballPrefab = bouncyBall;
                hitcounter = 0;
                type = BallType.Bouncy;
            }
            else if (misscounter >= 3)
            {
                ballPrefab = heavyBall;
                misscounter = 0;
                type = BallType.Heavy;
            }
            else
            {
                ballPrefab = normalBall;
                type = BallType.Normal;
            }
            if (ballPrefab != null)
            {
                var spawnedBall = Instantiate(ballPrefab, spawnPos, Quaternion.identity);
                var ballScript = spawnedBall.GetComponent<Ball>();
                ballScript.ballType = type;

                return spawnedBall;
            }
        }

        

        return null;
    }

    private GameObject GetBallPrefab(BallType type)
    {
        return type switch
        {
            BallType.Normal => normalBall1,
            BallType.Bouncy => bouncyBall1,
            BallType.Heavy => heavyBall1,
            _ => normalBall,
        };
    }
    public void AddScore(int points,BallType type)
    {
        currentscore ++;
        ScoreTable.GetComponent<TextMeshPro>().text = currentscore.ToString();
        if (type == BallType.Normal)
        {
            hitcounter++;
        }
        scoresinarow++;
        if (scoresinarow >= 3)
        {
            StreakText.GetComponent<TextMeshProUGUI>().text = "Streak x" + scoresinarow.ToString();
            StreakText.SetActive(true);
        }
        currentTime += points;
        PlusPoints.SetActive(false);
        MinusPoints.SetActive(false);
        StartCoroutine(ShowPositiveScore(points));
        
        Debug.Log("Scored! +" + points + " points. Total Score: " + currentscore);
    }

    public void Miss(BallType type,bool isitfirst)
    {
        if (!isitfirst)
        {
            if (type == BallType.Normal)
            {
                misscounter++;
                
            }
            if (type == BallType.Heavy)
            {
                currentTime -= 10;
                PlusPoints.SetActive(false);
                MinusPoints.SetActive(false);
                StartCoroutine(ShowNegativeScore(10));
            }
            /*else if (type == BallType.Bouncy || type == BallType.Normal)
            {
                currentTime -= 5;
                PlusPoints.SetActive(false);
                MinusPoints.SetActive(false);
                StartCoroutine(ShowNegativeScore(5));
            }*/
                
                
            scoresinarow = 0; // Reset streak if missed
            StreakText.SetActive(false);
        }
        
        
        
        Debug.Log("Missed! Current misses: " + misscounter);
    }

    IEnumerator BiggerRim()
    {
        currentTime -= 5;

        PlusPoints.SetActive(false);
        MinusPoints.SetActive(false);
        StartCoroutine(ShowNegativeScore(5));
        
        abilityhighlight.enabled = false;
        float endTime = 3f;

        abilitytimer = 0;
        
        abilityfill.fillAmount = 0;
        oncooldown = true;
        Pota.SetActive(false);
        BuyukPota.SetActive(true);

        while (abilitytimer < endTime)
        {
            abilitytimer += Time.deltaTime;
            abilityfill.fillAmount = abilitytimer / 10f;
            yield return null;
        }

        Pota.SetActive(true);
        BuyukPota.SetActive(false);

        
        endTime = 10f;

        while (abilitytimer < endTime)
        {
            abilitytimer += Time.deltaTime;
            abilityfill.fillAmount = abilitytimer / 10f;
            yield return null;
        }

        abilityhighlight.enabled = true;
        oncooldown = false;
        
    }

    void Update()
    {
        
        if (gameRunning)
        {
            if (!FPcontroller.closed)
            {
                float timeReduction = secondstage ? Time.deltaTime * 2 : Time.deltaTime;
                currentTime = Mathf.Max(currentTime - timeReduction, 0f);
                UpdateTimerUI();
            }
            if (currentTime <= 0f)
            {
                EndGame();
            }
            
            if (Input.GetKeyDown(KeyCode.E) && !oncooldown)
            {
                StartCoroutine(BiggerRim());
            }
            

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscMenu();
        }

    }

    void Start()
    {
        DynamicGI.UpdateEnvironment();
        Invoke(nameof(OneMinutePassed), 60f);
        

    }
    private void UpdateTimerUI()
    {
        int seconds = Mathf.FloorToInt(currentTime);
        timerText.text = seconds.ToString();
    }

    private void EndGame()
    {
        Time.timeScale = 1f;
        gameRunning = false;
        UI.enabled = false;
        ESC.SetActive(false);
        FinalPointsText.text = currentscore.ToString ();
        LoseScreen.SetActive(true);
        if (Sunshine.currentMusic != null)
            Sunshine.currentMusic.Stop();
        Sunshine.mainMusic.Stop();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Player.GetComponent<Rigidbody>().isKinematic = true;
        Debug.Log("Game Over! Final Score: " + currentscore);
        
    }
    private void EscMenu()
    {

        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        ESC.SetActive(!ESC.activeSelf);

        gameRunning = !gameRunning;
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = !Cursor.visible;
        Player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        if (ESC.activeSelf == false)
        {
            SettingsPanel.SetActive(false);
            Panel.SetActive(true);
        }
        

    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        Sunshine.restarted = true;
        SceneManager.LoadScene(1);

    }

    private IEnumerator ShowPositiveScore(int score)
    {
        PlusPointsText.text = "+ " + score.ToString();
        PlusPoints.SetActive(true);
        yield return new WaitForSeconds(1);
        PlusPoints.SetActive(false);
    }
    private IEnumerator ShowNegativeScore(int score)
    {
        MinusPointsText.text = "- " + score.ToString();
        MinusPoints.SetActive(true);
        yield return new WaitForSeconds(1) ;
        MinusPoints.SetActive(false);
    }

    public void CloseGame()
    {
        Time.timeScale = 1f;
        firsttime = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
        
    }
    private void OneMinutePassed()
    {
        
        secondstage = true;
        TwiceSpeedText.SetActive(true);
        
    }

    public void OpenSettings()
    {
        Panel.SetActive(false);
        SettingsPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void BackfromSettings()
    {
        SettingsPanel.SetActive(false);
        Panel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

}
