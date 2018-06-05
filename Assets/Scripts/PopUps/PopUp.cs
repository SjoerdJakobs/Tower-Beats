using UnityEngine;

public class PopUp : MonoBehaviour
{
    public virtual void Show(Tile calledFrom)
    {
        print("Showing: " + name);
        transform.position = calledFrom.transform.position;
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        print("Hiding: " + name);
        gameObject.SetActive(false);
    }
}
