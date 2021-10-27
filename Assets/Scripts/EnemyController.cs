using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 1f;
    [SerializeField] private int life = 6;
    [SerializeField] private int range;
    
    [SerializeField] private GameObject dieParticle;

  
    


    private bool startFollowing = false;
    
    void Update()
    {
        if ((transform.position - player.position).sqrMagnitude < range * range)
        {
            startFollowing = true;
        }

        if (startFollowing == true)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
            transform.LookAt(player.position);
        }

       
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("bullet"))
        {
            life--;
            if (life == 0)
            {
                Instantiate(dieParticle, transform.position+ new Vector3(0,1f,0), Quaternion.identity);
                Destroy(gameObject);
                
            }
        }
    }
}
