using Assets.MCVlibrary.Model;
using ProtoMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.MCVlibrary.Controller
{
    public class LoginCtrl : Singleton<LoginCtrl>
    {
        /// <summary>
        /// 保存角色數據
        /// </summary>
        /// <param name="rolesInfo"></param>
        public void SaveRolesInfo(RolesInfo rolesInfo)
        {
            PlayerModel.Instance.rolesInfo = rolesInfo;
        }
    }
}
