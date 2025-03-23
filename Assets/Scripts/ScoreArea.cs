using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreArea : MonoBehaviour
{
    public GameManager gameManager;
    public GameManager.BallType type;
    public FPSController FPController;
    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.gameRunning)
        {
            if (other.CompareTag("FirstThrownBall"))
            {
                if (other.transform.position.y > transform.position.y)
                {
                    FPController.currentBall = null;
                    Ball ballScript = other.GetComponent<Ball>();
                    type = ballScript.ballType;
                    if (ballScript != null)
                    {
                        int points = ballScript.ballValue + 5;
                        if (gameManager.scoresinarow >= 2)
                            points += 5;

                        gameManager.AddScore(points, type);
                        other.tag = "FirstScoredBall";

                    }
                }
            }
            else if (other.CompareTag("SecondThrownBall"))
            {
                if (other.transform.position.y > transform.position.y)
                {
                    FPController.currentBall = null;
                    Ball ballScript = other.GetComponent<Ball>();
                    type = ballScript.ballType;
                    if (ballScript != null)
                    {
                        int points = ballScript.ballValue;
                        if (gameManager.scoresinarow >= 2)
                            points += 5;

                        gameManager.AddScore(points, type);
                        other.tag = "SecondScoredBall";

                    }
                }
            }
        }
    }
        
}
