using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorGral : MonoBehaviour
{
    float timer;
    private List<bool> tablero = new List<bool>();
    public GameObject[] mano = new GameObject[5];
    public GameObject[] mano2 = new GameObject[5];
    private int cartaSeleccionada = -1;
    private List<int> posicionbloques = new List<int>();


    public enum Estado { EnMano, EnTablero, enAnimacion, finalizado };
    public Estado estado;
    public enum Turno { TurnoJ1, TurnoJ2 }
    public Turno turno;

    private void Awake()
    {
        estado = Estado.enAnimacion;
        turno = Turno.TurnoJ1;

        for (int i = 0; i < 16; i++)
        {
            tablero.Add(false);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        crearBloques();
        crearMano();

    }

    // Update is called once per frame
    void Update()
    {
        EstablecerCartaSeleccionada();
        ordenarAltura();
        ordenarMano();
        
        switch (estado)
        {
            case Estado.enAnimacion:
                if (FindObjectOfType<Moneda>() == null)
                {
                    generarBloques();                                       
                }
                else if (FindObjectOfType<Moneda>().GetComponent<Animator>().GetInteger("estadomoneda") == 0) timer += Time.deltaTime;
                if (timer >= 1.5f)
                {
                    timer = 0;
                    if (Random.Range(1, 3) == 1)
                    {
                        FindObjectOfType<Moneda>().GetComponent<Animator>().SetInteger("estadomoneda", 2);
                        turno = Turno.TurnoJ1;
                        FindObjectOfType<Movimiento>().PosicionRelativa = 17;
                    }
                    else
                    {
                        FindObjectOfType<Moneda>().GetComponent<Animator>().SetInteger("estadomoneda", 1);
                        turno = Turno.TurnoJ2;
                        FindObjectOfType<Movimiento>().PosicionRelativa = 22;
                    }
                }
                break;
            case Estado.EnMano:
                axis();
                if (Input.GetKeyDown("z"))
                {
                    estado = Estado.EnTablero;
                    FindObjectOfType<Movimiento>().PosicionRelativa = 1;
                }
                break;
            case Estado.EnTablero:
                axis();
                if (Input.GetKeyDown("z"))
                {
                    
                    if (tablero[FindObjectOfType<Movimiento>().PosicionRelativa - 1] == false) //esta ocupado?
                    {
                        tablero[FindObjectOfType<Movimiento>().PosicionRelativa - 1] = true;

                        if (turno == Turno.TurnoJ1)
                        {
                            mano[cartaSeleccionada].GetComponent<Carta>().PosicionRelativa = FindObjectOfType<Movimiento>().PosicionRelativa;
                            juego(FindObjectOfType<Movimiento>().PosicionRelativa);

                            if(estado!=Estado.finalizado) FindObjectOfType<Movimiento>().PosicionRelativa = 22;
                            turno = Turno.TurnoJ2;
                        }
                        else
                        {
                            mano2[cartaSeleccionada].GetComponent<Carta>().PosicionRelativa = FindObjectOfType<Movimiento>().PosicionRelativa;
                            juego(FindObjectOfType<Movimiento>().PosicionRelativa);

                            if (estado != Estado.finalizado) FindObjectOfType<Movimiento>().PosicionRelativa = 17;
                            turno = Turno.TurnoJ1;
                        }

                        estado = Estado.EnMano;

                    }
                    if (cantidadCartasMano() == 0 && cantidadCartasMano2() == 0)
                    {
                        estado = Estado.finalizado;
                    }

                }
                if (Input.GetKeyDown("x"))
                {
                    estado = Estado.EnMano;
                    if (turno == Turno.TurnoJ1) FindObjectOfType<Movimiento>().PosicionRelativa = 17;
                    else FindObjectOfType<Movimiento>().PosicionRelativa = 22;
                }
                break;
            case Estado.finalizado:
                if (FindObjectOfType<Movimiento>() != null)
                {
                    //Instantiate(Resources.Load("Bloque"));
                    //Instantiate(Resources.Load("Bloque"));

                    if (Ganador() == 1)
                    {
                    }
                    else if (Ganador() == 2)
                    {

                    }
                    else
                    {

                    }
                    FindObjectOfType<Movimiento>().gameObject.SetActive(false);
                }

                break;
        }
    }

    void crearBloques()
    {
        for (int i = 0; i < 6; i++)
        {
            if (Random.Range(1, 3) == 1)
            {
                int pos = Random.Range(1, 17);
                while (tablero[pos-1]) pos = Random.Range(1, 17);            
                posicionbloques.Add(pos);
            }
        }
    }

    void crearMano()
    {
        for (int i = 0; i < 5; i++)
        {
            mano[i] = (GameObject)Instantiate(Resources.Load("Carta"));
            mano[i].GetComponent<Carta>().generarCarta(Carta.Color.Azul, Random.Range(0, 100), 17 + i);

            mano2[i] = (GameObject)Instantiate(Resources.Load("Carta"));
            mano2[i].GetComponent<Carta>().generarCarta(Carta.Color.Rojo, Random.Range(0, 100), 22 + i);
        }
    }

    void ordenarAltura()
    {
        for (int i = 0; i < 5; i++)
        {
            if (i == cartaSeleccionada)
            {
                if (turno == Turno.TurnoJ2) foreach (SpriteRenderer item in mano2[i].gameObject.GetComponentsInChildren<SpriteRenderer>()) item.sortingOrder = 5;
                else foreach (SpriteRenderer item in mano[i].gameObject.GetComponentsInChildren<SpriteRenderer>()) item.sortingOrder = 5;
            }
            else
            {
                foreach (SpriteRenderer item in mano[i].gameObject.GetComponentsInChildren<SpriteRenderer>()) item.sortingOrder = i;
                foreach (SpriteRenderer item in mano2[i].gameObject.GetComponentsInChildren<SpriteRenderer>()) item.sortingOrder = i;
            }           
        }
    }

    void ordenarMano()
    {
        int EnMano=0, EnMano2 = 0;
        for (int i = 0; i < 5; i++)
        {
            if (mano[i].GetComponent<Carta>().PosicionRelativa > 16)
            {
                EnMano++;
                mano[i].GetComponent<Carta>().PosicionRelativa = 16 + EnMano;
            }
            if (mano2[i].GetComponent<Carta>().PosicionRelativa > 21)
            {
                EnMano2++;
                mano2[i].GetComponent<Carta>().PosicionRelativa = 21 + EnMano2;
            }
        }
    }
    /*bool estaEnMano()
    {
        if(turno == Turno.TurnoJ1)
        {
            if (mano[cartaSeleccionada].GetComponent<Carta>().PosicionRelativa == FindObjectOfType<Movimiento>().PosicionRelativa) return true;
        }
        else
        {
            if (mano2[cartaSeleccionada].GetComponent<Carta>().PosicionRelativa == FindObjectOfType<Movimiento>().PosicionRelativa) return true;
        }
        return false;
    } */

    int cantidadCartasMano()
    {
        int cantidad=0;
        for (int i = 0; i < 5; i++)
        {
            if (mano[i].GetComponent<Carta>().PosicionRelativa > 16) cantidad++;
        }
        return cantidad;
    }
    int cantidadCartasMano2()
    {
        int cantidad = 0;
        for (int i = 0; i < 5; i++)
        {
            if (mano2[i].GetComponent<Carta>().PosicionRelativa > 21) cantidad++;
        }
        return cantidad;
    }
    void EstablecerCartaSeleccionada()
    {
        if(estado == Estado.EnMano)
        {
            if (turno == Turno.TurnoJ1)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (mano[i].GetComponent<Carta>().PosicionRelativa == FindObjectOfType<Movimiento>().PosicionRelativa) cartaSeleccionada = i;
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (mano2[i].GetComponent<Carta>().PosicionRelativa == FindObjectOfType<Movimiento>().PosicionRelativa) cartaSeleccionada = i;
                }
            }
        }      
    }  

    private void juego(int pos)
    {
        switch (pos)
        {
            case 1:
                int[] x1 = { 2,5,6 };
                cambiar(pos, x1.Length, x1);
                break;
            case 2:
                int[] x2 = { 1,3,5,6,7 };
                cambiar(pos, x2.Length, x2);
                break;
            case 3:
                int[] x3 = { 2,4,6,7,8 };
                cambiar(pos, x3.Length, x3);
                break;
            case 4:
                int[] x4 = { 3, 7, 8 };
                cambiar(pos, x4.Length, x4);
                break;
            case 5:
                int[] x5 = { 1, 2, 6, 9, 10 };
                cambiar(pos, x5.Length, x5);
                break;
            case 6:
                int[] x6 = { 1,2,3,5,7,9,10,11 };
                cambiar(pos, x6.Length, x6);
                break;
            case 7:
                int[] x7 = { 2,3,4,6,8,10,11,12 };
                cambiar(pos, x7.Length, x7);
                break;
            case 8:
                int[] x8 = { 3,4,7,11,12 };
                cambiar(pos, x8.Length, x8);
                break;
            case 9:
                int[] x9 = { 5,6,10,13,14 };
                cambiar(pos, x9.Length, x9);
                break;
            case 10:
                int[] x10 = { 5,6,7,9,11,13,14,15 };
                cambiar(pos, x10.Length, x10);
                break;
            case 11:
                int[] x11 = { 6, 7, 8, 10, 12, 14, 15, 16 };
                cambiar(pos, x11.Length, x11);
                break;
            case 12:
                int[] x12 = { 7,8,11,15,16 };
                cambiar(pos, x12.Length, x12);
                break;
            case 13:
                int[] x13 = { 9,10,14 };
                cambiar(pos, x13.Length, x13);
                break;
            case 14:
                int[] x14 = { 9,10,11,13,15 };
                cambiar(pos, x14.Length, x14);
                break;
            case 15:
                int[] x15 = { 10,11,12,14,16 };
                cambiar(pos, x15.Length, x15);
                break;
            case 16:
                int[] x16 = { 11,12,15 };
                cambiar(pos, x16.Length, x16);
                break;
        }
    }

    GameObject cartaEnPosicion(int pos)
    {
        if (deQueMano(pos) == 1) return mano[posicionEnMano(pos)];
        else return mano2[posicionEnMano(pos)];
    }
    Carta.Color colorDe(int pos)
    {
        return cartaEnPosicion(pos).GetComponent<Carta>().color;
    }
    bool apunta(int de, int a)
    {
        int flecha = 0;
        switch (a - de)
        {
            case -5: flecha = 0; break;
            case -4: flecha = 1; break;
            case -3: flecha = 2; break;
            case -1: flecha = 3; break;
            case 1: flecha = 4; break;
            case 3: flecha = 5; break;
            case 4: flecha = 6; break;
            case 5: flecha = 7; break;
        }
        return cartaEnPosicion(de).transform.GetChild(flecha).gameObject.activeSelf;
    }
    void cambiar(int pos, int nfor, int[] valores)
    {
        for (int i = 0; i < nfor; i++)
        {
            if (posicionEnMano(valores[i]) != -1)
            {
                if (apunta(pos,valores[i]) && colorDe(pos) != colorDe(valores[i]))
                {
                    if (deQueMano(valores[i]) == 1)
                    {                       
                        if(turno == Turno.TurnoJ1) mano[posicionEnMano(valores[i])].GetComponent<Carta>().cambiarAColorAzul();
                        else mano[posicionEnMano(valores[i])].GetComponent<Carta>().cambiarAColorRojo();
                    }
                    else
                    {
                        if (turno == Turno.TurnoJ1) mano2[posicionEnMano(valores[i])].GetComponent<Carta>().cambiarAColorAzul();
                        else mano2[posicionEnMano(valores[i])].GetComponent<Carta>().cambiarAColorRojo();
                    }                            
                    if (pos == FindObjectOfType<Movimiento>().PosicionRelativa && apunta(valores[i], pos))
                    {
                        juego(valores[i]);                  
                    }
                }
            }
        }
    }
    
    int deQueMano(int pos)
    {
        for (int i = 0; i < 5; i++)
        {
            if (mano[i].GetComponent<Carta>().PosicionRelativa == pos) return 1;
            if (mano2[i].GetComponent<Carta>().PosicionRelativa == pos) return 2;
        }
        return -1;
    }

    int posicionEnMano(int pos)
    {
        for (int i = 0; i < 5; i++)
        {
            if (mano[i].GetComponent<Carta>().PosicionRelativa == pos) return i;
            if (mano2[i].GetComponent<Carta>().PosicionRelativa == pos) return i;
        }
        return -1;
    }
    void axis()
    {
        if (estado == Estado.EnTablero)
        {
            if(Input.GetKeyDown("right") && FindObjectOfType<Movimiento>().PosicionRelativa<16)
            {
                FindObjectOfType<Movimiento>().PosicionRelativa += 1;
            }
            if (Input.GetKeyDown("left") && FindObjectOfType<Movimiento>().PosicionRelativa > 1)
            {
                FindObjectOfType<Movimiento>().PosicionRelativa -= 1;
            }
            if (Input.GetKeyDown("up") && FindObjectOfType<Movimiento>().PosicionRelativa - 4 > 0)
            {
                FindObjectOfType<Movimiento>().PosicionRelativa -= 4;
            }
            if (Input.GetKeyDown("down") && FindObjectOfType<Movimiento>().PosicionRelativa + 4 < 17)
            {
                FindObjectOfType<Movimiento>().PosicionRelativa += 4;
            }
        }
        else  //en mano
        {
            if (Input.GetKeyDown("up") && FindObjectOfType<Movimiento>().PosicionRelativa - 1 != 16 && FindObjectOfType<Movimiento>().PosicionRelativa - 1 != 21)
            {
                FindObjectOfType<Movimiento>().PosicionRelativa -= 1;
            }
            if (Input.GetKeyDown("down") && FindObjectOfType<Movimiento>().PosicionRelativa + 1 != 17+cantidadCartasMano() && FindObjectOfType<Movimiento>().PosicionRelativa + 1 != 22+cantidadCartasMano2())
            {
                FindObjectOfType<Movimiento>().PosicionRelativa += 1;
            }
        }
    }

    void generarBloques()
    {
        if (posicionbloques.Count > 0)
        {
            if (FindObjectOfType<Bloque>() != null)
            {
                if (!FindObjectOfType<Bloque>().AnimatorIsPlaying())
                {
                    Instantiate(Resources.Load("Bloque"));
                    tablero[posicionbloques[0] - 1] = true;
                    FindObjectOfType<Bloque>().GetComponent<Bloque>().PosicionRelativa = posicionbloques[0];
                    posicionbloques.RemoveAt(0);
                }
            }
            else
            {
                Instantiate(Resources.Load("Bloque"));
                tablero[posicionbloques[0] - 1] = true;
                FindObjectOfType<Bloque>().GetComponent<Bloque>().PosicionRelativa = posicionbloques[0];
                posicionbloques.RemoveAt(0);
            }
        }
        else if (!FindObjectOfType<Bloque>().AnimatorIsPlaying())
        {
            foreach (SpriteRenderer item in FindObjectOfType<Movimiento>().gameObject.GetComponentsInChildren<SpriteRenderer>()) item.enabled = true;
            estado = Estado.EnMano;
        }
    }

    int Ganador()
    {
        int cartasAzules = 0;
        for (int i = 0; i < 5; i++)
        {
            if (mano[i].GetComponent<Carta>().color == Carta.Color.Azul) cartasAzules++;
            if (mano2[i].GetComponent<Carta>().color == Carta.Color.Azul) cartasAzules++;
        }
        if (cartasAzules > 5) return 1;
        if (cartasAzules < 5) return 2;
        return 3;
    }
}
