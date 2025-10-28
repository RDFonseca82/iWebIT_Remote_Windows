using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace RemoteAgent;
public static class ScreenCapturer
{
    public static byte[] CaptureToJpegBytes(int quality = 60)
    {
        var bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
        using var bmp = new Bitmap(bounds.Width, bounds.Height);
        using var g = Graphics.FromImage(bmp);
        g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
        using var ms = new MemoryStream();
        var codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
        var encParams = new EncoderParameters(1);
        encParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
        bmp.Save(ms, codec, encParams);
        return ms.ToArray();
    }
}
