using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MisCode.BitsExamples
{
    public class BitPackingExample : MonoBehaviour
    {

        void Start()
        {
            string A = "110111";
            string B = "10001";
            string C = "1101";

            // Convert string bits into ints, with base 2
            int aBits = Convert.ToInt32(A, 2);
            int bBits = Convert.ToInt32(B, 2);
            int cBits = Convert.ToInt32(C, 2);

            // Packed all the bits in the same bits, shifting each element
            // Fill all with 0 = 32 bits = 00000000 00000000 00000000 0000000
            int packed = 0; 

            // Pack each string bit to the far left of the int
            // int == 32bits
            packed = packed | (aBits << 26);
            packed = packed | (bBits << 21);
            packed = packed | (cBits << 17);

            Debug.Log(Convert.ToString(packed, 2).PadLeft(32, '0'));

        }

    }
}
