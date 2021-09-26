using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View
{
    public class BaseWindow
    {
        protected Transform transform;//窗體
        protected string resName;//資源名稱
        protected bool resident;//是否常駐
        protected bool visible;//是否可見
        protected WindowType selfType;//窗體類型
        protected ScenesType scenesType;//場景類型

        //UI控件 按鈕
        protected Button[] buttonList;//按鈕列表

        //需要給子類提供的接口

        //初始化
        protected virtual void Awake()
        {
            buttonList = transform.GetComponentsInChildren<Button>(true);
            RegisterUIEvent();
        }
        //UI事件的註冊
        protected virtual void RegisterUIEvent()
        {

        }
        //添加監聽遊戲事件
        protected virtual void OnAddListener()
        {

        }
        //移除遊戲事件
        protected virtual void OnRemoveListener()
        {

        }
        //每次打開
        protected virtual void OnEnable()
        {

        }
        //每次關閉
        protected virtual void OnDisable()
        {

        }
        //每禎更新
        public virtual void Update(float deltaTime)
        {

        }

        //-----------------------WindowManager

        public void Open()
        {
            if (transform == null)
            {
                if (Create())
                {
                    Awake();
                }
            }
            if (transform.gameObject.activeSelf == false)
            {
                UIRoot.SetParent(transform, true, selfType == WindowType.TipsWindow);
                transform.gameObject.SetActive(true);
                visible = true;
                OnEnable(); //調用激活時候的事件
                OnAddListener(); //添加事件
            }
        }
        public void Close(bool isDestroy = false)
        {
            if (transform.gameObject.activeSelf == true)
            {
                OnRemoveListener(); //移除遊戲事件
                OnDisable(); //隱藏時候觸發的事件
                if (isDestroy == false)
                {
                    if (resident) //是否為常駐
                    {
                        transform.gameObject.SetActive(false);
                        UIRoot.SetParent(transform, false, false);
                    }
                    else
                    {
                        GameObject.Destroy(transform.gameObject);
                        transform = null;
                    }
                }
                else
                {
                    GameObject.Destroy(transform.gameObject);
                    transform = null;
                }
            }
            //不可見狀態
            visible = false;
        }
        public void PreLoad()
        {
            if (transform == null)
            {
                if (Create())
                {
                    Awake();
                }
            }
        }
        //獲取場景類型
        public ScenesType GetScenesType()
        {
            return scenesType;
        }
        //獲取窗體類型
        public WindowType GetWindowType()
        {
            return selfType;
        }

        public Transform GetRoot()
        {
            return transform;
        }
        public bool GetVisable()
        {
            return visible;
        }
        public bool GetResident()
        {
            return resident;
        }


        //------內部-----
        private bool Create()
        {
            if (string.IsNullOrEmpty(resName))
            {
                return false;
            }

            if (transform == null)
            {
                var obj = Resources.Load<GameObject>(resName);
                if (obj == null)
                {
                    Debug.LogError($"未找到UI預製物{selfType}");
                    return false;
                }
                transform = GameObject.Instantiate(obj).transform;

                transform.gameObject.SetActive(false);
                UIRoot.SetParent(transform, false, selfType == WindowType.TipsWindow);
                return true;
            }
            return true;
        }
    }
}

/// <summary>
/// 窗體類型
/// </summary>
public enum WindowType
{
    LoginWindow,
    LobbyWindow,
    RolesWindow,
    BattleWindow,
    StoreWindow,
    TipsWindow,
    RoomWindow,
}

/// <summary>
/// 場景類型，目的:提供根據場景類型進行預加載
/// </summary>
public enum ScenesType
{
    None,
    Login,
    Battle,
}
