using TMPro;

namespace Game
{
    public class FollowSpeechBubble : FollowHUD
    {
        #region Inspector

        public TMP_Text speechText;

        #endregion

        /// <summary>
        /// 말풍선에 표시할 문구 설정
        /// </summary>
        public void SetText(string text)
        {
            speechText.text = text;
        }
    }
}
