using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BoosterData  {

    enum BoosterDataType { TapStrengthBoosterMultiplier, TapStrengthBoosterDuration, ExtraTimeBoosterBonus, AutoTapBoosterInterval, AutoTapBoosterDuration };
    public float TapStrengthBoosterMultiplier;
    public float TapStrengthBoosterDuration;
    public float ExtraTimeBoosterBonus;
    public float AutoTapBoosterInterval;
    public float AutoTapBoosterDuration;


    public void LoadBoostersData()
    {
        List<string> BoosterTypeNamesList = new List<string>(Enum.GetNames(typeof(BoosterDataType)));
        foreach (GeneralData data in GameData.instance.GeneralDataList)
        {
            if (BoosterTypeNamesList.Contains(data.name))
            {
                BoosterDataType MyBooster = (BoosterDataType)Enum.Parse(typeof(BoosterDataType), data.name, true);
                switch (MyBooster)
                {
                    case BoosterDataType.TapStrengthBoosterMultiplier:
                        float.TryParse(data.name, out TapStrengthBoosterMultiplier);
                        break;
                    case BoosterDataType.TapStrengthBoosterDuration:
                        float.TryParse(data.name, out TapStrengthBoosterDuration);
                        break;
                    case BoosterDataType.ExtraTimeBoosterBonus:
                        float.TryParse(data.name, out ExtraTimeBoosterBonus);
                        break;
                    case BoosterDataType.AutoTapBoosterInterval:
                        float.TryParse(data.name, out AutoTapBoosterInterval);
                        break;
                    case BoosterDataType.AutoTapBoosterDuration:
                        float.TryParse(data.name, out AutoTapBoosterDuration);
                        break;
                    default:
                        break;
                }
            }

        }
     } 
    }
