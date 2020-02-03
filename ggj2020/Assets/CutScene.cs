using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class CutScene : MonoBehaviour
{
    private List<AudioSource> l;

    // Start is called before the first frame update
    void Start()
    {
        l = SceneManagement.Instance.GetComponents<AudioSource>().ToList();
        foreach (var item in l)
        {
            item.mute = true;
        }

        GetComponent<VideoPlayer>().loopPointReached += CutScene_loopPointReached;
    }

    private void CutScene_loopPointReached(VideoPlayer source)
    {
        Skip();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Skip();   
        }   
    }
    
    private void Skip()
    {
        gameObject.SetActive(false);
        foreach (var item in l)
        {
            item.mute = false;
        }
    }
}
