using TMPro;

namespace Game
{
    public class FollowName : FollowHUD
    {
        #region Inspector

        public TMP_Text nameText;

        #endregion

        public void SetName(string name)
        {
            nameText.text = name;
        }
    }
}