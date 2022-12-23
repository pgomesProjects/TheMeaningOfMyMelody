using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    private bool canBePressed;
    private SpriteRenderer noteTexture;

    private double timeInstantiated;
    private float assignedTime;

    private Vector3 spawnPosition;
    private Vector3 despawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        canBePressed = false;
        timeInstantiated = LevelManager.GetSongTime();

        spawnPosition = transform.parent.position;

        float despawnX = (-8 * GameSettings.leftScrollMultiplier) - (spawnPosition.x - (-8 * GameSettings.leftScrollMultiplier));
        despawnPosition = new Vector3(despawnX, spawnPosition.y, spawnPosition.z);

        noteTexture = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetNoteDirection(NOTETYPE noteDirection)
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
        double timeSinceInstantiated = LevelManager.GetSongTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (LevelManager.Instance.noteTime * 2));

        if(t > 1)
            Destroy(gameObject);
        else
        {
            transform.position = Vector3.Lerp(spawnPosition, despawnPosition, t);
        }
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

    public float GetAssignedTime() => assignedTime;
    public void SetAssignedTime(float newTime)
    {
        assignedTime = newTime;
    }
}
