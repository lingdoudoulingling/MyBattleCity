using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour {
    //引用
    public Sprite fail;
    public GameObject explosion;
    public AudioClip dieAudio; 

    private SpriteRenderer sr;
    
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
	}
	private void Die()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Invoke("Fall",0.4f);
    }
    private void Fall()
    {
        sr.sprite = fail;
        GameManager.Instance.isDefeated = true;
        AudioSource.PlayClipAtPoint(dieAudio, transform.position);
        
    }
}
