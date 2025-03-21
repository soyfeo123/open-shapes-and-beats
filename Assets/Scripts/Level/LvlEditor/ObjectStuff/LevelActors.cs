using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Data;
using System;

namespace OSB.Editor
{
    /// <summary>
    /// PATENT PENDING SUPER USEFUL CLASS BECAUSE IW ANT TO MAKE IT BECAUSE YES I LIKE THE Put
    /// </summary>
    public static class PaloUtils
    {
        static DataTable _internalTable;

        [RuntimeInitializeOnLoadMethod]
        static void initUtils()
        {
            _internalTable = new DataTable();
        }

        public struct ExpressionVariables
        {
            public string textToReplace;
            public float value;

            public ExpressionVariables(string text, float val)
            {
                textToReplace = text;
                value = val;
            }
        }

        public static float ConvertExpression(string parse, params ExpressionVariables[] vars)
        {
            if (string.IsNullOrEmpty(parse))
            {
                return 0;
            }

            string input = parse;
            foreach(ExpressionVariables var in vars)
            {
                input = input.Replace($"[{var.textToReplace}]", var.value.ToString());
            }

            float val = (float)Convert.ToDouble(_internalTable.Compute(input, ""));
            return val;
        }
    }

    public struct OSBPoint
    {
        public float x;
        public float y;

        public OSBPoint(float x_, float y_)
        {
            x = x_;
            y = y_;
            
        }
    }

    public class OSBTweening
    {

    }

    public class RenderComponent
    {
        public SpriteRenderer renderer;
        public LevelActor parent;
        public GameObject visibilityRenderer;

        public static Color pink = new Color(1, 0, 0.5058824f);

        public static void AddToLA(LevelActor actor, Sprite sprite)
        {
            actor.rc = new RenderComponent();
            actor.rc.parent = actor;

            actor.rc.renderer = actor.visual.AddComponent<SpriteRenderer>();

            //Debug.Log(sprite);
            actor.rc.renderer.sprite = sprite;

            
        }

        /// <summary>
        /// only useful if you have the sprite as white
        /// </summary>
        public void SetColorToPink()
        {
            renderer.color = pink;
        }
    }

    public class LogicHitbox
    {
        public PolygonCollider2D collider;

        //public LevelActor parent;

        public static void AddToLA(LevelActor actor)
        {
            actor.logicHitbox = new LogicHitbox();

            Sprite hitboxSprite = actor.rc.renderer.sprite;

            actor.hitbox = new GameObject("Hitbox");
            actor.hitbox.transform.parent = actor.mainObject.transform;
            

            actor.logicHitbox.collider = actor.hitbox.AddComponent<PolygonCollider2D>();

            actor.logicHitbox.collider.isTrigger = true;
            actor.hitbox.transform.localScale = actor.rc.renderer.transform.localScale;

            int shapeCount = actor.rc.renderer.sprite.GetPhysicsShapeCount();

            for(int i = 0; i < shapeCount; i++)
            {
                List<Vector2> points = new List<Vector2>();
                actor.rc.renderer.sprite.GetPhysicsShape(i, points);
                actor.logicHitbox.collider.SetPath(i, points.ToArray());
            }

            actor.hitbox.transform.localPosition = Vector3.zero;
            actor.hitbox.tag = "LevelHitbox";
        }
    }

    public class ActorParam
    {
        public ActorNumber number;
        public string text;

        public ActorParam(ActorNumber num)
        {
            number = num;
            text = "";
        }

        public ActorParam(string text_,ActorNumber num)
        {
            text = text_;
            number = num;
        }
    }

    public class ActorNumber
    {
        public float Value
        {
            get
            {
                float val = 0;

                string[] parsed = expression.Split('|');
                if (parsed.Length > 1)
                {
                    if(parsed[0] == "rand")
                    {
                        float min = PaloUtils.ConvertExpression(parsed[1], Overrides.ToArray());
                        float max = PaloUtils.ConvertExpression(parsed[2], Overrides.ToArray());
                        val = UnityEngine.Random.Range(min, max);
                    }
                }
                else
                {
                    val = PaloUtils.ConvertExpression(expression, Overrides.ToArray());
                }
                return val;
            }
            set
            {
                expression = value.ToString();
            }
        }
        string expression;
        public List<PaloUtils.ExpressionVariables> Overrides;

