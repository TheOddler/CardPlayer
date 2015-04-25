using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Card : MonoBehaviour
{

    [SerializeField]
    private string _name;

    private CardInfo _info;

    void Start()
    {
        _info = CardInfo.Get.ByName(_name);
        GetComponent<Renderer>().material = ImageLoader.Instance.GetMaterialFor(_info);
    }

}
