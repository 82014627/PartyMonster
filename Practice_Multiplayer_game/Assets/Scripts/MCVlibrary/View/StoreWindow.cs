using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.View
{
    public class StoreWindow : BaseWindow
    {
        public StoreWindow()
        {
            resName = "UI/Windows/StoreWindow";
            resident = true;
            selfType = WindowType.StoreWindow;
            scenesType = ScenesType.Login;
        }
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnAddListener()
        {
            base.OnAddListener();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnRemoveListener()
        {
            base.OnRemoveListener();
        }

        protected override void RegisterUIEvent()
        {
            base.RegisterUIEvent();
            foreach (var button in buttonList)
            {
                switch (button.name)
                {
                    case "BuyButton":
                        button.onClick.AddListener(OnBuyButtonClick);
                        break;
                }
            }
        }

        public void OnBuyButtonClick()
        {
            Debug.Log("點擊成功");
            /*StoreCtrl.Instance.SaveProp(new Prop());
            var prop = StoreCtrl.Instance.GetProp(1001);*/ //Controller調用
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (Input.GetKeyDown(KeyCode.A))
            {
                Close();
            }
        }
    }

}
