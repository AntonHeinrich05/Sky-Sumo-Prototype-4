using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed = 10f;
    

    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
       


        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
        
        
    }
}
