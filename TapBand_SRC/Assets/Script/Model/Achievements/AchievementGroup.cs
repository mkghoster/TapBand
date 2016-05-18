using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AchievementGroup
{
    public AchievementGroup(NumericValueType valueType)
    {
        achievements = new List<Achievement>();
        ValueType = valueType;
        // TODO: load achievements in increasing order
        UpdateCurrentlyUnlocking();
    }

    public string Code { get; private set; }

    protected List<Achievement> achievements;
    /**
     * The list of achievements in the group (IEnumerable to the outside world)
     * The achievements must be loaded in increasing requirement order!
     */
    public IEnumerable<Achievement> Achievements
    {
        get
        {
            return achievements;
        }
    }

    /**
     * The next achievement to unlock 
     */
    public Achievement currentlyUnlocking { get; private set; }

    /**
     * The numeric type the achievement is attached to
     */
    public NumericValueType ValueType { get; private set; }

    /** 
     * accumlating the current value
     */
    //TODO: persist this!
    private double currentValue;

    /**
     *  Value changed event handler
     *  Check if the received value type is the correct one, and check unlocking, if it is.
     */
    public void OnNumericValueChanged(object sender, NumericValueChangedEventArgs e)
    {
        if (e.Type == ValueType)
        {
            currentValue += e.Delta;
            if (currentlyUnlocking != null && currentlyUnlocking.TrySetClaimable(currentValue))
            {
                // TODO: notify if achievement is claimable
                UpdateCurrentlyUnlocking();
            }
        }
    }
    /**
     * Loads the first unlocked achievement (or null, if every achievement is unlocked) into the currentlyUnlocking variable.
     */
    private void UpdateCurrentlyUnlocking()
    {
        // set it to null, in case every level is unlocked
        currentlyUnlocking = null;
        // search for the first locked achievement, and set it
        for (var i = 0; i < achievements.Count; i++)
        {
            if (!achievements[i].IsClaimed)
            {
                currentlyUnlocking = achievements[i];
                break;
            }
        }
    }
}
