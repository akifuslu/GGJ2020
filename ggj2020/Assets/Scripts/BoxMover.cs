using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMover : MonoBehaviour
{
    bool upDown;
    int speedX;
    int speedY;
    int xConstraint;
    int n_xConstraint;
    [SerializeField]
    int yConstraint;
    [SerializeField]
    int n_yConstraint;
    public GameObject parentObj;

    // Start is called before the first frame update
    void Start()
    {
        n_xConstraint = (int) gameObject.transform.parent.GetChild(0).transform.position.x + 1; 
        xConstraint = (int) gameObject.transform.parent.GetChild(1).transform.position.x - 1;

        yConstraint = (int)gameObject.transform.parent.GetChild(2).transform.position.y - 1;
        n_yConstraint = (int)gameObject.transform.parent.GetChild(3).transform.position.y + 1;

        upDown = Random.RandomRange(0, 2) == 0 ? false : true;
        speedX = (upDown==false) ? Random.RandomRange(1, 4) :  0;
        speedY = (upDown==true) ? Random.RandomRange(1, 4) : 0;

        if (upDown)
        {
            gameObject.GetComponent<Rigidbody>().constraints
                = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;

        }
        else
        {
            gameObject.GetComponent<Rigidbody>().constraints
                = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        }


    }

    // Update is called once per frame
    void Update()
    {
        if(upDown)
        {
            gameObject.transform.position += new Vector3(0, speedY*Time.deltaTime, 0);
            if(gameObject.transform.position.y+0.5f>yConstraint)
            {
                speedY *= (speedY > 0 ? -1 : 1);
            }
            else if(gameObject.transform.position.y - 1 < n_yConstraint)
            {
                speedY *= (speedY < 0 ? -1 : 1);
            }
        }
        else
        {
            gameObject.transform.position += new Vector3(speedX*Time.deltaTime, 0, 0);
            if(gameObject.transform.position.x+0.5f>xConstraint)
            {
                speedX *= (speedX > 0 ? -1 : 1);
            }
            else if (gameObject.transform.position.x - 1 < n_xConstraint)
            {
                speedX *= (speedX < 0 ? -1 : 1);
            }
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected");
        if(collision.gameObject.tag!="Player")
        {
            speedX *= -1;
            speedY*=-1;
        }
        if(collision.gameObject.tag=="Player" && Camera.main.ScreenToWorldPoint(collision.gameObject.transform.position).y
            < Camera.main.ScreenToWorldPoint(gameObject.transform.position).y)
        {
            speedY *= -1;
        }
    }
}
