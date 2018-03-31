using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePlayAudio : MonoBehaviour {

    public AudioClip audioCilp;
    public string tagOfCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //只有具有指定tag（player bullet）的对象才会播放音效
        if (collision.tag == tagOfCollider) 
        {
            PlayAudio();
           
        }
    }
    private void PlayAudio()
    {
        AudioSource.PlayClipAtPoint(audioCilp, new Vector3(0, 0, -10));
    }
}
