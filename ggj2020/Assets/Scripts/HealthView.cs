using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HealthView : MonoBehaviour
{

    public GameObject HealthIcon;

    private List<GameObject> _icons;

    public void Bind(Level level)
    {
        _icons = new List<GameObject>();
        for (int i = 0; i < level.Health.Value; i++)
        {
            _icons.Add(Instantiate(HealthIcon, transform));
        }

        level.Health.Subscribe(ev => 
        {
            if(ev < _icons.Count && _icons.Count > 0)
            {
                Destroy(_icons[0]);
                _icons.RemoveAt(0);
            }
            else if(ev > _icons.Count)
            {
                _icons.Add(Instantiate(HealthIcon, transform));
            }
        });
    }
}
