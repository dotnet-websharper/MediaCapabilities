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
            
            let resultElement = JS.Document.GetElementById("result")

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
                let! result = navigator.MediaCapabilities.DecodingInfo(mediaConfig)
                let supported = result.Supported
                let smooth = result.Smooth
                let powerEfficient = result.PowerEfficient

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
