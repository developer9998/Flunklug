using Flunklug.Behaviours;
using GorillaLocomotion.Swimming;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Flunklug.Tools
{
    internal static class AssetLoader
    {
        public static AssetBundle bundle;

        public static GameObject LoadTemplateLug()
        {
            if (bundle is null)
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Flunklug.Resources.flunklug");
                bundle = AssetBundle.LoadFromStream(stream);
                stream.Close();
            }

            GameObject flunklug = bundle.LoadAsset<GameObject>("Flunklug");
            flunklug.AddComponent<FlunklugBall>();
            flunklug.name = "MonoObject 'Flunklug'";

            RigidbodyWaterInteraction flunklugWaterInteraction = flunklug.AddComponent<RigidbodyWaterInteraction>();
            flunklugWaterInteraction.enablePreciseWaterCollision = true;
            flunklugWaterInteraction.underWaterBuoyancyFactor = 1f;
            flunklugWaterInteraction.underWaterDampingHalfLife = 0.5f;
            flunklugWaterInteraction.waterSurfaceDampingHalfLife = 0.5f;

            return flunklug;
        }

        public static AudioClip LoadClip(string name) => bundle.LoadAsset<AudioClip>(name);
    }
}
