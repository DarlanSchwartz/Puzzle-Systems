

public class SimpleSaveableObject : SaveableObject
{

    private void OnDrawGizmos()
    {
        
    }

    public override void Start()
    {
        // Add myself to the save game manager and set myTrans to this.transfrom in the base
        base.Start();
        // Do this class code logic
    }

    public override void LoadFromCurrentData()
    {
        // Set the current default data to the (data to save info)
        base.LoadFromCurrentData();
        // Load additional values to myself from 7 to up
    }

    public override void UpdateDataToSaveToCurrentData()
    {
        // Update the default data to the current default data
        base.UpdateDataToSaveToCurrentData();
        // Add "|" + my additionalInfo
    }

    public override void DestroySaveable()
    {
        // Destroy the saveable class and remove it from the saveable objects list
        base.DestroySaveable();
        // Actually Destroy this object
        Destroy(gameObject);
    }
}
