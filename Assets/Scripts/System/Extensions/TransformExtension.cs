using UnityEngine;

namespace STIGRADOR
{
    public static class TransformExtension
    {
        public static void Activate(this Transform transform)
        {
            transform.gameObject.SetActive(true);
        }
        
        public static void Deactivate(this Transform transform)
        {
            transform.gameObject.SetActive(false);
        }

        public static void Destroy(this Transform transform)
        {
            Object.Destroy(transform.gameObject);
        }
    }
}

