using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CardInfo
{
    //
    // Constructors
    // ---
	public CardInfo(string name, Material mat)
    {
	    _name = name;
	    _material = mat;
    }

    //
    // Info
    // ---
	private string _name;
	public string Name
	{
		get { return _name; }
	}
	
	public string Id { get; set; }

	private Material _material;
	public Material Material
	{
		get { return _material; }
	}
}
