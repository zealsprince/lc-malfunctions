using UnityEngine;
using UnityEngine.VFX;

namespace Malfunctions.MonoBehaviours
{
    public class Sparks : MonoBehaviour
    {
        // Delay between bursts.
        public float delayMin = 0.5f;
        public float delayMax = 10f;

        // The maximum intensity of the light.
        public float maxIntensity = 40000f;

        private float currentIntensity = 0f;

        // Track timings.
        private float now = 0f;
        private float next = 0f;

        // Track if we sent the stop event to not infinitely ping the VFX.
        private bool sentStop = false;

        private AudioSource audioSource;
        private Light pointLight;
        private VisualEffect visualEffect;

        // The two triggers we will work with.
        static VFXExposedProperty beginBurstTrigger;
        static VFXExposedProperty stopBurstTrigger;

        // Start is called before the first frame update
        void Start()
        {
            beginBurstTrigger.name = "BeginBurst";
            stopBurstTrigger.name = "StopBurst";

            // Assign our audio source, light and visual effect component.
            audioSource = GetComponent<AudioSource>();
            pointLight = GetComponent<Light>();
            visualEffect = GetComponent<VisualEffect>();

            // Get the player and their camera. Set the tag to MainCamera to work with
            // the VFXGraph depth buffer without having to do property binder shenanigans.
            Camera camera;
            GameObject player = GameObject.Find("Player");
            if (player == null)
            {
                camera = GameObject.FindAnyObjectByType<Camera>();
            }
            else
            {
                camera = player.GetComponentInChildren<Camera>();
            }

            // Set the tag that VFXGraph expects.
            camera.gameObject.tag = "MainCamera";
        }

        // Update is called once per frame
        void Update()
        {
            // Check the current time. If it's past next, burst. Reset next.
            now = Time.time;
            if (now > next)
            {
                // Make sure we add a bit of randomness to our effect.
                next = now + Random.Range(delayMin, delayMax);

                if (!Config.MalfunctionVFXDisableSparks.Value)
                {
                    // Reset the stop signal being sent.
                    sentStop = false;

                    // Let's make sure we don't enable the light until now.
                    pointLight.enabled = true;

                    // Flash the light to the maximum set value.
                    currentIntensity = maxIntensity;

                    if (!Config.MalfunctionVFXDisableSparksSound.Value)
                    {
                        // Play the zap sound.
                        audioSource.pitch = Random.Range(0.7f, 2f);
                        audioSource.Play();
                    }

                    // Send the effect event signal.
                    visualEffect.SendEvent(beginBurstTrigger.name);
                }
            }
            else
            {
                if (!sentStop)
                {
                    visualEffect.SendEvent(stopBurstTrigger.name);
                    sentStop = true;
                }
            }

            // Set the light to the current brightness.
            pointLight.intensity = currentIntensity;

            // Fade out the flash.
            currentIntensity = Mathf.Lerp(currentIntensity, 0, Time.deltaTime * 100);
        }
    }
}
