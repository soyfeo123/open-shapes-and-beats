using UnityEngine;
using OSB.Editor;
using static LevelSave;
using DG.Tweening;

public class Piston : LevelActor
{
    private GameObject m_squareTop;
    private GameObject m_squareBottom;

    private float m_gravX;
    private float m_velX;

    private bool m_hasExpanded = false;

    public Piston() : base()
    {
        objParams.Add("XPos", new(640));
        objParams.Add("YPos", new(360));
        objParams.Add("Size", new(100));

        needsWarning = false;
    }

    public override void Prepare()
    {
        Debug.Log("helo");

        m_gravX = 0;
        m_velX = 0;

        m_hasExpanded = false;

        base.Prepare();
        RenderComponent.AddToLA(this, LevelSpawnSprites.GENERIC_SQUARE);

        visual.transform.localScale = new Vector3(0.1f * (GetFParam("Size") * 0.01f), 0.6f * (GetFParam("Size") * 0.01f));

        mainObject.transform.position = new Vector3(Utils.ConvertPixelToPosition(GetFParam("XPos"), UtilsDirection.X), Utils.ConvertPixelToPosition(GetFParam("YPos"), UtilsDirection.Y));

        rc.SetColorToPink();

        LogicHitbox.AddToLA(this);

        m_squareTop = rc.CreateChildVisual(LevelSpawnSprites.GENERIC_SQUARE);
        m_squareTop.GetComponent<SpriteRenderer>().color = rc.pink;
        // x is 4f to account for parent scaling
        m_squareTop.transform.localScale = new Vector3(4f, 0.4f, 0);
        m_squareTop.transform.localPosition = new Vector3(0f, 0.5f);

        m_squareBottom = rc.CreateChildVisual(LevelSpawnSprites.GENERIC_SQUARE);
        m_squareBottom.GetComponent<SpriteRenderer>().color = rc.pink;
        m_squareBottom.transform.localScale = new Vector3(4f, 0.4f, 0);
        m_squareBottom.transform.localPosition = new Vector3(0f, -0.5f);

        Debug.Log("i have reached");

        Flash();
    }

    public override void Frame()
    {
        base.Frame();

        if (m_squareTop == null || m_squareBottom == null || rc == null || rc.renderer == null)
            return;

        Vector3 parentScale = visual.transform.localScale;
        SpriteRenderer mainRenderer = rc.renderer;
        float halfHeight = mainRenderer.sprite.bounds.size.y * parentScale.y * 0.5f;

        Vector3 desiredWorldScale = new Vector3(0.4f, 0.4f, 1f);
        Vector3 childLocalScale = new Vector3(desiredWorldScale.x / parentScale.x, desiredWorldScale.y / parentScale.y, 0);

        m_squareTop.transform.localScale = childLocalScale;
        m_squareTop.transform.localPosition = new Vector3(0f, 0.5f, 0);

        m_squareBottom.transform.localScale = childLocalScale;
        m_squareBottom.transform.localPosition = new Vector3(0f, -0.5f, 0);

        if (mainObject.transform.position.x > 10.5f || mainObject.transform.position.x < -10.5f || mainObject.transform.position.y > 6 || mainObject.transform.position.y < -6)
        {
            Dispose();
        }

        rb.MovePosition(rb.position + (new Vector2(m_velX, 0) * Time.fixedDeltaTime));

        m_velX += m_gravX * Time.deltaTime;
    }

    public override void Message(string msg)
    {
        if(msg.ToLower() == "piston_step")
        {
            Flash();

            if (m_hasExpanded)
            {
                m_gravX = 100;
            }
            else
            {
                visual.transform.DOKill();

                // hack, this sucks
                float prev = visual.transform.localScale.y;

                visual.transform.localScale = new Vector3(visual.transform.localScale.x, 2f);
                LogicHitbox.AddToLA(this);

                visual.transform.localScale = new Vector3(visual.transform.localScale.x, prev);

                visual.transform.DOScaleY(2f, 0.05f).SetEase(Ease.InExpo);
                
                visual.transform.DOShakeRotation(0.2f, new Vector3(0, 0, 15f), 100, 10f, true).SetDelay(0.05f);
                m_hasExpanded = true;
            }
        }
    }

    public void Flash()
    {
        rc.renderer.color = Color.white;
        m_squareTop.GetComponent<SpriteRenderer>().color = Color.white;
        m_squareBottom.GetComponent<SpriteRenderer>().color = Color.white;

        rc.renderer.DOColor(rc.pink, 0.25f).SetEase(Ease.Linear);
        m_squareTop.GetComponent<SpriteRenderer>().DOColor(rc.pink, 0.25f).SetEase(Ease.Linear);
        m_squareBottom.GetComponent<SpriteRenderer>().DOColor(rc.pink, 0.25f).SetEase(Ease.Linear);
    }
}