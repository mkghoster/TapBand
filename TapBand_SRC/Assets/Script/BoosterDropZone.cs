using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoosterDropZone : MonoBehaviour, IDropHandler
{
    private Image image;
    private BoosterController boosterController;

    private readonly Color dimColor = new Color(0.5f, 0.5f, 0.5f, 0.6f);
    private Color normalColor = new Color(0f, 0f, 0f, 0f);
    private BoosterUI[] boosterUIs;

    public event BoosterEvent OnBoosterDropped;

    private void Awake()
    {
        image = GetComponent<Image>();
        boosterController = (BoosterController)FindObjectOfType(typeof(BoosterController));
        normalColor = image.color;
        boosterUIs = FindObjectsOfType<BoosterUI>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < boosterUIs.Length; i++)
        {
            boosterUIs[i].OnBeginBoosterDrag += HandleBoosterBeginDrag;
            boosterUIs[i].OnEndBoosterDrag += HandleBoosterEndDrag;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        BoosterUI droppedUI = eventData.pointerDrag.GetComponent<BoosterUI>();
        if (droppedUI == null)
        {
            return;
        }
        BoosterType droppedBooster = droppedUI.boosterType;

        if (boosterController.CanActivateBooster(droppedBooster))
        {
            if (OnBoosterDropped != null)
            {
                OnBoosterDropped(this, new BoosterEventArgs(droppedBooster));
            }
        }
    }

    private void HandleBoosterBeginDrag(object sender, BoosterEventArgs e)
    {
        image.color = dimColor;
    }

    private void HandleBoosterEndDrag(object sender, BoosterEventArgs e)
    {
        image.color = normalColor;
    }

}
