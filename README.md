# WebSharper Media Capabilities API Binding

This repository provides an F# [WebSharper](https://websharper.com/) binding for the [Media Capabilities API](https://developer.mozilla.org/en-US/docs/Web/API/Media_Capabilities_API), enabling WebSharper applications to assess the playback capabilities of different media formats.

## Repository Structure

The repository consists of two main projects:

1. **Binding Project**:

   - Contains the F# WebSharper binding for the Media Capabilities API.

2. **Sample Project**:
   - Demonstrates how to use the Media Capabilities API with WebSharper syntax.
   - Includes a GitHub Pages demo: [View Demo](https://dotnet-websharper.github.io/MediaCapabilities/)

## Installation

To use this package in your WebSharper project, add the NuGet package:

```bash
   dotnet add package WebSharper.MediaCapabilities
```

## Building

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/dotnet-websharper/MediaCapabilities.git
   cd MediaCapabilities
   ```

2. Build the Binding Project:

   ```bash
   dotnet build WebSharper.MediaCapabilities/WebSharper.MediaCapabilities.fsproj
   ```

3. Build and Run the Sample Project:

   ```bash
   cd WebSharper.MediaCapabilities.Sample
   dotnet build
   dotnet run
   ```

## Example Usage

Below is an example of how to use the Media Capabilities API in a WebSharper project:

```fsharp
namespace WebSharper.MediaCapabilities.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.MediaCapabilities

[<JavaScript>]
module Client =
    // The templates are loaded from the DOM, so you just can edit index.html
    // and refresh your browser, no need to recompile unless you add or remove holes.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let resultMessage = Var.Create "Click the button to check video playback capabilities."
    let navigator = As<Navigator>(JS.Window.Navigator)

    let checkMedia () =
        promise {
            // Reference to the result display element
            let resultElement = JS.Document.GetElementById("result")

            // Define media configuration for decoding check
            let mediaConfig = MediaDecodingConfig(
                Type = "file",
                Video = VideoInfo(
                    ContentType = "video/mp4; codecs=avc1.42E01E",
                    Width = 1920,
                    Height = 1080,
                    Bitrate = 5000000, // 5 Mbps
                    Framerate = 30
                )
            )

            try
                // Check media capabilities
                let! result = navigator.MediaCapabilities.DecodingInfo(mediaConfig)
                let supported = result.Supported
                let smooth = result.Smooth
                let powerEfficient = result.PowerEfficient

                // Update the UI with results
                resultElement.InnerHTML <-
                    $"✅ Supported: {supported}<br>✅ Smooth: {smooth}<br>✅ Power Efficient: {powerEfficient}"
            with ex ->
                resultMessage.Value <- $"❌ Error: {ex.Message}"
        }

    [<SPAEntryPoint>]
    let Main () =
        IndexTemplate.Main()
            .CheckMedia(fun _ ->
                async {
                    do! checkMedia () |> Promise.AsAsync
                }
                |> Async.StartImmediate
            )
            .Result(resultMessage.V)
            .Doc()
        |> Doc.RunById "main"
```

## Important Considerations

- **Limited Browser Support**: Some browsers may not fully support the Media Capabilities API; check [MDN Media Capabilities API](https://developer.mozilla.org/en-US/docs/Web/API/Media_Capabilities_API) for compatibility details.
- **Performance Impact**: Checking media capabilities dynamically may introduce minor latency.
- **Use Cases**: Helps developers optimize media playback by selecting the best format for the user's device.
