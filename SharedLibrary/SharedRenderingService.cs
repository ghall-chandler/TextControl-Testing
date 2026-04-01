using TXTextControl;

namespace SharedLibrary;

public class SharedRenderingService
{
    /// <remarks>
    /// This method is in a shared library that is used by both our front end (WPF client) and back end (ASP Website)
    /// </remarks>
    public static byte[] GeneratePdf(byte[] bytes)
    {
        using (var serverTextControl = new ServerTextControl())
        {
            serverTextControl.Create();

            serverTextControl.Load(bytes, BinaryStreamType.InternalUnicodeFormat);

            // Here we would do our custom rendering code...

            serverTextControl.Save(out var pdfBytes, BinaryStreamType.AdobePDF);

            return pdfBytes;
        }
    }
}
