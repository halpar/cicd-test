using UnityEngine;


public static class PlayerPrefKeys
{
    public static int ReachedLevel
    {
        get => PlayerPrefs.GetInt("ReachedLevel", 1);
        set
        {
            Helper.Log("-----> ReachedLevel Set : ", value);
            PlayerPrefs.SetInt("ReachedLevel", value);
        }
    }

    public static int CurrentLevel
    {
        get => PlayerPrefs.GetInt("CurrentLevel", 1);
        set
        {
            Helper.Log("-----> CurrentLevel Set : ", value);
            PlayerPrefs.SetInt("CurrentLevel", value);
        }
    }    
    public static int CurrentLeague
    {
        get => PlayerPrefs.GetInt("CurrentLeague", 0);
        set
        {
            Helper.Log("-----> CurrentLeague Set : ", value);
            PlayerPrefs.SetInt("CurrentLeague", value);
        }
    }
}