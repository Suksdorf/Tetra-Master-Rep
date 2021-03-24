using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bloque : MonoBehaviour
{
    public Posiciones posiciones;
    public int PosicionRelativa;
    public enum TipoBloque { A, B };
    public TipoBloque tipobloque; 

    private void Awake()
    {
        if (Random.Range(1, 3) == 1) tipobloque = Bloque.TipoBloque.A;
        else tipobloque = Bloque.TipoBloque.B;
        if (tipobloque == TipoBloque.A)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Tetra Master Sprite", typeof(Sprite)).Single(s => s.name == "Bloque1") as Sprite;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Tetra Master Sprite", typeof(Sprite)).Single(s => s.name == "Bloque2") as Sprite;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        posiciones = FindObjectOfType<Posiciones>();
        gameObject.transform.SetPositionAndRotation(posiciones.returnposition(PosicionRelativa - 1), new Quaternion());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AnimatorIsPlaying()
    {
        return GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length >
               GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
