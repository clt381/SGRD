using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
        }
    }
}
