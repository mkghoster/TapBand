using System;

public delegate void CharacterEvent(object sender, CharacterEventArgs e);

public class CharacterEventArgs : EventArgs
{
    public CharacterType Character { get; private set; }
    public CharacterData Data { get; private set; }
    
    public CharacterEventArgs(CharacterType character, CharacterData data)
    {
        Character = character;
        Data = data;
    }
}
