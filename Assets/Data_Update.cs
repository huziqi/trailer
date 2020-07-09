using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class Data_Update : MonoBehaviour
{
    // 记录幕中激活的状态动作组
    public int[,,,] active_state = new int[150, 30, 9, 9];
    public float[,,,] scores = new float[150, 30, 9, 9];

    public int last_state_x;
    public int last_state_y;
    public int current_state_x;
    public int current_state_y;
    public int agent_current_action;
    public int operator_current_action;

    public bool is_first = true;//判断是否为首个状态
    public bool is_record = false;//判断是否记录过本幕数据
    public int count = 0;//计算该幕经过多少状态

    private float Player1control_x;
    private float Player1control_y;
    private float Player2control_x;
    private float Player2control_y;
    // Start is called before the first frame update
    void Start()
    {
        last_state_x = (int)Math.Floor(this.transform.position.x);
        last_state_y = (int)Math.Floor(this.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        //判断游戏是否开始
        if(this.GetComponent<ResetPosition>().is_begin)
        {
            is_record = false;//本幕尚未开始记录
            //首个状态单独存储
            if (is_first)
            {
                //记录operator动作
                Player1control_x = Input.GetAxisRaw("Horizontal");
                Player1control_y = Input.GetAxisRaw("Vertical");
                //记录agent动作
                Player2control_x = Input.GetAxisRaw("Horizontal2");
                Player2control_y = Input.GetAxisRaw("Vertical2");
                //计算对应动作
                operator_current_action = (int)((Player1control_x + 1) * 3 + Player1control_y + 1);
                agent_current_action = (int)((Player2control_x + 1) * 3 + Player2control_y + 1);
                //计算当前状态
                current_state_x = (int)Math.Floor(this.transform.position.x) + 75;
                current_state_y = (int)Math.Floor(this.transform.position.y) + 15;
                //更新激活状态
                active_state[current_state_x, current_state_y, operator_current_action, agent_current_action]++;
                //关闭首次操作
                is_first = false;
                //将当前状态记录到last_state中
                last_state_x = current_state_x;
                last_state_y = current_state_y;
                count++;
            }
            else
            {
                //计算当前状态
                current_state_x = (int)Math.Floor(this.transform.position.x) + 75;
                current_state_y = (int)Math.Floor(this.transform.position.y) + 15;
                if ((current_state_x == last_state_x) && (current_state_y == last_state_y))
                {
                    return;
                }
                else
                {
                    //记录operator动作
                    Player1control_x = Input.GetAxisRaw("Horizontal");
                    Player1control_y = Input.GetAxisRaw("Vertical");
                    //记录agent动作
                    Player2control_x = Input.GetAxisRaw("Horizontal2");
                    Player2control_y = Input.GetAxisRaw("Vertical2");
                    //计算对应动作
                    operator_current_action = (int)((Player1control_x + 1) * 3 + Player1control_y + 1);
                    agent_current_action = (int)((Player2control_x + 1) * 3 + Player2control_y + 1);
                    //更新激活状态
                    active_state[current_state_x, current_state_y, operator_current_action, agent_current_action]++;
                    //利用碰撞对所在状态再次更新(此出碰撞次数的5倍作为惩罚，可以依据情况调整)
                    active_state[current_state_x, current_state_y, operator_current_action, agent_current_action] += this.GetComponent<ResetPosition>().CollideInOneRound * 5;
                    //存储为last状态
                    last_state_x = current_state_x;
                    last_state_y = current_state_y;
                    count++;
                }
            }
        }
        //一幕游戏结束且尚未进行数据记录
        if (this.GetComponent<ResetPosition>().is_over && (!is_record))
        {
            //用以往数据初始化scores
            FileStream file_r = new FileStream("scores.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(file_r);
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
            file_r.Close();
            //用该幕得分更新scores
            FileStream file_w = new FileStream("scores.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(file_w);
            //该幕得分
            float score = 10 / this.GetComponent<ResetPosition>().total_time;
            //对应激活状态的得分增加
            for (int x = 0; x < 150; x++)
            {
                for (int y = 0; y < 30; y++)
                {
                    for (int op_action = 0; op_action < 9; op_action++)
                    {
                        for (int ag_action = 0; ag_action < 9; ag_action++)
                        {
                            if (active_state[x, y, op_action, ag_action] > 0)
                            {
                                float state_score = score / active_state[x, y, op_action, ag_action] + scores[x, y, op_action, ag_action];
                                string record = x + "," + y + "," + op_action + "," + ag_action + "," + state_score;
                                sw.WriteLine(record);
                            }
                            else if (active_state[x, y, op_action, ag_action] == 0 && scores[x, y, op_action, ag_action] > 0)
                            {
                                string record = x + "," + y + "," + op_action + "," + ag_action + "," + scores[x, y, op_action, ag_action];
                                sw.WriteLine(record);
                            }
                        }
                    }
                }
            }
            sw.Close();
            file_w.Close();
            clear();
        }
    }
    void clear()
    {
        active_state.Initialize();
        is_first = true;
        is_record = true;
        last_state_x = (int)Math.Floor(this.transform.position.x);
        last_state_y = (int)Math.Floor(this.transform.position.y);
        count = 0;
    }
}