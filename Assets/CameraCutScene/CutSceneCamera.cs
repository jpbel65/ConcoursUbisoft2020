﻿using System.Collections.Generic;
using UnityEngine;

namespace CameraCutScene
{
    public class CutSceneCamera : MonoBehaviour
    {
        public Animation _animation;
        private AnimationClip _animationClip;
        public List<CameraTransform> positions = new List<CameraTransform>();
        public float travelSpeed;
        
        public void Start()
        {
            _animation = gameObject.GetComponent<Animation>();
        }
        public void AddPosition()
        {
            positions.Add(new CameraTransform(gameObject.transform));
        }

        public void Awake()
        {
            _animationClip = new AnimationClip {legacy = true, name = "CamTravel"};
            Keyframe[] keysPositionX = new Keyframe[positions.Count];
            Keyframe[] keysPositionY = new Keyframe[positions.Count];
            Keyframe[] keysPositionZ = new Keyframe[positions.Count];
            Keyframe[] keysRotationX = new Keyframe[positions.Count];
            Keyframe[] keysRotationY = new Keyframe[positions.Count];
            Keyframe[] keysRotationZ = new Keyframe[positions.Count];
            for (int i = 0; i < positions.Count; i++)
            {
                keysPositionX[i] = new Keyframe(travelSpeed * i, positions[i].position.x);
                keysPositionY[i] = new Keyframe(travelSpeed * i, positions[i].position.y);
                keysPositionZ[i] = new Keyframe(travelSpeed * i, positions[i].position.z);
                keysRotationX[i] = new Keyframe(travelSpeed * i, positions[i].rotation.x);
                keysRotationY[i] = new Keyframe(travelSpeed * i, positions[i].rotation.y);
                keysRotationZ[i] = new Keyframe(travelSpeed * i, positions[i].rotation.z);
            }
            AnimationCurve animationCurvePositionsX = new AnimationCurve(keysPositionX);
            AnimationCurve animationCurvePositionsY = new AnimationCurve(keysPositionY);
            AnimationCurve animationCurvePositionsZ = new AnimationCurve(keysPositionZ);
            AnimationCurve animationCurveRotationsX = new AnimationCurve(keysRotationX);
            AnimationCurve animationCurveRotationsY = new AnimationCurve(keysRotationY);
            AnimationCurve animationCurveRotationsZ = new AnimationCurve(keysRotationZ);
            _animationClip.SetCurve("",typeof(Transform),"localPosition.x",animationCurvePositionsX);
            _animationClip.SetCurve("",typeof(Transform),"localPosition.y",animationCurvePositionsY);
            _animationClip.SetCurve("",typeof(Transform),"localPosition.z",animationCurvePositionsZ);
            _animationClip.SetCurve("",typeof(Transform),"localEulerAngles.x",animationCurveRotationsX);
            _animationClip.SetCurve("",typeof(Transform),"localEulerAngles.y",animationCurveRotationsY);
            _animationClip.SetCurve("",typeof(Transform),"localEulerAngles.z",animationCurveRotationsZ);
            _animation.AddClip(_animationClip, _animationClip.name);
            AnimationEvent animationEvent = new AnimationEvent
            {
                time = (positions.Count-1) * travelSpeed, functionName = "CloseCamera"
            };
            _animationClip.AddEvent(animationEvent);
            _animation.Play(_animationClip.name);
        }

        public void CloseCamera()
        {
            gameObject.SetActive(false);
        }
    }
}
