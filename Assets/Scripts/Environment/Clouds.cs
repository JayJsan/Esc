using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float speedFactor = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveClouds();
    }

    private void MoveClouds()
    {
        transform.localPosition += new Vector3(speed * speedFactor * Time.deltaTime, 0, 0);
        if (transform.localPosition.x >= 15)
        {
            transform.localPosition = new Vector3(-15, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
