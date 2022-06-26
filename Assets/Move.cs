using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 5f;
    public float waitTime = 3f;
    float time;
    Vector3 dir;
    public Transform obj;
    // Start is called before the first frame update
    void Start()
    {
        time = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(time <= 0)
        {
            float randX = Random.Range(-1f, 1);
            float randZ = Random.Range(-1f, 1);
            
            dir = new Vector3(randX, 0, randZ).normalized;
            time = waitTime;
        }
        time -= Time.deltaTime;
        Vector3 deltaMove = dir * speed * Time.deltaTime;
        transform.Translate(deltaMove);
    }
}
