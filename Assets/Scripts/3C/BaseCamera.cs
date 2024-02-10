using UnityEngine;
using System.Collections;

public class BaseCamera : MonoBehaviour
{
    #region Attributes

        
        [SerializeField]
        protected float     m_FOVOnBoost                    = 80;

        
        [SerializeField]
        protected float     m_BoostFovTransitionDuration    = 1;

        
        protected float     m_DefaultFOV                    = 60;

        
        protected Camera    m_Camera                        = null;

        
        protected Coroutine m_RunningBoostCoroutine         = null;

    #endregion

    #region Getters & Setters

        
        public Camera CameraComponent
        {
            get { return m_Camera; }
        }

    #endregion

    #region MonoBehaviour

        
        protected void Start()
        {
            
            m_Camera = GetComponent<Camera>();

            
            m_DefaultFOV = m_Camera.fieldOfView;
        }

    #endregion

    #region Public Manipulators

        /// <summary>
        /// 
        /// <param name="_Mode">Enabled / Disabled mode</param>
        /// </summary>
        public void SetBoostView(bool _Mode)
        {
            
            if (m_RunningBoostCoroutine != null)
            {
                StopCoroutine(m_RunningBoostCoroutine);
            }

            
            if (_Mode)
            {
                m_RunningBoostCoroutine = StartCoroutine(CR_SetBoostView(m_Camera.fieldOfView, m_FOVOnBoost));
            }
            else
            {
                m_RunningBoostCoroutine = StartCoroutine(CR_SetBoostView(m_Camera.fieldOfView, m_DefaultFOV));
            }
        }

    #endregion

    #region Private Manipulators

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_FromFOV">Start FOV</param>
        /// <param name="_ToFOV">End FOV</param>
        private IEnumerator CR_SetBoostView(float _FromFOV, float _ToFOV)
        {
            float t = 0;

            float duration = Mathf.Abs(_ToFOV - _FromFOV) / Mathf.Abs(m_FOVOnBoost - m_DefaultFOV);
            duration *= m_BoostFovTransitionDuration;

            while (t < duration)
            {
                t += Time.deltaTime;

                if (t > duration)
                {
                    t = duration;
                }

                m_Camera.fieldOfView = Mathf.Lerp(_FromFOV, _ToFOV, t / duration);

                yield return null;
            }
        }

    #endregion
}
