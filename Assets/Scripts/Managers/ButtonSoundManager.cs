using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSoundManager : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Button[] buttons;

    private void Awake()
    {
        foreach (Button button in buttons)
        {
            if (button != null)
            {
                button.onClick.AddListener(PlayClickSound);

                EventTrigger trigger = button.GetComponent<EventTrigger>();

                if (trigger == null)
                    trigger = button.gameObject.AddComponent<EventTrigger>();

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((eventData) => PlayHoverSound());

                trigger.triggers.Add(entry);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();
    }

    private void PlayClickSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(SoundType.UIClick);
        }
    }

    private void PlayHoverSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(SoundType.UIHover);
        }
    }

    private void OnDestroy()
    {
        foreach (Button button in buttons)
        {
            if (button != null)
                button.onClick.RemoveListener(PlayClickSound);
        }
    }
}