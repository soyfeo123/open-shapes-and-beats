using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace OSB.Editor
{
    // i swear if this code continues being a jerk
    // i will shoot my screen and throw it out the window
    // i made it work yo
    public class EditorPlayheadDragArea : Button, IPointerMoveHandler
    {
        RectTransform rt;

        public Vector2 pos;

        public bool isInArea;
        bool isMouseDown;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            rt = transform.GetComponent<RectTransform>();
        }

        void Update()
        {
            
        }

        public void JustWorkGoshDarnIt()
        {
            if (Input.GetMouseButton(0))
            {
                MovePlayhead();
            }
        }

        void MovePlayhead()
        {


            //Vector2 localPoint;
            RectTransform contentRT = rt.parent.GetComponent<RectTransform>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, null, out pos);


            //pos.x = Mathf.Clamp(pos.x, 0, 9999f);
            //Debug.Log(pos.x);
            EditorPlayhead.Singleton.SetRelativePos(pos.x);


        }

        


        public void OnPointerMove(PointerEventData data)
        {
            
        }
    }
}