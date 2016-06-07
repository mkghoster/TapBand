using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BandMemberController : MonoBehaviour
{
    public event BandMemberSkillEvent OnSkillUpgraded;

    public Dictionary<CharacterType, IList<CharacterData>> UnlockedUpgrades { get; private set; }
    public Dictionary<CharacterType, CharacterData> NextUpgrades { get; private set; }

    private CurrencyController currencyController;

    private EquipmentState currentEquipmentState;
    private GameData gameData;

    public BandMemberController()
    {
        UnlockedUpgrades = new Dictionary<CharacterType, IList<CharacterData>>();
        NextUpgrades = new Dictionary<CharacterType, CharacterData>();
    }

    void Awake()
    {
        gameData = GameData.instance;
        currentEquipmentState = GameState.instance.Equipment;

        currencyController = FindObjectOfType<CurrencyController>();
        
        UnlockedUpgrades[CharacterType.Bass] = new List<CharacterData>();
        UnlockedUpgrades[CharacterType.Drums] = new List<CharacterData>();
        UnlockedUpgrades[CharacterType.Guitar1] = new List<CharacterData>();
        UnlockedUpgrades[CharacterType.Guitar2] = new List<CharacterData>();
        UnlockedUpgrades[CharacterType.Keyboards] = new List<CharacterData>();

        // Fill bass data, and state
        for (int i = 0; i < gameData.CharacterData1List.Count; i++)
        {
            var currentDataItem = gameData.CharacterData1List[i];
            if (currentDataItem.id <= currentEquipmentState.BassEquipmentId)
            {
                UnlockedUpgrades[CharacterType.Bass].Add(currentDataItem);
            }
            else if (currentDataItem.id == currentEquipmentState.BassEquipmentId + 1)
            {
                NextUpgrades[CharacterType.Bass] = currentDataItem;
            }
        }

        //Fill drums data and state
        for (int i = 0; i < gameData.CharacterData2List.Count; i++)
        {
            var currentDataItem = gameData.CharacterData2List[i];
            if (currentDataItem.id <= currentEquipmentState.DrumEquipmentId)
            {
                UnlockedUpgrades[CharacterType.Drums].Add(currentDataItem);
            }
            else if (currentDataItem.id == currentEquipmentState.DrumEquipmentId + 1)
            {
                NextUpgrades[CharacterType.Drums] = currentDataItem;
            }
        }

        // Guitar 1 data
        for (int i = 0; i < gameData.CharacterData3List.Count; i++)
        {
            var currentDataItem = gameData.CharacterData3List[i];
            if (currentDataItem.id <= currentEquipmentState.Guitar1EquipmentId)
            {
                UnlockedUpgrades[CharacterType.Guitar1].Add(currentDataItem);
            }
            else if (currentDataItem.id == currentEquipmentState.Guitar1EquipmentId + 1)
            {
                NextUpgrades[CharacterType.Guitar1] = currentDataItem;
            }
        }

        // Guitar 2 data
        for (int i = 0; i < gameData.CharacterData4List.Count; i++)
        {
            var currentDataItem = gameData.CharacterData4List[i];
            if (currentDataItem.id <= currentEquipmentState.Guitar2EquipmentId)
            {
                UnlockedUpgrades[CharacterType.Guitar2].Add(currentDataItem);
            }
            else if (currentDataItem.id == currentEquipmentState.Guitar2EquipmentId + 1)
            {
                NextUpgrades[CharacterType.Guitar2] = currentDataItem;
            }
        }

        // Keyboard data
        for (int i = 0; i < gameData.CharacterData5List.Count; i++)
        {
            var currentDataItem = gameData.CharacterData5List[i];
            if (currentDataItem.id <= currentEquipmentState.KeyboardEquipmentId)
            {
                UnlockedUpgrades[CharacterType.Keyboards].Add(currentDataItem);
            }
            else if (currentDataItem.id == currentEquipmentState.KeyboardEquipmentId + 1)
            {
                NextUpgrades[CharacterType.Keyboards] = currentDataItem;
            }
        }
    }

    public void UpgradeSkill(CharacterType character)
    {
        var unlockedUpgrade = NextUpgrades[character];
        UnlockedUpgrades[character].Add(unlockedUpgrade);

        switch (character)
        {
            case CharacterType.Bass:
                currentEquipmentState.BassEquipmentId = unlockedUpgrade.id;
                break;
            case CharacterType.Drums:
                currentEquipmentState.DrumEquipmentId = unlockedUpgrade.id;
                break;
            case CharacterType.Guitar1:
                currentEquipmentState.Guitar1EquipmentId = unlockedUpgrade.id;
                break;
            case CharacterType.Guitar2:
                currentEquipmentState.Guitar2EquipmentId = unlockedUpgrade.id;
                break;
            case CharacterType.Keyboards:
                currentEquipmentState.KeyboardEquipmentId = unlockedUpgrade.id;
                break;
            default:
                throw new NotImplementedException("You have added a character type, and not handled it here!");
        }

        NextUpgrades[character] = GetNextUpgrade(character);

        if (OnSkillUpgraded != null)
        {
            OnSkillUpgraded(this, new BandMemberSkillEventArgs(character, unlockedUpgrade));
        }
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

    public bool CanBuyNextUpgrade(CharacterType bandMember)
    {
        if (NextUpgrades[bandMember] == null)
        {
            return false;
        }
        return currencyController.CanBuyFromCoin(NextUpgrades[bandMember].upgradeCost);
    }
}
