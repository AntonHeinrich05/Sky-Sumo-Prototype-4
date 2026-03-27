using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isBoss;


    public float speed = 5f;
    public float knockBackStrength = 5f;

    private GameObject player;
    private Rigidbody enemyRb;
    public GameObject explosion;

    private SpawnManager spawnManager;
    public float spawnIntervall = 2;
    private float nextSpawn;

    //Audio
    private AudioSource enemyAudio;
    public AudioClip[] audioClips;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAudio = GetComponent<AudioSource>();
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        if (isBoss)
        {
            spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetVector = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(targetVector * speed);

        if (transform.position.y < -4)
        {
            Destroy(gameObject);
            Instantiate(explosion, transform.position, explosion.gameObject.transform.rotation);
        }

        if (isBoss)
        {
            if (Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnIntervall;
                spawnManager.SpawnMiniEnemy();
            }

        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyAudio.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
            Rigidbody otherEnemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 knockBackDirection = collision.gameObject.transform.position - transform.position;

            // the right one
            //otherEnemyRb.AddForce(-knockBackDirection * knockBackStrength, ForceMode.Impulse);
            
            //accidantly funy
            enemyRb.AddForce(-knockBackDirection * knockBackStrength, ForceMode.Impulse);
        }
    }
}
