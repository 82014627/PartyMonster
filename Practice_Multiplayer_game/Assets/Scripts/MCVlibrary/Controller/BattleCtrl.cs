using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCtrl : Singleton<BattleCtrl>
{
    public void SaveDamage(int rolesID, int damage)
    {
        int totalDamage;
        if (!BattleModel.Instance.PlayersDamage.ContainsKey(rolesID))
        {
            BattleModel.Instance.PlayersDamage.Add(rolesID, damage);
        }
        else
        {
            totalDamage = BattleModel.Instance.PlayersDamage[rolesID] + damage;
            BattleModel.Instance.PlayersDamage[rolesID] = totalDamage;
        }
    }
    public int GetDamage(int rolesID)
    {
        if (!BattleModel.Instance.PlayersDamage.ContainsKey(rolesID))
        {
            BattleModel.Instance.PlayersDamage.Add(rolesID, 0);
            return 0;
        }
        else
        {
            return BattleModel.Instance.PlayersDamage[rolesID];
        }
    }
    public void SaveKills(int rolesID, int Kills)
    {
        int totalKills;
        if (!BattleModel.Instance.PlayersKills.ContainsKey(rolesID))
        {
            BattleModel.Instance.PlayersKills.Add(rolesID, Kills);
        }
        else
        {
            totalKills = BattleModel.Instance.PlayersKills[rolesID] + Kills;
            BattleModel.Instance.PlayersKills[rolesID] = totalKills;
        }
    }
    public int GetKills(int rolesID)
    {
        if (!BattleModel.Instance.PlayersKills.ContainsKey(rolesID))
        {
            BattleModel.Instance.PlayersKills.Add(rolesID, 0);
            return 0;
        }
        else
        {
            return BattleModel.Instance.PlayersKills[rolesID];
        }
    }
}
