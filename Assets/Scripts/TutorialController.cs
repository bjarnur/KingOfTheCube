using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TutorialController : MonoBehaviour {
    private enum TouchSize
    {
        LeftHalf, RightHalf, Full
    }
    
    class TutorialTouch
    {
        public TouchSize Size;
        public float Rotation;
        public string Text;
    }

    class TutorialStep
    {
        public List<TutorialTouch> touches;
        public bool onePress; // Should all the touches should be at the same time
    }

    public RectTransform touchZonePrefab;

    List<TutorialStep> steps;
    List<TutorialStep>.Enumerator enumerator;
    int touchCount;

    bool ended;

    public void InitiateForPlayer () {
        ended = false;
        steps = new List<TutorialStep>();

        AddStep(new TutorialTouch { Size = TouchSize.RightHalf, Rotation = 0, Text = "Tryck och håll på höger sida av skärmen för att springa höger" });
        /*AddStep(new List<TutorialTouch> {
            new TutorialTouch { Size = TouchSize.LeftHalf, Rotation = 90, Text = "Tryck på vänster sida av skärmen när du springer till höger för att hoppa till höger" },
            new TutorialTouch { Size = TouchSize.RightHalf, Rotation = 0, Text = "Tryck på vänster sida av skärmen när du springer till höger för att hoppa till höger" }
        });*/
        AddStep(new TutorialTouch { Size = TouchSize.LeftHalf, Rotation = 180, Text = "Tryck och håll på vänster sidan av skärmen för att springa vänster" });
        /*AddStep(new List<TutorialTouch> {
            new TutorialTouch { Size = TouchSize.LeftHalf, Rotation = 180, Text = "Tryck på högersidan av skärmen när du springer till vänster för att hoppa till vänster" },
            new TutorialTouch { Size = TouchSize.RightHalf, Rotation = 90, Text = "Tryck på högersidan av skärmen när du springer till vänster för att hoppa till vänster" }
        });*/
        AddStep(new List<TutorialTouch> {
            new TutorialTouch { Size = TouchSize.LeftHalf, Rotation = 90, Text = "Tryck båda sidor samtidigt för att hoppa upp eller klättra stege" },
            new TutorialTouch { Size = TouchSize.RightHalf, Rotation = 90, Text = "Tryck båda sidor samtidigt för att hoppa upp eller klättra stege" }
        });
    }

    public void InitiateForKing()
    {
        ended = false;
        steps = new List<TutorialStep>();

        AddStep(new TutorialTouch { Size = TouchSize.RightHalf, Rotation = 0, Text = "Tryck och håll på höger sidan av skärmen för att springa höger" });
        /*AddStep(new List<TutorialTouch> {
            new TutorialTouch { Size = TouchSize.LeftHalf, Rotation = 90, Text = "Tryck på vänstersidan av skärmen när du springer till höger för att hopppa till höger" },
            new TutorialTouch { Size = TouchSize.RightHalf, Rotation = 0, Text = "Tryck på vänstersidan av skärmen när du springer till höger för att hopppa till höger" }
        });*/
        AddStep(new TutorialTouch { Size = TouchSize.LeftHalf, Rotation = 180, Text = "Tryck och håll på vänster sidan av skärmen för att springa vänster" });
        /*AddStep(new List<TutorialTouch> {
            new TutorialTouch { Size = TouchSize.LeftHalf, Rotation = 180, Text = "Tryck på högersidan av skärmen när du springer till vänster för att hopppa till vänster" },
            new TutorialTouch { Size = TouchSize.RightHalf, Rotation = 90, Text = "Tryck på högersidan av skärmen när du springer till vänster för att hopppa till vänster" }
        });*/
        AddStep(new List<TutorialTouch> {
            new TutorialTouch { Size = TouchSize.LeftHalf, Rotation = 90, Text = "Tryck båda sidor samtidigt för att droppa en bomb" },
            new TutorialTouch { Size = TouchSize.RightHalf, Rotation = 90, Text = "Tryck båda sidor samtidigt för att droppa en bomb" }
        });
    }

    public void Begin()
    {
        enumerator = steps.GetEnumerator();
        NextStep();
    }

    void Update () {
		
	}

    void AddStep(TutorialTouch touch)
    {
        steps.Add(new TutorialStep { touches = new List<TutorialTouch> { touch }, onePress = true });
    }

    void AddStep(List<TutorialTouch> touches, bool onePress = true)
    {
        steps.Add(new TutorialStep { touches = touches, onePress = onePress });
    }

    void NextStep()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        ended = !enumerator.MoveNext();

        touchCount = enumerator.Current.touches.Count;
        foreach (TutorialTouch touch in enumerator.Current.touches)
        {
            RectTransform touchZone = Instantiate<RectTransform>(touchZonePrefab, transform);
            switch (touch.Size)
            {
                case TouchSize.Full:
                    touchZone.anchorMin = new Vector2(0, 0);
                    touchZone.anchorMax = new Vector2(1, 1);
                    break;
                case TouchSize.LeftHalf:
                    touchZone.anchorMin = new Vector2(0, 0);
                    touchZone.anchorMax = new Vector2(0.5f, 1);
                    break;
                case TouchSize.RightHalf:
                    touchZone.anchorMin = new Vector2(0.5f, 0);
                    touchZone.anchorMax = new Vector2(1, 1);
                    break;
            }
            touchZone.Find("Arrow").localRotation = Quaternion.Euler(0, 0, touch.Rotation);
            touchZone.Find("Text").GetComponent<TextMeshProUGUI>().text = touch.Text;

            EventTrigger et = touchZone.GetComponent<EventTrigger>();

            EventTrigger.Entry downEntry = new EventTrigger.Entry();
            downEntry.eventID = EventTriggerType.PointerDown;
            downEntry.callback.AddListener(OnTouchDown);
            et.triggers.Add(downEntry);

            if (enumerator.Current.onePress) {
                EventTrigger.Entry upEntry = new EventTrigger.Entry();
                upEntry.eventID = EventTriggerType.PointerUp;
                upEntry.callback.AddListener(OnTouchUp);
                et.triggers.Add(upEntry);
            }
        }
    }

    void OnTouchDown(BaseEventData data)
    {
        touchCount--;
        if (touchCount == 0)
        {
            if (!ended)
            {
                NextStep();
            } else
            {
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }

    void OnTouchUp(BaseEventData data)
    {
        touchCount++;
    }
}
