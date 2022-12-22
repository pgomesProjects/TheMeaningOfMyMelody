using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{
    [SerializeField] private float beatTempo;
    private bool canScroll;
    private float scrollSpeed;

    // Start is called before the first frame update
    void Start()
    {
        canScroll = true;
        scrollSpeed = beatTempo / 60f;
    }

    // Update is called once per frame
    void Update()
    {
        if (canScroll)
        {
            transform.position += new Vector3(scrollSpeed * Time.deltaTime, 0f, 0f);
        }
    }
}
