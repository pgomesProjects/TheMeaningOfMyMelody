using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryTree
{
    public StoryNode[] storyNodes;
}

public class StoryTreeController : MonoBehaviour
{
    [SerializeField] private StoryNode[] storyNodes;
    [SerializeField] private Node nodeObject;

    private float verticalSpacing = 278f;

    private void Awake()
    {
        GameData.storyNodes = storyNodes;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        CheckForUnlock();

        for (int i = 0; i < GameData.storyNodes.Length; i++)
        {
            if (GameData.storyNodes[i].hasUnlocked)
            {
                Node newNode = Instantiate(nodeObject, transform);
                newNode.SetStoryNode(GameData.storyNodes[i]);

                Vector3 rectPos = newNode.GetComponent<RectTransform>().anchoredPosition;
                rectPos.y = -(i * verticalSpacing);
                newNode.GetComponent<RectTransform>().anchoredPosition = rectPos;
            }
            else
            {
                continue;
            }
        }
    }

    private void CheckForUnlock()
    {
        //Make sure the first node is always unlocked
        GameData.storyNodes[0].hasUnlocked = true;

        for (int i = 0; i < GameData.storyNodes.Length - 1; i++)
        {
            if (GameData.storyNodes[i].hasViewed)
            {
                GameData.storyNodes[i + 1].hasUnlocked = true;
            }
            else
                break;
        }
    }

    private void ResetNodes()
    {
        for (int i = 0; i < GameData.storyNodes.Length; i++)
        {
            GameData.storyNodes[i].hasUnlocked = false;
            GameData.storyNodes[i].hasViewed = false;
        }

        GameData.storyNodes[0].hasUnlocked = true;
    }
}
