using Assets.Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    public List<GameObject> Infos;

    private PlayerController _pl;
    private CameraController _cm;
    private SceneLoader _sm;
    private WeaponController _wc;

    private int _c;

    // Start is called before the first frame update
    void Start()
    {
        _c = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("x"))
        {
            if (_c == -1)
            {
                GetComponent<Image>().enabled = false;
                _c++;
                Infos[_c].SetActive(true);
                Infos[_c].transform.parent.SetAsLastSibling();
            }
            else
            {
                Infos[_c].SetActive(false);
                _c++;
                if (_c == Infos.Count)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(1).gameObject.SetActive(false);
                    enabled = false;
                }
                else
                {
                    Infos[_c].SetActive(true);
                    Infos[_c].transform.parent.SetAsLastSibling();
                }
            }
        }
    }
}
