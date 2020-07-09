using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ResetPosition : MonoBehaviour
{
    private Transform pos;
    public Vector2 startpos= new Vector2(-45f,0f);

    public bool is_over;
    public bool is_begin;

    private float start_time;
    private float end_time;
    public float total_time;

    public int CollideInOneRound;

    public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        pos = GetComponent<Transform>();
        is_over = true;
        this.GetComponent<Movement>().enabled = false;
        button = GameObject.Find("Canvas/Button");
        start_time = Time.fixedTime;
        CollideInOneRound = 0;
        total_time = 0;
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
        total_time = 0;
        CollideInOneRound = 0;
        GetComponentInChildren<ColliderDetection>().num_collide = 0;
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
            CollideInOneRound = GetComponentInChildren<ColliderDetection>().num_collide;
            record(total_time, CollideInOneRound);
            Debug.Log("collide in one round: " + CollideInOneRound);
        }
    }

    void initalizepos()
    {
        this.transform.position = new Vector2(-45f, 0f);
        this.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    void record(float time, int collide)
    {
        FileStream file = new FileStream("GameData.txt", FileMode.Append, FileAccess.Write, FileShare.Write);
        StreamWriter sr = new StreamWriter(file);
        string line;
        line = time + "," + collide;
        sr.WriteLine(line);
        sr.Close();
        file.Close();
    }
}
