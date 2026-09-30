using System.Collections.Generic;

namespace Game
{
    public class HUDManager : SingletonBehaviour<HUDManager>
    {
        public ObjectPool followNamePool;
        public ObjectPool followSpeechBubblePool;
        public ObjectPool followHealthBarPool;

        /// <summary>
        /// Pawn에 연결된 모든 추적 HUD 숨기기
        /// </summary>
        public void DetachFollowHUD(List<FollowHUD> followHUDs)
        {
            foreach (FollowHUD followHUD in followHUDs)
            {
                DetachFollowHUD(followHUD);
            }
        }

        /// <summary>
        /// 추적 HUD 하나를 숨겨 오브젝트 풀에서 재사용할 수 있게 처리
        /// </summary>
        public void DetachFollowHUD(FollowHUD followHUD)
        {
            followHUD.Hide();
        }

        /// <summary>
        /// 이름 HUD를 풀에서 가져와 Pawn의 이름 앵커에 연결
        /// </summary>
        public FollowName AttachFollowName(Pawn pawn, string name)
        {
            FollowName followName = followNamePool.Get<FollowName>();
            followName.SetTarget(pawn.nameAnchor);
            followName.SetName(name);
            followName.Show();
            return followName;
        }

        /// <summary>
        /// 말풍선 HUD를 풀에서 가져와 Pawn의 말풍선 앵커에 연결
        /// </summary>
        public FollowSpeechBubble AttachFollowSpeechBubble(Pawn pawn, string text)
        {
            FollowSpeechBubble followSpeechBubble = followSpeechBubblePool.Get<FollowSpeechBubble>();
            followSpeechBubble.SetTarget(pawn.balloonAnchor);
            followSpeechBubble.SetText(text);
            followSpeechBubble.Show();
            return followSpeechBubble;
        }

        /// <summary>
        /// 체력 HUD를 풀에서 가져와 Pawn의 체력바 앵커에 연결
        /// </summary>
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
