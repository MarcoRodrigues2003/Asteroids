using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerMovement : MonoBehaviour
{

    public Rigidbody2D rb;
    public float thrust;
    public float turnthrust;
    private float thrustinput;
    private float turninput;
    public float screentop;
    public float screenbottom;
    public float screenleft;
    public float screenright;


    public GameObject Bullet;
    public float bulletforce;

    public float deathforce;

    public int score;
    public int lives;

    public Text scoreText;
    public Text livesText;

    public AudioSource audio;
    public GameObject explosion;

    public Color incolor;
    public Color normalcolor;

    public GameObject gameoverpanel;
    public GameObject newHighScorePanel;
    public InputField highScoreInput;
    public Text highScoreListText;

    public SpriteRenderer spriteRenderer;
    public Collider2D collider;
    private bool hyperspace;

    public AlienScript alien;

    public GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        hyperspace = false;
        score = 0;
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;

    }

    // Update is called once per frame
    void Update()
    {
        thrustinput = Input.GetAxis("Vertical") * thrust;
        turninput = Input.GetAxis("Horizontal") * turnthrust;


        if(Input.GetButtonDown("Fire1"))
        {
            
            GameObject newBullet= Instantiate(Bullet, transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletforce);
            Destroy(newBullet, 2.0f);
        }


        if(Input.GetButtonDown("Hyperspace") && !hyperspace)
        {
            hyperspace = true;
            spriteRenderer.enabled = false;
            collider.enabled = false;
            Invoke("Hyperspace", 1f);
        }


        //Rodar nave
        transform.Rotate(Vector3.forward * turninput * -turnthrust * Time.deltaTime);


        Vector2 newpos = transform.position;

        if(transform.position.y > screentop)
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


    private void FixedUpdate()
    {
        rb.AddRelativeForce(Vector2.up * thrustinput);
    }

    void ScorePoints(int points_to_add)
    {
        score += points_to_add;
        scoreText.text = "Score: " + score;
    }


    void Respawn()
    {
        rb.velocity = Vector2.zero;

        transform.position = Vector2.zero;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = true;
        sr.color = incolor;

        Invoke("Invulnerable", 3f);


    }

    void Invulnerable()
    {
        collider.enabled = true;
        spriteRenderer.color = normalcolor;
    }


    void Hyperspace()
    {
        Vector2 newPosition = new Vector2(Random.Range(-12f, 12f), Random.Range(-9f, 9));
        transform.position = newPosition;

        spriteRenderer.enabled = true;
        collider.enabled = true;

        hyperspace = false;
    }


    void loselife()
    {
        GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(newExplosion, 4f);

        lives--;
        livesText.text = "Lives: " + lives;

        spriteRenderer.enabled = false;
        collider.enabled = false;

        Invoke("Respawn", 3f);


        if (lives <= 0)
        {
            GameOver();
        }
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        audio.Play();

        if(col.relativeVelocity.magnitude > deathforce)
        {

            loselife();
           
        }
        else
        {
            audio.Play();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("AlienBullet"))
        {
            loselife();
            alien.Disable();
            Invoke("EnableAlien", 10f);
        }
    }


    void GameOver()
    {
        CancelInvoke();
        

        if(gm.checkhighscore(score))
        {
            newHighScorePanel.SetActive(true);
        }
        else
        {
            gameoverpanel.SetActive(true);
            highScoreListText.text = "HIGH SCORES!" + "\n" + "\n" + PlayerPrefs.GetString("highscorename") + " " + PlayerPrefs.GetInt("highscore");
        }
    }


    public void HighScoreInput()
    {
        string newInput = highScoreInput.text;
        newHighScorePanel.SetActive(false);
        gameoverpanel.SetActive(true);
        PlayerPrefs.SetString("highscorename",newInput);
        PlayerPrefs.SetInt("highscore",score);
        highScoreListText.text = "HIGH SCORE!" + "\n" + "\n" + PlayerPrefs.GetString("highscorename") + " " + PlayerPrefs.GetInt("highscore");

    }

    public void Playagain()
    {
        SceneManager.LoadScene("Game");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void EnableAlien()
    {
        alien.Enable();
    }
}
