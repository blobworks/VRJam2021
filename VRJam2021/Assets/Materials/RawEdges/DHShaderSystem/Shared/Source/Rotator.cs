using UnityEngine;

namespace DHShaderSystem
{
    public class Rotator : MonoBehaviour
    {
        public Vector3 Axis = Vector3.one;
        public float Speed = 1.0f;

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(Axis, Speed * Time.deltaTime);
        }
    }
}