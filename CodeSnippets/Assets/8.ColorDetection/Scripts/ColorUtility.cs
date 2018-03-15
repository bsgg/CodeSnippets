using UnityEngine;
using System.Collections;

namespace ColorDetection
{
    public class ColorUtility : MonoBehaviour
    {
        /// <summary>
        /// Creates color with corrected brightness.
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
        /// Negative values produce darker colors.</param>
        /// <returns>
        /// Corrected <see cref="Color"/> structure.
        /// </returns>
        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            // Color is in range between 0 to 1
            float red = color.r;
            float green = color.g;
            float blue = color.b;
            float auxCF = Mathf.Clamp(correctionFactor, -1.0f, 1.0f);
            // Make the color darker          
            if (auxCF < 0.0f)
            {
                auxCF = 1 + auxCF;
                red *= auxCF;
                green *= auxCF;
                blue *= auxCF;
            }
            // Make the color lighter
            else if (auxCF > 0.0f)
            {
                red = red * auxCF + red;
                green = green * auxCF + green;
                blue = blue * auxCF + blue;
            }

            return new Color(red, green, blue);
        }


        /// <summary>
        /// Get the average colors among an array of pixels
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public static Color GetAverageColor(Color[] pixels)
        {
            float sumR = 0.0f, sumG = 0.0f, sumB = 0.0f;
            float avR, avG, avB;
            int pixelCount = pixels.Length;
            for (int i = 0; i < pixelCount; i++)
            {
                sumR += pixels[i].r;
                sumG += pixels[i].g;
                sumB += pixels[i].b;
            }

            avR = (sumR / pixelCount);
            avG = (sumG / pixelCount);
            avB = (sumB / pixelCount);

            return new Color(avR, avG, avB, 1.0f);
        }


        /// <summary>
        /// Get the distance between two colors in LAB space
        /// Lab color space is a color-opponent space with dimension L for lightness and a and b for the color-opponent dimensions
        /// The L*a*b* color space includes all perceivable colors,
        /// </summary>
        /// <param name="labA"></param>
        /// <param name="labB"></param>
        /// <returns>Distance between the colors</returns>
        public static float CIE94ColorDistance(LABColor labA, LABColor labB)
        {
            float Kl = 1.0f;
            float K1 = 0.048f;
            float K2 = 0.014f;

            float deltaL = labA.L - labB.L;
            float deltaA = labA.A - labB.A;
            float deltaB = labA.B - labB.B;

            float c1 = Mathf.Sqrt((labA.A * labA.A) + (labA.B * labA.B));
            float c2 = Mathf.Sqrt((labB.A * labB.A) + (labB.B * labB.B));
            float deltaC = c1 - c2;

            var deltaH = (deltaA * deltaA) + (deltaB * deltaB) - (deltaC * deltaC);
            deltaH = deltaH < 0 ? 0 : Mathf.Sqrt(deltaH);

            const float sl = 1.0f;
            const float kc = 1.0f;
            const float kh = 1.0f;

            float sc = 1.0f + K1 * c1;
            float sh = 1.0f + K2 * c1;

            float i = Mathf.Pow(deltaL / (Kl * sl), 2) +
                            Mathf.Pow(deltaC / (kc * sc), 2) +
                            Mathf.Pow(deltaH / (kh * sh), 2);
            //float finalResult = i < 0 ? 0 : Mathf.Sqrt(i);
            float finalResult = i < 0 ? 0 : i;

            return finalResult;
        }

        #region Constants_ CIE2000
        const float deg360InRad = Mathf.Deg2Rad * 360.0f;
        const float deg180InRad = Mathf.Deg2Rad * 180.0f;
        const float deg30InRad = Mathf.Deg2Rad * 30.0f;

        const float deg6InRad = Mathf.Deg2Rad * 6.0f;
        const float deg63InRad = Mathf.Deg2Rad * 63.0f;

        const float deg275InRad = Mathf.Deg2Rad * 275.0f;
        const float deg25InRad = Mathf.Deg2Rad * 25.0f;


