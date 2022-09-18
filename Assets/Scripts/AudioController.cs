using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    private AudioSource source;

    [SerializeField]
    AudioClip def;

    private float oldvolume, volume;
    private bool isSmothPlaying = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        source = instance.GetComponent<AudioSource>();
        source.volume = PlayerPrefs.GetFloat("Volume");
        volume = source.volume;
        source.clip = def;
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        oldvolume = volume;
        volume = PlayerPrefs.GetFloat("Volume");
        if (oldvolume != volume)
        {
            source.volume = volume;
        }
        
    }

    public void PlayBgm(string sPath)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/" + sPath);
        source.clip = clip;
        source.Play();
    }

    public void SmothPlayBgm(string sPath, float speed = 1)
    {
        AudioClip _clip = Resources.Load<AudioClip>("Audio/" +  sPath);
        if (_clip) StartCoroutine(SmothPlay(_clip, speed));
    }
    private IEnumerator SmothPlay(AudioClip ac, float speed)
    {
        yield return new WaitUntil( () =>  isSmothPlaying == false );
        isSmothPlaying = true;
        float cur = source.volume;
        float process = 0;
        while(source.volume != 0)
        {
            source.volume = Mathf.Lerp(cur, 0, process);
            process += Time.deltaTime * speed;
            yield return null;
        }

        source.clip = ac;
        source.Play();

        process = 0;
        while (source.volume != cur)
        {
            source.volume = Mathf.Lerp(0, cur, process);
            process += Time.deltaTime * speed;
            yield return null;
        }
        isSmothPlaying = false;
    }

    public void Mute(float speed = 1)
    {
        StartCoroutine(MuteRoutine(speed));
    }
    private IEnumerator MuteRoutine(float speed)
    {
        yield return new WaitUntil(() => isSmothPlaying == false);
        isSmothPlaying = true;
        float process = 0;
        while (source.volume != 0)
        {
            source.volume = Mathf.Lerp(volume, 0, process);
            process += Time.deltaTime * speed;
            yield return null;
        }
        isSmothPlaying = false;
    }

    public void Open(float speed = 1)
    {
        StartCoroutine(OpenRoutine(speed));
    }
    public IEnumerator OpenRoutine(float speed)
    {
        yield return new WaitUntil(() => isSmothPlaying == false);
        isSmothPlaying = true;
        float process = 0;
        while (source.volume != volume)
        {
            source.volume = Mathf.Lerp(0, volume, process);
            process += Time.deltaTime * speed;
            yield return null;
        }
        isSmothPlaying = false;
    }
}
