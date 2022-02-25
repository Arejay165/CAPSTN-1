using UnityEngine;

public class VersionCheck : MonoBehaviour
{
    private void Awake()
    {
        var version = PlayerPrefs.GetString("Version", string.Empty);

        if (string.IsNullOrWhiteSpace(version))
        {
            // Probably not more to do since there is no stored data apparently
            // Just to be sure you could still do
            PlayerPrefs.DeleteAll();

            // => THIS IS THE FIRST RUN
            PlayerPrefs.SetString("Version", Application.version);
        }
        else
        {
            if (version != Application.version)
            {
                // => THIS IS A VERSION MISMATCH -> UPDATED
                PlayerPrefs.DeleteAll();

                PlayerPrefs.SetString("Version", Application.version);
            }
            // else
            //{
            //     // Otherwise it could either mean you re-installed the same version 
            //     // or just re-started the app -> There should be no difference between these two in behavior of your app
            //}
        }
    }
}