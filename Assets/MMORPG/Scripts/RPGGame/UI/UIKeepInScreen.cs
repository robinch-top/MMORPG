using UnityEngine;
using System.Collections;

namespace Assets.MMORPG.Scripts.RPGGame.UI
{
    public class UIKeepInScreen : MonoBehaviour
    {
        [Header("Components")]
        public RectTransform rectTransform;


        // Update is called once per frame
        void Update()
        {
            Rect rect = rectTransform.rect;

            Vector2 minWorld = transform.TransformPoint(rect.min);
            Vector2 maxWorld = transform.TransformPoint(rect.min);
            Vector2 sizeWorld = maxWorld - minWorld;
            maxWorld = new Vector2(Screen.width, Screen.height) - sizeWorld;
            float x = Mathf.Clamp(minWorld.x, 0, maxWorld.x);
            float y = Mathf.Clamp(minWorld.y, 0, maxWorld.y);
            Vector2 offset = (Vector2)transform.position - minWorld;
            transform.position = new Vector2(x, y) + offset;
        }
    }
}