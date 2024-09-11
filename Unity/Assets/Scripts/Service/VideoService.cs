using Nfynt;
using UnityEngine;

public class VideoService : MonoBehaviour
{
    bool isOn = false;
    private void OnMouseDown()
    {
        if (isOn)
        {
            isOn = false;
            GetComponent<NVideoPlayer>().Pause();
        }
        else
        {
            isOn = true;
            GetComponent<NVideoPlayer>().Play();
            GetComponent<NVideoPlayer>().Unmute();
            GetComponent<AudioSource>().mute = false;
        }
    }
}
