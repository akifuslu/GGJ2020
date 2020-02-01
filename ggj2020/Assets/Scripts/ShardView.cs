using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ShardView : MonoBehaviour
{
    private Text _text;

    public void Bind(Level level)
    {
        _text = GetComponent<Text>();
        level.Counter.Subscribe(ev => 
        {
            _text.text = ev + "/" + level.CollectCount;
        });
    }
}
