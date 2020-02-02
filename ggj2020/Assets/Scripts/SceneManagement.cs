using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using System;
public class SceneManagement : MonoBehaviour
{
    [SerializeField]
    bool newScene = false;
    [SerializeField]
    float currentSec = 0f;
    int RoomCount = 2;
    int CurrentRoom = 0;
    public int CollectCount;
    public bool AllCollected { get => Counter.Value == CollectCount; }
    public IntReactiveProperty Counter = new IntReactiveProperty(0);
    public AudioSource monsterLaugh;
    [SerializeField]
    List<AudioSource> dimThemes = new List<AudioSource>();
    private static SceneManagement _instance;

    public static SceneManagement Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SceneManagement>();
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        monsterLaugh = gameObject.GetComponents<AudioSource>()[0];
        foreach (AudioSource a in gameObject.GetComponents<AudioSource>())
        {
            dimThemes.Add(a);
        }
        dimThemes.RemoveAt(0);

    }
    private void Start()
    {

        Counter.Value = 0;
        SceneManager.sceneLoaded += OnSceneLoaded;

        MessageBroker.Default.Receive<CollectedEvent>().Subscribe(ev =>
        {
            Counter.Value++;
        });

        MessageBroker.Default.Receive<RepairCrackEvent>().Subscribe(rep =>
        {
            if (Counter.Value != CollectCount)
                return;

            Observable.Timer(TimeSpan.FromSeconds(2.5f)).Subscribe(ti =>
            {
                loadNextScene();
            });
        });
        dimThemes[0].Play();

    }

    void Update()
    {
        if (Input.GetKeyDown("e") && CurrentRoom < RoomCount - 1)
        {
            if (newScene)
            {
                currentSec = 0f;
                newScene = false;
            }
            currentSec = dimThemes[CurrentRoom].time;
            dimThemes[CurrentRoom].Stop();
            CurrentRoom++;
            dimThemes[CurrentRoom].time = currentSec;

            dimThemes[CurrentRoom].Play();
        }
        else if (Input.GetKeyDown("q") && CurrentRoom > 0)
        {
            if (newScene)
            {
                currentSec = 0f;
                newScene = false;
            }
            currentSec = dimThemes[CurrentRoom].time;
            dimThemes[CurrentRoom].Stop();

            CurrentRoom--;
            dimThemes[CurrentRoom].time = currentSec;

            dimThemes[CurrentRoom].Play();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        newScene = true;
        if (scene.buildIndex <= 4)
        {
            RoomCount = 2;
            CollectCount = 1;
        }
        if ((4 < scene.buildIndex) && (scene.buildIndex <= 7))
        {
            RoomCount = 3;
            CollectCount = 2;
        }
        if (7 < scene.buildIndex && (scene.buildIndex <= 8))
        {
            RoomCount = 4;
            CollectCount = 3;
        }
        Counter.Value = 0;
        if (scene.buildIndex > 0)
        {
        }
    }

    public void loadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLevel(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
