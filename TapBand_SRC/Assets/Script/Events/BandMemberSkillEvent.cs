using UnityEngine;
using System.Collections;
using System;

public delegate void BandMemberSkillEvent(object sender, BandMemberSkillEventArgs e);

public class BandMemberSkillEventArgs : EventArgs
{
    public BandMemberSkillEventArgs(CharacterType character, CharacterData unlockedSkill)
    {
        UnlockedSkill = unlockedSkill;
        Character = character;
    }

    public CharacterData UnlockedSkill { get; private set; }
    public CharacterType Character { get; private set; }
}
