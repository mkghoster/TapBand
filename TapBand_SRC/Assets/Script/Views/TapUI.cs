using System;
using System.Collections.Generic;

using UnityEngine;

public class TapArgs
{
    public ICollection<Vector2> positions;
    public ICollection<Vector2> spotlightPositions;

    public TapArgs()
    {
        positions = new List<Vector2>();
        spotlightPositions = new List<Vector2>();
    }

    public bool HasAnyTap()
    {
        return positions.Count > 0 || spotlightPositions.Count > 0;
    }
}

public class TapUI : MonoBehaviour
{
    private Collider2D _collider;

    public GameObject risingText;

    public delegate void TapEvent(TapArgs args);
    public event TapEvent OnTap;

    private BandMemberView[] bandMembers;

	void Start()
    {
        _collider = GetComponent<Collider2D>();
        bandMembers = FindObjectsOfType<BandMemberView>();
	}
	
	// Update is called once per frame
	void Update()
    {
        TapArgs args = CalculateTapEventArgs();
        if (args.HasAnyTap())
        {
            AnimateCharacters();
            if (OnTap != null)
                OnTap(args);
        }

	}

    private void AnimateCharacters()
    {
        foreach (BandMemberView view in bandMembers)
        {
            view.ForceClickOnBandMember();
        }
    }
    
    // waaaaaaay too much parameters, should be less than 3
    public void DisplayTapValueAt(Vector2 position, BigInteger value, bool special)
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject text = (GameObject) Instantiate(risingText);
        text.transform.position = position;
        text.transform.SetParent(canvas.transform);

        RisingText rising = text.GetComponent<RisingText>();
        rising.Text = "+" + value.ToString();
        rising.Duration = 3f;
        rising.UpSpeed = 100f;
        
        if (special) {
            rising.Color = Color.yellow;
            rising.FontSize = 20;
        } else
        {
            rising.Color = Color.white;
            rising.FontSize = 16;
        }

        rising.Init();
    }

    private TapArgs CalculateTapEventArgs()
    {
        TapArgs args = new TapArgs();

        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer)
        {

            for (var i = 0; i < Input.touchCount; ++i)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began)
                {
                    CalculateWithPosition(touch.position, args);
                }
            }
        } else
        {
            if (Input.GetMouseButtonDown(0))
            {
                CalculateWithPosition(Input.mousePosition, args);
            }
        }
        return args;
    }

    private void CalculateWithPosition(Vector2 pos, TapArgs args)
    {
        
        Vector2 wp = Camera.main.ScreenToWorldPoint(pos);
        Collider2D hit = Physics2D.OverlapPoint(wp);
        if (hit.gameObject.tag == "Spotlight")
        {
            args.spotlightPositions.Add(pos);
        }
        else if (hit == _collider)
        {
            args.positions.Add(pos);
        }
    }
}
