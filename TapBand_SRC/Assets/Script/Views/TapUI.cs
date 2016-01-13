using System;
using System.Collections.Generic;

using UnityEngine;

public class TapArgs
{
    public ICollection<Vector3> positions;

    public TapArgs()
    {
        positions = new List<Vector3>();
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
        if (Input.touchCount > 0)
        {
            TapArgs args = CalculateTapEventArgs();
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

    void OnMouseDown()
    {
        TapArgs args = new TapArgs();
        args.positions.Add(Input.mousePosition);
        AnimateCharacters();
        if (OnTap != null)
            OnTap(args);
    }

    public void DisplayTapValueAt(Vector3 position, BigInteger value)
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject text = (GameObject) Instantiate(risingText);
        text.transform.position = position;
        text.transform.SetParent(canvas.transform);
        text.GetComponent<RisingText>().Setup("+" + value.ToString(), 3f, 100f);
    }

    private TapArgs CalculateTapEventArgs()
    {
        TapArgs args = new TapArgs();
        for (var i = 0; i < Input.touchCount; ++i)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 wp = Camera.main.ScreenToWorldPoint(touch.position);
                if (_collider == Physics2D.OverlapPoint(wp))
                {
                    args.positions.Add(touch.position);
                }
            }
        }

        return args;
    }
}
