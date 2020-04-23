using UnityEngine;

[System.Serializable]
public class Sound
{
    public string title;

    public AudioClip sound;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;
}
