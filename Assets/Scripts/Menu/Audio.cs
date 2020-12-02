using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference the audio manager to access the volume levels of all sources.")]
    private AudioMixer mixer;
    [SerializeField]
    [Tooltip("Reference the Music Slider, so it can modify the Mixers value for Music, is dynamic.")]
    private Slider musicSlider;
    [SerializeField]
    [Tooltip("Reference the SFX Slider, so it can modify the Mixers value for Sound Effects, is dynamic.")]
    private Slider SFXSlider;

    [SerializeField]
    [Tooltip("Sound Effects onValueChange button sound")]
    private AudioSource buttonSound;

    private void Start()
    {        
        LoadPlayerPrefs();        
    }

    #region Change Settings
    /// <summary>
    /// On value change of Music Slider Volume, applies to mixers music volume.
    /// </summary>
    /// <param name="value">How loud, passed dynamically</param>
    public void SetMusicVolume(float value) //on value change of UI sliders
    {
        mixer.SetFloat("MusicVol", value); //Set value of mixers exposed parameter MusicVol
    }

    /// <summary>
    /// On value change of SFX Slider Volume, applies to mixers sound effects volume.
    /// </summary>
    /// <param name="value">How loud, passed dynamically</param>
    public void SetSFXVolume(float value) //on value change of UI sliders
    {
        mixer.SetFloat("SFXVol", value); //Set value of mixers exposed parameter SFXVol
        if (buttonSound.enabled)
        {
            buttonSound.PlayDelayed(0.1f);//plays SFX w delay, so user can hear the change
        }
    }
    #endregion

    #region Save and Load Player Prefs
    /// <summary>
    /// saves settings to PlayerPrefs, called when exiting slider UI.
    /// </summary>
    public void SavePlayerPrefs()
    {
        //save audio sliders
        float musicVol; //declair a music volume variable for out
        if (mixer.GetFloat("MusicVol", out musicVol)) //returns true if mixer HAS a value for the parameter MusicVol and stores mixers MusicVol value in musicVol variable
        {
            PlayerPrefs.SetFloat("MusicVol", musicVol); //when successful, musicVol value is applied to PlayerPrefs MusicVol
        }

        //same but Sound FX
        float SFXVol;
        if (mixer.GetFloat("SFXVol", out SFXVol))
        {
            PlayerPrefs.SetFloat("SFXVol", SFXVol);
        }

        PlayerPrefs.Save();//above sets changes, this line saves those changes to PlayerPrefs on device.
    }

        /// <summary>
        /// Loads and applies PlayerPref Values if values exist.
        /// </summary>
        public void LoadPlayerPrefs()
    {
        
        //load audio sliders
        if (PlayerPrefs.HasKey("MusicVol"))//checks to see if a key called MusicVol exists, before assigning values, to avoid errors
        {
            float musicVol = PlayerPrefs.GetFloat("MusicVol"); //set musicVol to equal PlayerPref
            musicSlider.value = musicVol; //apply value to UI
            mixer.SetFloat("MusicVol", musicVol); //apply value to mixer (actual volume control)
        }

        
        //same but Sound FX
        if (PlayerPrefs.HasKey("SFXVol"))
        {
            float SFXVol = PlayerPrefs.GetFloat("SFXVol");

            buttonSound.enabled = false;
            SFXSlider.value = SFXVol;
            buttonSound.enabled = true;

            mixer.SetFloat("SFXVol", SFXVol);
        }
    }
    #endregion
}
