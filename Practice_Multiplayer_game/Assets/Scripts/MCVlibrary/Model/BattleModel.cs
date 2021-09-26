using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleModel : Singleton<BattleModel>
{
    public Dictionary<int, int> PlayersDamage = new Dictionary<int, int>();
    public Dictionary<int, int> PlayersKills = new Dictionary<int, int>();

    public void Clear()
    {
        PlayersDamage.Clear();
        PlayersKills.Clear();
    }
}
