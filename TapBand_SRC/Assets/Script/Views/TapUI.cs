using System;
using System.Collections.Generic;
using UnityEngine;

public class TapUI : MonoBehaviour
{
    public GameObject risingText;

    public event RawTapEvent OnScreenTap;

    #region Private fields
    private Collider2D _collider;

    private SongController songController;
    private DailyEventController dailyEventController;

    private GameObject canvas;

    private GameObject firework;
    #endregion

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        songController = FindObjectOfType<SongController>();
        dailyEventController = FindObjectOfType<DailyEventController>();
        canvas = GameObject.Find("Canvas");

        _collider = GetComponent<Collider2D>();
        firework = GameObject.Find("Firework");
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
        songController.OnSongStarted += TurnOnColliderHandler;
        songController.OnSongFinished += TurnOffColliderHandler;

        //        dailyEventController.OnDailyEventStarted += TurnOffColliderHandler;
        //        dailyEventController.OnDailyEventFinished += TurnOnColliderHandler;
    }

    void OnDisable()
    {
        songController.OnSongStarted -= TurnOnColliderHandler;
        songController.OnSongFinished -= TurnOffColliderHandler;

        //        dailyEventController.OnDailyEventStarted -= TurnOffColliderHandler;
        //        dailyEventController.OnDailyEventFinished -= TurnOnColliderHandler;
    }

    public void DisplayTapValueAt(RawTapData data, double value)
    {
        GameObject text = Instantiate(risingText);
        text.transform.position = data.position;
        text.transform.SetParent(canvas.transform);

        Vector2 mouseScreenPositionToWorld = Camera.main.ScreenToWorldPoint(data.position);
        GameObject firework = (GameObject)Instantiate(Resources.Load("Firework"), mouseScreenPositionToWorld, Quaternion.identity);
        ParticleSystem tapParticle = firework.GetComponent<ParticleSystem>();
        tapParticle.Play();
        Destroy(firework, 2);

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

    public void AutoTap(RawTapEventArgs incomingEvent)
    {
        if (OnScreenTap != null)
        {
            OnScreenTap(this, incomingEvent);
        }
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
                    RawTapData tap;
                    if (GetValidTap(touch.position, out tap))
                    {
                        result.Add(tap);
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                RawTapData tap;
                if (GetValidTap(Input.mousePosition, out tap))
                {
                    result.Add(tap);
                }
            }
        }

        return result;
    }

    private bool GetValidTap(Vector2 pos, out RawTapData tapData)
    {
        Vector2 wp = Camera.main.ScreenToWorldPoint(pos);
        Collider2D hit = Physics2D.OverlapPoint(wp);
        bool isValid = false;
        bool isSpotlight = false;

        if (hit != null && hit.gameObject.tag == Tags.SPOTLIGHT)
        {
            isSpotlight = true;
            isValid = true;
        }
        else if (hit != null && hit.gameObject.tag == Tags.TAPAREA)
        {
            isValid = true;
        }
        tapData = new RawTapData(pos, isSpotlight);
        return isValid;
    }

    private void TurnOnColliderHandler(object sender, EventArgs e)
    {
        _collider.enabled = true;
    }
    private void TurnOffColliderHandler(object sender, EventArgs e)
    {
        _collider.enabled = false;
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }
}
