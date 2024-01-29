using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinSound;
    [SerializeField] int coinScore = 100;
    PlayerMovement player;
    bool wasCollected = false;  
    void Start() 
    {
        player = FindObjectOfType<PlayerMovement>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !wasCollected)
        {   
            wasCollected = true;
            FindObjectOfType<GameSession>().Score(coinScore);   
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
            Destroy(gameObject);
        }
    }
}
