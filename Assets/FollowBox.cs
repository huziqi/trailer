using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBox : MonoBehaviour
{
    public bool follow_rotation = false;

    private Rigidbody2D person;
    private Rigidbody2D father;
    public Vector2 pos;
    // Start is called before the first frame update
    void Start()
    {
        person = GetComponent<Rigidbody2D>();
        father = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        person.transform.localPosition = pos;
        if (follow_rotation) person.transform.localRotation = new Quaternion(0, 0, 0, 0);
    }
}
