using UnityEngine;

public class SortingLayerExposer : MonoBehaviour
{
    public string SortingLayerName = "Default";
    public int SortingOrder = 5;

    void Awake()
    {
        gameObject.GetComponent<MeshRenderer>().sortingLayerName = SortingLayerName;
        gameObject.GetComponent<MeshRenderer>().sortingOrder = SortingOrder;
    }
}