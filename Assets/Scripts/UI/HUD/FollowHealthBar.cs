using UnityEngine.UI;

namespace Game
{
    public class FollowHealthBar : FollowHUD
    {
        #region Inspector

        public Image fillImage;

        #endregion

        /// <summary>
        /// 체력 정보를 표시할 Pawn 연결
        /// </summary>
        public void SetPawn(Pawn pawn)
        {

        }
    }
}
