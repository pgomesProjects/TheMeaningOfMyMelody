using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    [SerializeField] private NOTETYPE noteDirection;
    private bool canBePressed;
    private SpriteRenderer noteTexture;

    // Start is called before the first frame update
    void Start()
    {
        canBePressed = false;
        noteTexture = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        SetNoteDirection();
    }

    private void SetNoteDirection()
    {
        switch (noteDirection)
        {
            case NOTETYPE.LEFT:
                transform.Rotate(0f, 0f, 180f, Space.World);
                break;
            case NOTETYPE.DOWN:
                transform.Rotate(0f, 0f, -90f, Space.World);
                break;
            case NOTETYPE.UP:
                transform.Rotate(0f, 0f, 90f, Space.World);
                break;
            case NOTETYPE.RIGHT:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "ArrowButton")
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ArrowButton")
        {
            canBePressed = false;
        }
    }

    public void NoteHit()
    {
        if (canBePressed)
        {
            gameObject.SetActive(false);
        }
    }
}
