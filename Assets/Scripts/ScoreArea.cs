using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreArea : MonoBehaviour
{
    public GameManager gameManager;
    public GameManager.BallType type;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ThrownBall"))
        {
            if (other.transform.position.y > transform.position.y)
            {
                Ball ballScript = other.GetComponent<Ball>();
                type = ballScript.ballType;
                if (ballScript != null)
                {
                    gameManager.AddScore(ballScript.ballValue,type);
                    other.tag = "ScoredBall";
                }
            }
        }
    }
}
