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

        [Serializable]
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

            try
            {
                string input = parse;
                foreach (ExpressionVariables var in vars)
                {
                    input = input.Replace($"[{var.textToReplace}]", var.value.ToString());
                }

                float val = (float)Convert.ToDouble(_internalTable.Compute(input, ""));
                return val;
            }
            catch
            {
                return 0;
            }
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

        public bool customSprite = false;

        public Color pink
        {
            get
            {
                if (customSprite)
                    return new Color(1, 1, 1);
                else
                    return new Color(1, 0, 0.5058824f);
            }
        }

        public static void AddToLA(LevelActor actor, Sprite sprite)
        {
            actor.rc = new RenderComponent();
            actor.rc.parent = actor;

            actor.rc.renderer = actor.visual.AddComponent<SpriteRenderer>();

            //Debug.Log(sprite);
            if (string.IsNullOrEmpty(actor.objParams["Visual"].text))
            {
                actor.rc.renderer.sprite = sprite;
                actor.rc.customSprite = false;
            }
            else
            {
                actor.rc.renderer.sprite = MainLevelManager.Singleton.imageResources[actor.objParams["Visual"].text].sprite;
                actor.rc.customSprite = true;
            }

            
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

    [Serializable]
    public class ActorParam
    {
        public ActorNumber number;
        public string text;
        public LevelActor actor;

        public ActorParam()
        {

        }

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

        public ActorParam(LevelActor actor_)
        {
            actor = actor_;
        }
    }

    [Serializable]
    public class ActorNumber
    {
        
        public string expression;
        public List<PaloUtils.ExpressionVariables> Overrides;

        public ActorNumber()
        {

        }

        public float GetValue()
        {
            float val = 0;

            string[] parsed = expression.Split('|');
            if (parsed.Length > 1)
            {
                if (parsed[0] == "rand")
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

        public bool isControlledByItself = true;

        public string ID;

        public Dictionary<string, ActorParam> objParams = new Dictionary<string, ActorParam>();

        public List<PaloUtils.ExpressionVariables> overrides;

        public LevelActor()
        {
            objParams.Add("ID", new("", ""));
            objParams.Add("Warning", new(1000));
            objParams.Add("Time", new(0));
            objParams.Add("Duration", new(1000));
            objParams.Add("OutroDuration", new(200));
            objParams.Add("Visual", new("", ""));
        }

        public virtual void Prepare()
        {
            mainObject = new GameObject(Utils.GenerateUniqueName("Spawn"));
            mainObject.tag = "Level";
            visual = new GameObject("Visual");
            visual.transform.parent = mainObject.transform;
            if(OSBLevelEditorStaticValues.IsInEditor)
            MainLevelManager.Singleton.onFrame.AddListener(Frame);
            hasPrepared = true;

            
        }

        public virtual void SetRandomPos(float x = 0, float y = 0)
        {

        }

        public virtual void SetPosition()
        {
            mainObject.transform.position = new Vector3(Utils.ConvertPixelToPosition(objParams["XPos"].number.GetValue(), UtilsDirection.X), Utils.ConvertPixelToPosition(objParams["YPos"].number.GetValue(), UtilsDirection.Y));
        }

        public virtual void SetSize(float baseSizeX, float baseSizeY)
        {
            mainObject.transform.localScale = new Vector3(Utils.CalculateSize(objParams["Size"].number.GetValue(), baseSizeX), Utils.CalculateSize(objParams["Size"].number.GetValue(), baseSizeY), 1);
        }

        public virtual void OverridePositionParam(float x, float y)
        {

        }

        public virtual void Frame()
        {
            if (!OSBLevelEditorStaticValues.IsInEditor && isControlledByItself)
            {
                //Debug.Log("Not on editor");
                float timeValue = objParams["Time"].number.GetValue();
                float warningValue = objParams["Warning"].number.GetValue();
                float durationValue = objParams["Duration"].number.GetValue();

                if ((MainLevelManager.Singleton.msTime >= timeValue - warningValue) && needsWarning && !hasPrepared)
                {
                    Debug.Log("MUST PREPARE!");
                    Prepare();
                }
                else if (MainLevelManager.Singleton.msTime >= timeValue && !needsWarning && !hasPrepared)
                {
                    Prepare();
                }
                if (MainLevelManager.Singleton.msTime >= timeValue && !hasActivated && needsWarning)
                {
                    Debug.Log("GO!");
                    ActivateAttack();
                }
                if (MainLevelManager.Singleton.msTime >= timeValue + durationValue && !shouldBeDisposed && needsWarning)
                {
                    Debug.Log("bye bye");
                    shouldBeDisposed = true;
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
            if (mainObject != null)
            {
                GameObject.Destroy(mainObject);
                MainLevelManager.Singleton.onFrame.RemoveListener(Frame);
            }
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