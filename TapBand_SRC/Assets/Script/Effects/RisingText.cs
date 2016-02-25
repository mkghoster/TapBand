using UnityEngine;
using UnityEngine.UI;

public class RisingText : MonoBehaviour
{
    private Vector2 speedVector;
    private float currentAlpha;
    private float fadeDuration;

    private Color color;
    private int fontSize;
    private string text;
    private float duration;
    private float upSpeed;

    public Color Color
    {
        set
        {
            color = value;
        }
    }

    public int FontSize
    {
        set
        {
            fontSize = value;
        }
    }

    public string Text
    {
        set
        {
            text = value;
        }
    }

    public float Duration
    {
        set
        {
            duration = value;
        }
    }

    public float UpSpeed
    {
        set
        {
            upSpeed = value;
        }
    }

    RisingText()
    {
        currentAlpha = 1f;
        speedVector = new Vector2(0f, 1f);
        fadeDuration = 0.5f;
    }

    public void Init()
    {
        Text textComponent = GetComponent<Text>();
        textComponent.text = text;
        textComponent.color = color;
        textComponent.fontSize = fontSize;
        fadeDuration = 1f / duration;
        speedVector = new Vector2(0f, upSpeed);
    }

    void Update() 
    {
        // Move upwards
        transform.Translate(speedVector * Time.deltaTime, Space.World);

        // Change alpha
        currentAlpha -= Time.deltaTime * fadeDuration;
        Color color = GetComponent<Text>().color;
        color.a = currentAlpha;
        GetComponent<Text>().color = color;

        // If completely faded out, die
        if (currentAlpha <= 0f) Destroy(gameObject);
    }
}
