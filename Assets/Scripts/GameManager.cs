using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Pota;
    public GameObject BuyukPota;

    private bool oncooldown = false;

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
