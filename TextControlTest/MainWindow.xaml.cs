using SharedLibrary;
using System.Drawing;
using System.Windows;
using TXTextControl;
using TXTextControl.DataVisualization;
using TXTextControl.Drawing;
using TXTextControl.WPF;
using TXTextControl.WPF.Drawing;

namespace TextControlTest;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void TextControl_Loaded(object sender, RoutedEventArgs e)
    {
        AddShape(ShapeType.Rectangle, Color.Red, 0);
        AddShape(ShapeType.Rectangle, Color.Green, 1);
        AddShape(ShapeType.Rectangle, Color.Blue, 2);
    }

    private void AddShape(ShapeType shapeType, Color color, int textPosition)
    {
        var drawingControl = new TXDrawingControl(3000, 3000);

        var shape = new Shape(shapeType);
        shape.ShapeFill.Color = color;

        drawingControl.Shapes.Add(shape, ShapeCollection.AddStyle.Fill);

        var drawingFrame = new DrawingFrame(drawingControl);

        drawingFrame.Activate();

        TextControl.Drawings.Add(drawingFrame, textPosition);
    }

    /// <summary>
    /// When you call the GeneratePdf synchronously it will work correctly.
    /// </summary>
    private async void SyncPreview(object sender, RoutedEventArgs e)
    {
        TextControl.Save(out var bytes, BinaryStreamType.InternalUnicodeFormat);

        var pdfBytes = SharedRenderingService.GeneratePdf(bytes);

        var pdfViewerWindow = new PdfViewer("SYNC PREVIEW", pdfBytes) { Owner = this };

        pdfViewerWindow.Show();
    }

    /// <summary>
    /// When you call the GeneratePdf asynchronously there are rendering errors.
    /// </summary>
    private async void AsyncPreview(object sender, RoutedEventArgs e)
    {
        TextControl.Save(out var bytes, BinaryStreamType.InternalUnicodeFormat);

        var pdfBytes = await Task.Run(() =>
        {
            return SharedRenderingService.GeneratePdf(bytes);
        });

        var pdfViewerWindow = new PdfViewer("ASYNC PREVIEW", pdfBytes) { Owner = this };

        pdfViewerWindow.Show();
    }

    /// <summary>
    /// When you call the GeneratePdf on an STA thread it now renders correctly again.
    /// </summary>
    private async void StaThreadPreview(object sender, RoutedEventArgs e)
    {
        TextControl.Save(out var bytes, BinaryStreamType.InternalUnicodeFormat);

        var pdfBytes = await ExecuteOnStaThread(() =>
        {
            return SharedRenderingService.GeneratePdf(bytes);
        });

        var pdfViewerWindow = new PdfViewer("STA THREAD PREVIEW", pdfBytes) { Owner = this };

        pdfViewerWindow.Show();
    }

    private static Task<T> ExecuteOnStaThread<T>(Func<T> func)
    {
        var tcs = new TaskCompletionSource<T>();
        var thread = new Thread(() =>
        {
            try
            {
                tcs.SetResult(func());
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        return tcs.Task;
    }

}