
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Assets
{
    public class SunLookAt : MonoBehaviour
    {
        public GameObject target;

        private void Update()
        {
            // If you can view planet then light is on and following planet else it is turned off
            if (target.gameObject.GetComponentInChildren<Renderer>().isVisible)
            {
                transform.LookAt(target.gameObject.transform);
            }
        }
    }
}
