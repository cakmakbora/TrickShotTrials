using UnityEngine;
using System.Collections;

public class MissArea : MonoBehaviour
{
    public GameObject ballPosition;
    

    
    IEnumerator DestroyBallnGenerate(GameObject ball)
    {
        float randZ = Random.Range(-9.3f, 9.3f);
        float randX = Random.Range(-12f, -1.5f);

        Vector3 spawnpos = new Vector3(randX, 0.4f, randZ);
        gameManager.GenerateBall(spawnpos);
        yield return new WaitForSeconds(3);  
        Destroy(ball);
    }
    IEnumerator DestroyBall(GameObject ball)
    {
        Vector3 spawnpos = ballPosition.transform.position;
        gameManager.GenerateBall(spawnpos);
        yield return new WaitForSeconds(3);
        Destroy(ball);
    }
    public GameManager gameManager;
    public GameManager.BallType type;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FirstThrownBall"))
        {
            
            if (collision.transform.position.y > transform.position.y)
            {
                Ball ballScript = collision.gameObject.GetComponent<Ball>();
                type = ballScript.ballType;
                gameManager.Miss(type,true);
                collision.gameObject.tag = "MissedBall";
                
                gameManager.again = true;
            }
            StartCoroutine(DestroyBall(collision.gameObject));
        }
        if (collision.gameObject.CompareTag("SecondThrownBall"))
        {

            if (collision.transform.position.y > transform.position.y)
            {
                Ball ballScript = collision.gameObject.GetComponent<Ball>();
                type = ballScript.ballType;
                gameManager.Miss(type,false);
                collision.gameObject.tag = "MissedBall";
                gameManager.again = false;

            }
            StartCoroutine(DestroyBallnGenerate(collision.gameObject));
        }
        if (collision.gameObject.CompareTag("FirstScoredBall") || collision.gameObject.CompareTag("SecondScoredBall"))
        {
            collision.gameObject.tag = "Ball";
            
            StartCoroutine(DestroyBallnGenerate(collision.gameObject));
        }
       



    }

    
}

