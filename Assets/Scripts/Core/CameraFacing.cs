using TMPro;
using UnityEngine;
namespace RPG.Core
{

    public class CameraFacing : MonoBehaviour
    {
        new Camera camera;
        private void Awake()
        {
            camera = Camera.main;
        }

        void LateUpdate()
        {
            transform.forward = camera.transform.forward;
        }
    }

}
