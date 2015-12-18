using UnityEngine;
using System.Collections;

public class OpenURL : MonoBehaviour
{

    public string Url;

	public void Open()
        { Application.OpenURL(Url); }
}
