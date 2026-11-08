namespace WebSharper.MediaCapabilities.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Notation
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.MediaCapabilities

[<JavaScript>]
module Client =
    // The template is loaded from the DOM, so editing index.html and refreshing
    // the browser is enough; a rebuild is only needed when holes change.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let status = Var.Create "Click the button to check video playback capabilities."
    let supported = Var.Create "—"
    let smooth = Var.Create "—"
    let powerEfficient = Var.Create "—"

    let checkMedia () =
        promise {
            let config =
                MediaDecodingConfig(
                    Type = "file",
                    Video = VideoInfo(
                        ContentType = "video/mp4; codecs=avc1.42E01E",
                        Width = 1920,
                        Height = 1080,
                        Bitrate = 5000000,
                        Framerate = 30
                    )
                )

            try
                let! result = JS.Window.Navigator.MediaCapabilities.DecodingInfo(config)
                supported := string result.Supported
                smooth := string result.Smooth
                powerEfficient := string result.PowerEfficient
                status := "Media capabilities checked."
            with ex ->
                status := $"Error: {ex.Message}"
        }

    [<SPAEntryPoint>]
    let Main () =
        IndexTemplate()
            .checkMedia(fun _ -> checkMedia () |> ignore)
            .status(status.V)
            .supported(supported.V)
            .smooth(smooth.V)
            .powerEfficient(powerEfficient.V)
            .Bind()
