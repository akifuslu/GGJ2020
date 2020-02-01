using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelEndView : MonoBehaviour
{

    public Button NextLevel;
    public Button MainMenu;
    private IDisposable _d;

    // Start is called before the first frame update
    void Start()
    {
        NextLevel.onClick.AddListener(() => 
        {
            var s = SceneManager.GetActiveScene().name;
            int i = int.Parse(s.Substring(5));
            SceneManager.LoadScene("Level" + (i + 1).ToString());
        });
        MainMenu.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainMenu");
        });

        _d = MessageBroker.Default.Receive<RepairCrackEvent>().Subscribe(ev =>
        {
            if(FindObjectOfType<Level>().AllCollected)
            {
                Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(t =>
                {
                    foreach (Transform c in transform)
                    {
                        c.gameObject.SetActive(true);
                    }
                });
            }
        });
    }

    private void OnDestroy()
    {
        _d?.Dispose();
    }
}
