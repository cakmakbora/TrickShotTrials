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


    public GameObject normalBall;
    public GameObject bouncyBall;
    public GameObject heavyBall;

    public GameObject normalBall1;
    public GameObject bouncyBall1;
    public GameObject heavyBall1;

    public FPSController FPcontroller;
    public GameObject Player;

    public GameObject LoseScreen;

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
        if (type == BallType.Normal)
        {
            hitcounter++;
        }
        scoresinarow++;
        currentTime += points;
        
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
                currentTime -= 10;
            scoresinarow = 0; // Reset streak if missed
        }
        
        
        
        Debug.Log("Missed! Current misses: " + misscounter);
    }

    IEnumerator BiggerRim()
    {
        oncooldown = true;
        Pota.SetActive(false);
        BuyukPota.SetActive(true);
        yield return new WaitForSeconds(3);
        Pota.SetActive(true);
        BuyukPota.SetActive(false);
        yield return new WaitForSeconds(7);
        oncooldown = false;
    }

    void Update()
    {
        
        if (gameRunning)
        {
            if (Input.GetKeyDown(KeyCode.E) && !oncooldown)
            {
                StartCoroutine(BiggerRim());
            }
            currentTime -= Time.deltaTime;
            currentTime = Mathf.Max(currentTime, 0f); // clamp at 0
            UpdateTimerUI();

            if (currentTime <= 0f)
            {
                EndGame();
            }
        }
    }
    private void UpdateTimerUI()
    {
        int seconds = Mathf.FloorToInt(currentTime);
        timerText.text = "Time: " + seconds.ToString();
    }

    private void EndGame()
    {
        gameRunning = false;
        LoseScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Player.GetComponent<Rigidbody>().velocity = Vector3.zero;  
        Debug.Log("Game Over! Final Score: " + currentscore);
        // You can show a UI panel here or load an end scene
    }
    public void RestartGame()
    {

        SceneManager.LoadScene(0);
    }

}
