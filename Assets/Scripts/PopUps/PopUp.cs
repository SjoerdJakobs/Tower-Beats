using UnityEngine;

public class PopUp : MonoBehaviour
{
    public Tile LastClickedFromTile { get; private set; }

    public virtual void Show(Tile calledFrom)
    {
        LastClickedFromTile = calledFrom;
        transform.position = calledFrom.transform.position;
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        print("hiding: " + name);
        ClearLastClickedTile();
        gameObject.SetActive(false);
    }

    public void ClearLastClickedTile()
    {
        LastClickedFromTile = null;
    }
}
