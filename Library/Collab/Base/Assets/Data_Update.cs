using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Data_Update : MonoBehaviour
{
    // 记录幕中激活的状态动作组
    public float[,,,] active_state = new float[150, 30, 9, 9];

    public int last_state_x;
    public int last_state_y;
    public int current_state_x;
    public int current_state_y;
    public int agent_current_action;
    public int operator_current_action;

    public bool is_first = true;//判断是否为首个状态

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
            active_state[current_state_x, current_state_y, operator_current_action, agent_current_action] = 1;
            //关闭首次操作
            is_first = false;
            //将当前状态记录到last_state中
            last_state_x = current_state_x;
            last_state_y = current_state_y;
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
                Debug.Log(operator_current_action);
                active_state[current_state_x, current_state_y, operator_current_action, agent_current_action] = 1;
                last_state_x = current_state_x;
                last_state_y = current_state_y;
            }
        }
    }
}