using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts.Utils;

public class LevelEndView : MonoBehaviour
{

    public Text Header;
    public Button Retry;
    public Button NextLevel;
    public Button MainMenu;
    public GameObject Success;
    public GameObject Fail;

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
        Retry.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

        MessageBus.OnEvent<LevelEndedEvent>().Subscribe(ev =>
        {
            Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(t =>
            {
                if(ev.Success)
                {
                    Header.text = "Time crack repaired!";
                    if(!SceneManager.GetActiveScene().name.Equals("Level8"))
                        NextLevel.gameObject.SetActive(true);
                    else
                        Header.text = "You repaired all time cracks!";
                    Success.SetActive(true);
                }
                else
                {
                    Header.text = "You failed!";
                    Retry.gameObject.SetActive(true);
                    Fail.SetActive(true);
                }
                Header.gameObject.SetActive(true);
                MainMenu.gameObject.SetActive(true);
            });
        });
    }    
}
