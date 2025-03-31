namespace WebSharper.MediaCapabilities

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =

    let AudioConfig = 
        Pattern.Config "AudioConfig" {
            Required = []
            Optional = [
                "encryptionScheme", T<string>
                "robustness", T<string>
            ]
        }

    let VideoConfig = 
        Class "VideoConfig"
        |=> Inherits AudioConfig

    let MediaKeySystemConfig =
        Pattern.Config "MediaKeySystemConfig" {
            Required = []
            Optional = [
                "keySystem", T<string>
                "initDataType", T<string>
                "distinctiveIdentifier", T<string>
                "persistentState", T<string>
                "sessionTypes", T<string[]> 
                "audio", AudioConfig.Type
                "video", VideoConfig.Type
            ]
        }

    let VideoInfo = 
        Pattern.Config "VideoInfo" {
            Required = []
            Optional = [
                "contentType", T<string>
                "width", T<int>
                "height", T<int>
                "bitrate", T<int>
                "framerate", T<int>
            ]
        }

    let AudioInfo = 
        Pattern.Config "AudioInfo" {
            Required = []
            Optional = [
                "contentType", T<string>
                "channels", T<int>
                "bitrate", T<int>
                "samplerate", T<int>
            ]
        }

    let MediaDecodingConfig =
        Pattern.Config "MediaDecodingConfig" {
            Required = []
            Optional = [
                "type", T<string>
                "video", VideoInfo.Type
                "audio", AudioInfo.Type
                "keySystemConfiguration", MediaKeySystemConfig.Type
            ]
        }

    let MediaEncodingConfig =
        Class "MediaEncodingConfig"
        |=> Inherits MediaDecodingConfig

    let MediaCapabilitiesInfo =
        Pattern.Config "MediaCapabilitiesInfo" {
            Required = []
            Optional = [
                "supported", T<bool>
                "smooth", T<bool>
                "powerEfficient", T<bool>
                "keySystemAccess", T<obj>
            ]
        }

    let MediaCapabilities =
        Class "MediaCapabilities"
        |+> Instance [
            "encodingInfo" => MediaEncodingConfig?config ^-> T<Promise<_>>[MediaCapabilitiesInfo]
            "decodingInfo" => MediaDecodingConfig?config ^-> T<Promise<_>>[MediaCapabilitiesInfo]
        ]

    let Assembly =
        Assembly [
            Namespace "WebSharper.MediaCapabilities" [
                MediaCapabilities
                MediaCapabilitiesInfo
                MediaEncodingConfig
                MediaDecodingConfig
                AudioInfo
                VideoInfo
                MediaKeySystemConfig
                VideoConfig
                AudioConfig
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
