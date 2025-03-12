using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSB.Editor
{
    public class EditorPlayhead : MBSingletonDestroy<EditorPlayhead>
    {
        public float relativeXPos = 0;

        [Header("Clamps")]
        public float minX = -8596.9f;
        public float maxX = 9999f;


        // 1 unity unit = 10ms

        /// <summary>
        /// Gets the playhead's position in the song in milliseconds.
        /// </summary>
        public float SongPosMS
        {
            get
            {
                return relativeXPos * 10f;
            }
            set
            {
                SetRelativePos(value / 10f);
            }
        }

        /// <summary>
        /// Gets the playhead's position in the song in seconds.
        /// </summary>
        public float SongPosS
        {
            get
            {
                return SongPosMS * 0.001f;
            }
            set
            {
                SongPosMS = value * 1000f;
            }
        }

        RectTransform    rt;

        private void Start()
        {
            rt = GetComponent<RectTransform>();
        }

        private void Update()
        {
            //Debug.Log(rt.anchoredPosition.x);
            relativeXPos = rt.anchoredPosition.x - minX;

        }

        public void SetRelativePos(float xPos)
        {
            xPos = Mathf.Clamp(xPos, 0, 99999f);
            Vector2 pos = rt.anchoredPosition;
            pos.x = xPos + minX;
            rt.anchoredPosition = pos;
        }
    }
}