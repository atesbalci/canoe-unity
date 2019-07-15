using UnityEngine;
using ZXing;
using ZXing.Common;

namespace Framework.Scripts.Utils
{
    public class Barcode
    {
        public static Texture2D Generate(string data, BarcodeFormat format, int width, int height)
        {
            // Generate the BitMatrix
            BitMatrix bitMatrix = new MultiFormatWriter()
                .encode(data, format, width, height);

            // Generate the pixel array
            Color[] pixels = new Color[bitMatrix.Width * bitMatrix.Height];
            int pos = 0;
            for (int y = 0; y < bitMatrix.Height; y++)
            {
                for (int x = 0; x < bitMatrix.Width; x++)
                {
                    pixels[pos++] = bitMatrix[x, y] ? Color.black : Color.white;
                }
            }

            // Setup the texture
            Texture2D tex = new Texture2D(bitMatrix.Width, bitMatrix.Height);
            tex.SetPixels(pixels);
            tex.Apply();

            return tex;
        }
    }
}