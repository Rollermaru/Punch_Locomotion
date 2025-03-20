/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using Meta.XR.Util;
using UnityEngine;

namespace Oculus.Interaction.OVR.Input
{
    // [Feature(Feature.Interaction)]
    public class MyActiveState : MonoBehaviour, IActiveState
    {
        [SerializeField]
        private OVRInput.Button _button;
        public OVRInput.Controller controller;
        public GameObject inGameController;


        // public bool Active => OVRInput.Get(_button);
        public bool Swing => checkVelocity();
        public bool Active => OVRInput.Get(_button);

        private bool checkVelocity() {
            if (!OVRInput.Get(_button)) {
                return false;
            }

            Debug.DrawLine(OVRInput.GetLocalControllerPosition(controller), OVRInput.GetLocalControllerRotation(controller) * (OVRInput.GetLocalControllerPosition(controller) * 10), Color.red);

            Vector3 velocity = OVRInput.GetLocalControllerVelocity(controller);
            float magnitude = velocity.magnitude;
            // Debug.Log("Passed trigger test. Magnitude = " + magnitude);

            if (magnitude < 1.0) {
                return false;
            }

            // Debug.Log(Vector3.Dot(inGameController.transform.forward, velocity));
            // Debug.Log(inGameController.transform.forward);

            // Quaternion times vector
            

            // Debug.Log("Passed magnitude test");

            if (Vector3.Dot(inGameController.transform.forward, velocity) < 0.0) return false;
            // forward vector is incorrect

            return true;

        }
    }

    
}
