using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    #region Attributes



        // Movement speed
        [SerializeField]
        private float       m_MovementSpeed         = 10;

        // Sprint scale
        [SerializeField]
        private float       m_SprintScale           = 3;

        // Rotation speed
        [SerializeField]
        private float       m_RotationSpeed         = 60;


        // Rigidbody
        private Rigidbody   m_Rigidbody             = null;

        // Rotation limit
        [SerializeField]
        [Range(0, 90)]
        private float       m_PitchLimit            = 45;
        [SerializeField]
        [Range(0, 90)]
        private float       m_RollLimit             = 45;

        // Yaw sensibility on roll
        [SerializeField]
        [Range(0, 1)]
        private float       m_YawSensibilityOnRoll  = 0.33f;

        // Is dodging
        private bool        m_IsDodging             = false;

        // Game camera
        private BaseCamera  m_Camera             = null;

    #endregion

    #region MonoBehaviour

        // Use this for initialization
        void Start()
        {


            // Get rigidbody
            m_Rigidbody = GetComponent<Rigidbody>();

            if (m_Rigidbody != null)
            {
                m_Rigidbody.useGravity = false;
            }

        }

        // Called at fixed time
        void FixedUpdate()
        {
            // Update yaw from roll angle. Writtent in fixed update to avoid camera lerp break
            UpdateYawFromRoll();
        }

        // On collision enter
        void OnCollisionEnter(Collision collision)
        {
            if (m_Rigidbody != null)
            {
                /*
                // Falling
                m_Rigidbody.useGravity = true;

                // Disable controller
                m_Controller.enabled = false;

                // Stop boost view
                if (m_Camera != null)
                {
                    m_Camera.SetBoostView(false);
                }
                 * */
            }
        }

    #endregion

    #region Getters & Setters

        // Base camera accessors
        public BaseCamera BaseCamera
        {
            get { return m_Camera; }
            set { m_Camera = value; }
        }

        // Is dodging
        public bool IsDodging
        {
            get { return m_IsDodging; }
        }



    #endregion

    #region Public Manipulators


        public void Move(Vector3 _Direction, bool _Sprint, bool _Dodge = false)
        {
            if (m_Rigidbody != null)
            {
                // Speed
                float speed = m_MovementSpeed;



                if (_Sprint)
                {
                    speed *= m_SprintScale;
                }

                // Movement
                m_Rigidbody.MovePosition(m_Rigidbody.position + _Direction.normalized * Time.deltaTime * speed);
            }
        }

        public void ResetRoll()
        {
            // Calculate rotation
            Vector3 rightNoY = Vector3.Cross(Vector3.up, transform.forward);
            rightNoY.y = 0;
            Quaternion rotator = Quaternion.FromToRotation(transform.right, rightNoY);

            // Apply rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, rotator * transform.rotation, Time.deltaTime);
        }



        public void ResetPitch()
        {
            // Calculate rotation
            Vector3 forwardNoY = transform.forward;
            forwardNoY.y = 0;
            Quaternion rotator = Quaternion.FromToRotation(transform.forward, forwardNoY);

            // Apply rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, rotator * transform.rotation, Time.deltaTime);
        }


        public void ResetOrientation()
        {
            // Projected forward
            Vector3 forwardNoY = transform.forward;
            forwardNoY.y = 0;
            forwardNoY.Normalize();

            // Apply projected forward
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forwardNoY), Time.deltaTime);
        }



        public void AddRoll(float _AdditiveRoll, bool _Dodge = false)
        {
            // Time based rotation
            if (!_Dodge)
            {
                _AdditiveRoll *= Time.deltaTime * m_RotationSpeed;
            }


            // Add rotation
            Quaternion rotator = Quaternion.AngleAxis(_AdditiveRoll, transform.forward);
            transform.rotation = rotator * transform.rotation;
        }

        public void AddPitch(float _AdditivePitch)
        {

            // Time based rotation
            _AdditivePitch *= Time.deltaTime * m_RotationSpeed;

            // Add rotation
            Quaternion rotator = Quaternion.AngleAxis(_AdditivePitch, transform.right);
            transform.rotation = rotator * transform.rotation;
        }

    #endregion

    #region Private Manipulators

        private bool CheckPitchLimit(float _AdditivePitch)
        {
            Vector3 forwardNoY = transform.forward;
            forwardNoY.y = 0;
            forwardNoY.Normalize();

            // Roll angle
            float pitch = Vector3.Angle(transform.forward, forwardNoY);

            if (Vector3.Cross(transform.right, forwardNoY).y < 0)
            {
                pitch *= -1;
            }

            // Roll limit
            if (m_PitchLimit > 0)
            {
                if (pitch + _AdditivePitch > m_RollLimit)
                {
                    return false;
                }
            }

            return true;
        }


        private bool CheckRollLimit(float _AdditiveRoll)
        {
            Vector3 rightNoY = transform.right;
            rightNoY.y = 0;
            rightNoY.Normalize();

            // Roll angle
            float roll = Vector3.Angle(transform.right, rightNoY);

            if (Vector3.Cross(transform.forward, rightNoY).y < 0)
            {
                roll *= -1;
            }

            // Roll limit
            if (m_RollLimit > 0)
            {
                if (roll + _AdditiveRoll > m_RollLimit)
                {
                    return false;
                }
            }

            return true;
        }


        private void UpdateYawFromRoll()
        {
            if (!m_IsDodging)
            {
                float upSign = 1;

                if (transform.up.y < 0)
                {
                    upSign = -1;
                }

                float yawSensibility = m_YawSensibilityOnRoll;
                Vector3 rightNoY = transform.right;
                rightNoY.y = 0;
                rightNoY.Normalize();
                float dot = Vector3.Dot(transform.up, rightNoY);

                yawSensibility *= dot * upSign;

                Quaternion rotator = Quaternion.AngleAxis(yawSensibility, Vector3.up);
                transform.rotation = rotator * transform.rotation;
            }
        }

    #endregion
}
