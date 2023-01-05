using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienScript : MonoBehaviour
{

    public Rigidbody2D rb;
    public Vector2 direction;
    public float speed;
    public float shootingDelay;
    public float lastTimeShot = 0f;
    public float BulletSpeed;

    public Transform player;
    public GameObject bullet;

    public GameObject explosion;

    public SpriteRenderer spriteRenderer;
    public Collider2D collider;

    public bool disabled;
    public int points;

    public float timeBeforeSpawning;

    public Transform startPosition;

    public int currentLevel = 0;

    public float rot = 500f;




    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        NewLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if(disabled)
        {
            return;
        }

       

        if (player == null)
        {
            return;
        }
        else
        {
            Vector3 dir = player.position - transform.position;
            dir.Normalize();

            float zed = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

            Quaternion suprot = Quaternion.Euler(0, 0, zed);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, suprot, rot * Time.deltaTime);

            if (Time.time > lastTimeShot + shootingDelay)
            {
                GameObject newBullet = Instantiate(bullet, transform.position, suprot);

                newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0f, BulletSpeed));

                lastTimeShot = Time.time;
                Destroy(newBullet, 8.0f);
            }

        }

       
    }

    private void FixedUpdate()
    {
        if (disabled)
        {
            return;
        }

        direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }


    public void Enable()
    {
        transform.position = startPosition.position;

        collider.enabled = true;
        spriteRenderer.enabled = true;

        disabled = false;
    }

    public void NewLevel()
    {
        Disable();
        currentLevel++;

        timeBeforeSpawning = Random.Range(5f, 20f);
        
        Invoke("Enable", timeBeforeSpawning);

        speed = currentLevel;
        BulletSpeed = (250 * currentLevel)/2;

        points = 100 * currentLevel;
    }


    public void Disable()
    {
        collider.enabled = false;
        spriteRenderer.enabled = false;

        disabled = true;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("bullet"))
        {
            player.SendMessage("ScorePoints", points);

            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(newExplosion, 3f);
            Disable();
        }
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.CompareTag("Player"))
        {
            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(newExplosion, 3f);
            Disable();
        }
    }
}
