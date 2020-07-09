using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    private Transform pos;
    public Vector2 startpos= new Vector2(-45f,0f);

    public bool is_over;
    public bool is_begin;

    private float start_time;
    private float end_time;
    private float total_time;

    public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        pos = GetComponent<Transform>();
        is_over = true;
        this.GetComponent<Movement>().enabled = false;
        button = GameObject.Find("Canvas/Button");
        start_time = Time.fixedTime;
    }

    // Update is called once per frame
    void Update()
    {
        Reset();
    }
    public void OnClick()
    {
        this.GetComponent<Movement>().enabled = true;
        initalizepos();
        is_begin = true;
        is_over = false;
        button.SetActive(false);
        start_time = Time.fixedTime;
    }

    void Reset()
    {
        if (pos.transform.position.x >= GameObject.Find("Background/stopline").transform.position.x)
        {
            this.transform.position = new Vector2(-45f, 0f);
            this.GetComponent<Movement>().enabled = false;
            is_over = true;
            is_begin = false;
            button.SetActive(true);
            end_time = Time.fixedTime;
            total_time = end_time - start_time;
        }
    }

    void initalizepos()
    {
        this.transform.position = new Vector2(-45f, 0f);
        this.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}
