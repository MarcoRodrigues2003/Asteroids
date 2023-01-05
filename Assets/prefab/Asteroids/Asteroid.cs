using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float maxthrust;
    public float maxtorque;
    public Rigidbody2D rb;

    public float screentop;
    public float screenbottom;
    public float screenleft;
    public float screenright;

    public int asteroidsize;

    public GameObject asteroidMedium;
    public GameObject asteroidSmall;

    public int points;
    public GameObject player;

    public GameObject explosion;

    public GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        Vector2 thrust = new Vector2(Random.Range(-maxthrust, maxthrust), Random.Range(-maxthrust, maxthrust));
        float torque = Random.Range(-maxtorque, maxtorque);


        rb.AddForce(thrust);
        rb.AddTorque(torque);

        player = GameObject.FindWithTag("Player");

        gm = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newpos = transform.position;

        if (transform.position.y > screentop)
        {
            newpos.y = screenbottom;
        }

        if (transform.position.y < screenbottom)
        {
            newpos.y = screentop;
        }

        if (transform.position.x < screenleft)
        {
            newpos.x = screenright;
        }

        if (transform.position.x > screenright)
        {
            newpos.x = screenleft;
        }

        transform.position = newpos;

    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.CompareTag("bullet"))
        {
            Destroy(other.gameObject);

            if(asteroidsize == 3)
            {
                Instantiate(asteroidMedium, transform.position, transform.rotation);
                Instantiate(asteroidMedium, transform.position, transform.rotation);

                gm.UpdateNumberOfAsteroids(1);

            }
            else if(asteroidsize == 2)
            {
                Instantiate(asteroidSmall, transform.position, transform.rotation);
                Instantiate(asteroidSmall, transform.position, transform.rotation);

                gm.UpdateNumberOfAsteroids(1);

            }
            else if(asteroidsize == 1)
            {
                gm.UpdateNumberOfAsteroids(-1);
            }

            player.SendMessage("ScorePoints",points);

            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(newExplosion, 3f);
            
            Destroy(gameObject);
        }
        

    }


}
