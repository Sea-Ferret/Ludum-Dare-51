using UnityEngine;

namespace Murgn
{
    public class Manager : Singleton<Manager>
    {
        [HideInInspector] public Camera mainCamera;

        public override void Awake()
        {
            base.Awake();
            mainCamera = Camera.main;
        }
    }
}