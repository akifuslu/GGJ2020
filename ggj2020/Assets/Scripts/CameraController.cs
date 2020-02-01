using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public int RoomCount;
    public IntReactiveProperty CurrentRoom;
    public float RoomOffset;

    private float _targetPos;
    private LocationView _locationView;

    private void Start()
    {
        _targetPos = transform.position.x;
        _locationView = FindObjectOfType<LocationView>();
        _locationView.Bind(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("e") && CurrentRoom.Value < RoomCount - 1)
        {
            _targetPos += RoomOffset;
            CurrentRoom.Value++;
        }
        else if(Input.GetKeyDown("q") && CurrentRoom.Value > 0)
        {
            _targetPos -= RoomOffset;
            CurrentRoom.Value--;
        }
        
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, _targetPos, Time.deltaTime * 5), transform.position.y, transform.position.z);
    }

    public void Reset()
    {
        _targetPos -= CurrentRoom.Value * RoomOffset;
        CurrentRoom.Value = 0;
    }
}
