using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Carta : MonoBehaviour
{
    Sprite spriteRojo;
    Sprite spriteAzul;

    public Posiciones posiciones;
    public int PosicionRelativa;
    public enum Color { Azul, Rojo };
    public Color color;
    
    
    private int tipo;

    private enum TipoAtaque { P, M, X }
    private TipoAtaque tipoataque;
    int ataque;
    int defFisica;
    int defMagica;

    private void Awake()
    {
        spriteRojo = Resources.LoadAll("Tetra Master Sprite", typeof(Sprite)).Single(s => s.name == "FondoRojo") as Sprite;
        spriteAzul = Resources.LoadAll("Tetra Master Sprite", typeof(Sprite)).Single(s => s.name == "FondoAzul") as Sprite;
        posiciones = FindObjectOfType<Posiciones>();  
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.SetPositionAndRotation(posiciones.returnposition(PosicionRelativa - 1), new Quaternion());

    }

    void establecerBordes()
    {
        for (int i = 0; i < 8; i++)
        {
            if (Random.Range(1, 3) == 1) gameObject.transform.GetChild(i).gameObject.SetActive(true);
        }
        
    }

    public void generarCarta(Color c, int valor, int pos)
    {
        color = c;
        if (color == Color.Azul)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteAzul;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteRojo;
        }

        gameObject.transform.GetChild(8).gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Tetra Master Sprite", typeof(Sprite))[valor+6] as Sprite;
        gameObject.transform.GetChild(8).gameObject.SetActive(true);

        establecerBordes();

        PosicionRelativa = pos;
    }

    public void cambiarColor()
    {
        if (color == Color.Azul)
        {
            color = Color.Rojo;
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteRojo;
        }
        else
        {
            color = Color.Azul;
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteAzul;
        }
    }
    public void cambiarAColorRojo()
    {
        color = Color.Rojo;
        gameObject.GetComponent<SpriteRenderer>().sprite = spriteRojo;
    }
    public void cambiarAColorAzul()
    {
        color = Color.Azul;
        gameObject.GetComponent<SpriteRenderer>().sprite = spriteAzul;
    }
}