        public ActorNumber(string exp)
        {
            expression = exp;
            Overrides = new();
        }

        public static implicit operator ActorNumber(float value)
        {
            return new ActorNumber(value.ToString());
        }

        public static implicit operator ActorNumber(string value)
        {
            return new ActorNumber(value);
        }
    }

    public class LevelActor
    {
        public OSBPoint position;
        public GameObject visual;
        public GameObject hitbox;
        public GameObject mainObject;
        public RenderComponent rc;
        public LogicHitbox logicHitbox;
        public bool shouldBeDisposed = false;

        public bool needsWarning = false;
        public bool hasActivated = false;
        public bool hasPrepared = false;

        public string ID;

        public Dictionary<string, ActorParam> objParams = new Dictionary<string, ActorParam>();

        public LevelActor()
        {
            objParams.Add("Warning", new(2000));
            objParams.Add("Time", new(99999));
            objParams.Add("Duration", new(1000));
            objParams.Add("OutroDuration", new(200));
        }

        public virtual void Prepare()
        {
            mainObject = new GameObject(Utils.GenerateUniqueName("Spawn"));
            visual = new GameObject("Visual");
            visual.transform.parent = mainObject.transform;
            MainLevelManager.Singleton.onFrame.AddListener(Frame);
            hasPrepared = true;

            
        }

        public virtual void SetRandomPos(float x = 0, float y = 0)
        {

        }

        public virtual void Frame()
        {
            if (!OSBLevelEditorStaticValues.IsInEditor)
            {
                float timeValue = objParams["Time"].number.Value;
                float warningValue = objParams["Warning"].number.Value;
                float durationValue = objParams["Duration"].number.Value;

                if ((MainLevelManager.Singleton.msTime >= timeValue - warningValue) && needsWarning && !hasPrepared)
                {
                    Prepare();
                }
                else if (MainLevelManager.Singleton.msTime >= timeValue && !needsWarning && !hasPrepared)
                {
                    Prepare();
                }
                if (MainLevelManager.Singleton.msTime >= timeValue && !hasActivated && needsWarning)
                {
                    ActivateAttack();
                }
                if (MainLevelManager.Singleton.msTime >= timeValue + durationValue && !shouldBeDisposed && needsWarning)
                {
                    Dispose();
                }
            }
        }

        //public virtual void ApplyParams() { }

        public virtual void ActivateAttack()
        {
            hasActivated = true;
        }

        public void MoveBy(float x, float y)
        {
            if(mainObject == null)
            {
                return;
            }
            mainObject.transform.position += new Vector3(x, y, 0);

            if(rc != null)
            {
                if(mainObject.transform.position.x > 10.5f || mainObject.transform.position.x < -10.5f || mainObject.transform.position.y > 6 || mainObject.transform.position.y < -6)
                {
                    Dispose();
                }
                
            }
        }

        public virtual void Dispose()
        {
            shouldBeDisposed = true;
            GameObject.Destroy(mainObject);
            MainLevelManager.Singleton.onFrame.RemoveListener(Frame);
        }
    }

    // yeah
    // blender is laggy even when rendering stuff that's very simple

    /*
     * HOW TO CREATE A LEVEL ACTOR
     * 
     * create a class inheriting level actor
     * override Prepare (add base.Prepare at the beginning of the function)
     * 
     * to render the sprite do RenderComponent.AddToLA(this, LevelSpawnSprites.SOMETHING_HERE)
     * to add hitboxes (most likely used in ActivateAttack) do LogicHitbox.AddToLA(this) (IMPORTANT: ADD TO ACTOR AFTER ADDING THE RENDER COMPONENT AND MAKE SURE IT'S NORMAL SCALED BEFORE APPLYING THE HITBOX!)
     * 
     * animations are self explanatory
     */
}