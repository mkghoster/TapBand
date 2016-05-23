using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BandMemberController : MonoBehaviour
{
    public Dictionary<CharacterType, IList<CharacterData>> UnlockedUpgrades { get; private set; }
    public Dictionary<CharacterType, CharacterData> NextUpgrades { get; private set; }

    private EquipmentState currentEquipmentState;
    private GameData gameData;

    public BandMemberController()
    {
        UnlockedUpgrades = new Dictionary<CharacterType, IList<CharacterData>>();
        NextUpgrades = new Dictionary<CharacterType, CharacterData>();
    }

    void Awake()
    {

    }

    void Start()
    {

    }

    private void UnlockUpgrade(object sender, BandMemberSkillEventArgs e)
    {
        UnlockedUpgrades[e.Character].Add(e.UnlockedSkill);

        switch (e.Character)
        {
            case CharacterType.Bass:
                currentEquipmentState.BassEquipmentId = e.UnlockedSkill.id;
                break;
            case CharacterType.Drums:
                currentEquipmentState.DrumEquipmentId = e.UnlockedSkill.id;
                break;
            case CharacterType.Guitar1:
                currentEquipmentState.Guitar1EquipmentId = e.UnlockedSkill.id;
                break;
            case CharacterType.Guitar2:
                currentEquipmentState.Guitar2EquipmentId = e.UnlockedSkill.id;
                break;
            case CharacterType.Keyboards:
                currentEquipmentState.KeyboardEquipmentId = e.UnlockedSkill.id;
                break;
            default:
                throw new NotImplementedException("You have added a character type, and not handled it here!");
        }

        NextUpgrades[e.Character] = GetNextUpgrade(e.Character);
    }

    private CharacterData GetNextUpgrade(CharacterType character)
    {
        IList<CharacterData> characterUpgradeList;
        var currentCharacterUpgrades = UnlockedUpgrades[character];

        switch (character)
        {
            case CharacterType.Bass:
                characterUpgradeList = gameData.CharacterData1List;
                break;
            case CharacterType.Drums:
                characterUpgradeList = gameData.CharacterData2List;
                break;
            case CharacterType.Guitar1:
                characterUpgradeList = gameData.CharacterData3List;
                break;
            case CharacterType.Guitar2:
                characterUpgradeList = gameData.CharacterData4List;
                break;
            case CharacterType.Keyboards:
                characterUpgradeList = gameData.CharacterData5List;
                break;
            default:
                throw new NotImplementedException("You have added a character type, and not handled it here!");
        }

        for (int i = 0; i < characterUpgradeList.Count; i++)
        {
            if (characterUpgradeList[i].id == currentCharacterUpgrades[currentCharacterUpgrades.Count - 1].id + 1)
            {
                return characterUpgradeList[i];
            }
        }
        return null;//TODO: error handling;
    }
}
