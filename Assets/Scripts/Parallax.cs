using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    [SerializeField]
    private float velocity;
    [SerializeField]
    private Vector3 direction;
    private float widthImage;

    private Vector3 initPosition;

    
    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
        widthImage = transform.GetChild(0).localPosition.x * transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        //to cover distance between images
        float distance = (velocity * Time.time) % widthImage;
        //Init position plus new distance and direction
        transform.position = initPosition + distance * direction;
    }



}
