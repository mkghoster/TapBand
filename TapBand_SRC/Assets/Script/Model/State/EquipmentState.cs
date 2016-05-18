using UnityEngine;
using System.Collections;

[System.Serializable]
public class EquipmentState {
	private int drumEquipmentId;
	private int guitarEquipmentId;
	private int bassEquipmentId;
	private int keyboardEquipmentId;

	public int DrumEquipmentId
	{
		get
		{
			return drumEquipmentId;
		}
		
		set
		{
			drumEquipmentId = value;
		}
	}

	public int GuitarEquipmentId
	{
		get
		{
			return guitarEquipmentId;
		}
		
		set
		{
			guitarEquipmentId = value;
		}
	}

	public int BassEquipmentId
	{
		get
		{
			return bassEquipmentId;
		}
		
		set
		{
			bassEquipmentId = value;
		}
	}

	public int KeyboardEquipmentId
	{
		get
		{
			return keyboardEquipmentId;
		}
		
		set
		{
			keyboardEquipmentId = value;
		}
	}

	public CharacterData CurrentDrumEquipment
	{
		get
		{
			if (drumEquipmentId == 0)
			{
				return null;
			}
			return GameData.instance.EquipmentDataList.Find(x => x.id == drumEquipmentId);
		}
	}

	public CharacterData CurrentGuitarEquipment
	{
		get
		{
			if (guitarEquipmentId == 0)
			{
				return null;
			}
			return GameData.instance.EquipmentDataList.Find(x => x.id == guitarEquipmentId);
		}
	}

	public CharacterData CurrentBassEquipment
	{
		get
		{
			if (bassEquipmentId == 0)
			{
				return null;
			}
			return GameData.instance.EquipmentDataList.Find(x => x.id == bassEquipmentId);
		}
	}

	public CharacterData CurrentKeyboardEquipment
	{
		get
		{
			if (keyboardEquipmentId == 0)
			{
				return null;
			}
			return GameData.instance.EquipmentDataList.Find(x => x.id == keyboardEquipmentId);
		}
	}

	public CharacterData NextDrumEquipment
	{

        get
        {
            CharacterData nextDrum = ListUtils.NextOf<CharacterData>(GameData.instance.EquipmentDataList, CurrentDrumEquipment);
            if (nextDrum.equipmentType == EquipmentType.DRUM)
            { 
                return nextDrum;
            }
            else
            { 
                return null;
            }
        }

	}

	public CharacterData NextGuitarEquipment
	{
        get
        {
            CharacterData nextGuitar = ListUtils.NextOf<CharacterData>(GameData.instance.EquipmentDataList, CurrentGuitarEquipment);
            if (nextGuitar.equipmentType == EquipmentType.GUITAR)
            {
                return nextGuitar;
            }
            else
            {
                return null;
            }
        }
    }

	public CharacterData NextBassEquipment
	{
        get
        {
            CharacterData nextBass = ListUtils.NextOf<CharacterData>(GameData.instance.EquipmentDataList, CurrentBassEquipment);
            if (nextBass.equipmentType == EquipmentType.BASS)
            {
                return nextBass;
            }
            else
            {
                return null;
            }
        }
    }

	public CharacterData NextKeyboardEquipment
	{
        get
        {
            CharacterData nextKeyboard = ListUtils.NextOf<CharacterData>(GameData.instance.EquipmentDataList, CurrentKeyboardEquipment);
            if (nextKeyboard.equipmentType == EquipmentType.KEYBOARD)
            {
                return nextKeyboard;
            }
            else
            {
                return null;
            }
        }
    }

}
