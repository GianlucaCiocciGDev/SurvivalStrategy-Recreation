using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace GDev
{
    public static class Helper
    {
        private static Camera _camera;
        public static Camera camera
        {
            get
            {
                if (_camera == null) _camera = Camera.main;
                return _camera;
            }
        }


        public static async Task WaitUntil(Func<bool> predicate, int sleep = 50)
        {
            while (!predicate())
            {
                await Task.Delay(sleep);
            }
        }
        //private static readonly Dictionary<float , WaitForSeconds> waitDic= new Dictionary<float , WaitForSeconds>();
        //public static WaitForSeconds GetWait(int seconds)
        //{
        //    if(waitDic.TryGetValue(seconds,out var wait)) return wait;

        //    waitDic[seconds]=new WaitForSeconds(seconds);
        //    return waitDic[seconds];
        //}
        public static async Task WaitForSecond(int millisecond)
        {
            await Task.Delay(millisecond);
        }


        public static List<T> GetClone<T>(this List<T> source)
        {
            return source.GetRange(0, source.Count);
        }


        public static Vector3 MousePosition()
        {
            Vector3 position = Vector3.zero;
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out RaycastHit info))
                position = info.point;
            return position;
        }
        public static RaycastHit CameraRay()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out RaycastHit info))
                return info;
            return new RaycastHit();
        }


        private static PointerEventData _eventDataCurrentPosition;
        private static List<RaycastResult> _raycastResults;
        public static bool IsHoverUI()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            _eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = mousePosition
            };

            _raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_eventDataCurrentPosition,_raycastResults);
            return _raycastResults.Count > 0;
        }
        public static Vector2 GetWorldPositionOfCanvasElemnt(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, _camera, out Vector3 result);
            return result;
        }


        public static void DeleteAllChildren(Transform t)
        {
            foreach (Transform child in t) UnityEngine.Object.Destroy(child.gameObject);
        }


        private static Dictionary<ResourceType, string> resToname = new Dictionary<ResourceType, string>() 
        {
            {ResourceType.Wood,"Legno" },
            {ResourceType.Iron,"Ferro" },
            {ResourceType.Gems,"Gemme" }
        };
        public static string GetDisplayResourceName(ResourceType type)
        {
            return resToname[type];
        }
    }
}
