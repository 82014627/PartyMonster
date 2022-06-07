using Assets.MCVlibrary.Model;
using Game.Net;
using Game.View;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomWindow : BaseWindow
{
    public RoomWindow()
    {
        selfType = WindowType.RoomWindow;
        scenesType = ScenesType.Login;
        resident = false;
        resName = "UI/Windows/RoomWindow";
    }
 
    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        //發送訊息
        if (Input.GetKeyDown(KeyCode.Return))
        {
            BufferFactory.CreateAndSendPackage(1404, new RoomSendMsgC2S()
            {
                Text = ChatInput.text
            }); ;
        }
    }

    Text Time, ChatText;
    int time;
    Transform TeamA_Player, TeamB_Player;
    Scrollbar ChatVertical;
    InputField ChatInput;
    Dictionary<int, GameObject> rolesDic;
    Dictionary<int, GameObject> playerLoadDic;
    CancellationTokenSource ct;
    bool isLock;
    int lockHeroID;
    protected override void Awake()
    {
        base.Awake();
        BattleListener.Instance.Init();
        isLock = false;
        lockHeroID = 0;
        time = 20;
        rolesDic = new Dictionary<int, GameObject>();
        playerLoadDic = new Dictionary<int, GameObject>();

        Time = transform.Find("SelectTime/Time").GetComponent<Text>();
        ChatText = transform.Find("ChatBar/Viewport/Content/ChatText").GetComponent<Text>();
        TeamA_Player = transform.Find("TeamA/TeamA_Player");
        TeamB_Player = transform.Find("TeamB/TeamB_Player");
        ChatVertical = transform.Find("ChatBar/Scrollbar Vertical").GetComponent<Scrollbar>();
        ChatInput = transform.Find("Chat_Input").GetComponent<InputField>();

        RoomInfo roomInfo = RolesCtrl.Instance.GetRoomInfo();

        for (int i = 0; i < roomInfo.TeamA.Count; i++)
        {
            GameObject go = GameObject.Instantiate(TeamA_Player.gameObject, TeamA_Player.parent, false);
            if (i == 1)
            {
                go.transform.position += new Vector3(0, -247, 0);
            }
            go.transform.Find("NickName").GetComponent<Text>().text = roomInfo.TeamA[i].NickName;
            go.gameObject.SetActive(true);
            rolesDic[roomInfo.TeamA[i].RolesID] = go;
        }
        for (int i = 0; i < roomInfo.TeamB.Count; i++)
        {
            GameObject go = GameObject.Instantiate(TeamB_Player.gameObject, TeamB_Player.parent, false);
            if (i == 1)
            {
                go.transform.position += new Vector3(0, -247, 0);
            }
            go.transform.Find("NickName").GetComponent<Text>().text = roomInfo.TeamB[i].NickName;
            go.gameObject.SetActive(true);
            rolesDic[roomInfo.TeamB[i].RolesID] = go;
        }

        ct = new CancellationTokenSource();
        TimeDown();
    }
    /// <summary>
    /// 倒數計時
    /// </summary>
    async void TimeDown()
    {
        while (time > 0)
        {
            await Task.Delay(1000); //每隔一秒
            if (!ct.IsCancellationRequested)
            {
                time -= 1;
                Time.text = time.ToString();
            }
        }
    }

    protected override void OnAddListener()
    {
        base.OnAddListener();
        NetEvent.Instance.AddEventListener(1400, HandleRoomSelectHeroS2C);
        NetEvent.Instance.AddEventListener(1403, HandleRoomCloseS2C);
        NetEvent.Instance.AddEventListener(1404, HandleRoomSendMsgS2C);
        NetEvent.Instance.AddEventListener(1405, HandleRoomLockHeroS2C);
        NetEvent.Instance.AddEventListener(1406, HandleRoomLoadProgressS2C);
        NetEvent.Instance.AddEventListener(1407, HandleRoomToBattleS2C);
    }
    Transform TeamALoad_Player, TeamBLoad_Player;
    AsyncOperation async;
    /// <summary>
    /// 處理房間進入戰鬥
    /// </summary>
    /// <param name="response"></param>
    private void HandleRoomToBattleS2C(BufferEntity response)
    {
        Debug.Log("加載戰鬥");
        RoomToBattleS2C s2cMSG = ProtobufHelper.FromBytes<RoomToBattleS2C>(response.proto);
        RoomCtrl.Instance.SavePlayerList(s2cMSG.PlayerList);
        transform.Find("LoadBG/BG").gameObject.SetActive(true);
        TeamALoad_Player = transform.Find("LoadBG/TeamA_Load/TeamALoad_Player");
        TeamBLoad_Player = transform.Find("LoadBG/TeamB_Load/TeamBLoad_Player");

        for (int i = 0; i < s2cMSG.PlayerList.Count; i++)
        {
            GameObject go;
            if (s2cMSG.PlayerList.Count == 2)//兩人PK
            {
                //A隊伍
                if (s2cMSG.PlayerList[i].TeamID == 0)
                {
                    go = GameObject.Instantiate(TeamALoad_Player.gameObject, TeamALoad_Player.parent, false);
                }
                //B隊伍
                else
                {
                    go = GameObject.Instantiate(TeamBLoad_Player.gameObject, TeamBLoad_Player.parent, false);
                }
            }
            else//雙人團體戰
            {
                //A隊伍
                if (s2cMSG.PlayerList[i].TeamID == 0)
                {
                    go = GameObject.Instantiate(TeamALoad_Player.gameObject, TeamALoad_Player.parent, false);
                    if (i == 0)
                    {
                        go.transform.position += new Vector3(-150, 0, 0);
                    }
                    else if (i == 1)
                    {
                        go.transform.position += new Vector3(150, 0, 0);
                    }
                }
                //B隊伍
                else
                {
                    go = GameObject.Instantiate(TeamBLoad_Player.gameObject, TeamBLoad_Player.parent, false);
                    if (i == 2)
                    {
                        go.transform.position += new Vector3(-150, 0, 0);
                    }
                    else if (i == 3)
                    {
                        go.transform.position += new Vector3(150, 0, 0);
                    }
                }
            }

            go.transform.Find("Hero_head").GetComponent<Image>().sprite 
                = ResManager.Instance.LoadRoundHead(s2cMSG.PlayerList[i].HeroID);
            go.transform.Find("NickName").GetComponent<Text>().text
                = s2cMSG.PlayerList[i].RolesInfo.NickName;
            go.transform.Find("Progress").GetComponent<Text>().text = "0%";

            go.gameObject.SetActive(true);
            //緩存克隆出來的物件 更新進度
            playerLoadDic[s2cMSG.PlayerList[i].RolesInfo.RolesID] = go;
        }
        async = SceneManager.LoadSceneAsync("Battle");
        async.allowSceneActivation = false;

        //定時的去發送加載進度
        SendProgress();
    }
    /// <summary>
    /// 發送Server載入場景的進度
    /// </summary>
    async void SendProgress()
    {
        BufferFactory.CreateAndSendPackage(1406, new RoomLoadProgressC2S()
        {
            LoadProgress = (int)(async.progress >= 0.89f ? 100 : async.progress * 100)
        });
        await Task.Delay(500);
        if (ct.IsCancellationRequested == true)
        {
            return;
        }
        SendProgress();
    }
    /// <summary>
    /// 處理載入遊戲場景
    /// </summary>
    /// <param name="response"></param>
    private async void HandleRoomLoadProgressS2C(BufferEntity response)
    {
        //更新介面
        RoomLoadProgressS2C s2cMSG = ProtobufHelper.FromBytes<RoomLoadProgressS2C>(response.proto);
        if (s2cMSG.IsBattleStart == true)
        {
            for (int i = 0; i < s2cMSG.RolesID.Count; i++)
            {
                playerLoadDic[s2cMSG.RolesID[i]].transform.Find("Progress").GetComponent<Text>().text = "100%";
            }
            await Task.Delay(1000);
            async.allowSceneActivation = true;
            Close();
        }
        else
        {
            //如果還不能進入戰鬥
            for (int i = 0; i < s2cMSG.RolesID.Count; i++)
            {
                playerLoadDic[s2cMSG.RolesID[i]].transform.Find("Progress").GetComponent<Text>().text
                    = $"{s2cMSG.LoadProgress[i]}%";
            }
        }
    }
    /// <summary>
    /// 處理鎖定英雄
    /// </summary>
    /// <param name="response"></param>
    private void HandleRoomLockHeroS2C(BufferEntity response)
    {
        RoomLockHeroS2C s2cMSG = ProtobufHelper.FromBytes<RoomLockHeroS2C>(response.proto);

        rolesDic[s2cMSG.RolesID].transform.Find("Hero_State").GetComponent<Text>().text
            = "已鎖定";

        if (RoomCtrl.Instance.CheckIsSelfRoles(s2cMSG.RolesID))
        {
            isLock = true;//已鎖定英雄
        }
    }
    /// <summary>
    /// 處理房間訊息
    /// </summary>
    /// <param name="response"></param>
    private void HandleRoomSendMsgS2C(BufferEntity response)
    {
        RoomSendMsgS2C s2cMSG = ProtobufHelper.FromBytes<RoomSendMsgS2C>(response.proto);
        ChatText.text += $"{RoomCtrl.Instance.GetNickName(s2cMSG.RolesID)}:{s2cMSG.Text}\n";
        ChatInput.text = null;
        ChatVertical.value = 0;
    }
    /// <summary>
    /// 處理房間關閉
    /// </summary>
    /// <param name="response"></param>
    private void HandleRoomCloseS2C(BufferEntity response)
    {
        RoomCloseS2C s2cMSG = ProtobufHelper.FromBytes<RoomCloseS2C>(response.proto);
        RoomCtrl.Instance.RemoveRoomInfo();
        BattleListener.Instance.Release();
        WindowManager.Instance.OpenWindow(WindowType.LobbyWindow);
        Close();
    }
    /// <summary>
    /// 處理選擇英雄
    /// </summary>
    /// <param name="response"></param>
    private void HandleRoomSelectHeroS2C(BufferEntity response)
    {
        RoomSelectHeroS2C s2cMSG = ProtobufHelper.FromBytes<RoomSelectHeroS2C>(response.proto);
        rolesDic[s2cMSG.RolesID].transform.Find("hero_head").GetComponent<Image>().sprite
               = ResManager.Instance.LoadRoundHead(s2cMSG.HeroID.ToString());
        if (RoomCtrl.Instance.CheckIsSelfRoles(s2cMSG.RolesID))
        {
            //lockHeroID 緩存當前英雄選擇的ID
            lockHeroID = s2cMSG.HeroID;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ct.Cancel();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnRemoveListener()
    {
        base.OnRemoveListener();
        NetEvent.Instance.RemoveEventListener(1400, HandleRoomSelectHeroS2C);
        NetEvent.Instance.RemoveEventListener(1403, HandleRoomCloseS2C);
        NetEvent.Instance.RemoveEventListener(1404, HandleRoomSendMsgS2C);
        NetEvent.Instance.RemoveEventListener(1405, HandleRoomLockHeroS2C);
        NetEvent.Instance.RemoveEventListener(1406, HandleRoomLoadProgressS2C);
        NetEvent.Instance.RemoveEventListener(1407, HandleRoomToBattleS2C);
    }

    protected override void RegisterUIEvent()
    {
        base.RegisterUIEvent();
        for (int i = 0; i < buttonList.Length; i++)
        {
            switch (buttonList[i].name)
            {
                case "Hero1001":
                    SendSelectHero(buttonList[i], 1001);
                    break;
                case "Hero1002":
                    SendSelectHero(buttonList[i], 1002);
                    break;
                case "Hero1003":
                    SendSelectHero(buttonList[i], 1003);
                    break;
                case "Hero1004":
                    SendSelectHero(buttonList[i], 1004);
                    break;
                case "Hero1005":
                    SendSelectHero(buttonList[i], 1005);
                    break;
                case "LockBtn":
                    buttonList[i].onClick.AddListener(() =>
                    {
                        AudioManager.Instance.OKBtn();
                        if (isLock == false)
                        {
                            if (lockHeroID == 0)
                            {
                                Debug.Log("請先選擇英雄再鎖定");
                                return;
                            }

                            isLock = true;
                            BufferFactory.CreateAndSendPackage(1405, new RoomLockHeroC2S()
                            {
                                HeroID = lockHeroID
                            });
                        }
                    });
                    break;
                default:
                    break;
            }
        }
}
    /// <summary>
    /// 發送Server選擇英雄
    /// </summary>
    /// <param name="button">按鈕</param>
    /// <param name="heroID">英雄ID</param>
    private void SendSelectHero(Button button, int heroID)
    {
        button.onClick.AddListener(() =>
        {
            AudioManager.Instance.OKBtn();
            if (isLock == false)
            {
                BufferFactory.CreateAndSendPackage(1400, new RoomSelectHeroC2S()
                {
                    HeroID = heroID
                });
            }
        }
        );
    }
}
