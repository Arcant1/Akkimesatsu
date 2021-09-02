using Cinemachine;
/*using UniRx;
using UniRx.Triggers;
using UnityEngine;
namespace RPG.Controllers
{
    public class CameraController : MonoBehaviour
    {
        public CinemachineFreeLook currentCamera;

        private CinemachineFreeLook.Orbit[] originalOrbits;
        [Tooltip("The minimum scale for the orbits")]
        [Range(0.01f, 3f)]
        public float minZoom;

        [Tooltip("The maximum scale for the orbits")]
        [Range(1f, 20f)]
        public float maxZoom;

        [Tooltip("The zoom axis.  Value is 0..1.  How much to scale the orbits")]
        [AxisStateProperty]
        public AxisState zAxis = new AxisState(0, 1, false, true, 50f, 0.1f, 0.1f, "Mouse ScrollWheel", true);

        private float defaultValue;
        void OnValidate()
        {
            minZoom = Mathf.Max(0.01f, minZoom);
            maxZoom = Mathf.Max(minZoom, maxZoom);
        }
        private void OnEnable()
        {
            zAxis.Value = defaultValue;
        }
        private void Start()
        {
            defaultValue = zAxis.Value;
            var events = this.UpdateAsObservable();

            var initMovement = events
                .Where(_ => Input.GetMouseButton(1))
                .Subscribe(_ =>
                {
                    if (currentCamera)
                    {
                        currentCamera.m_XAxis.m_InputAxisValue = Input.GetAxis("Mouse X");
                        currentCamera.m_YAxis.m_InputAxisValue = Input.GetAxis("Mouse Y");
                    }
                });

            var endMovement = events
                .Where(_ => Input.GetMouseButtonUp(1))
                .Subscribe(_ =>
                {
                    if (currentCamera)
                    {
                        currentCamera.m_XAxis.m_InputAxisValue = 0f;
                        currentCamera.m_YAxis.m_InputAxisValue = 0f;
                    }
                });

            if (currentCamera != null)
            {
                originalOrbits = new CinemachineFreeLook.Orbit[currentCamera.m_Orbits.Length];
                for (int i = 0; i < originalOrbits.Length; i++)
                {
                    originalOrbits[i].m_Height = currentCamera.m_Orbits[i].m_Height;
                    originalOrbits[i].m_Radius = currentCamera.m_Orbits[i].m_Radius;
                }
            }
            events.Subscribe(_ => UpdateOrbits());
        }
        void UpdateOrbits()
        {
            if (originalOrbits != null && currentCamera)
            {
                zAxis.Update(Time.deltaTime);
                float scale = Mathf.Lerp(minZoom, maxZoom, zAxis.Value);
                for (int i = 0; i < originalOrbits.Length; i++)
                {
                    currentCamera.m_Orbits[i].m_Height = originalOrbits[i].m_Height * scale;
                    currentCamera.m_Orbits[i].m_Radius = originalOrbits[i].m_Radius * scale;
                }
            }
        }
    }
}
*/
