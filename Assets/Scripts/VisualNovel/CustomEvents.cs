using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CustomEvents : MonoBehaviour
{
    [SerializeField] private Transform spriteHolder;
    [SerializeField] private Image backgroundSprite;

    public struct FileInformation
    {
        public string name;
        public string directory;
    }

    private List<Sprite> characterSprites = new List<Sprite>();
    private List<Sprite> backgroundSprites = new List<Sprite>();

    private List<string>[] commandList;

    public void GenerateEventCommands(int listSize)
    {
        commandList = new List<string>[listSize];
        for(int i = 0; i < listSize; i++)
        {
            commandList[i] = new List<string>();
        }

        VisualNovelNode visualNovelNode = (VisualNovelNode)GameData.currentStoryNode;
        string[] rawCommandsList = visualNovelNode.visualNovelEventsScript.text.Split('\n');

        foreach(var commandLine in rawCommandsList)
        {
            //If the command line isn't empty
            if(commandLine.Trim().Length > 1)
            {
                //If the current line contains an equals symbol, make a vairable
                if (commandLine.Contains('='))
                {
                    CreateVariable(commandLine.Split('='));
                }

                else
                {
                    CreateCommand(commandLine.Split(','));
                }
            }
        }
    }

    private void CreateVariable(string [] codeLine)
    {
        string[] definition = codeLine[0].Split(' ');

        //Debug.Log("Data Type: " + definition[0].Trim());
        //Debug.Log("Variable Name: " + definition[1].Trim());

        //Debug.Log("Value: " + codeLine[1].Trim());

        FileInformation newFileInfo = new FileInformation();

        //Find a valid data type
        switch (definition[0].Trim())
        {
            case "Sprite":
                newFileInfo.name = definition[1].Trim();
                if (ValidStringFormat(codeLine[1].Trim()))
                {
                    newFileInfo.directory = codeLine[1].Trim().Substring(1, codeLine[1].Trim().Length - 2);
                    CreateSprite(newFileInfo);
                }
                else
                {
                    Debug.LogError("Syntax Error: " + codeLine[1].Trim() + " is must be properly contained in quotes.");
                }
                break;
            case "Background":
                newFileInfo.name = definition[1].Trim();
                if (ValidStringFormat(codeLine[1].Trim()))
                {
                    newFileInfo.directory = codeLine[1].Trim().Substring(1, codeLine[1].Trim().Length - 2);
                    CreateBackground(newFileInfo);
                }
                else
                {
                    Debug.LogError("Syntax Error: " + codeLine[1].Trim() + " is must be properly contained in quotes.");
                }
                break;
            default:
                Debug.LogError("Syntax Error: " + definition[0].Trim() + " is not a valid data type.");
                return;
        }
    }

    private void CreateSprite(FileInformation fileInfo)
    {
        Sprite newSprite = Resources.Load<Sprite>(fileInfo.directory);
        newSprite.name = fileInfo.name;
        characterSprites.Add(newSprite);
    }

    private void CreateBackground(FileInformation fileInfo)
    {
        Sprite newSprite = Resources.Load<Sprite>(fileInfo.directory);
        newSprite.name = fileInfo.name;
        backgroundSprites.Add(newSprite);
    }

    public bool ValidStringFormat(string currentString)
    {
        //Check to see if the first and last characters of the string are quotes
        return currentString[0] == '"' && currentString[currentString.Length - 1] == '"';
    }

    private void CreateCommand(string [] command)
    {
        int index = int.Parse(command[1].Trim()) - 1;

        Debug.Log("Index: "+index);

        commandList[index].Add(command[0]);
    }

    public void CheckForCustomEvent(int indexNumber)
    {
        if(commandList[indexNumber] != null)
        {
            foreach(var command in commandList[indexNumber])
            {
                RunCommand(command);
            }
        }
    }

    private void RunCommand(string command)
    {
        string[] splitCommand = command.Split(' ');

        switch (splitCommand[0].Trim())
        {
            case "scene":
                ShowBackground(splitCommand[1].Trim());
                break;
            case "show":
                ShowSprite(splitCommand[1].Trim(), splitCommand[2].Trim());
                break;
            case "hide":
                HideSprite(splitCommand[1].Trim());
                break;
            default:
                Debug.LogError("Syntax Error: '" + splitCommand[0].Trim() + "' command does not exist.");
                return;
        }
    }

    private void ShowBackground(string backgroundName)
    {
        backgroundSprite.sprite = backgroundSprites.Where(bg => bg.name == backgroundName).SingleOrDefault();
        if(backgroundSprite.sprite != null)
            backgroundSprite.gameObject.SetActive(true);
        else
        {
            Debug.LogError("Background '" + backgroundName + "' could not be found.");
        }
    }

    private void ShowSprite(string spriteName, string pos)
    {
        GameObject spriteObject = null;

        //If the object cannot be found already
        if (!spriteHolder.Find(spriteName))
        {
            //If the sprite is found in the array, make it
            if (characterSprites.Where(bg => bg.name == spriteName).SingleOrDefault() != null)
            {
                spriteObject = new GameObject();
                spriteObject.transform.SetParent(spriteHolder, false);
                spriteObject.name = spriteName;

                spriteObject.AddComponent<RectTransform>();
                spriteObject.AddComponent<Image>();

                spriteObject.GetComponent<Image>().sprite = characterSprites.Where(bg => bg.name == spriteName).SingleOrDefault();
                spriteObject.GetComponent<RectTransform>().sizeDelta = new Vector2(spriteObject.GetComponent<Image>().sprite.rect.width, spriteObject.GetComponent<Image>().sprite.rect.height);
            }
            else
            {
                Debug.LogError("Sprite '" + spriteName + "' could not be found.");
            }
        }
        else
        {
            spriteObject = spriteHolder.Find(spriteName).gameObject;
            spriteObject.SetActive(true);
        }


        Vector2 position = new Vector2(0, 0);

        Debug.Log("Position: " + pos);

        switch (pos)
        {
            case "left":
                position.x = -500f;
                break;
            case "center":
                position.x = 0f;
                break;
            case "right":
                position.x = 500f;
                break;
        }

        spriteObject.GetComponent<RectTransform>().anchoredPosition = position;
    }

    private void HideSprite(string spriteName)
    {
        //If the object can be found, set inactive
        if (spriteHolder.Find(spriteName))
        {
            spriteHolder.Find(spriteName).gameObject.SetActive(false);
        }
    }

    public void CustomOnEventComplete()
    {
        foreach(var story in GameData.storyNodes)
        {
            if(story == GameData.currentStoryNode)
            {
                story.hasViewed = true;
                break;
            }
        }
    }
}
