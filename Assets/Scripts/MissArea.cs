using UnityEngine;
using System.Collections;

public class MissArea : MonoBehaviour
{

    IEnumerator DestroyBall(GameObject ball)
    {
        Vector3 spawnpos = Vector3.zero;
        gameManager.GenerateBall(spawnpos);
        yield return new WaitForSeconds(3);  
        Destroy(ball);
    }
    public GameManager gameManager;
    public GameManager.BallType type;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ThrownBall"))
        {
            
            if (collision.transform.position.y > transform.position.y)
            {
                Ball ballScript = collision.gameObject.GetComponent<Ball>();
                type = ballScript.ballType;
                gameManager.Miss(type);
                collision.gameObject.tag = "MissedBall";  
            }
            StartCoroutine(DestroyBall(collision.gameObject));
        }
        if (collision.gameObject.CompareTag("ScoredBall"))
        {
            StartCoroutine(DestroyBall(collision.gameObject));
        }



    }

    
}

