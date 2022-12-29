using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    [SerializeField] private List<MenuTab> tabButtons;
    [SerializeField] private Color tabIdle;
    [SerializeField] private Color tabHover;
    [SerializeField] private Color tabActive;

    [SerializeField] private List<GameObject> pages;

    private MenuTab selectedTab;


    private void OnEnable()
    {
        //Select the first tab by default
        Invoke("SelectFirstTab", 0);
    }

    private void SelectFirstTab()
    {
        OnTabSelected(tabButtons[0]);
    }

    public void SubscribeButton(MenuTab button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<MenuTab>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(MenuTab button)
    {
        ResetTabs();
        if(selectedTab == null || button != selectedTab)
        {
            button.background.color = tabHover;
        }
    }

    public void OnTabExit(MenuTab button)
    {
        ResetTabs();
    }

    public void OnTabSelected(MenuTab button)
    {
        if(selectedTab != null)
        {
            selectedTab.Deselect();
        }

        selectedTab = button;

        selectedTab.Select();

        ResetTabs();
        button.background.color = tabActive;
        int index = button.transform.GetSiblingIndex();

        for(int i = 0; i < pages.Count; i++)
        {
            if(i == index)
                pages[i].SetActive(true);
            else
                pages[i].SetActive(false);
        }
    }

    public void ResetTabs()
    {
        foreach(MenuTab button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
                continue;
            button.background.color = tabIdle;
        }
    }
}
