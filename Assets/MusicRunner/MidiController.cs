using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Devices;

public class MidiController : MonoBehaviour
{
    public MidiFile BgmMidi;

    // Start is called before the first frame update
    void Start()
    {
        string outputDeviceName = "";
        foreach (var outputDevice in OutputDevice.GetAll())
        {
            Debug.Log(outputDevice.Name);
            outputDeviceName = outputDevice.Name;
        }

        string appDataPath = Application.dataPath;
        string midiPath = appDataPath + "/MusicRunner/Audio/Track01.mid";
        using (OutputDevice outputDevice = OutputDevice.GetByName(outputDeviceName))
        {

           MidiFile.Read(midiPath).Play(outputDevice);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
