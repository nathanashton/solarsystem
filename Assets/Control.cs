using UnityEngine;

namespace Assets
{
    public class Control : MonoBehaviour
    {
        public double Speed = 0.3;
        public double PlanetVisualScale = 500;
        public double OrbitScale = 50000;
        public double MoonOrbitMultipler = 40;
        public double SunSize = 500;
        public int Test { get; set; }
        public double SpeedTest { get; set; }

        public void ChangeSpeed(float value)
        {
            Speed = value;
        }

        public void ChangeOrbit(float value)
        {
            OrbitScale = value;
        }

        public void ChangePlanetScale(float value)
        {
            PlanetVisualScale = value;
        }
    }
}
