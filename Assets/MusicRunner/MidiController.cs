using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Devices;

[RequireComponent(typeof(AudioSource))]

public class MidiController : MonoBehaviour
{
    public GameObject player;
    public AudioSource audioSource;
    public AudioClip jumpClip;
    public AudioClip crouchClip;
    private OutputDevice outputDevice;
    private Playback playback;
    private MidiFile midiFile;
    private CircleCollider2D collider;
    //private int i;

    // Start is called before the first frame update
    void Start()
    {
        DefineCollider();
        DefineMIDI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    protected void DefineMIDI() 
    {
        string midiPath = Application.dataPath + "/MusicRunner/Audio/Track01.mid";
        Debug.Log(midiPath);
        //midiFile = MidiFile.Read(midiPath);

        //outputDevice = OutputDevice.GetByName("Microsoft GS Wavetable Synth");
        //playback = midiFile.GetPlayback(outputDevice, new MidiClockSettings
        //{
          //  CreateTickGeneratorCallback = interval => null
        //});

        //StartCoroutine(PlayMusic());
    }

    protected IEnumerator PlayMusic() 
    {
        playback.Start();
        while (playback.IsRunning)
        {
            playback.TickClock();
            //playback.NotesPlaybackStarted
            //Debug.Log("test "+i);
            //i += 1;
           // playback.EventPlayed.
            yield return null;
        }
    }

    protected void DefineCollider() 
    {
        collider = this.gameObject.GetComponent<CircleCollider2D>();

        Bounds bound = player.GetComponent<SpriteRenderer>().bounds;
        //Debug.Log(bound.size.x + " " + bound.size.y + " " + bound.max.x + " " + bound.max.y);
        collider.radius = GetColliderRadius(bound);

        UpdatePosition();
    }

    protected void UpdatePosition()
    {
        if (player != null) this.gameObject.transform.position = player.transform.position;
    }

    protected float GetColliderRadius(Bounds bound)
    {
        float r = bound.size.x > bound.size.y ? bound.size.x*2 : bound.size.y*2;

        return r;
    }

    private float x_water;
    private float curr_water = 0;
    private float x_spikes;
    private float curr_spikes = 0;
    private float x_fly;
    private float curr_fly = 0;
    void OnTriggerEnter2D(Collider2D item)
    {
        x_water = this.gameObject.transform.position.x;
        

        if (item.gameObject.tag == "kill")
        {
            if (item.gameObject.name.Equals("spikes"))
            {
                x_spikes = this.gameObject.transform.position.x;
                if ((curr_spikes == 0) || (curr_spikes + 3 < x_spikes))
                {
                    curr_spikes = x_spikes;
                    audioSource.clip = jumpClip;
                    audioSource.Play();
                    Debug.Log("spikes");

                }
            }
            if (item.gameObject.name.Equals("evilBlock") || item.gameObject.name.Equals("evilSaw"))
            {
                x_fly = this.gameObject.transform.position.x;
                if ((curr_fly == 0) || (curr_fly + 3 < x_fly))
                {
                    curr_fly = x_fly;
                    audioSource.clip = crouchClip;
                    audioSource.Play();
                    Debug.Log("fly");

                }
            }
        }
        else if (item.gameObject.tag == "water")
        {
            x_water = this.gameObject.transform.position.x;
            if ((curr_water == 0) || (curr_water+5 < x_water))
            {
                curr_water = x_water;
                audioSource.clip = jumpClip;
                audioSource.Play();
                Debug.Log("water");
                
            }
            
        }
        //else if (item.gameObject.tag == "coin" || (item.gameObject.tag == "coinL" && leftArm) || (item.gameObject.tag == "coinR" && rightArm) || (item.gameObject.tag == "coinB" && isBendDown))
        //{
            
        //}
    }

    void OnTriggerExit2D(Collider2D item)
    {
        
    }
}
