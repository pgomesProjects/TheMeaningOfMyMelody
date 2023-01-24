using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Node : MonoBehaviour
{
    private StoryNode currentStory;

    [SerializeField] private TextMeshProUGUI chapterNumber;
    [SerializeField] private TextMeshProUGUI chapterTitle;
    [SerializeField] private Image portrait;

    // Start is called before the first frame update
    void Start()
    {
        NodeSetup();
    }

    private void NodeSetup()
    {
        chapterNumber.text = currentStory.header;
        chapterTitle.text = currentStory.nodeTitle;

        Color newColor = portrait.color;

        if (currentStory.graphic == null)
        {
            newColor.a = 0f;
        }
        else
        {
            newColor.a = 1f;
            portrait.sprite = currentStory.graphic;
        }

        portrait.color = newColor;
    }

    public void PlaySection()
    {
        //Return to the story menu when returning
        GameData.returnOnMenuState = MenuState.STORY;

        //Activate node differently based on the node information given
        switch (currentStory.GetType().ToString())
        {
            //Go to the visual novel scene
            case "VisualNovelNode":
                GameData.currentStoryNode = currentStory;
                SceneManager.LoadScene("Cutscene");
                break;
            //Go to whichever rhythm game setting is specified
            case "RhythmGameNode":
                RhythmGameNode rhythmGameNode = (RhythmGameNode)currentStory;
                SceneManager.LoadScene(rhythmGameNode.setting);
                break;
        }
    }

    public void SetStoryNode(StoryNode story)
    {
        currentStory = story;
    }
}
