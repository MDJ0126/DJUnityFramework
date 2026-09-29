using System.Collections.Generic;

namespace Game
{
    public class HUDManager : SingletonBehaviour<HUDManager>
    {
        public ObjectPool followNamePool;
        public ObjectPool followSpeechBubblePool;
        public ObjectPool followHealthBarPool;

        public void DetachFollowHUD(List<FollowHUD> followHUDs)
        {
            foreach (FollowHUD followHUD in followHUDs)
            {
                DetachFollowHUD(followHUD);
            }
        }

        public void DetachFollowHUD(FollowHUD followHUD)
        {
            followHUD.Hide();
        }

        public FollowName AttachFollowName(Pawn pawn, string name)
        {
            FollowName followName = followNamePool.Get<FollowName>();
            followName.SetTarget(pawn.nameAnchor);
            followName.SetName(name);
            followName.Show();
            return followName;
        }

        public FollowSpeechBubble AttachFollowSpeechBubble(Pawn pawn, string text)
        {
            FollowSpeechBubble followSpeechBubble = followSpeechBubblePool.Get<FollowSpeechBubble>();
            followSpeechBubble.SetTarget(pawn.balloonAnchor);
            followSpeechBubble.SetText(text);
            followSpeechBubble.Show();
            return followSpeechBubble;
        }

        public FollowHealthBar AttachFollowHealthBar(Pawn pawn)
        {
            FollowHealthBar followHealthBar = followHealthBarPool.Get<FollowHealthBar>();
            followHealthBar.SetTarget(pawn.healthBarAnchor);
            followHealthBar.SetPawn(pawn);
            followHealthBar.Show();
            return followHealthBar;
        }
    }
}