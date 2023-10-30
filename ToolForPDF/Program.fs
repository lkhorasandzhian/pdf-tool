open PdfSharp.Pdf
open PdfSharp.Pdf.IO
open System
open System.IO
open System.Text

let getFilesInDirectory (directoryPath: string) =
    if Directory.Exists(directoryPath) then List.ofArray(Directory.GetFiles(directoryPath))
    else raise (new System.ArgumentException("Invalid path was providen or directory is empty."))

let combinePDFs (pdfFiles: string list) (outputPdfFile: string) =
    let outputDocument = new PdfDocument()

    for pdfFile in pdfFiles do
        use inputDocument = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import)
        for page in inputDocument.Pages do
            outputDocument.AddPage(page) |> ignore

    use stream = new FileStream(outputPdfFile, FileMode.Create)
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)  // This code line is necessary for specific encodings, such as 1252, etc.
    outputDocument.Save(stream)

[<EntryPoint>]
let main _ =
    Console.Write("Write the full path to the directory to be scanned: ")
    let directoryPath = Console.ReadLine()
    let pdfFiles = getFilesInDirectory directoryPath
    let outputPdfFile = "combined.pdf"

    combinePDFs pdfFiles outputPdfFile

    printfn "PDF files combined successfully."
    0
