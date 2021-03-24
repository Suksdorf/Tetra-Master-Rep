using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posiciones : MonoBehaviour
{
    private Vector3[] posiciones = new Vector3[26];

    private void Awake()
    {
        posiciones[0] = new Vector3(-6.7f, 8.65f, 0);
        posiciones[1] = new Vector3(-2.4f, 8.65f, 0);
        posiciones[2] = new Vector3(1.9f, 8.65f, 0);
        posiciones[3] = new Vector3(6.2f, 8.65f, 0);

        posiciones[4] = new Vector3(-6.7f, 3.45f, 0);
        posiciones[5] = new Vector3(-2.4f, 3.45f, 0);
        posiciones[6] = new Vector3(1.9f, 3.45f, 0);
        posiciones[7] = new Vector3(6.2f, 3.45f, 0);

        posiciones[8] = new Vector3(-6.7f, -1.75f, 0);
        posiciones[9] = new Vector3(-2.4f, -1.75f, 0);
        posiciones[10] = new Vector3(1.9f, -1.75f, 0);
        posiciones[11] = new Vector3(6.2f, -1.75f, 0);

        posiciones[12] = new Vector3(-6.7f, -6.95f, 0);
        posiciones[13] = new Vector3(-2.4f, -6.95f, 0);
        posiciones[14] = new Vector3(1.9f, -6.95f, 0);
        posiciones[15] = new Vector3(6.2f, -6.95f, 0);


        posiciones[16] = new Vector3(-13f, 8.65f, 0); //mano jugador 1
        posiciones[17] = new Vector3(-13f, 5.65f, 0);
        posiciones[18] = new Vector3(-13f, 2.35f, 0);
        posiciones[19] = new Vector3(-13f, -0.65f, 0);
        posiciones[20] = new Vector3(-13f, -3.65f, 0);

        posiciones[21] = new Vector3(13f, 8.65f, 0); //mano jugador 2
        posiciones[22] = new Vector3(13f, 5.65f, 0);
        posiciones[23] = new Vector3(13f, 2.35f, 0);
        posiciones[24] = new Vector3(13f, -0.65f, 0);
        posiciones[25] = new Vector3(13f, -3.65f, 0);

    }

    public Vector3 returnposition(int x)
    {
        return posiciones[x];
    }    
}