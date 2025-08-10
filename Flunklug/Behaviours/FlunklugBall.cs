using Flunklug.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Flunklug.Behaviours
{
    public class FlunklugBall : DevHoldable
    {
        public float throwStrength = 1f;

        public Rigidbody rb;
        public PhysicMaterial material;
        public GorillaVelocityEstimator velocityEstimator;

        public GameObject defaultLug;
        public int style;
        public string flunkName;
        public Color color;
        private Text nameTag;
        private AudioSource teleportSound;

        public readonly List<string> names =
        [
            "Toby Fox",
            "Slinker",
            "Glumpob",
            "Ladder",
            "Flunker",
            "Spunchbob",
            "Yourburt",
            "Klungo",
            "Goober",
            "Lugflunk",
            "Yodeler",
            "Joe",
            "Sponge",
            "John",
            "Luina",
            "Yogurt",
            "Chickne butt",
            "PhotonNetwork.FindFriends",
            "Color Transparent Ruler Plastic Rulers - Ruler 12 inch, Kids Ruler for School, Ruler with Centimeters, Millimeter and Inches, Assorted Colors, Clear Rulers, 7 Pack School Rulers",
            "Bob",
            "John Roblox",
            "Lamppost",
            "24€ and 50¢"
        ];

        public void Awake()
        {
            nameTag = transform.Find("NameTag").GetComponent<Text>();
            teleportSound = GetComponent<AudioSource>();
            defaultLug = transform.Find("Default").gameObject;

            if (!gameObject.TryGetComponent(out velocityEstimator))
                velocityEstimator = gameObject.AddComponent<GorillaVelocityEstimator>();

            rb = GetComponent<Rigidbody>();
            material = new PhysicMaterial();
            GetComponent<Collider>().material = material;

            color.r = (float)Math.Round(Random.value, 1);
            color.g = (float)Math.Round(Random.value, 1);
            color.b = (float)Math.Round(Random.value, 1);

            SetColor(color);
            ChangePhysics(1f, 1f);
            flunkName = names[Random.Range(0, names.Count)];
            nameTag.text = flunkName.ToUpper();
        }

        public void ChangePhysics(float bounce, float friction)
        {
            material.bounciness = bounce;
            material.staticFriction = friction;
            material.dynamicFriction = friction;
        }

        public void Teleport(Vector3 position, bool doSound)
        {
            rb.velocity = Vector3.zero;
            transform.position = position;
            if (doSound) teleportSound.Play();
        }

        public void PlayBirthSound()
        {
            AudioClip audioClip = AssetLoader.LoadClip("glitter");
            AudioSource.PlayClipAtPoint(audioClip, transform.position);
        }

        public void SetColor(Color color)
        {
            defaultLug.GetComponent<MeshRenderer>().material.color = color;
        }

        public override void OnDrop(bool isLeft)
        {
            base.OnDrop(isLeft);
            rb.isKinematic = false;
            rb.velocity = velocityEstimator.linearVelocity * throwStrength;
            rb.angularVelocity = velocityEstimator.angularVelocity;
        }

        public override void OnGrab(bool isLeft)
        {
            base.OnGrab(isLeft);
            rb.isKinematic = true;
        }

        public void SetStyle(int style)
        {
            this.style = style;
            for (int i = 0; i < FlunkController.stylesAndNames.Count; i++)
            {
                transform.Find(FlunkController.stylesAndNames.Values.ElementAt(i)).gameObject.SetActive(false);
            }
            transform.Find(FlunkController.stylesAndNames[style]).gameObject.SetActive(true);

            if (transform.Find($"NameTag_{FlunkController.stylesAndNames[style]}") is Transform tagPlacement && tagPlacement)
            {
                nameTag.transform.localPosition = tagPlacement.localPosition;
                nameTag.transform.localEulerAngles = tagPlacement.localEulerAngles;
            }
        }
    }
}
