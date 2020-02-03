using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using System;
public class SceneManagement : Singleton<SceneManagement>
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

    protected override void Awake()
    {
        base.Awake();
        foreach (AudioSource a in gameObject.GetComponents<AudioSource>())
        {
            dimThemes.Add(a);
        }
    }
    private void Start()
    {

        Counter.Value = 0;
        SceneManager.sceneLoaded += OnSceneLoaded;
        dimThemes[0].Play();
        MessageBroker.Default.Receive<PlayerDamagedEvent>().Subscribe(ev => { dimThemes[CurrentRoom].Stop(); CurrentRoom = 0; PlayAmbient(); });
    }

    void Update()
    {
        if (FindObjectOfType<Tutorial>() && FindObjectOfType<Tutorial>().enabled)
            return;

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
            PlayAmbient();
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
            PlayAmbient();
        }
    }

    private void PlayAmbient()
    {
        dimThemes[CurrentRoom].time = currentSec;
        dimThemes[CurrentRoom].Play();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        newScene = true;
        CurrentRoom = 0;
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
