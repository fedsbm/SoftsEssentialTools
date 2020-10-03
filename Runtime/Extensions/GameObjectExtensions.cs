using System;
using System.Linq;
using UnityEngine;

namespace Packages.SoftsEssentialKit.Runtime.Extensions
{
    public static class GameObjectExtensions
    {
        public static Vector3 _GetScreenToLocalPosition(this GameObject gameObject, Camera camera, Vector2 screenPos)
        {
            return _GetScreenToWorldPosition(gameObject, camera, screenPos) - gameObject.transform.position;
        }

        public static Vector3 _GetScreenToWorldPosition(this GameObject gameObject, Camera camera, Vector2 screenPos)
        {
            float zDist = gameObject.transform.position.z - camera.transform.position.z;

            return camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, zDist));
        }

        public static void _TurnToZ(this GameObject gameObject, Vector2 pos)
        {
            Transform transform = gameObject.transform;
            float angle = pos._AngleFromUp();
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, angle);
        }

        public static void _TurnToZ(this GameObject gameObject, float angle)
        {
            Transform transform = gameObject.transform;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, angle);
        }

        public static void _TurnToLocalZ(this GameObject go, float angle)
        {
            go.transform._TurnToLocalZ(angle);
        }

        public static void _TurnToLocalZ(this Transform trans, float angle)
        {
            Vector3 pos = trans.localEulerAngles;
            pos.z = angle;
            trans.localEulerAngles = pos;
        }

        public static void _SetPositionByAngle(this GameObject gameObject, float angleDegrees, float distance)
        {
            float radians = Mathf.Deg2Rad * angleDegrees;
            gameObject.transform.localPosition = new Vector3(Mathf.Sin(radians) * distance,
                Mathf.Cos(radians) * distance,
                gameObject.transform.localPosition.z);
        }

        public static string _GetPath(this GameObject gameObject)
        {
            return string.Join("/",
                gameObject.GetComponentsInParent<Transform>().Reverse().Select(t => t.name).ToArray());
        }

//        // Do not use gameObject.SetActive(bool) because OnDisable() will stop the listeners
//        public static void _SetVisibility(this GameObject gameObject, bool visible)
//        {
//            /*
//         * For 3D Objects
//         */
//            SetEnableAllRendererOfType<Renderer>(gameObject, visible);
//
//
//            gameObject._SetColliderEnabled(visible);
//
//            /*
//         * For UI Sprites Object
//         */
//            SetEnableAllBehaviourOfType<Image>(gameObject, visible);
//
//            /*
//         * For UI Text Object
//         */
//            SetEnableAllBehaviourOfType<TextMeshProUGUI>(gameObject, visible);
//        }

        private static void SetEnableAllBehaviourOfType<T>(GameObject gameObject, bool isEnabled) where T : Behaviour
        {
            T image = gameObject.GetComponent<T>();
            if (image != null)
            {
                image.enabled = isEnabled;
            }

            T[] images = gameObject.GetComponentsInChildren<T>();
            if (images != null)
            {
                foreach (var img in images)
                {
                    img.enabled = isEnabled;
                }
            }
        }

        private static void SetEnableAllRendererOfType<T>(GameObject gameObject, bool isEnabled) where T : Renderer
        {
            T image = gameObject.GetComponent<T>();
            if (image != null)
            {
                image.enabled = isEnabled;
            }

            T[] images = gameObject.GetComponentsInChildren<T>();
            if (images != null)
            {
                foreach (var img in images)
                {
                    img.enabled = isEnabled;
                }
            }
        }

        public static void _SetColliderEnabled(this GameObject gameObject, bool colliderOn)
        {
            Collider collider = gameObject.GetComponent<Collider>();

            if (collider != null)
            {
                collider.enabled = colliderOn;
            }

            Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
            if (colliders != null)
            {
                foreach (var c in colliders)
                {
                    c.enabled = colliderOn;
                }
            }
        }

        public static void _SetFade(this GameObject gameObject, byte alphaValue)
        {
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                if (renderer.material.HasProperty("_Color"))
                {
                    Color32 col = renderer.material.GetColor("_Color");
                    col.a = alphaValue;
                    renderer.material.SetColor("_Color", col);
                }
            }

            MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in renderers)
            {
                if (meshRenderer.material.HasProperty("_Color"))
                {
                    Color32 col = meshRenderer.material.GetColor("_Color");
                    col.a = alphaValue;
                    meshRenderer.material.SetColor("_Color", col);
                }
            }
        }

        /*
     * Float Interpolation 
     */
        private static GameObject interpolatingGameObject;

        private static Action<GameObject, float, bool> currentFloatCallback;

