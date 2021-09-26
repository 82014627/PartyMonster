using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager : Singleton<ResManager>
{
    /// <summary>
    /// 加載UI窗體
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public GameObject LoadUI(string path)
    {
        GameObject go = Resources.Load<GameObject>($"UIPrefab/{path}");
        if (go == null)
        {
            Debug.LogError($"沒有找到UI窗體:{path}");
            return null;
        }
        GameObject obj = GameObject.Instantiate(go);
        return obj;
    }
    /// <summary>
    /// 加載圓形頭像
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public Sprite LoadRoundHead(string path)
    {
        return Resources.Load<Sprite>($"Image/Round/{path}");
    }
    public Sprite LoadRoundHead(int heroID)
    {
        return Resources.Load<Sprite>($"Image/Round/{heroID}");
    }
    /// <summary>
    /// 加載玩家載入畫面
    /// </summary>
    /// <param name="HeroID"></param>
    /// <returns></returns>
    public Sprite LoadHeroTexture(int HeroID)
    {
        return Resources.Load<Sprite>($"Image/HeroTexture/{HeroID}");
    }
    /// <summary>
    /// 加載角色模型
    /// </summary>
    /// <param name="heroID"></param>
    /// <returns></returns>
    public GameObject LoadHero(int heroID)
    {
        GameObject go = Resources.Load<GameObject>($"Hero/{heroID}/Model/{heroID}");
        return GameObject.Instantiate(go);
    }
    /// <summary>
    /// 加載角色血條 藍條 等級 暱稱
    /// </summary>
    /// <returns></returns>
    public GameObject LoadHUD()
    {
        GameObject go = Resources.Load<GameObject>($"HUD/HeroHead");
        return GameObject.Instantiate(go);
    }
    public GameObject LoadEffect(int heroID, string skillName)
    {
        GameObject go = Resources.Load<GameObject>($"Hero/{heroID}/Effect/{skillName}");
        return GameObject.Instantiate(go);
    }

    internal Sprite LoadHeroSkill(int heroID, string skillName)
    {
        return Resources.Load<Sprite>($"Hero/{heroID}/UI_Skill/{skillName}");
    }

    internal Sprite LoadHeadIcon(int heroID)
    {
        return Resources.Load<Sprite>($"Hero/{heroID}/UI_Head/{heroID}");
    }
    internal Sprite LoadSkillIcon(int heroID,string skill)
    {
        return Resources.Load<Sprite>($"Hero/{heroID}/Skillicon/{skill}");
    }
    internal GameObject LoadProps(int propsID, int x)
    {
        GameObject props = Resources.Load<GameObject>($"Props/{propsID}/Model/{propsID}");
        return GameObject.Instantiate(props, new Vector3(x, 9, 2), props.transform.rotation);
    }
    internal GameObject LoadPropsEffect(int propsID, Vector3 vector3)
    {
        GameObject props = Resources.Load<GameObject>($"Props/{propsID}/Effect/{propsID}");
        return GameObject.Instantiate(props);
    }
}

