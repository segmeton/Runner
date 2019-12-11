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
        midiFile = MidiFile.Read(midiPath);

        outputDevice = OutputDevice.GetByName("Microsoft GS Wavetable Synth");
        playback = midiFile.GetPlayback(outputDevice, new MidiClockSettings
        {
            CreateTickGeneratorCallback = interval => null
        });

        StartCoroutine(PlayMusic());
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
        float r = bound.size.x > bound.size.y ? bound.size.x : bound.size.y;

        return r;
    }

    void OnTriggerEnter2D(Collider2D item)
    {
        if (item.gameObject.tag == "kill")
        {
            
        }
        else if (item.gameObject.tag == "water")
        {
            
        }
        //else if (item.gameObject.tag == "coin" || (item.gameObject.tag == "coinL" && leftArm) || (item.gameObject.tag == "coinR" && rightArm) || (item.gameObject.tag == "coinB" && isBendDown))
        //{
            
        //}
    }
}
