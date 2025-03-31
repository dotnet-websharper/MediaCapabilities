namespace WebSharper.MediaCapabilities

open WebSharper
open WebSharper.JavaScript

[<JavaScript; AutoOpen>]
module Extensions =

    type Navigator with
        [<Inline "$this.mediaCapabilities">]
        member this.MediaCapabilities with get(): MediaCapabilities = X<MediaCapabilities>

    type WorkerNavigator with
        [<Inline "$this.mediaCapabilities">]
        member this.MediaCapabilities with get(): MediaCapabilities = X<MediaCapabilities>
