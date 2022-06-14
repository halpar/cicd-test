using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ElephantSettings : ScriptableObject
{
    private const string ELEPHANTSETTINGSFILEPATH = "ElephantSettings";
    public string GameID;
    public string GameSecret;
    
    public static ElephantSettings GetElephantSettingsSO()
    {
        return Resources.Load<ElephantSettings>(ELEPHANTSETTINGSFILEPATH);
    }

    public void SetGameID(string GameID)
    {
        this.GameID = GameID;
    }

    public void SetGameSecret(string GameSecret)
    {
        this.GameSecret = GameSecret;
    }
    
}
