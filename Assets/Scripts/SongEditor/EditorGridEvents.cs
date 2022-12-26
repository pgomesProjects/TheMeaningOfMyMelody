using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditorGridEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private SongEditorManager _songEditorManager;
    [SerializeField] private RectTransform highlightTransform;
    [SerializeField] private Transform editorArrowParent;
    [SerializeField] private Sprite[] editorArrows;

    private float positionSnap = 1f;

    public void OnPointerDown(PointerEventData eventData)
    {
        AddArrowToChart();
    }

    private void AddArrowToChart()
    {
        //Create arrow object
        GameObject newArrow = Instantiate(new GameObject(), editorArrowParent);

        //Adjust position
        newArrow.AddComponent<RectTransform>().position = highlightTransform.position;

        int arrowIndex = 0;

        //Add arrow based on row position
        switch (newArrow.GetComponent<RectTransform>().localPosition.y - 50)
        {
            case 0:
                arrowIndex = 3;
                break;
            case 100:
                arrowIndex = 2;
                break;
            case 200:
                arrowIndex = 1;
                break;
            case 300:
                arrowIndex = 0;
                break;
        }


        newArrow.AddComponent<Image>().sprite = editorArrows[arrowIndex];

        newArrow.name = "Note";

        //Add to chart
        _songEditorManager.AddNoteToChart(new Vector2(arrowIndex, newArrow.GetComponent<RectTransform>().localPosition.x - 50));

        _songEditorManager.GetChartData();
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

            //Offset based on pivot
            highlightPosition += new Vector3(50, 50, 0);

            highlightTransform.localPosition = highlightPosition;
        }
    }

    private void SnapPosition(ref Vector3 highlightPosition)
    {
        highlightPosition.x = (int)FloorToMultiple(highlightPosition.x, 100 * positionSnap);
        highlightPosition.y = (int)FloorToMultiple(highlightPosition.y, 100);
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
