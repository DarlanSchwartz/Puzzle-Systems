using UnityEngine;

public class LaserBox : SaveableObject
{
    public Transform LaserStart;
    public LineRenderer laserGraphics;
    private Transform lastHit;
    public float laserEndOffset = 1;
    private RaycastHit hit;

    public bool hasEnergy;
    public bool isMainSource = false;

    void Update()
    {
        laserGraphics.enabled = true;

        if (Physics.Raycast(LaserStart.position, LaserStart.up, out hit))
        {
            if(hasEnergy)
            {
                laserGraphics.SetPosition(1, new Vector3(0, Vector3.Distance(transform.position, hit.point) + laserEndOffset, 0));
                laserGraphics.enabled = true;

                if (hit.transform.tag == "LaserBox" || hit.transform.tag == "LaserBoxReceiver")
                {
                    if (!anim.IsInTransition(0))
                    {
                        hit.transform.SendMessage("LaserHit");
                    }

                    if (lastHit && hit.transform != lastHit)
                    {
                        lastHit.SendMessage("LaserOut");
                    }

                    if (!anim.IsInTransition(0))
                    {
                        lastHit = hit.transform;
                    }
                }
            }
            else
            {
                if (lastHit)
                {
                    lastHit.SendMessage("LaserOut");
                    lastHit = null;
                }

                laserGraphics.enabled = false;
            }
        }
        else if (lastHit)
        {
            lastHit.SendMessage("LaserOut");

            lastHit = null;
        }
        else
        {
            if(hasEnergy)
            {
                laserGraphics.SetPosition(1, new Vector3(0, 500, 0));
                laserGraphics.enabled = true;
            }
            else
            {
                laserGraphics.enabled = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        float radius = 0.01f;

        if (LaserStart && hit.point != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(LaserStart.position, hit.point);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(hit.point, radius);
        }
    }

    public void LaserHit()
    {
        hasEnergy = true;
    }

    public void LaserOut()
    {
        if(!isMainSource)
        hasEnergy = false;
    }

    public int AmountPlaces;
    public Animator anim;

    public override void Start()
    {
        // Add myself to the save game manager and set myTrans to this.transfrom in the base
        base.Start();
        // Do this class code logic
    }

    public override void LoadFromCurrentData()
    {
        string[] loadedData = dataToSave.Split('|');

        hasEnergy = bool.Parse(loadedData[0]);
        anim.SetInteger("State", int.Parse(loadedData[1]));
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        dataToSave = hasEnergy.ToString() + "|" + anim.GetInteger("State");
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }

    [ContextMenu("Press")]
    public void Press()
    {
        anim.SetInteger("State", (anim.GetInteger("State") + 1) <= (AmountPlaces - 1) ? anim.GetInteger("State") + 1 : 0);
    }
}
