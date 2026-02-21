# Steam API Upgrade

Provides an updated version of the Steamworks SDK, granting access to new features such as the Steam timeline API. In order to accomplish this, an up-to-date (v1.61) copy of the SDK is shipped as `steam_api64_patched.dll`. Additionally, `Facepunch.Steamworks` is updated to v2.4.1 and modified to reference the patched SDK. The corresponding netcode library is similarly patched. This mod uses preloader patches to completely replace the files—be wary mixing it with other mods which rely on or modify these files.
