using UnityEngine.UI;

namespace Game
{
    public class FollowHealthBar : FollowHUD
    {
        #region Inspector

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
        /// 체력 정보를 표시할 Pawn 연결
        /// </summary>
        public void SetPawn(Pawn pawn)
        {
            _pawn = pawn;
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