        const float pow25To7 = 6103515625.0f;  /* pow(25, 7) */

        const float k_L = 1.0f;
        const float k_C = 1.0f;
        const float k_H = 1.0f;

        #endregion Constants_ CIE2000

        /// <summary>
        /// Gets the distance between two colors in LAB space acording to CIE2000 alogirthm
        /// Lab color space is a color-opponent space with dimension L for lightness and a and b for the color-opponent dimensions
        /// </summary>
        /// <param name="lab1">LAB Color 1</param>
        /// <param name="lab2">LAB Color 2</param>
        /// <returns>Returns the distance</returns>
        public static float CIE2000ColorDistance(LABColor lab1, LABColor lab2)
        {            

            float C1 = Mathf.Sqrt((lab1.A * lab1.A) + (lab1.B * lab1.B));
            float C2 = Mathf.Sqrt((lab2.A * lab2.A) + (lab2.B * lab2.B));

            float barC = (C1 + C2) / 2.0f;
            float G = 0.5f * (1f - Mathf.Sqrt(Mathf.Pow(barC, 7) / (Mathf.Pow(barC, 7) + pow25To7)));

            float a1Prime = (1.0f + G) * lab1.A;
            float a2Prime = (1.0f + G) * lab2.A;

            float CPrime1 = Mathf.Sqrt((a1Prime * a1Prime) + (lab1.B * lab1.B));
            float CPrime2 = Mathf.Sqrt((a2Prime * a2Prime) + (lab2.B * lab2.B));

            // Step 1
            float hPrime1;
            if (lab1.B == 0.0f && a1Prime == 0.0f)
            {
                hPrime1 = 0.0f;
            }
            else
            {
                hPrime1 = Mathf.Atan2(lab1.B, a1Prime);
                /* 
                 * This must be converted to a hue angle in degrees between 0 
                 * and 360 by addition of 2􏰏 to negative hue angles.
                 */
                if (hPrime1 < 0)
                    hPrime1 += deg360InRad;
            }


            float hPrime2;
            if (lab2.B == 0.0f && a2Prime == 0.0f)
                hPrime2 = 0.0f;
            else
            {
                hPrime2 = Mathf.Atan2(lab2.B, a2Prime);
                /* 
                 * This must be converted to a hue angle in degrees between 0 
                 * and 360 by addition of 2􏰏 to negative hue angles.
                 */
                if (hPrime2 < 0)
                    hPrime2 += deg360InRad;
            }


            // Step 2
            /* Equation 8 */
            float deltaLPrime = lab2.L - lab1.L;
            /* Equation 9 */
            float deltaCPrime = CPrime2 - CPrime1;
            /* Equation 10 */
            float deltahPrime;
            float CPrimeProduct = CPrime1 * CPrime2;
            if (CPrimeProduct == 0)
                deltahPrime = 0;
            else {
                /* Avoid the fabs() call */
                deltahPrime = hPrime2 - hPrime1;
                if (deltahPrime < -deg180InRad)
                    deltahPrime += deg360InRad;
                else if (deltahPrime > deg180InRad)
                    deltahPrime -= deg360InRad;
            }
            /* Equation 11 */
            float deltaHPrime = 2.0f * Mathf.Sqrt(CPrimeProduct) * Mathf.Sin(deltahPrime / 2.0f);

            // Step 3
            /* Equation 12 */
            float barLPrime = (lab1.L + lab2.L) / 2.0f;
            /* Equation 13 */
            float barCPrime = (CPrime1 + CPrime2) / 2.0f;
            /* Equation 14 */
            float barhPrime, hPrimeSum = hPrime1 + hPrime2;
            if (CPrime1 * CPrime2 == 0.0f)
            {
                barhPrime = hPrimeSum;
            }
            else {
                if (Mathf.Abs(hPrime1 - hPrime2) <= deg180InRad)
                    barhPrime = hPrimeSum / 2.0f;
                else {
                    if (hPrimeSum < deg360InRad)
                        barhPrime = (hPrimeSum + deg360InRad) / 2.0f;
                    else
                        barhPrime = (hPrimeSum - deg360InRad) / 2.0f;
                }
            }


            /* Equation 15 */
            float T = 1.0f - (0.17f * Mathf.Cos(barhPrime - deg30InRad)) +
                (0.24f * Mathf.Cos(2.0f * barhPrime)) +
                (0.32f * Mathf.Cos((3.0f * barhPrime) + deg6InRad)) -
                (0.20f * Mathf.Cos((4.0f * barhPrime) - deg63InRad));
            
            /* Equation 16 */
            float deltaTheta = deg30InRad * Mathf.Exp(-Mathf.Pow((barhPrime - deg275InRad) / deg25InRad, 2.0f));
            /* Equation 17 */
            float R_C = 2.0f * Mathf.Sqrt(Mathf.Pow(barCPrime, 7.0f) / (Mathf.Pow(barCPrime, 7.0f) + pow25To7));
            /* Equation 18 */
            float S_L = 1.0f + ((0.015f * Mathf.Pow(barLPrime - 50.0f, 2.0f)) / Mathf.Sqrt(20 + Mathf.Pow(barLPrime - 50.0f, 2.0f)));
            /* Equation 19 */
            float S_C = 1.0f + (0.045f * barCPrime);
            /* Equation 20 */
            float S_H = 1.0f + (0.015f * barCPrime * T);
            /* Equation 21 */
            float R_T = (-Mathf.Sin(2.0f * deltaTheta)) * R_C;

            /* Equation 22 */
            /*float deltaE = Mathf.Sqrt(
                Mathf.Pow(deltaLPrime / (k_L * S_L), 2.0f) +
                Mathf.Pow(deltaCPrime / (k_C * S_C), 2.0f) +
                Mathf.Pow(deltaHPrime / (k_H * S_H), 2.0f) +
                (R_T * (deltaCPrime / (k_C * S_C)) * (deltaHPrime / (k_H * S_H))));*/

            // It's oke to get rid of the Mathf.Sqrt in this case
            float deltaE =(
               Mathf.Pow(deltaLPrime / (k_L * S_L), 2.0f) +
               Mathf.Pow(deltaCPrime / (k_C * S_C), 2.0f) +
               Mathf.Pow(deltaHPrime / (k_H * S_H), 2.0f) +
               (R_T * (deltaCPrime / (k_C * S_C)) * (deltaHPrime / (k_H * S_H))));

            return deltaE;
        }

