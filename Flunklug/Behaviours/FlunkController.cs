using Flunklug.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flunklug.Behaviours
{
    internal class FlunkController : MonoBehaviour
    {
        public static FlunkController Instance;

        public static List<FlunklugBall> lugs = [];

        private GameObject templateLug;

        private GameObject kill;

        public static Dictionary<int, string> stylesAndNames = new()
        {
            { 0, "Default" },
            { 1, "Phoenix" },
            { 2, "Staircase" }
        };

        public void Awake()
        {
            Instance = this;

            templateLug = AssetLoader.LoadTemplateLug();
            kill = new GameObject();
            AudioSource audioSource = kill.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = AssetLoader.LoadClip("kill");
            audioSource.spatialBlend = 1f;
            
            if (Plugin.SpawnOnLoad.Value) GenerateFlunklug(0).Teleport(new Vector3(-65.7576f, 12.1956f, -83.4815f), false);
        }

        public FlunklugBall GenerateFlunklug(int style)
        {
            FlunklugBall component = Instantiate(templateLug).GetComponent<FlunklugBall>();
            component.gameObject.layer = 8;
            component.SetStyle(style);
            lugs.Add(component);
            return component;
        }

        public void DestroyBall(FlunklugBall ball, bool removeFromList)
        {
            StartCoroutine(BallDeathSound(ball.transform.position));
            Destroy(ball.gameObject);
            if (removeFromList) lugs.Remove(ball);
        }

        private IEnumerator BallDeathSound(Vector3 pos)
        {
            GameObject tempKill = Instantiate(kill);
            tempKill.transform.position = pos;
            AudioSource souyrc = tempKill.GetComponent<AudioSource>();
            souyrc.Play();
            yield return new WaitForSeconds(souyrc.clip.length);
            Destroy(tempKill);
            yield break;
        }
    }
}
