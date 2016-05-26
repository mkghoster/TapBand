using System.Collections.Generic;
using UnityEngine;

public class TapUI : MonoBehaviour
{
    private Collider2D _collider;
    private SongController songController;

    public GameObject risingText;

    public event RawTapEvent OnScreenTap;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        songController = FindObjectOfType<SongController>();
    }

    // Update is called once per frame
    void Update()
    {
        var taps = CalculateTaps();
        if (taps.Count > 0 && OnScreenTap != null)
        {
            OnScreenTap(this, new RawTapEventArgs(taps));
        }
    }

    void OnEnable()
    {
        songController.OnSongStarted += HandleSongStarted;
        songController.OnSongFinished += HandleSongFinished;
    }

    void OnDisable()
    {
        songController.OnSongStarted -= HandleSongStarted;
        songController.OnSongFinished -= HandleSongFinished;
    }

    public void DisplayTapValueAt(RawTapData data, double value)
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject text = Instantiate(risingText);
        text.transform.position = data.position;
        text.transform.SetParent(canvas.transform);

        RisingText rising = text.GetComponent<RisingText>();
        rising.Text = "+" + value.ToString("F0");
        rising.Duration = 3f;
        rising.UpSpeed = 100f;

        if (data.isSpotlight)
        {
            rising.Color = Color.yellow;
            rising.FontSize = 20;
        }
        else
        {
            rising.Color = Color.white;
            rising.FontSize = 16;
        }

        rising.Init();
    }

    public void AutoTap()
    {
        RawTapData rawTapData = RandomTapEventArgs();
        if (OnScreenTap != null)
        {
            OnScreenTap(this, new RawTapEventArgs(new RawTapData[] { rawTapData }));
        }
    }

    private RawTapData RandomTapEventArgs()
    {
        int x = Random.Range(20, 481);
        int y = Random.Range(120, 701);
        Vector2 autoTapPosition = new Vector2(x, y);
        return new RawTapData(autoTapPosition, false);
    }

    private IList<RawTapData> CalculateTaps()
    {
        var result = new List<RawTapData>();

        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer)
        {
            for (var i = 0; i < Input.touchCount; ++i)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began)
                {
                    result.Add(new RawTapData(touch.position, IsSpotlightTap(touch.position)));
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                result.Add(new RawTapData(Input.mousePosition, IsSpotlightTap(Input.mousePosition)));
            }
        }

        return result;
    }

    private bool IsSpotlightTap(Vector2 pos)
    {
        Vector2 wp = Camera.main.ScreenToWorldPoint(pos);
        Collider2D hit = Physics2D.OverlapPoint(wp);

        if (hit != null && hit.gameObject.tag == Tags.SPOTLIGHT)
        {
            return true;
        }
        return false;
    }

    private void HandleSongStarted(object sender, SongEventArgs e)
    {
        _collider.enabled = true;
    }
    private void HandleSongFinished(object sender, SongEventArgs e)
    {
        _collider.enabled = false;
    }
}
