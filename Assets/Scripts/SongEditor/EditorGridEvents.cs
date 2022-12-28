using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum CHARTTYPE { OPPONENT, PLAYER };

public class EditorGridEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private SongEditorManager _songEditorManager;
    [SerializeField] private CHARTTYPE chartType;
    [SerializeField] private RectTransform highlightTransform;
    [SerializeField] private Transform editorArrowParent;
    [SerializeField] private Sprite[] editorArrows;

    private List<GameObject> chartObjects;

    private void OnEnable()
    {
        chartObjects = new List<GameObject>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AddArrowToChart();
    }

    private void AddArrowToChart()
    {
        Vector2 newNotePos = highlightTransform.localPosition;

        //Add arrow based on column position
        int arrowIndex = (int)(newNotePos.x / 100f);
        Vector2 chartNoteData = new Vector2(arrowIndex, newNotePos.y);

        //If the note already exists in the chart, remove it
        if (_songEditorManager.NoteExistsInChart(chartType, chartNoteData))
        {
            int index = _songEditorManager.GetNoteIndex(chartType, chartNoteData);
            _songEditorManager.RemoveNoteFromChart(chartType, chartNoteData);

            GameObject noteToDestroy = chartObjects[index];

            chartObjects.RemoveAt(index);
            Destroy(noteToDestroy);
        }

        //If not, add the note
        else
        {
            //Create arrow object
            GameObject newArrow = new GameObject();

            //Add to parent
            newArrow.transform.parent = editorArrowParent;

            //Adjust position
            newArrow.AddComponent<RectTransform>().anchorMin = Vector2.up;
            newArrow.GetComponent<RectTransform>().anchorMax = Vector2.up;
            newArrow.GetComponent<RectTransform>().pivot = Vector2.up;
            newArrow.GetComponent<RectTransform>().localPosition = highlightTransform.localPosition;
            newArrow.GetComponent<RectTransform>().localScale = Vector3.one;

            newArrow.AddComponent<Image>().sprite = editorArrows[arrowIndex];

            newArrow.name = "Note";

            _songEditorManager.AddNoteToChart(chartType, chartNoteData);
            chartObjects.Add(newArrow);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightTransform.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(highlightTransform.gameObject.activeInHierarchy)
        {
            Vector3 highlightPosition = new Vector3(GetMousePos().x, GetMousePos().y, highlightTransform.position.z);
            SnapPosition(ref highlightPosition);

            //Debug.Log("Highlight Pos: " + highlightPosition);

            highlightTransform.localPosition = highlightPosition;
        }
    }

    private void SnapPosition(ref Vector3 highlightPosition)
    {
        highlightPosition.x = (int)FloorToMultiple(highlightPosition.x, 100);
        highlightPosition.y = (int)FloorToMultiple(highlightPosition.y, 100 / _songEditorManager.GetPositionSnap());
    }

    private double FloorToMultiple(float number, double multiple)
    {
        double remainder = (double)number % multiple;
        return (double)(number - remainder);
    }

    private Vector2 GetMousePos()
    {
        Vector3 screenPos = Mouse.current.position.ReadValue();

        Vector2 localPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), screenPos, null, out localPos);

        return localPos;
    }
}
