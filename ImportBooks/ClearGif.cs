using System;

/// <summary>
/// Summary description for ClearGif
/// </summary>
public class ClearGif {
    private static byte[] _ClearGif= ClearGifBytes();
    public static byte[] Bytes { get { return _ClearGif; } }
    public static int Length { get { return _ClearGif.Length; } }

    private ClearGif() { }
    private static byte[] ClearGifBytes() {
        // work around for not being able to find a literal encoding
        char[] gif= "GIF89a1010f10000---!g410010,00001010022L10;".ToCharArray();
        byte[] r= new byte[gif.Length];
        for (int j= 0; j < gif.Length; j++) {
            switch (gif[j]) {
                case '0': r[j]= 0x00; break;
                case '1': r[j]= 0x01; break;
                case '2': r[j]= 0x02; break;
                case '4': r[j]= 0x04; break;
                case 'f': r[j]= 0xf0; break;
                case 'g': r[j]= 0xf9; break;
                case '-': r[j]= 0xff; break;
                default: r[j]= Convert.ToByte(gif[j]); break;
            }
        }
        return r;
    }
}
