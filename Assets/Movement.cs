///
/// Movement部分，需要增加数据时打开此部分并关闭Auto部分
///

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Movement : MonoBehaviour
//{
//    [Header("Input the speed")]
//    public float speed;

//    private Rigidbody2D Body;

//    private float Player1control_x;
//    private float Player1control_y;
//    private float Player2control_x;
//    private float Player2control_y;

//    private Vector2 movement;
//    private float rotation;
//    private float theta; //the angle of the box
//    void Start()
//    {
//        Body = GetComponent<Rigidbody2D>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        // input from arrows
//        Player1control_x = Input.GetAxisRaw("Horizontal");
//        Player1control_y = Input.GetAxisRaw("Vertical");
//        //input from AWSD
//        Player2control_x = Input.GetAxisRaw("Horizontal2");
//        Player2control_y = Input.GetAxisRaw("Vertical2");

//        movement = new Vector2(Player1control_x + Player2control_x, Player1control_y + Player2control_y);

//        theta = -Body.transform.rotation.z;
//        //Debug.Log("theta: " + theta);
//        rotation = Player1control_x * Mathf.Sin(theta) - Player2control_x * Mathf.Sin(theta) + Player1control_y * Mathf.Cos(theta) - Player2control_y * Mathf.Cos(theta);
//    }

//    private void FixedUpdate()
//    {
//        Body.AddForce(movement * speed * 10f);
//        Body.AddTorque(rotation * speed);
//    }
//}





///
/// Auto_Movement部分
///
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Random = System.Random;

public class Movement : MonoBehaviour
{
    [Header("Input the speed")]
    public float speed;
    //存储得分矩阵
    public float[,,,] scores = new float[150, 30, 9, 9];
    //存储状态动动作
    public int current_state_x;
    public int current_state_y;
    public int operator_current_action;
    public int agent_current_action;
    //给定epsilon
    public double epsilon = 0.01;

    private Rigidbody2D Body;

    private float Player1control_x;
    private float Player1control_y;
    private float Player2control_x;
    private float Player2control_y;

    private Vector2 movement;
    private float rotation;
    private float theta; //the angle of the box

    void Start()
    {
        Body = GetComponent<Rigidbody2D>();
        //用外部数据初始化scores
        FileStream file = new FileStream("scores.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamReader sr = new StreamReader(file);
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            string[] arr = line.Split(',');
            int x = int.Parse(arr[0]);
            int y = int.Parse(arr[1]);
            int op = int.Parse(arr[2]);
            int ag = int.Parse(arr[3]);
            float s = float.Parse(arr[4]);
            scores[x, y, op, ag] = s;
        }
        sr.Close();
        file.Close();
    }

    // Update is called once per frame
    void Update()
    {
        //记录Operator动作
        Player1control_x = Input.GetAxisRaw("Horizontal");
        Player1control_y = Input.GetAxisRaw("Vertical");
        //计算当前状态
        current_state_x = (int)Math.Floor(this.transform.position.x) + 75;
        current_state_y = (int)Math.Floor(this.transform.position.y) + 15;
        //计算Operator动作
        operator_current_action = (int)((Player1control_x + 1) * 3 + Player1control_y + 1);
        //利用数据选取Agent动作
        agent_current_action = select_action();
        //根据action解码Agentcontrol_x与Agentcontrol_y
        decode(agent_current_action);

        Debug.Log("111" + Player2control_x);
        Debug.Log("222" + Player2control_y);

        movement = new Vector2(Player1control_x + Player2control_x, Player1control_y + Player2control_y);

        theta = -Body.transform.rotation.z;
        //Debug.Log("theta: " + theta);
        rotation = Player1control_x * Mathf.Sin(theta) - Player2control_x * Mathf.Sin(theta) + Player1control_y * Mathf.Cos(theta) - Player2control_y * Mathf.Cos(theta);
    }

    private void FixedUpdate()
    {
        Body.AddForce(movement * speed * 10f);
        Body.AddTorque(rotation * speed);
    }

    int select_action()
    {
        int action;//存储最优动作
        float max = -1;//存储最大数
        ArrayList ties = new ArrayList();//存储值相同动作
        Random rd = new Random();
        if (rd.NextDouble() > epsilon)
        {
            for (int a = 0; a < 9; a++)
            {
                if (scores[current_state_x, current_state_y, operator_current_action, a] > max)
                {
                    max = scores[current_state_x, current_state_y, operator_current_action, a];
                    ties.Clear();
                }
                if (scores[current_state_x, current_state_y, operator_current_action, a] == max)
                {
                    ties.Add(a);
                }
            }
            int num = ties.Count;
            action = (int)ties[rd.Next(0, num)];
            Debug.Log(action);
            Debug.Log(scores[current_state_x, current_state_y, operator_current_action, action]);
        }
        else
        {
            action = rd.Next(0, 9);
        }
        return action;
    }

    void decode(int action)
    {
        if (action == 0) { Player2control_x = -1; Player2control_y = -1; }
        else if (action == 1) { Player2control_x = -1; Player2control_y = 0; }
        else if (action == 2) { Player2control_x = -1; Player2control_y = 1; }
        else if (action == 3) { Player2control_x = 0; Player2control_y = -1; }
        else if (action == 4) { Player2control_x = 0; Player2control_y = 0; }
        else if (action == 5) { Player2control_x = 0; Player2control_y = 1; }
        else if (action == 6) { Player2control_x = 1; Player2control_y = -1; }
        else if (action == 7) { Player2control_x = 1; Player2control_y = 0; }
        else if (action == 8) { Player2control_x = 1; Player2control_y = 1; }
    }
}
