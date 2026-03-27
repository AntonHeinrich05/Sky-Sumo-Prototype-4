using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    
    public float playerSpeed = 10f;
    public float knockBackStrength = 5;

    public bool hasPowerUp = false;
    public float powerUpStrength = 30;
    


    

    

    //powerups
    public PowerUpType currentPowerUp = PowerUpType.None;
    public GameObject powerUpIndicator;
    private Coroutine powerupCountdown;

    //rocket PowerUp
    public GameObject rocketPrefab;
    private GameObject tmpRocket;
    

    //smash Powerup
    public float hangTime;
    public float smashSpeed;
    public float explosionForce;
    public float explosionRadius;
    //bool smashing = false;
    float floorY;

    //camera
    private GameObject focalPoint;

    private Rigidbody rb;

    //Audio
    private AudioSource playerAudio;
    public AudioClip[] audioClips;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        Physics.gravity *= 5;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Physics.gravity /= 5;
        }

        if ( Input.GetKeyDown(KeyCode.Space))
        {
            if (currentPowerUp == PowerUpType.Rockets)
            {
                LaunchRockets();
                Debug.Log("Launch Rockets");
            }
            else if (currentPowerUp == PowerUpType.Smash)
            {
                //smashing = true;
                StartCoroutine(Smash());
            }
            
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //bewegung vor zuück kamera richtung  
        float verticalInput = Input.GetAxis("Vertical");
        rb.AddForce(focalPoint.transform.forward * verticalInput * playerSpeed);


        //powerupIndicator zu player position
        powerUpIndicator.gameObject.transform.position = transform.position + new Vector3(0,-0.5f,0);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerUp"))
        {
            hasPowerUp = true;
            currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType;
            powerUpIndicator.gameObject.SetActive(true);

            if(powerupCountdown != null)
            {
                StopCoroutine(powerupCountdown);
            }
            
            
            powerupCountdown = StartCoroutine(PowerUpCountDownRoutine());
            Destroy(other.gameObject);
        }   
    }

    IEnumerator PowerUpCountDownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;

        currentPowerUp = PowerUpType.None;
        powerUpIndicator.gameObject.SetActive(false);
    }

    IEnumerator Smash()
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        floorY = transform.position.y;

        float jumpTime = Time.time + hangTime;


        //move player up
        while (Time.time < jumpTime)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, smashSpeed);
            yield return null;
        }
        //move player down
        while (transform.position.y > floorY)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -smashSpeed * 2);
            yield return null;
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0, ForceMode.Impulse);
        }

            //smashing = false;   
        }


    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerAudio.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);

            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 knockBackDirection = (collision.gameObject.transform.position - transform.position).normalized;

            if (currentPowerUp == PowerUpType.PushBack){
                enemyRb.AddForce(knockBackDirection * powerUpStrength, ForceMode.Impulse);
            }
            else
            {
                enemyRb.AddForce(knockBackDirection * knockBackStrength, ForceMode.Impulse);
            }


                Debug.Log("Collided with" + collision.gameObject.name + "with PowerUp set to" + currentPowerUp.ToString());
        }
    }

    void LaunchRockets()
    {
        foreach (var enemy in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
            {
            tmpRocket = Instantiate(rocketPrefab,transform.position + Vector3.up, rocketPrefab.transform.rotation);
            tmpRocket.GetComponent<RocketBehavior>().Fire(enemy.transform);
        }
    }
}
