using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Pota;
    public GameObject BuyukPota;

    private bool oncooldown = false;

    
    public int scoresinarow;
    public int hitcounter;
    public int misscounter;
    public int currentscore;

    public GameObject normalBall;
    public GameObject bouncyBall;
    public GameObject heavyBall;

    public enum BallType
    {
        Normal,
        Bouncy,
        Heavy
    }

    public GameObject GenerateBall( Vector3 spawnPos)
    {
        GameObject ballPrefab = null;
        BallType type;

        if (hitcounter == 3)
        {
            ballPrefab = bouncyBall;
            hitcounter = 0;
            type = BallType.Bouncy;
        }
        else if (misscounter == 3)
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
            Quaternion spawnRot = Quaternion.Euler(0, 0, 0);
            GameObject spawnedBall = Instantiate(ballPrefab, spawnPos, spawnRot);
            Ball ballScript = spawnedBall.GetComponent<Ball>();
            ballScript.ballType = type; // Assign type
            return spawnedBall;
        }

        return null;

    }
    public void AddScore(int points,BallType type)
    {
        currentscore += points;
        if (type == BallType.Normal)
        {
            hitcounter++;
        }
        scoresinarow++;
        Debug.Log("Scored! +" + points + " points. Total Score: " + currentscore);
    }

    public void Miss(BallType type)
    {
        if (type == BallType.Normal)
        {
            misscounter++;
        }
        scoresinarow = 0; // Reset streak if missed
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
        if (Input.GetKeyDown(KeyCode.E) && !oncooldown)
        {
            StartCoroutine(BiggerRim());
        }
    }
}
