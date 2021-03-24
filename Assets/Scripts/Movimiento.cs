using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    public Posiciones posiciones;
    public int PosicionRelativa;

    // Start is called before the first frame update
    void Start()
    {
        PosicionRelativa = 1;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        posiciones = FindObjectOfType<Posiciones>();
    }

    // Update is called once per frame
    void Update()
    {        

        gameObject.transform.SetPositionAndRotation(posiciones.returnposition(PosicionRelativa-1), new Quaternion());
        gameObject.transform.Translate(-2f, 1f, 0);

    }
}
