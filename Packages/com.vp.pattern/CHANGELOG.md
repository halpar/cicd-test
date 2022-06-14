# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.3.0] - 2020-03-08
### Added
- Automated xCode build feature (experimental)
- Icon control utility
- Rollic & VP splash scene canvas prefab

### Changed
- Recorder package updated from v2.1.0 p1 to v2.5.4
- Textmeshpro & Visual Studio packages updated
- gitignore improved to support new build features
- iOS & Android platform automation pipelines improved
- Automated texture import settings (texture compress features)

### Removed
- Splash scene lightning data
- Old VP logo
- VP_GA_EXISTS symbol
- Deprecated VP scene management editor tools

## [0.2.9] - 2020-01-30
### Added
- Build Report Tool v3.5

## [0.2.8] - 2020-01-28
### Changed
- Unity version upgraded from 2019.4.2f1 to 2020.2.2f1
- All packages updated
- Moved CHANGELOG.md file to VP Pattern package

### Added
- Cinemachine package
- AssetImportWorker0.log file to .gitignore

### Removed
- Unity Collaborater package

## [0.2.7] - 2020-01-11
### Changed
- UIManager.cs and Canvas prefab (Green start button replaced with fullscreen TapToStart button) 

## [0.2.6] - 2021-01-10
### Added
- PostProcessiOSInfoPlist.cs (Removes UIRequiredDeviceCapabilities from xCode)
- DisablingBitcodeiOS.cs (Disable Bitcode from xCode)

### Removed
- ExcemptFromEncryption.cs

## [0.2.5] - 2021-01-10
### Added
- ExcemptFromEncryption.cs (set ITSAppUsesNonExemptEncryption to False in Info.plist)
- IncrementBuildNumber.cs (get the build number through PlayerSettings.iOS.buildNumber and increment it before iOS builds)

## [0.2.4] - 2020-11-08
### Added
- UIManager.cs.

### Changed
- Game Manager no longer check elephant sdk. 
- Elephant.Init() added to AnalyticsManager.cs
- UIManager added to the canvas as an component.
- Canvas added to the template scene.

## [0.2.3] - 2020-10-06
### Added
- Materials folder and Content file

### Fixed
- Icon switch 

## [0.2.2] - 2020-08-11
### Changed
- "_GameName" folder renamed to "_Main"
- VP Logo Canvas removed from Splash Scene
- VP Logo Canvas becomes a prefab
- Company Name assignment removed
- Scripting Define Symbols added for FB, GA, Elephant
- Elephant namespace check removed from GameManager.cs
- SDK checks removed
- Bundle Id assignment simplified
- FB.Init() added to AnalyticsManager.cs

### Added
- Auto initialization
- iOS build target check
- B_GameScene_001 lighting generated
- PlayerPrefs Editor Tool added
- TMP added as default
- Procedural Image added as default
- NiceVibrations added as default
- VP Games Standards added in Pattern as default

### Fixed
- Async platform switch

## [0.1.5] - 2020-05-12
### Changed
- NaughtyAttributes removed
- Icon "Non-Power of 2" setting disabled to get real resolution
- Moved some classes to Game Folder
- GameManager.cs !isElephantExist scene load is now in build too

### Added
- Missing scope warnings
- Icon naming and path check
- Extension methods for adding and removing EventTriggers
- Helper.Log() can show context object

### Fixed
- Editor Windows version strings

## [0.0.3] - 2020-04-08
### Fixed
- Fix Singleton Instance access modifier to public
- Fix SDK ToDo Texts and code orders

## [0.0.2] - 2020-04-08
### Added
- Change Log
- Directional Light Prefab
- Directional Light in D_TestScene

### Changed
- C_LoadingScene and D_TestScene moved into Scenes > UtilityScenes

## [0.0.1] - 2020-04-07
### Added
- Initial version