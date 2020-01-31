using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public int RoomCount;
    public int CurrentRoom;
    public float RoomOffset;

    private float _targetPos;

    private void Start()
    {
        _targetPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("e") && CurrentRoom < RoomCount - 1)
        {
            _targetPos += RoomOffset;
            CurrentRoom++;
        }
        else if(Input.GetKeyDown("q") && CurrentRoom > 0)
        {
            _targetPos -= RoomOffset;
            CurrentRoom--;
        }
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, _targetPos, Time.deltaTime * 5), transform.position.y, transform.position.z);
    }
}
