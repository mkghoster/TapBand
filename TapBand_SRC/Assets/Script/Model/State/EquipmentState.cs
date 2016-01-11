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

	public EquipmentData CurrentDrumEquipment
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

	public EquipmentData CurrentGuitarEquipment
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

	public EquipmentData CurrentBassEquipment
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

	public EquipmentData CurrentKeyboardEquipment
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

	public EquipmentData NextDrumEquipment
	{

        get
        {
            EquipmentData nextDrum = ListUtils.NextOf<EquipmentData>(GameData.instance.EquipmentDataList, CurrentDrumEquipment);
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

	public EquipmentData NextGuitarEquipment
	{
        get
        {
            EquipmentData nextGuitar = ListUtils.NextOf<EquipmentData>(GameData.instance.EquipmentDataList, CurrentGuitarEquipment);
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

	public EquipmentData NextBassEquipment
	{
        get
        {
            EquipmentData nextBass = ListUtils.NextOf<EquipmentData>(GameData.instance.EquipmentDataList, CurrentBassEquipment);
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

	public EquipmentData NextKeyboardEquipment
	{
        get
        {
            EquipmentData nextKeyboard = ListUtils.NextOf<EquipmentData>(GameData.instance.EquipmentDataList, CurrentKeyboardEquipment);
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
