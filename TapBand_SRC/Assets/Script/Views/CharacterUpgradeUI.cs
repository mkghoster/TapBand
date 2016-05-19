using UnityEngine;
using System.Collections;

public class CharacterUpgradeUI : MonoBehaviour
{
    public event CharacterEvent OnCharacterUpgrade;

    public CharacterType CharacterType { get; set; }

    private CharacterData upgradeableCharacter;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
