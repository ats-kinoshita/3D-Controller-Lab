using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float lift = 20f;
    private Rigidbody playerRB;

    
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float jumpInput = Input.GetAxis("Jump");

        playerRB.AddForce(Vector3.forward * speed * verticalInput * Time.deltaTime, ForceMode.Impulse);
        playerRB.AddForce(Vector3.right * speed * horizontalInput * Time.deltaTime, ForceMode.Impulse);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRB.AddForce(Vector3.up * lift * Time.deltaTime, ForceMode.Impulse);
        }
        
        
        //transform.Translate(Vector3.forward * speed * verticalInput * Time.deltaTime + Vector3.right * speed * horizontalInput * Time.deltaTime);
    }
}
