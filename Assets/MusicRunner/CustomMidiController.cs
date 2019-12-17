using System;
using System.Collections;
using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Text;
using UnityEngine;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;
//using Melanchall.DryWetMidi;
//using Melanchall.DryWetMidi.Core;

[RequireComponent(typeof(AudioSource))]

public class CustomMidiController : MonoBehaviour
{
    public GameObject player;
    public AudioSource audioSource;
    public AudioClip jumpClip;
    public AudioClip crouchClip;
    private int outDeviceID = 0;
    private OutputDevice outputDevice;
    private Sequence mainSequence;
    private Sequencer mainSequencer;
    //private Playback playback;
    //private MidiFile midiFile;
    private CircleCollider2D collider;
    //private int i;

    // Start is called before the first frame update
    void Start()
    {
        DefineCollider();
        DefineMIDI();
        InitMidiHandler();
        InitMusic();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    protected void InitMusic()
    {
        try
        {
            mainSequencer.Stop();
            Debug.Log("Load Music");
            //playing = false;
            string midiPath = Application.dataPath + "/MusicRunner/Audio/Track01.mid";
            Debug.Log(midiPath);
            mainSequence.LoadAsync(midiPath);
            //this.Cursor = Cursors.WaitCursor;
            //startButton.Enabled = false;
            //continueButton.Enabled = false;
            //stopButton.Enabled = false;
            //openToolStripMenuItem.Enabled = false;
        }
        catch (Exception ex)
        {
            Debug.Log("ERROR INIT MUSIC!!! : " + ex.Message);
        }
    }

    protected void InitMidiHandler() 
    {
        mainSequence = new Sequence();
        mainSequence.Format = 1;
        mainSequencer = new Sequencer();
        mainSequencer.Sequence = mainSequence;
        mainSequencer.Position = 0;

        mainSequencer.PlayingCompleted += new EventHandler(HandlePlayingCompleted);
        mainSequencer.ChannelMessagePlayed += new EventHandler<Sanford.Multimedia.Midi.ChannelMessageEventArgs>(HandleChannelMessagePlayed);
        //this.sequencer1.Stopped += new System.EventHandler<Sanford.Multimedia.Midi.StoppedEventArgs>(this.HandleStopped);
        //this.sequencer1.SysExMessagePlayed += new System.EventHandler<Sanford.Multimedia.Midi.SysExMessageEventArgs>(this.HandleSysExMessagePlayed);
        mainSequencer.Chased += new EventHandler<Sanford.Multimedia.Midi.ChasedEventArgs>(HandleChased);

        InitOutputDevice();

        mainSequence.LoadProgressChanged += HandleLoadProgressChanged;
        mainSequence.LoadCompleted += HandleLoadCompleted;

        

        
    }

    protected void InitOutputDevice() 
    {
        if (OutputDevice.DeviceCount == 0)
        {
            Debug.Log("no output device");
        }
        else
        {

            Debug.Log("Device Count : " + OutputDevice.DeviceCount);
            try
            {
                outputDevice = new OutputDevice(outDeviceID);
                
            }
            catch (Exception ex)
            {
                //Debug.Log("ERROR INIT OUTPUT!!! : " + ex.Message);

                //try
                //{
                //    outputDevice.Dispose();
                //    InitOutputDevice();
                //}
                //catch (Exception ex2)
                //{
                //    Debug.Log("ERROR INIT OUTPUT 2!!! : " + ex2.Message);
                //}
            }
        }
    }

    private void HandleLoadProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        Debug.Log("Progress : " + e.ProgressPercentage);
    }

    private void HandleLoadCompleted(object sender, AsyncCompletedEventArgs e)
    {
        //this.Cursor = Cursors.Arrow;
        //startButton.Enabled = true;
        //continueButton.Enabled = true;
        //stopButton.Enabled = true;
        //openToolStripMenuItem.Enabled = true;
        //toolStripProgressBar1.Value = 0;

        //if (e.Error == null)
        //{
        //    positionHScrollBar.Value = 0;
        //    positionHScrollBar.Maximum = sequence1.GetLength();
        //}
        //else
        //{
        //    MessageBox.Show(e.Error.Message);
        //}
        Debug.Log("Load Complete");

        mainSequencer.Start();
    }

    private void HandleChannelMessagePlayed(object sender, ChannelMessageEventArgs e)
    {
        outputDevice.Send(e.Message);
        Debug.Log(e.Message);
    }

    private void HandleChased(object sender, ChasedEventArgs e)
    {
        foreach (ChannelMessage message in e.Messages)
        {
            //outputDevice.Send(message);
            Debug.Log(message);
        }
    }

    private void HandlePlayingCompleted(object sender, EventArgs e)
    {
        Debug.Log("Play Completed");
    }

    protected void DefineMIDI() 
    {
        //string midiPath = Application.dataPath + "/MusicRunner/Audio/Track01.mid";
        //Debug.Log(midiPath);
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
        //playback.Start();
        //while (playback.IsRunning)
        //{
        //    playback.TickClock();
        //playback.NotesPlaybackStarted
        //Debug.Log("test "+i);
        //i += 1;
        // playback.EventPlayed.
        //    yield return null;
        //}
        yield return null;
    }

    protected void DefineCollider() 
    {

        if (player == null) return;

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
