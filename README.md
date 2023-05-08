# SpaceLab Plugin

This is WAY under testing and development. But with a lot of cool features.

## Contributing

Open folder SDK and run `Setup.bat` fill directorys of SpaceEngineers Dedicated Server and Torch Server for SDK References 

## How-to use it

TODO

## Folders

* `Build` => Pre-built Torch Plugin. It doesn't include the SpaceLabAPI (which its dll is put in a `apps` folder that is created.
* `SDK` => Just the SDK (Torch and Space Enginners DS) symbolic links
* `SharpBoss` => SharpBoss DLLs (check DLLs section)
* `SpaceLab` => The Torch Plugin itself. This is the one that loads the `SpaceLabAPI` using SharpBoss
* `SpaceLabAPI` => The HTTP REST API. This is hot-reloadable by SharpBoss. So you don't need to restart your DS.

## DLLs

This plugin uses SharpBoss for exposing it's API (since it has a nice hot-reload feature). The Torch command system is not hot-reloadable (yet) but the exposed API is. I provided the DLLs directly at this repo to facilitate the build, but if you want to build yourself, the project is opensource and available here: https://github.com/racerxdl/AppServer - the changes to make it work nicely with Torch are in SpaceLabFixes Branch.


## License

The source-code itself is under MIT License.
