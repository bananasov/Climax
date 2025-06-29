# PeakPlug
Buttplug integration for [PEAK](https://store.steampowered.com/app/3527290/PEAK/)!

## Setting up

### Requirements
- [Intiface Central](https://github.com/intiface/intiface-central/releases) or [Intiface Engine](https://github.com/intiface/intiface-engine/releases) (former preferred)
- A buttplug or a device that can vibrate and has [buttplug.io](https://buttplug.io) support (See [here](https://iostindex.com/?filter0Availability=Available%2CDIY&filter1ButtplugSupport=4&filter2Features=OutputsVibrators) for a list of supported devices)

### Installation

#### Automatic

- For a quick installation and organiser, [r2modman](https://github.com/ebkr/r2modmanPlus) or [Gale](https://github.com/Kesomannen/gale) can be used

#### Manual
- Install BepInEx (see [BepInEx Installation Guide](https://docs.bepinex.dev/articles/user_guide/installation/index.html))
- Launch PEAK once with BepInEx installed to ensure that its working and needed folders are present
- Navigate to your PEAK install directory and go to `./BepInEx/plugins`
- Download the mod and unzip it in the installation directory

### Usage
- Open Intiface Central (or Engine if you know how to use that)
- Start it via the big play button
- Launch PEAK with the mod installed
  - If it doesn't work, go to Intiface settings and enable `Listen on all network interfaces` in the server settings

## Thunderstore Packaging

This template comes with Thunderstore packaging built-in, using [TCLI](<https://github.com/thunderstore-io/thunderstore-cli>).

You can build Thunderstore packages by running:

```sh
dotnet build -c Release -target:PackTS -v d
```

> [!NOTE]  
> You can learn about different build options with `dotnet build --help`.  
> `-c` is short for `--configuration` and `-v d` is `--verbosity detailed`.

The built package will be found at `artifacts/thunderstore/`.
