using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RocketBehavior : MonoBehaviour
{
    public Transform target;
    private float speed = 15.0f;
    private bool homing;

    public float rocketStrength = 5f;
    private float aliveTimer = 0.75f;

    //audio
    private AudioSource rocketAudio;
    public AudioClip[] audioClips;

    private void Start()
    {
        rocketAudio = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if(homing && target != null)
        {
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.LookAt(target);
        }
    }
    public void Fire(Transform newTarget)
    {
        target = newTarget;
        homing = true;
        Destroy(gameObject, aliveTimer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (target != null)
        {

            if (collision.gameObject.CompareTag("Enemy"))

            {
                collision.gameObject.GetComponent<AudioSource>().PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
                Rigidbody targetRB = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 knockBackDirection = (target.transform.position - transform.position).normalized;
                targetRB.AddForce(knockBackDirection * rocketStrength, ForceMode.Impulse);
                Destroy(gameObject);

            }
        }
    }
}
