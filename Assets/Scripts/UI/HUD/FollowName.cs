using TMPro;

namespace Game
{
    public class FollowName : FollowHUD
    {
        #region Inspector

        public TMP_Text nameText;

        #endregion

        /// <summary>
        /// 캐릭터 머리 위에 표시할 이름 설정
        /// </summary>
        public void SetName(string name)
        {
            nameText.text = name;
        }
    }
}
