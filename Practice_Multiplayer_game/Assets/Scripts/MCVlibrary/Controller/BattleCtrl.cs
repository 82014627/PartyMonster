using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCtrl : Singleton<BattleCtrl>
{
    /// <summary>
    /// 保存傷害數據
    /// </summary>
    /// <param name="rolesID"></param>
    /// <param name="damage"></param>
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
    /// <summary>
    /// 獲取傷害數據
    /// </summary>
    /// <param name="rolesID"></param>
    /// <returns></returns>
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
    /// <summary>
    /// 保存擊殺數據
    /// </summary>
    /// <param name="rolesID"></param>
    /// <param name="Kills"></param>
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
    /// <summary>
    /// 獲取擊殺數據
    /// </summary>
    /// <param name="rolesID"></param>
    /// <returns></returns>
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
