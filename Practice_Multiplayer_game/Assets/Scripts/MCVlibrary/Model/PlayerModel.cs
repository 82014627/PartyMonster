using ProtoMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.MCVlibrary.Model
{
    public class PlayerModel : Singleton<PlayerModel>
    {
        public RolesInfo rolesInfo;
        public RoomInfo roomInfo;
        /// <summary>
        /// 確認是否為自己
        /// </summary>
        /// <param name="rolesID"></param>
        /// <returns></returns>
        public bool CheckIsSelf(int rolesID)
        {
            return rolesInfo.RolesID == rolesID;
        }
    }
}
