using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Sunshine : MonoBehaviour
{
    public Transform cameraTransform; // Camera Transform
    public AudioSource currentMusic;
    public AudioSource mainMusic;
    public GameObject musicObject;
    public bool inUsage = false;
    public GameManager gameManager;
    public bool isPlaying = false;
    

    private Quaternion targetRotation;
    private float rotationSpeed = 3f;
    void Update()
    {
        if (gameManager.gameRunning)
        {
            if (!isPlaying)
            {
                StartCoroutine(PlayMusic());
            }
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                inUsage = true;
                // Get the current rotation, set x and z to 0, and add 180 to the y rotation
                cameraTransform.rotation = Quaternion.Euler(0, cameraTransform.rotation.eulerAngles.y, 0);

                targetRotation = Quaternion.Euler(0, cameraTransform.rotation.eulerAngles.y + 180, 0);



                currentMusic = musicObject.GetComponent<AudioSource>();
                mainMusic.Stop();
                currentMusic.Play();
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                inUsage = false;
                // Reset camera rotation to original state (x=0, y=0, z=0)
                cameraTransform.rotation = Quaternion.Euler(0, 0, 0);
                currentMusic.Stop();
                isPlaying = false;
                currentMusic = null;
            }

            if (inUsage)
            {
                cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        
    }
    IEnumerator PlayMusic()
    {
        
        isPlaying = true;
        
        mainMusic.Play();

        yield return new WaitForSeconds(mainMusic.clip.length);
        isPlaying = false;

        
            
        
    }
}
