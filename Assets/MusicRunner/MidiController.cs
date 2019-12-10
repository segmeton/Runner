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

    // Start is called before the first frame update
    void Start()
    {
        Bounds bound = player.GetComponent<SpriteRenderer>().bounds;
        Debug.Log(bound.max.x + " " + bound.max.y);
        CircleCollider2D collider = player.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = (float)0.5;
        collider.offset = new Vector2(0, (float)0.32);

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

    // Update is called once per frame
    void Update()
    {
        
    }

    protected IEnumerator PlayMusic() {
        playback.Start();
        while (playback.IsRunning)
        {
            playback.TickClock();
            yield return null;
        }
    }
}
