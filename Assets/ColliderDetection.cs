using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDetection : MonoBehaviour
{
    public int num_collide;
    private bool flag;

    private void Start()
    {
        num_collide = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        num_collide++;
        Debug.Log("num_collide: "+num_collide);
    }
}
