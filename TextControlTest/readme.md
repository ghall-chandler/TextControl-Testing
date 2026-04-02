# TextControlTest Project

This project is reproducing a possible issue with the ServerTextControl PDF rendering.

## Details

There seems to be a bug when rendering a document that contains shapes. When we try to save that document to PDF using the ServerTextControl component from a WPF application the colours of the shapes are not correct. It seems like the Red and Blue values are inverted (ie. red becomes blue and blue become red).
This problem only seems to occur when the code is run asynchronously (ie. off the main UI thread).


## Code example

The contained code example has two buttons, one to render the PDF synchronously demonstrating the correct behaviour, and one to render asynchronously to demonstrate the incorrect behaviour.

Original document in the example app:
![Original document editor](./Images/app.png)

Sync rendered PDF with red and blue correct:
![Example with red and blue correct](./Images/syncpreview.png)

Async rendered PDF with red and blue inverted:
![Example with red and blue inverted](./Images/asyncpreview.png)


### Repro steps
1. Create a document with some coloured shapes (using red and blue colours)
2. Save the document into the TX format.
3. Start a new Task/Background thread.
4. On a new thread, load the TX format into a new ServerTextControl.
5. On a new thread, save the ServerTextControl into a PDF format.
6. Save the PDF to file and view it.
