using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    private bool canScroll;
    private float scrollSpeed;

    // Start is called before the first frame update
    void Start()
    {
        canScroll = true;
        scrollSpeed = LevelManager.Instance.beatTempo / 60f * LevelManager.Instance.scrollSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (canScroll)
        {
            transform.position += new Vector3(-(scrollSpeed * Time.deltaTime) * GameSettings.leftScrollMultiplier, 0f, 0f);
        }
    }
}
