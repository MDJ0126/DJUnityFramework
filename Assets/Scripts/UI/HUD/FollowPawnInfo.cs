using TMPro;
using UnityEngine.UI;

namespace Game
{
    public class FollowPawnInfo : FollowHUD
    {
        #region Inspector

        public TMP_Text nameText;
        public Image fillImage;

        #endregion

        private Pawn _pawn = null;

        protected override void OnDisable()
        {
            base.OnDisable();
            if (_pawn)
            {
                _pawn.Status.OnChangedHp -= OnChangedHp;
                //_pawn.BaseStatus.OnChangedMp -= OnChangedMp;
            }
        }

        /// <summary>
        /// Pawn 세팅
        /// </summary>
        public void SetPawn(Pawn pawn)
        {
            _pawn = pawn;

            nameText.text = pawn.Name;

            pawn.Status.OnChangedHp += OnChangedHp;
            //pawn.BaseStatus.OnChangedMp += OnChangedMp;

            UpdateHpGauge((float)pawn.Status.hp / pawn.Status.baseStatus.maxHp);
        }

        /// <summary>
        /// 체력 변화 이벤트
        /// </summary>
        /// <param name="statusInfo"></param>
        private void OnChangedHp(StatusInfo statusInfo)
        {
            UpdateHpGauge((float)statusInfo.hp / statusInfo.baseStatus.maxHp);
        }

        /// <summary>
        /// 체력 게이지 업데이트
        /// </summary>
        /// <param name="value"></param>
        private void UpdateHpGauge(float value)
        {
            fillImage.fillAmount = value;
        }
    }
}
