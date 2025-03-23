using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slower : MonoBehaviour
{
    public float slowFactor = 0.5f;
    public float bouncyslow = 0.3f;
    public GameManager.BallType type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ThrownBall"))
        {

            Rigidbody rb = other.GetComponent<Rigidbody>();
            type = other.GetComponent<Ball>().ballType;
            if (rb != null && type != GameManager.BallType.Bouncy)
            {
                
                rb.velocity *= slowFactor;
                rb.angularVelocity *= slowFactor; 
            }
            else if (rb != null && type == GameManager.BallType.Bouncy)
            {
                rb.velocity *= bouncyslow;
                rb.angularVelocity *= bouncyslow;
            }
        }
        

    }
}
