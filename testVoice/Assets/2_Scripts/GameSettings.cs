using UnityEngine;
using System.Collections;
using SimpleJSON;
public static class GameSettings
{    
    public static JSONNode Settings;
    static GameSettings()
    {
         // var N = JSON.Parse(the_JSON_string);
        if (!PlayerPrefs.HasKey(Constants.SettingsKey))
	    {
            PlayerPrefs.SetString(Constants.SettingsKey, Constants.DefaultJSONSettings); 
            PlayerPrefs.Save();
	    }
        string settings = PlayerPrefs.GetString(Constants.SettingsKey);
        Settings = JSON.Parse(settings);
        //var versionString = N["version"].Value;        // versionString will be a string containing "1.0"
        //var versionNumber = N["version"].AsFloat;      // versionNumber will be a float containing 1.0
        //var name = N["data"]["sampleArray"][2]["name"];// name will be a string containing "sub object"
    }
}
