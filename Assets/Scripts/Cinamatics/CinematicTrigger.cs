using UnityEngine.Playables;
using UnityEngine;
using RPG.Saving;
namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
    {
        private bool isAlreadyPlayed = false;

        public object CaptureState()
        {
            return isAlreadyPlayed;
        }

        public void RestoreState(object state)
        {
            isAlreadyPlayed = (bool)state;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && !isAlreadyPlayed)
            {
                GetComponent<PlayableDirector>().Play();
                isAlreadyPlayed = true;
            }
        }
    }
}
