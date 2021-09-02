using UnityEngine.AI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Control;
using RPG.Core;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D
        }
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint = null;
        [SerializeField] float fadeOutTime = 1;
        [SerializeField] float fadeInTime = 1;
        [SerializeField] float fadeWaitTime = 1;
        [SerializeField] DestinationIdentifier destination = DestinationIdentifier.A;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }


        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }
            DontDestroyOnLoad(gameObject);
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();

            //remove control
            Disablecontrol();
            yield return fader.FadeOut(fadeOutTime);

            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            //remove control
            Disablecontrol();

            wrapper.Load();

            Portal otherPortal = GetOtherPortal(destination);
            UpdatePlayer(otherPortal);

            wrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);

            EnableControl();
            // restore control
            Destroy(gameObject);
        }

        void Disablecontrol()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<ActionScheduler>().CancelCurrentAction();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = false;
        }


        void EnableControl()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = true;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            (player.transform.position, player.transform.rotation) = (otherPortal.spawnPoint.position, otherPortal.spawnPoint.rotation);
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal(DestinationIdentifier destination)
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;
                return portal;
            }
            return null;
        }
    }
}
