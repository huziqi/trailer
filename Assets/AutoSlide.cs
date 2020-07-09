using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSlide : MonoBehaviour
{
    private Rigidbody2D Body;
    private Vector2 startpos;

    private bool ontop = false;
    private bool onbottom = false;
    void Start()
    {
        Body = GetComponent<Rigidbody2D>();
        startpos = Body.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(!ontop)
        {
            //Body.AddForce(new Vector2(0,1));
            Body.velocity = new Vector2(0, 1f);
            onbottom = true;
            if(Body.transform.localPosition.y-startpos.y>=4.3)
            {
                ontop = true;
                onbottom = false;
            }
        }
        if (!onbottom)
        {
            //Body.AddForce(new Vector2(0, -1));
            Body.velocity = new Vector2(0, -1f);
            ontop = true;
            if (Body.transform.localPosition.y - startpos.y <= -4.3)
            {
                onbottom = true;
                ontop = false;
            }
        }
    }
}