//	public static void _Interpolate(this GameObject gameObject, MonoBehaviour monoBehaviour, float duration,
//		Action<GameObject, float, bool> callback, float from, float to, Interpolator interpolator)
//	{
//		if(interpolatingGameObject == null)
//		{
//			interpolatingGameObject = gameObject;
//			currentFloatCallback = callback;
//			monoBehaviour._Interpolate(duration, InterpolateFloat, from, to, interpolator);
//		}
//		else
//		{
//			Debug.Log("Can't start new Interpolation for " + gameObject + " before current one finish");
//		}
//	}
//
//	private static void InterpolateFloat(float progress, bool finished)
//	{
//		currentFloatCallback(interpolatingGameObject, progress, finished);
//		if(finished)
//		{
//			interpolatingGameObject = null;
//			currentFloatCallback = null;
//		}
//	}

        /*
     * Vector2 Interpolation 
     */

//	private static Action<GameObject, Vector2, bool> currentVector2Callback;
//
//	public static void _Interpolate(this GameObject gameObject, MonoBehaviour monoBehaviour, float duration,
//		Action<GameObject, Vector2, bool> callback, Vector2 from, Vector2 to, Interpolator interpolator)
//	{
//		if(interpolatingGameObject == null)
//		{
//			interpolatingGameObject = gameObject;
//			currentVector2Callback = callback;
//			monoBehaviour._Interpolate(duration, InterpolateVector2, from, to, interpolator);
//		}
//		else
//		{
//			Debug.Log("Can't start new Interpolation for " + gameObject + " before current one finish");
//		}
//	}
//
//	private static void InterpolateVector2(Vector2 progress, bool finished)
//	{
//		currentVector2Callback(interpolatingGameObject, progress, finished);
//		if(finished)
//		{
//			interpolatingGameObject = null;
//			currentVector2Callback = null;
//		}
//	}

        /*
//     * Vector3 Interpolation 
//     */
//
//	private static Action<GameObject, Vector3, bool> currentVector3Callback;
//
//	public static void _Interpolate(this GameObject gameObject, MonoBehaviour monoBehaviour, float duration,
//		Action<GameObject, Vector3, bool> callback, Vector3 from, Vector3 to, Interpolator interpolator)
//	{
//		if(interpolatingGameObject == null)
//		{
//			interpolatingGameObject = gameObject;
//			currentVector3Callback = callback;
//			monoBehaviour._Interpolate(duration, InterpolateVector3, from, to, interpolator);
//		}
//		else
//		{
//			Debug.Log("Can't start new Interpolation for " + gameObject + " before current one finish");
//		}
//	}

//	private static void InterpolateVector3(Vector3 progress, bool finished)
//	{
//		currentVector3Callback(interpolatingGameObject, progress, finished);
//		if(finished)
//		{
//			interpolatingGameObject = null;
//			currentVector3Callback = null;
//		}
//	}

        public static T GetComponentAtPath<T>(this GameObject gameObject, string path) where T : Component
        {
            Transform targetTrans = gameObject.transform.Find(path);

            if (targetTrans == null)
            {
                return null;
            }

            return targetTrans.gameObject.GetComponent<T>();
        }

        public static void _RemoveAllChildren(this GameObject go)
        {
            for (int i = go.transform.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(go.transform.GetChild(i).gameObject);
            }
        }
    }
}