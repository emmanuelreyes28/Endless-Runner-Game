using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //sounds effects 
    private AudioSource coinSound;
    private AudioSource fuelCollectSound;
    private AudioSource explosionSound;
    private AudioSource shuttleSound;
    //speed of ball
    public float speed = 10f;
    Rigidbody ballRB;
    Vector3 ballVelocity;
    //public TextMesh coinText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI diedText;
    public GameObject playAgainText;
    public GameObject mainMenuText;
    public GameObject quitGameText;
    private int score = 0;
    public GameObject player;
    public GameObject explosionEffect;
    public HealthBar fuelBar;
    private int maxFuel = 100;
    private int currentFuel;
    public static bool isFuelDeactivated = false;
    public static bool playerDied = false;
    private IEnumerator coroutine;
    private float time;
    private Transform playerChild;

    // Start is called before the first frame update
    void Start()
    {
        currentFuel = maxFuel;
        fuelBar.SetMaxHealth(maxFuel);

        ballRB = GetComponent<Rigidbody>();

        AudioSource[] audios = GetComponents<AudioSource>();
        coinSound = audios[0];
        explosionSound = audios[1];
        fuelCollectSound = audios[2];

        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();

        fuelText.enabled = false;
        diedText.enabled = false;
        playAgainText.SetActive(false);
        mainMenuText.SetActive(false);
        quitGameText.SetActive(false);

        playerChild = this.transform.GetChild(0);
        shuttleSound = playerChild.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ballVelocity = ballRB.velocity;

        time += Time.deltaTime;

        //fuel reduces every 5 secs.
        if(time >= 5)
        {
            LoseFuel(20);
            time = 0f;
        }

        UpdateScore();
    }

    void FixedUpdate()
    {
        //user input moving left/right
        float horizontalMove = Input.GetAxis("Horizontal") * speed;
        horizontalMove *= Time.deltaTime;
        transform.Translate(horizontalMove, 0, 0);

        //keep constant velocity on ball
        ballRB.velocity = Vector3.forward * speed;

        //destroy ball if it falls off
        if(this.transform.position.y < 0.30f)
        {
            ballRB.velocity = Vector3.forward * 0;
            Destroy(gameObject);
            diedText.enabled = true;
            playAgainText.SetActive(true);
            mainMenuText.SetActive(true);
            quitGameText.SetActive(true);
            Time.timeScale = 0;
        }
    }

    void AddFuel(int fuel)
    {
        currentFuel += fuel;
        fuelBar.SetHealth(currentFuel);
    }

    void LoseFuel(int fuel)
    {
        currentFuel -= fuel;
        fuelBar.SetHealth(currentFuel);
    
        if(currentFuel <= 0)
        {
            shuttleSound.Stop();

            //stop game and show text
            fuelText.enabled = true;
            playAgainText.SetActive(true);
            mainMenuText.SetActive(true);
            quitGameText.SetActive(true);
            Time.timeScale = 0;
        }
    }

    void UpdateScore()
    {
        if(score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScoreText.text = "High Score: " + score.ToString(); 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Obstacle")
        {
            explosionEffect.SetActive(true);
            explosionSound.Play();
            speed = 0;
            Destroy(player);

            diedText.enabled = true;
            playAgainText.SetActive(true);
            mainMenuText.SetActive(true);
            quitGameText.SetActive(true);

            //wait 3 seconds and then enable text
            playerDied = true;
            coroutine = Wait(3.0f);
            StartCoroutine(coroutine);
        }
        else if(other.gameObject.tag == "coin")
        {
            //increase player's coin score 
            other.gameObject.SetActive(false);
            coinSound.Play();
            score++;
            coinText.text = "COINS: " + score;
        }
        else if(other.gameObject.tag == "fuel")
        {
            //increase fuel 
            AddFuel(20);
            fuelCollectSound.Play();
            isFuelDeactivated = true;
        }
        
    }

    void OnCollisionStay(Collision other)
    {
       if(other.gameObject.tag == "floor")
        {
            ballRB.constraints = RigidbodyConstraints.FreezePositionY;
            ballRB.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    void OnCollisionExit()
    {
        //disable y pos constraint when ball no longer touching floor
        ballRB.constraints = RigidbodyConstraints.None;
    }

    private IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Time.timeScale = 0;
    }

}
