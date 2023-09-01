using UnityEngine;

public class UIAnimationLoadingIcon : MonoBehaviour
{
    public RectTransform trans;
    private Vector3 speed = Vector3.zero;

    private void Update()
    {
        if(gameObject.activeSelf)
        {
            speed.z += -360 * Time.deltaTime;

            trans.rotation = Quaternion.Euler(speed);
        }
    }
}
