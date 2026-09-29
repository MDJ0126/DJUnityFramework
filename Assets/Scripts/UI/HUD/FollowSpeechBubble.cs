using TMPro;

namespace Game
{
    public class FollowSpeechBubble : FollowHUD
    {
        #region Inspector

        public TMP_Text speechText;

        #endregion

        public void SetText(string text)
        {
            speechText.text = text;
        }
    }
}