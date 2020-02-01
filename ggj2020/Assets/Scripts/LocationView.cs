using UnityEngine;
using UniRx;
using System.Collections.Generic;

public class LocationView : MonoBehaviour
{

    public GameObject LocationSlot;
    public GameObject LocationConnector;
    public GameObject PlayerIndicator;

    private List<Transform> _slots;
    private Transform _indicator;

    public void Bind(CameraController controller)
    {
        _slots = new List<Transform>();
        for(int i = 0; i < controller.RoomCount; i++)
        {
            _slots.Add(Instantiate(LocationSlot, transform).transform);
            if (i != controller.RoomCount - 1)
                Instantiate(LocationConnector, transform);
        }
        _indicator = Instantiate(PlayerIndicator).transform;
        controller.CurrentRoom.Subscribe(cur =>
        {
            _indicator.transform.SetParent(_slots[cur]);
            _indicator.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            _indicator.transform.localScale = Vector3.one;
        });   
    }
}
