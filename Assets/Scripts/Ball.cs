using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameManager.BallType ballType;
    public int ballValue; 

    private void Start()
    {
        
        switch (ballType)
        {
            case GameManager.BallType.Normal:
                ballValue = 5;
                break;
            case GameManager.BallType.Bouncy:
                ballValue = 10;
                break;
            case GameManager.BallType.Heavy:
                ballValue = 7;
                break;
        }
    }
}
