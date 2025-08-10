using System;
using UnityEngine;
using UnityEngine.XR;
using Player = GorillaLocomotion.GTPlayer;

namespace Flunklug.Behaviours
{
    public class DevHoldable : HoldableObject
    {
        public bool
            InHand = false,
            InLeftHand = false,
            PickUp = true,
            DidSwap = false,
            SwappedLeft = true;

        public float
            GrabDistance = 0.25f,
            ThrowForce = 1.75f;

        public virtual void OnGrab(bool isLeft)
        {

        }

        public virtual void OnDrop(bool isLeft)
        {

        }

        public void FixedUpdate()
        {
            var Distance = GrabDistance * Player.Instance.scale;

            if (DidSwap && (!SwappedLeft ? ControllerInputPoller.GetGrabRelease(XRNode.LeftHand) : ControllerInputPoller.GetGrabRelease(XRNode.RightHand)))
                DidSwap = false;

            bool pickLeft = PickUp && ControllerInputPoller.GetGrab(XRNode.LeftHand) && Vector3.Distance(Player.Instance.leftControllerTransform.position, transform.position) < Distance && !InHand && EquipmentInteractor.instance.leftHandHeldEquipment == null && !DidSwap;
            bool swapLeft = InHand && ControllerInputPoller.GetGrab(XRNode.LeftHand) && ControllerInputPoller.GetGrab(XRNode.RightHand) && !DidSwap && (Vector3.Distance(Player.Instance.leftControllerTransform.position, transform.position) < Distance) && !SwappedLeft && EquipmentInteractor.instance.leftHandHeldEquipment == null;
            if (pickLeft || swapLeft)
            {
                Console.WriteLine("grab left");
                DidSwap = swapLeft;
                SwappedLeft = true;
                InLeftHand = true;
                InHand = true;

                transform.SetParent(GorillaTagger.Instance.offlineVRRig.leftHandTransform.parent);
                GorillaTagger.Instance.StartVibration(true, 0.1f, 0.05f);
                EquipmentInteractor.instance.leftHandHeldEquipment = this;
                if (DidSwap) EquipmentInteractor.instance.rightHandHeldEquipment = null;

                OnGrab(true);
            }
            else if (ControllerInputPoller.GetGrabRelease(XRNode.LeftHand) && InHand && InLeftHand)
            {
                Console.WriteLine("release left");
                InLeftHand = true;
                InHand = false;
                transform.SetParent(null);

                EquipmentInteractor.instance.leftHandHeldEquipment = null;
                OnDrop(true);
            }

            bool pickRight = PickUp && ControllerInputPoller.GetGrab(XRNode.RightHand) && Vector3.Distance(Player.Instance.rightControllerTransform.position, transform.position) < Distance && !InHand && EquipmentInteractor.instance.rightHandHeldEquipment == null && !DidSwap;
            bool swapRight = InHand && ControllerInputPoller.GetGrab(XRNode.RightHand) && ControllerInputPoller.GetGrab(XRNode.LeftHand) && !DidSwap && (Vector3.Distance(Player.Instance.rightControllerTransform.position, transform.position) < Distance) && SwappedLeft && EquipmentInteractor.instance.rightHandHeldEquipment == null;
            if (pickRight || swapRight)
            {
                Console.WriteLine("grab right");
                DidSwap = swapRight;
                SwappedLeft = false;

                InLeftHand = false;
                InHand = true;
                transform.SetParent(GorillaTagger.Instance.offlineVRRig.rightHandTransform.parent);

                GorillaTagger.Instance.StartVibration(false, 0.1f, 0.05f);
                EquipmentInteractor.instance.rightHandHeldEquipment = this;
                if (DidSwap) EquipmentInteractor.instance.leftHandHeldEquipment = null;

                OnGrab(false);
            }
            else if (ControllerInputPoller.GetGrabRelease(XRNode.RightHand) && InHand && !InLeftHand)
            {
                Console.WriteLine("release right");
                InLeftHand = false;
                InHand = false;
                transform.SetParent(null);

                EquipmentInteractor.instance.rightHandHeldEquipment = null;
                OnDrop(false);
            }
        }

        public override void DropItemCleanup()
        {

        }

        public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
        {

        }

        public override void OnHover(InteractionPoint pointHovered, GameObject hoveringHand)
        {

        }
    }
}
