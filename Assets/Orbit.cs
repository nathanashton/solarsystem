using System;
using System.Globalization;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    public class Orbit : MonoBehaviour
    {
        public bool CameraFollowEarth;
        public GameObject ParentToOrbit; // Optional body that is being orbited
        public bool IsOrbiting = true;
        public float iSunXOffset = 0;
        public float iSunYOffset = 0;
        public string PlanetName;
        public double Eccentricity;
        public double PlanetAU; // Distance from parent in AU, 1AU is the distance from Sun to Earth
        public double PlanetYear; // Orbit duration in earth years
        public double PlanetLongPeri;
        public double PlanetRadius; // Radius in km


        double dTime = 0.0,
            dTimeTick = 0.03,
            dTimeTickDefault = 0.1;

        private Control _control;

      //  private double _planetScaleFactor = 1000; // divide km by this to get unity units; Makes the moon about 0.1 unity units when 1737
        //private double _orbitScaleFactor = 50000; // 50000 is a nice value

        private int _au = 150000000;

        // Start is called before the first frame update
        void Start()
        {
            var t = GameObject.FindGameObjectWithTag("GameController");
            _control = t.gameObject.GetComponent<Control>();
            var scale = PlanetRadius / _control.PlanetVisualScale;
            gameObject.transform.localScale = new Vector3((float)scale, (float)scale , (float)scale);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            dTimeTick = _control.Speed;
            var scale = PlanetRadius / _control.PlanetVisualScale;
            var newScale = new Vector3((float)scale, (float)scale, (float)scale);

            if (gameObject.transform.localScale != newScale)
            {
                gameObject.transform.localScale = newScale;
            }



            if (IsOrbiting)
            {
                var dScalar = (PlanetAU * _au) / _control.OrbitScale; // scale just orbits

                if (PlanetName == "Moon")
                {
                    dScalar = dScalar * _control.MoonOrbitMultipler;
                }
              //  var dScalar = (PlanetAU * _au) / _planetScaleFactor; // scale to planets



                drawPlanet(Eccentricity, dScalar, PlanetYear, PlanetLongPeri);
                dTime += dTimeTick; // 0.02 seconds
            }
        }

        public double getE(double dTime, double dEccentricity)
        {
            double dM1, dD, dE0, dE = 0; // return value E = the mean anomaly  
            double dM; // local value of M in radians  

            dM = dTime.ToRadians();

            int iSign = 1;

            if (dM > 0) iSign = 1; else iSign = -1;

            dM = Math.Abs(dM) / (2 * Math.PI); // Meeus, p 206, line 110  
            dM = (dM - (long)dM) * (2 * Math.PI) * iSign; // line 120  
            if (dM < 0)
            {
                dM = dM + (2 * Math.PI);
            }
            iSign = 1;
            if (dM > Math.PI)
            {
                iSign = -1;
            }
            if (dM > Math.PI)
            {
                dM = 2 * Math.PI - dM;
            }

            dE0 = Math.PI / 2;
            dD = Math.PI / 4;

            for (int i = 0; i < 33; i++)
            {
                dM1 = dE0 - dEccentricity * Math.Sin(dE0);
                var tt = Math.Sign((float)(dM - dM1));
                dE0 = dE0 + dD * tt;
                dD = dD / 2;
            }

            dE = dE0 * iSign;
            return dE;
        }

        void OnDrawGizmos()
        {
           // Handles.Label(transform.position, PlanetName);
        }

        public double getRfromE(double dE, double dEccentricty, double dScalar)
        {
            return dScalar * (1 - (dEccentricty * Math.Cos(dE)));
        }

        public void drawPlanet(double dEccentricity, double dScalar, double dYear, double dLongPeri)
        {
            double dE, dr, dv, dSatX, dSatY, dSatXCorrected, dSatYCorrected;
            float fX, fY;

            if (ParentToOrbit != null)
            {
                iSunXOffset = ParentToOrbit.gameObject.transform.position.x;
                iSunYOffset = ParentToOrbit.gameObject.transform.position.z;
            } 

            dE = getE(dTime * (1 / dYear), dEccentricity);
            dr = getRfromE(dE, dEccentricity, dScalar);
            dv = 2 * Math.Atan(
                    Math.Sqrt((1 + dEccentricity) / (1 - dEccentricity))
                    *
                    Math.Tan(dE / 2)
                    );
            dSatX = dr / Math.Sin(Math.PI / 2) * Math.Sin(dv);
            dSatY = Math.Sin((Math.PI / 2) - dv) * (dSatX / Math.Sin(dv));

            dSatXCorrected = dSatX * (float)Math.Cos(dLongPeri.ToRadians()) -
                dSatY * (float)Math.Sin(dLongPeri.ToRadians());
            dSatYCorrected = dSatX * (float)Math.Sin(dLongPeri.ToRadians()) +
                dSatY * (float)Math.Cos(dLongPeri.ToRadians());

            fX = (float)dSatXCorrected + (float)iSunXOffset;
            fY = (float)dSatYCorrected + (float)iSunYOffset;

            if (PlanetName == "Earth" && CameraFollowEarth)
            {
                Camera.main.transform.position = new Vector3(fX, 2000, fY);
            }

            transform.position = new Vector3(fX, 0, fY);
        }
    }
}
