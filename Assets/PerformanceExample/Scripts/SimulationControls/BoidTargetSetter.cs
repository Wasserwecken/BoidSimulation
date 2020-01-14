using System;
using System.Collections;
using System.Collections.Generic;
using PerformanceExample;
using UnityEngine;


namespace PerformanceExample
{
    public class BoidTargetSetter : MonoBehaviour
    {
        public BoidBehaviour[] Settings;


        /// <summary>
        /// 
        /// </summary>
        void Update()
        {
            foreach (var setting in Settings)
                setting.Target = transform.position;
        }
    }
}