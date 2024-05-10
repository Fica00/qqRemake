using ZXing;
using ZXing.QrCode;
using UnityEngine;

public static class QrCreator
{
    public static Texture2D GenerateQr(string _text)
    {
        var _encoded = new Texture2D(256, 256);
        var _color32 = Encode(_text, _encoded.width, _encoded.height);
        _encoded.SetPixels32(_color32);
        _encoded.Apply();
        return _encoded;
    }

    private static Color32[] Encode(string _textForEncoding, int _width, int _height)
    {
        var _writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE, Options = new QrCodeEncodingOptions { Height = _height, Width = _width } };
        return _writer.Write(_textForEncoding);
    }
}