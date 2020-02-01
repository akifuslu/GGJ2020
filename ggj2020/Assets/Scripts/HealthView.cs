using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{

    public GameObject HealthIcon;

    [SerializeField]
    private List<Image> _hearts;

    public void Bind(Level level)
    {
        _hearts = new List<Image>();
        foreach(Image img in gameObject.GetComponentsInChildren<Image>())
        {
            _hearts.Add(img);
        }
        _hearts.RemoveAt(0);

        level.Health.Subscribe(ev => 
        {
            if(ev < _hearts.Count && _hearts.Count > 0 && ev>=0)
            {

                StartCoroutine( heartFillAmount(_hearts[ev], -1) );
            }
            else if(ev > _hearts.Count)
            {
                StartCoroutine(heartFillAmount(_hearts[ev], 1));

            }
        });
    }

    IEnumerator heartFillAmount(Image img, int sign)
    {
        while(img.fillAmount!=0)
        {
            img.fillAmount += sign*0.5f*Time.deltaTime;
            yield return null;

        }
        img.gameObject.SetActive((sign>0)? true:false);
        img.fillAmount = 1.0f;
    }
}
