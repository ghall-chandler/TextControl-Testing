using System.IO;
using System.Windows;

namespace TextControlTest;

public partial class PdfViewer : Window
{
    public PdfViewer(string title, byte[] pdfBytes)
    {
        InitializeComponent();

        var tempPath = Environment.ExpandEnvironmentVariables($"%TEMP%\\{Guid.NewGuid()}\\");
        Directory.CreateDirectory(tempPath);

        var tempFilePath = $"{tempPath}\\{Guid.NewGuid()}.pdf";
        File.WriteAllBytes(tempFilePath, pdfBytes);

        Title = title;
        webView.Source = new Uri(tempFilePath);
    }
}
