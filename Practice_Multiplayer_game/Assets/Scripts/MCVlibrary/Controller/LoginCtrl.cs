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
        public void SaveRolesInfo(RolesInfo rolesInfo)
        {
            PlayerModel.Instance.rolesInfo = rolesInfo;
        }
    }
}