        /// <summary>
        /// Gets the Delta E Distance to two LAB Colors
        /// </summary>
        /// <param name="labA"></param>
        /// <param name="labB"></param>
        /// <returns></returns>
        public static float DeltaEColorDistance(LABColor labA, LABColor labB)
        {
            float lDelta = (labA.L - labB.L);
            float aDelta = (labA.A - labB.A);
            float bDelta = (labA.B - labB.B);

            //return (Mathf.Sqrt((lDelta * lDelta) + (aDelta * aDelta) + (bDelta * bDelta)));
            return ((lDelta * lDelta) + (aDelta * aDelta) + (bDelta * bDelta));
        }


        const float WEIGHTHUE = 0.8f;
        const float WEIGHTSATURATION = 0.1f;
        const float WEIGHTVALUE = 0.1f;
        public static float HSVDistance(Color colorA, Color colorB)
        {
            float HA;
            float SA;
            float VA;
            Color.RGBToHSV(colorA, out HA, out SA, out VA);

            float HB;
            float SB;
            float VB;
            Color.RGBToHSV(colorB, out HB, out SB, out VB);

            float distH = HB - HA;
            float distS = SB - SA;
            float distV = VB - VA;
            //return (Mathf.Sqrt(WEIGHTHUE * distH * distH + WEIGHTSATURATION * distS * distS + WEIGHTVALUE * distV * distV));
            return (WEIGHTHUE * distH * distH + WEIGHTSATURATION * distS * distS + WEIGHTVALUE * distV * distV);
        }
    }
}
