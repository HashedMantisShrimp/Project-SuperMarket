using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThoughtBubble : MonoBehaviour
{
    [SerializeField] Transform head, camera;
    [SerializeField] float height;
    [SerializeField] Texture2D[] itemIcons;
    [SerializeField] RawImage icon;
    [SerializeField] AudioClip pop;
    AudioSource src;
    RawImage image;
    Vector3 deltaPos;

    public int itemShowing = -1;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(head.position.x, camera.position.y + height, head.position.z);
        foreach (RawImage img in GetComponentsInChildren<RawImage>())
        {
            if (img.gameObject != gameObject) { icon = img; }
            else { image = img; }
        }
        if (GetComponent<AudioSource>()) { src = GetComponent<AudioSource>(); }
        else { src = gameObject.AddComponent<AudioSource>(); }
        if (pop) { src.clip = pop; }
        if (src.isPlaying) { src.Pause(); }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(head.position.x, camera.position.y + height, head.position.z);
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(Vector3.up * 180);
    }

    public void SetIcon(int iconNum)
    {
        if (iconNum < itemIcons.Length)
        {
            if (iconNum == -1)
            {
                icon.texture = null;
            }
            else
            {
                icon.texture = itemIcons[iconNum];
            }
            itemShowing = iconNum;
        }
    }

    public void SetVisibility(bool visible, int icon)
    {
        image.enabled = visible;
        this.icon.enabled = visible;
        SetIcon(icon);
        if (visible && pop)
        {
            src.time = 0;
            src.Play();
        }
    }

    public void ShowForTime(float time, int icon)
    {
        image.enabled = true;
        this.icon.enabled = true;
        SetIcon(icon);
        src.time = 0;
        src.Play();
        StartCoroutine(Close(time));
    }

    IEnumerator Close(float delay)
    {
        yield return new WaitForSeconds(delay);

        SetVisibility(false, -1);
    }
}
