using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Speaker : MonoBehaviour
{     
    private int _speechId = 0;
    public InputField input;
    public Text status;
    // Use this for initialization
    void Start()
    {
        LoadSpeaker();
    }

    public void LoadSpeaker()
    {        
        // Screen.sleepTimeout = SleepTimeout.NeverSleep;
        TTSManager.Initialize(transform.name, "OnTTSInit");  
        Debug.Log("Initialized Speaker");
    }
    public void Speak(string text)
    {        
        status.text = "SpeakButton - StatusLoad -" + TTSManager.IsInitialized();
        if (TTSManager.IsInitialized())
        {
            //TTSManager.Stop();
            status.text = "Speaking";
            TTSManager.Speak(text, false, TTSManager.STREAM.Music, 1f, 0f, transform.name, "OnSpeechCompleted", "speech_" + (++_speechId));
        }

    }
    public void Speak()
    {
        Speak(input.text);
    }
    
    public void SpeakJSON()
    {
        input.text = GameSettings.Settings["test"];
        Speak();
    }
    void OnDestroy()
    {
        TTSManager.Shutdown();
    }

    void OnTTSInit(string message)
    {
        int response = int.Parse(message);
        status.text = "LoadEvent";
        Debug.Log("Loading Speaker");
        switch (response)
        {
            case TTSManager.SUCCESS:
                /*
                  _selectedLocale = GUILayout.SelectionGrid(_selectedLocale, _localeStrings, 3);
                  TTSManager.SetLanguage(TTSManager.GetAvailableLanguages() [_selectedLocale]);
                */
                status.text = "Loaded";
                TTSManager.SetLanguage(new TTSManager.Locale("es", "ES"));
                Debug.Log("Speaker Loaded");
                break;
            case TTSManager.ERROR:
                Debug.Log("Error on load");
                status.text = "Error";
                break;
        }
    }

    void OnSpeechCompleted(string id)
    {
        Debug.Log("Speech '" + id + "' is complete.");
        status.text = "Speech Complete";
    }

    void OnSynthesizeCompleted(string id)
    {
        Debug.Log("Synthesize of speech '" + id + "' is complete.");
        status.text = "Synthesis Complete";
    }
}
