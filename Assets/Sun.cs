using UnityEngine;

namespace Assets
{
    public class Sun : MonoBehaviour
    {
        public Control _control;
        public int SizeMultipler = 1;

        public void Start()
        {
            var t = GameObject.FindGameObjectWithTag("GameController");
            _control = t.gameObject.GetComponent<Control>();
            var scale = (696340 / _control.PlanetVisualScale) / SizeMultipler;
            gameObject.transform.localScale = new Vector3((float)scale, (float)scale, (float)scale);
        }

        public void FixedUpdate()
        {
            var scale = (696340 / _control.PlanetVisualScale) / SizeMultipler;
            

            var newScale = new Vector3((float)scale, (float)scale, (float)scale);

            if (gameObject.transform.localScale != newScale)
            {
                gameObject.transform.localScale = newScale;
            }
        }
    }
}
