using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

[RequireComponent(typeof(Renderer))]
public class Card : NetworkBehaviour, IDragHandler, IPointerClickHandler
{
    const float TAP_ANGLE = 45f;
    const float FLIP_ANGLE = 180f;

    //
    // Info
    // ---
    [SyncVar]
    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    private CardInfo _info;

    //
    // Mechanics
    // ---
    [SyncVar(hook = "SetTapped")]
    public bool _tapped;
    public void SetTapped(bool tapped) // no property so it works with unet.
    {
        _tapped = tapped;
        UpdateRotation();
    }

    [SyncVar(hook = "SetFlipped")]
    private bool _flipped;
    public void SetFlipped(bool flipped)
    {
        _flipped = flipped;
        UpdateRotation();
    }

    //
    // Initialization
    // ---
    void Start()
    {
        _info = CardInfoProvider.Get.ByName(_name);
        SetMaterial();
    }

    //
    // Commands
    // ---
    [Command]
    void CmdSetTapped(bool tapped)
    {
        Debug.Log("Tapped = " + tapped);
        SetTapped(tapped);
    }

    //
    // Events
    // ---
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Hey");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount % 2 == 0)
        {
            CmdSetTapped(!_tapped);
        }
    }

    //
    // Visuals
    // ---
    private void SetMaterial()
    {
        Assert.IsNotNull(_info);
        GetComponent<Renderer>().material = _info.Material;
    }

    private void UpdateRotation()
    {
        float rotation = 0f;
        if (_tapped)
        {
            rotation += TAP_ANGLE;
        }
        if (_flipped)
        {
            rotation += FLIP_ANGLE;
        }
        transform.localRotation = Quaternion.Euler(0, rotation, 0);
    }
}
