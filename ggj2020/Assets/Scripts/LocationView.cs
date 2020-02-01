using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System;

public class LocationView : MonoBehaviour
{

    public GameObject StartLocationSlot;
    public GameObject MidLocationSlot;
    public GameObject EndLocationSlot;
    public GameObject LocationConnector;

    private List<Transform> _slots;
    private IDisposable _d;

    public void Bind(CameraController controller)
    {
        _slots = new List<Transform>();

        _slots.Add(Instantiate(StartLocationSlot, transform).transform);
        Instantiate(LocationConnector, transform);
        for (int i = 1; i < controller.RoomCount - 1; i++)
        {
            _slots.Add(Instantiate(MidLocationSlot, transform).transform);
            Instantiate(LocationConnector, transform);
        }
        _slots.Add(Instantiate(EndLocationSlot, transform).transform);

        _d = controller.CurrentRoom.Subscribe(cur =>
        {
            foreach (var slot in _slots)
            {
                slot.GetChild(0).gameObject.SetActive(false);
            }
            _slots[cur].GetChild(0).gameObject.SetActive(true);
        });   
    }

    private void OnDestroy()
    {
        _d?.Dispose();
    }
}
