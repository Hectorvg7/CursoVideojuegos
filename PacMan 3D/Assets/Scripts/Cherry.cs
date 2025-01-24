using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{

public AudioClip eatFruit;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            AudioManager.Instance.PlaySound(eatFruit);
        }
    }
}
