using UnityEngine;
using System.Collections;

public class UISounds : Singleton<UISounds>
{

    /** Keyboard press effect. */
    public GameObject KeyEffect;

    /** Mouse click effect. */
    public GameObject MouseClickEffect;



    /** Spawns a keypress effect. */
    public void KeyPress()
        { Invoke("SpawnKeyPress", 0); }

    /** Spawns a mouse click effect. */
    public void MouseClick()
        { Invoke("SpawnMouseClick", 0); }

    private void SpawnKeyPress()
        { Instantiate(KeyEffect, transform.position, transform.rotation); }

    private void SpawnMouseClick()
        { Instantiate(MouseClickEffect, transform.position, transform.rotation);
}

}
