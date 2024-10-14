<a name='assembly'></a>
# Ssit.Pixel.Framework

## Contents

- [ButtonState](#T-Ssit-Pixel-Framework-Input-ButtonState 'Ssit.Pixel.Framework.Input.ButtonState')
  - [#ctor()](#M-Ssit-Pixel-Framework-Input-ButtonState-#ctor-System-Boolean,System-Boolean- 'Ssit.Pixel.Framework.Input.ButtonState.#ctor(System.Boolean,System.Boolean)')
  - [Empty](#F-Ssit-Pixel-Framework-Input-ButtonState-Empty 'Ssit.Pixel.Framework.Input.ButtonState.Empty')
  - [JustPressed](#F-Ssit-Pixel-Framework-Input-ButtonState-JustPressed 'Ssit.Pixel.Framework.Input.ButtonState.JustPressed')
  - [JustReleased](#F-Ssit-Pixel-Framework-Input-ButtonState-JustReleased 'Ssit.Pixel.Framework.Input.ButtonState.JustReleased')
  - [IsChanged](#P-Ssit-Pixel-Framework-Input-ButtonState-IsChanged 'Ssit.Pixel.Framework.Input.ButtonState.IsChanged')
  - [IsDown](#P-Ssit-Pixel-Framework-Input-ButtonState-IsDown 'Ssit.Pixel.Framework.Input.ButtonState.IsDown')
- [GameControllerAxis](#T-Ssit-Pixel-Framework-Input-GameControllerAxis 'Ssit.Pixel.Framework.Input.GameControllerAxis')
  - [DPadX](#F-Ssit-Pixel-Framework-Input-GameControllerAxis-DPadX 'Ssit.Pixel.Framework.Input.GameControllerAxis.DPadX')
  - [DPadY](#F-Ssit-Pixel-Framework-Input-GameControllerAxis-DPadY 'Ssit.Pixel.Framework.Input.GameControllerAxis.DPadY')
  - [LeftTrigger](#F-Ssit-Pixel-Framework-Input-GameControllerAxis-LeftTrigger 'Ssit.Pixel.Framework.Input.GameControllerAxis.LeftTrigger')
  - [LeftY](#F-Ssit-Pixel-Framework-Input-GameControllerAxis-LeftY 'Ssit.Pixel.Framework.Input.GameControllerAxis.LeftY')
  - [RightX](#F-Ssit-Pixel-Framework-Input-GameControllerAxis-RightX 'Ssit.Pixel.Framework.Input.GameControllerAxis.RightX')
  - [RightY](#F-Ssit-Pixel-Framework-Input-GameControllerAxis-RightY 'Ssit.Pixel.Framework.Input.GameControllerAxis.RightY')
- [GameControllerButton](#T-Ssit-Pixel-Framework-Input-GameControllerButton 'Ssit.Pixel.Framework.Input.GameControllerButton')
  - [LeftStickDown](#F-Ssit-Pixel-Framework-Input-GameControllerButton-LeftStickDown 'Ssit.Pixel.Framework.Input.GameControllerButton.LeftStickDown')
  - [LeftStickLeft](#F-Ssit-Pixel-Framework-Input-GameControllerButton-LeftStickLeft 'Ssit.Pixel.Framework.Input.GameControllerButton.LeftStickLeft')
  - [LeftStickRight](#F-Ssit-Pixel-Framework-Input-GameControllerButton-LeftStickRight 'Ssit.Pixel.Framework.Input.GameControllerButton.LeftStickRight')
  - [LeftStickUp](#F-Ssit-Pixel-Framework-Input-GameControllerButton-LeftStickUp 'Ssit.Pixel.Framework.Input.GameControllerButton.LeftStickUp')
  - [LeftTrigger](#F-Ssit-Pixel-Framework-Input-GameControllerButton-LeftTrigger 'Ssit.Pixel.Framework.Input.GameControllerButton.LeftTrigger')
  - [RightStickDown](#F-Ssit-Pixel-Framework-Input-GameControllerButton-RightStickDown 'Ssit.Pixel.Framework.Input.GameControllerButton.RightStickDown')
  - [RightStickLeft](#F-Ssit-Pixel-Framework-Input-GameControllerButton-RightStickLeft 'Ssit.Pixel.Framework.Input.GameControllerButton.RightStickLeft')
  - [RightStickRight](#F-Ssit-Pixel-Framework-Input-GameControllerButton-RightStickRight 'Ssit.Pixel.Framework.Input.GameControllerButton.RightStickRight')
  - [RightStickUp](#F-Ssit-Pixel-Framework-Input-GameControllerButton-RightStickUp 'Ssit.Pixel.Framework.Input.GameControllerButton.RightStickUp')
  - [RightTrigger](#F-Ssit-Pixel-Framework-Input-GameControllerButton-RightTrigger 'Ssit.Pixel.Framework.Input.GameControllerButton.RightTrigger')
- [IContentManager](#T-Ssit-Pixel-Framework-Content-IContentManager 'Ssit.Pixel.Framework.Content.IContentManager')
  - [Load\`\`1(path)](#M-Ssit-Pixel-Framework-Content-IContentManager-Load``1-System-String- 'Ssit.Pixel.Framework.Content.IContentManager.Load``1(System.String)')
  - [RegisterLoader\`\`1(loadFunc)](#M-Ssit-Pixel-Framework-Content-IContentManager-RegisterLoader``1-Ssit-Pixel-Framework-Content-LoadResourceDelegate- 'Ssit.Pixel.Framework.Content.IContentManager.RegisterLoader``1(Ssit.Pixel.Framework.Content.LoadResourceDelegate)')
- [IFilesProvider](#T-Ssit-Pixel-Framework-IO-IFilesProvider 'Ssit.Pixel.Framework.IO.IFilesProvider')
  - [Open(path)](#M-Ssit-Pixel-Framework-IO-IFilesProvider-Open-System-String- 'Ssit.Pixel.Framework.IO.IFilesProvider.Open(System.String)')
- [IFontsManager](#T-Ssit-Pixel-Framework-Graphics-IFontsManager 'Ssit.Pixel.Framework.Graphics.IFontsManager')
  - [Item](#P-Ssit-Pixel-Framework-Graphics-IFontsManager-Item-System-String- 'Ssit.Pixel.Framework.Graphics.IFontsManager.Item(System.String)')
  - [LoadFonts(jsonStream)](#M-Ssit-Pixel-Framework-Graphics-IFontsManager-LoadFonts-System-IO-Stream- 'Ssit.Pixel.Framework.Graphics.IFontsManager.LoadFonts(System.IO.Stream)')
- [IGameControllers](#T-Ssit-Pixel-Framework-Input-IGameControllers 'Ssit.Pixel.Framework.Input.IGameControllers')
  - [VibrationForce](#P-Ssit-Pixel-Framework-Input-IGameControllers-VibrationForce 'Ssit.Pixel.Framework.Input.IGameControllers.VibrationForce')
  - [GetAxis(player,axis)](#M-Ssit-Pixel-Framework-Input-IGameControllers-GetAxis-System-Int32,Ssit-Pixel-Framework-Input-GameControllerAxis- 'Ssit.Pixel.Framework.Input.IGameControllers.GetAxis(System.Int32,Ssit.Pixel.Framework.Input.GameControllerAxis)')
  - [GetButton(player,button)](#M-Ssit-Pixel-Framework-Input-IGameControllers-GetButton-System-Int32,Ssit-Pixel-Framework-Input-GameControllerButton- 'Ssit.Pixel.Framework.Input.IGameControllers.GetButton(System.Int32,Ssit.Pixel.Framework.Input.GameControllerButton)')
- [IInputMapping](#T-Ssit-Pixel-Framework-Input-IInputMapping 'Ssit.Pixel.Framework.Input.IInputMapping')
  - [GetAxis(axis)](#M-Ssit-Pixel-Framework-Input-IInputMapping-GetAxis-System-String- 'Ssit.Pixel.Framework.Input.IInputMapping.GetAxis(System.String)')
  - [GetButton(button)](#M-Ssit-Pixel-Framework-Input-IInputMapping-GetButton-System-String- 'Ssit.Pixel.Framework.Input.IInputMapping.GetButton(System.String)')
- [IInputMappings](#T-Ssit-Pixel-Framework-Input-IInputMappings 'Ssit.Pixel.Framework.Input.IInputMappings')
  - [Item](#P-Ssit-Pixel-Framework-Input-IInputMappings-Item-System-Int32- 'Ssit.Pixel.Framework.Input.IInputMappings.Item(System.Int32)')
  - [Mapper(player)](#M-Ssit-Pixel-Framework-Input-IInputMappings-Mapper-System-Int32- 'Ssit.Pixel.Framework.Input.IInputMappings.Mapper(System.Int32)')
- [IKeyboard](#T-Ssit-Pixel-Framework-Input-IKeyboard 'Ssit.Pixel.Framework.Input.IKeyboard')
  - [GetKey(key)](#M-Ssit-Pixel-Framework-Input-IKeyboard-GetKey-Ssit-Pixel-Framework-Input-Key- 'Ssit.Pixel.Framework.Input.IKeyboard.GetKey(Ssit.Pixel.Framework.Input.Key)')
- [IMapper](#T-Ssit-Pixel-Framework-Input-IMapper 'Ssit.Pixel.Framework.Input.IMapper')
  - [Clear()](#M-Ssit-Pixel-Framework-Input-IMapper-Clear 'Ssit.Pixel.Framework.Input.IMapper.Clear')
  - [MapAxis(axisName,axis)](#M-Ssit-Pixel-Framework-Input-IMapper-MapAxis-System-String,Ssit-Pixel-Framework-Input-GameControllerAxis- 'Ssit.Pixel.Framework.Input.IMapper.MapAxis(System.String,Ssit.Pixel.Framework.Input.GameControllerAxis)')
  - [MapAxis(axisName,negative,positive)](#M-Ssit-Pixel-Framework-Input-IMapper-MapAxis-System-String,Ssit-Pixel-Framework-Input-GameControllerButton,Ssit-Pixel-Framework-Input-GameControllerButton- 'Ssit.Pixel.Framework.Input.IMapper.MapAxis(System.String,Ssit.Pixel.Framework.Input.GameControllerButton,Ssit.Pixel.Framework.Input.GameControllerButton)')
  - [MapAxis(axisName,negative,positive)](#M-Ssit-Pixel-Framework-Input-IMapper-MapAxis-System-String,Ssit-Pixel-Framework-Input-Key,Ssit-Pixel-Framework-Input-Key- 'Ssit.Pixel.Framework.Input.IMapper.MapAxis(System.String,Ssit.Pixel.Framework.Input.Key,Ssit.Pixel.Framework.Input.Key)')
  - [MapButton(buttonName,button)](#M-Ssit-Pixel-Framework-Input-IMapper-MapButton-System-String,Ssit-Pixel-Framework-Input-GameControllerButton- 'Ssit.Pixel.Framework.Input.IMapper.MapButton(System.String,Ssit.Pixel.Framework.Input.GameControllerButton)')
  - [MapButton(buttonName,key)](#M-Ssit-Pixel-Framework-Input-IMapper-MapButton-System-String,Ssit-Pixel-Framework-Input-Key- 'Ssit.Pixel.Framework.Input.IMapper.MapButton(System.String,Ssit.Pixel.Framework.Input.Key)')
- [IMusicPlayer](#T-Ssit-Pixel-Framework-Audio-IMusicPlayer 'Ssit.Pixel.Framework.Audio.IMusicPlayer')
  - [ChangePlaylist(name,fadeTimeMs,resetProgress)](#M-Ssit-Pixel-Framework-Audio-IMusicPlayer-ChangePlaylist-System-String,System-Int32,System-Boolean- 'Ssit.Pixel.Framework.Audio.IMusicPlayer.ChangePlaylist(System.String,System.Int32,System.Boolean)')
  - [NextTrack(fadeTimeMs,loop)](#M-Ssit-Pixel-Framework-Audio-IMusicPlayer-NextTrack-System-Int32,System-Boolean- 'Ssit.Pixel.Framework.Audio.IMusicPlayer.NextTrack(System.Int32,System.Boolean)')
  - [PreviousTrack(fadeTimeMs,loop)](#M-Ssit-Pixel-Framework-Audio-IMusicPlayer-PreviousTrack-System-Int32,System-Boolean- 'Ssit.Pixel.Framework.Audio.IMusicPlayer.PreviousTrack(System.Int32,System.Boolean)')
  - [RegisterPlaylist(name,playlist)](#M-Ssit-Pixel-Framework-Audio-IMusicPlayer-RegisterPlaylist-System-String,Ssit-Pixel-Framework-Audio-MusicPlaylist- 'Ssit.Pixel.Framework.Audio.IMusicPlayer.RegisterPlaylist(System.String,Ssit.Pixel.Framework.Audio.MusicPlaylist)')
- [IRenderTarget](#T-Ssit-Pixel-Framework-Graphics-IRenderTarget 'Ssit.Pixel.Framework.Graphics.IRenderTarget')
- [ISoundEffect](#T-Ssit-Pixel-Framework-Audio-ISoundEffect 'Ssit.Pixel.Framework.Audio.ISoundEffect')
  - [CreateInstance()](#M-Ssit-Pixel-Framework-Audio-ISoundEffect-CreateInstance 'Ssit.Pixel.Framework.Audio.ISoundEffect.CreateInstance')
  - [PlayOnce(volume)](#M-Ssit-Pixel-Framework-Audio-ISoundEffect-PlayOnce-System-Single- 'Ssit.Pixel.Framework.Audio.ISoundEffect.PlayOnce(System.Single)')
- [ISoundEffectInstance](#T-Ssit-Pixel-Framework-Audio-ISoundEffectInstance 'Ssit.Pixel.Framework.Audio.ISoundEffectInstance')
  - [Emitter](#P-Ssit-Pixel-Framework-Audio-ISoundEffectInstance-Emitter 'Ssit.Pixel.Framework.Audio.ISoundEffectInstance.Emitter')
  - [Parameters](#P-Ssit-Pixel-Framework-Audio-ISoundEffectInstance-Parameters 'Ssit.Pixel.Framework.Audio.ISoundEffectInstance.Parameters')
  - [Pause()](#M-Ssit-Pixel-Framework-Audio-ISoundEffectInstance-Pause 'Ssit.Pixel.Framework.Audio.ISoundEffectInstance.Pause')
  - [Play()](#M-Ssit-Pixel-Framework-Audio-ISoundEffectInstance-Play 'Ssit.Pixel.Framework.Audio.ISoundEffectInstance.Play')
  - [Resume()](#M-Ssit-Pixel-Framework-Audio-ISoundEffectInstance-Resume 'Ssit.Pixel.Framework.Audio.ISoundEffectInstance.Resume')
  - [Stop()](#M-Ssit-Pixel-Framework-Audio-ISoundEffectInstance-Stop 'Ssit.Pixel.Framework.Audio.ISoundEffectInstance.Stop')
- [ISoundManager](#T-Ssit-Pixel-Framework-Audio-ISoundManager 'Ssit.Pixel.Framework.Audio.ISoundManager')
  - [Listener](#P-Ssit-Pixel-Framework-Audio-ISoundManager-Listener 'Ssit.Pixel.Framework.Audio.ISoundManager.Listener')
  - [MasterVolume](#P-Ssit-Pixel-Framework-Audio-ISoundManager-MasterVolume 'Ssit.Pixel.Framework.Audio.ISoundManager.MasterVolume')
- [ITexture](#T-Ssit-Pixel-Framework-Graphics-ITexture 'Ssit.Pixel.Framework.Graphics.ITexture')
  - [Size](#P-Ssit-Pixel-Framework-Graphics-ITexture-Size 'Ssit.Pixel.Framework.Graphics.ITexture.Size')
- [ImagesLoader](#T-Ssit-Pixel-Framework-Utils-ImagesLoader 'Ssit.Pixel.Framework.Utils.ImagesLoader')
  - [LoadImage(stream)](#M-Ssit-Pixel-Framework-Utils-ImagesLoader-LoadImage-System-IO-Stream- 'Ssit.Pixel.Framework.Utils.ImagesLoader.LoadImage(System.IO.Stream)')
- [IoCRegistrar](#T-Ssit-Pixel-Framework-Core-IoCRegistrar 'Ssit.Pixel.Framework.Core.IoCRegistrar')
  - [WithPixelCore(builder)](#M-Ssit-Pixel-Framework-Core-IoCRegistrar-WithPixelCore-Ssit-Utils-IoC-IIoCContainerBuilder- 'Ssit.Pixel.Framework.Core.IoCRegistrar.WithPixelCore(Ssit.Utils.IoC.IIoCContainerBuilder)')
- [Key](#T-Ssit-Pixel-Framework-Input-Key 'Ssit.Pixel.Framework.Input.Key')
- [LoadResourceDelegate](#T-Ssit-Pixel-Framework-Content-LoadResourceDelegate 'Ssit.Pixel.Framework.Content.LoadResourceDelegate')
- [MusicPlaylist](#T-Ssit-Pixel-Framework-Audio-MusicPlaylist 'Ssit.Pixel.Framework.Audio.MusicPlaylist')
  - [_playlist](#F-Ssit-Pixel-Framework-Audio-MusicPlaylist-_playlist 'Ssit.Pixel.Framework.Audio.MusicPlaylist._playlist')
  - [CurrentPosition](#P-Ssit-Pixel-Framework-Audio-MusicPlaylist-CurrentPosition 'Ssit.Pixel.Framework.Audio.MusicPlaylist.CurrentPosition')
  - [CurrentSong](#P-Ssit-Pixel-Framework-Audio-MusicPlaylist-CurrentSong 'Ssit.Pixel.Framework.Audio.MusicPlaylist.CurrentSong')
  - [List](#P-Ssit-Pixel-Framework-Audio-MusicPlaylist-List 'Ssit.Pixel.Framework.Audio.MusicPlaylist.List')
  - [Add(song)](#M-Ssit-Pixel-Framework-Audio-MusicPlaylist-Add-Ssit-Pixel-Framework-Audio-Song- 'Ssit.Pixel.Framework.Audio.MusicPlaylist.Add(Ssit.Pixel.Framework.Audio.Song)')
  - [System#Collections#Generic#IEnumerable{Ssit#Pixel#Framework#Audio#Song}#GetEnumerator()](#M-Ssit-Pixel-Framework-Audio-MusicPlaylist-System#Collections#Generic#IEnumerable{Ssit#Pixel#Framework#Audio#Song}#GetEnumerator 'Ssit.Pixel.Framework.Audio.MusicPlaylist.System#Collections#Generic#IEnumerable{Ssit#Pixel#Framework#Audio#Song}#GetEnumerator')
  - [System#Collections#IEnumerable#GetEnumerator()](#M-Ssit-Pixel-Framework-Audio-MusicPlaylist-System#Collections#IEnumerable#GetEnumerator 'Ssit.Pixel.Framework.Audio.MusicPlaylist.System#Collections#IEnumerable#GetEnumerator')
- [Rectangle](#T-Ssit-Pixel-Framework-Rectangle 'Ssit.Pixel.Framework.Rectangle')
  - [Height](#F-Ssit-Pixel-Framework-Rectangle-Height 'Ssit.Pixel.Framework.Rectangle.Height')
  - [Width](#F-Ssit-Pixel-Framework-Rectangle-Width 'Ssit.Pixel.Framework.Rectangle.Width')
  - [X](#F-Ssit-Pixel-Framework-Rectangle-X 'Ssit.Pixel.Framework.Rectangle.X')
  - [Y](#F-Ssit-Pixel-Framework-Rectangle-Y 'Ssit.Pixel.Framework.Rectangle.Y')
  - [Bottom](#P-Ssit-Pixel-Framework-Rectangle-Bottom 'Ssit.Pixel.Framework.Rectangle.Bottom')
  - [Center](#P-Ssit-Pixel-Framework-Rectangle-Center 'Ssit.Pixel.Framework.Rectangle.Center')
  - [IsEmpty](#P-Ssit-Pixel-Framework-Rectangle-IsEmpty 'Ssit.Pixel.Framework.Rectangle.IsEmpty')
  - [Right](#P-Ssit-Pixel-Framework-Rectangle-Right 'Ssit.Pixel.Framework.Rectangle.Right')
  - [Inflate(i)](#M-Ssit-Pixel-Framework-Rectangle-Inflate-System-Int32- 'Ssit.Pixel.Framework.Rectangle.Inflate(System.Int32)')
  - [Inflate(size)](#M-Ssit-Pixel-Framework-Rectangle-Inflate-Ssit-Pixel-Framework-Size- 'Ssit.Pixel.Framework.Rectangle.Inflate(Ssit.Pixel.Framework.Size)')
  - [Intersect(rectangle)](#M-Ssit-Pixel-Framework-Rectangle-Intersect-Ssit-Pixel-Framework-Rectangle- 'Ssit.Pixel.Framework.Rectangle.Intersect(Ssit.Pixel.Framework.Rectangle)')
  - [IsIntersecting(other)](#M-Ssit-Pixel-Framework-Rectangle-IsIntersecting-Ssit-Pixel-Framework-Rectangle- 'Ssit.Pixel.Framework.Rectangle.IsIntersecting(Ssit.Pixel.Framework.Rectangle)')
- [ResourceHandle\`1](#T-Ssit-Pixel-Framework-Content-ResourceHandle`1 'Ssit.Pixel.Framework.Content.ResourceHandle`1')
  - [Name](#P-Ssit-Pixel-Framework-Content-ResourceHandle`1-Name 'Ssit.Pixel.Framework.Content.ResourceHandle`1.Name')
  - [Resource](#P-Ssit-Pixel-Framework-Content-ResourceHandle`1-Resource 'Ssit.Pixel.Framework.Content.ResourceHandle`1.Resource')
  - [Dispose()](#M-Ssit-Pixel-Framework-Content-ResourceHandle`1-Dispose 'Ssit.Pixel.Framework.Content.ResourceHandle`1.Dispose')
- [RgbaColor](#T-Ssit-Pixel-Framework-RgbaColor 'Ssit.Pixel.Framework.RgbaColor')
- [Song](#T-Ssit-Pixel-Framework-Audio-Song 'Ssit.Pixel.Framework.Audio.Song')
  - [#ctor()](#M-Ssit-Pixel-Framework-Audio-Song-#ctor-System-String- 'Ssit.Pixel.Framework.Audio.Song.#ctor(System.String)')
  - [#ctor()](#M-Ssit-Pixel-Framework-Audio-Song-#ctor-System-ValueTuple{System-String,System-String}- 'Ssit.Pixel.Framework.Audio.Song.#ctor(System.ValueTuple{System.String,System.String})')
  - [#ctor()](#M-Ssit-Pixel-Framework-Audio-Song-#ctor-System-String,System-String- 'Ssit.Pixel.Framework.Audio.Song.#ctor(System.String,System.String)')
  - [Name](#P-Ssit-Pixel-Framework-Audio-Song-Name 'Ssit.Pixel.Framework.Audio.Song.Name')
  - [Path](#P-Ssit-Pixel-Framework-Audio-Song-Path 'Ssit.Pixel.Framework.Audio.Song.Path')
  - [op_Implicit(song)](#M-Ssit-Pixel-Framework-Audio-Song-op_Implicit-System-ValueTuple{System-String,System-String}-~Ssit-Pixel-Framework-Audio-Song 'Ssit.Pixel.Framework.Audio.Song.op_Implicit(System.ValueTuple{System.String,System.String})~Ssit.Pixel.Framework.Audio.Song')
  - [op_Implicit(path)](#M-Ssit-Pixel-Framework-Audio-Song-op_Implicit-System-String-~Ssit-Pixel-Framework-Audio-Song 'Ssit.Pixel.Framework.Audio.Song.op_Implicit(System.String)~Ssit.Pixel.Framework.Audio.Song')
- [SoundEmitter](#T-Ssit-Pixel-Framework-Audio-SoundEmitter 'Ssit.Pixel.Framework.Audio.SoundEmitter')
  - [#ctor()](#M-Ssit-Pixel-Framework-Audio-SoundEmitter-#ctor 'Ssit.Pixel.Framework.Audio.SoundEmitter.#ctor')
  - [Position](#P-Ssit-Pixel-Framework-Audio-SoundEmitter-Position 'Ssit.Pixel.Framework.Audio.SoundEmitter.Position')
  - [Velocity](#P-Ssit-Pixel-Framework-Audio-SoundEmitter-Velocity 'Ssit.Pixel.Framework.Audio.SoundEmitter.Velocity')
- [SoundListener](#T-Ssit-Pixel-Framework-Audio-SoundListener 'Ssit.Pixel.Framework.Audio.SoundListener')
  - [#ctor()](#M-Ssit-Pixel-Framework-Audio-SoundListener-#ctor 'Ssit.Pixel.Framework.Audio.SoundListener.#ctor')
  - [Direction](#P-Ssit-Pixel-Framework-Audio-SoundListener-Direction 'Ssit.Pixel.Framework.Audio.SoundListener.Direction')
  - [Position](#P-Ssit-Pixel-Framework-Audio-SoundListener-Position 'Ssit.Pixel.Framework.Audio.SoundListener.Position')
  - [Velocity](#P-Ssit-Pixel-Framework-Audio-SoundListener-Velocity 'Ssit.Pixel.Framework.Audio.SoundListener.Velocity')
- [SoundParameters](#T-Ssit-Pixel-Framework-Audio-SoundParameters 'Ssit.Pixel.Framework.Audio.SoundParameters')
  - [#ctor()](#M-Ssit-Pixel-Framework-Audio-SoundParameters-#ctor 'Ssit.Pixel.Framework.Audio.SoundParameters.#ctor')
  - [Pan](#P-Ssit-Pixel-Framework-Audio-SoundParameters-Pan 'Ssit.Pixel.Framework.Audio.SoundParameters.Pan')
  - [Pitch](#P-Ssit-Pixel-Framework-Audio-SoundParameters-Pitch 'Ssit.Pixel.Framework.Audio.SoundParameters.Pitch')
  - [Volume](#P-Ssit-Pixel-Framework-Audio-SoundParameters-Volume 'Ssit.Pixel.Framework.Audio.SoundParameters.Volume')
- [Vibration](#T-Ssit-Pixel-Framework-Input-Vibration 'Ssit.Pixel.Framework.Input.Vibration')

<a name='T-Ssit-Pixel-Framework-Input-ButtonState'></a>
## ButtonState `type`

##### Namespace

Ssit.Pixel.Framework.Input

##### Summary

Represents the state of a button, indicating whether it is pressed or not
and whether its state has changed.

<a name='M-Ssit-Pixel-Framework-Input-ButtonState-#ctor-System-Boolean,System-Boolean-'></a>
### #ctor() `constructor`

##### Summary

Represents the state of a button, indicating whether it is pressed or not
and whether its state has changed.

##### Parameters

This constructor has no parameters.

<a name='F-Ssit-Pixel-Framework-Input-ButtonState-Empty'></a>
### Empty `constants`

##### Summary

Represents default button state (unpressed and not changed in current pass).

<a name='F-Ssit-Pixel-Framework-Input-ButtonState-JustPressed'></a>
### JustPressed `constants`

##### Summary

Represents a state where the button has just been pressed.
This state indicates that the button is currently pressed down
and that there has been a state change.

<a name='F-Ssit-Pixel-Framework-Input-ButtonState-JustReleased'></a>
### JustReleased `constants`

##### Summary

Represents a button state where the button has just been released.
The button is not pressed down and its state has changed.

<a name='P-Ssit-Pixel-Framework-Input-ButtonState-IsChanged'></a>
### IsChanged `property`

##### Summary

Indicates whether the button state has changed in the current pass.

<a name='P-Ssit-Pixel-Framework-Input-ButtonState-IsDown'></a>
### IsDown `property`

##### Summary

Indicates whether the button is currently pressed.

<a name='T-Ssit-Pixel-Framework-Input-GameControllerAxis'></a>
## GameControllerAxis `type`

##### Namespace

Ssit.Pixel.Framework.Input

##### Summary

Defines the various axes available on a game controller.

<a name='F-Ssit-Pixel-Framework-Input-GameControllerAxis-DPadX'></a>
### DPadX `constants`

##### Summary

Represents the horizontal axis of the D-Pad on a game controller.

<a name='F-Ssit-Pixel-Framework-Input-GameControllerAxis-DPadY'></a>
### DPadY `constants`

##### Summary

Represents the vertical axis of the D-Pad on a game controller.

<a name='F-Ssit-Pixel-Framework-Input-GameControllerAxis-LeftTrigger'></a>
### LeftTrigger `constants`

##### Summary

Represents the axis corresponding to the left trigger of a game controller.
This axis is used to measure the analog input of the left trigger,
providing a value that ranges from 0 (not pressed) to 1 (fully pressed).

<a name='F-Ssit-Pixel-Framework-Input-GameControllerAxis-LeftY'></a>
### LeftY `constants`

##### Summary

Represents the vertical axis of the left analog stick on a game controller.

<a name='F-Ssit-Pixel-Framework-Input-GameControllerAxis-RightX'></a>
### RightX `constants`

##### Summary

Represents the horizontal axis of the right analog stick on a game controller.

<a name='F-Ssit-Pixel-Framework-Input-GameControllerAxis-RightY'></a>
### RightY `constants`

##### Summary

Represents the vertical of the right analog stick on a game controller.

<a name='T-Ssit-Pixel-Framework-Input-GameControllerButton'></a>
## GameControllerButton `type`

##### Namespace

Ssit.Pixel.Framework.Input

##### Summary

Indicates the movement of the right analog stick toward the left direction on a game controller.

<a name='F-Ssit-Pixel-Framework-Input-GameControllerButton-LeftStickDown'></a>
### LeftStickDown `constants`

##### Summary

Represents the state when the left stick of a game controller is pushed to the down.

<a name='F-Ssit-Pixel-Framework-Input-GameControllerButton-LeftStickLeft'></a>
### LeftStickLeft `constants`

##### Summary

Represents the state when the left stick of a game controller is pushed to the left.

<a name='F-Ssit-Pixel-Framework-Input-GameControllerButton-LeftStickRight'></a>
### LeftStickRight `constants`

##### Summary

Represents the state when the left stick of a game controller is pushed to the right.

<a name='F-Ssit-Pixel-Framework-Input-GameControllerButton-LeftStickUp'></a>
### LeftStickUp `constants`

##### Summary

Represents the state when the left stick of a game controller is pushed to the up.

<a name='F-Ssit-Pixel-Framework-Input-GameControllerButton-LeftTrigger'></a>
### LeftTrigger `constants`

##### Summary

Represents the state when the left trigger on a game controller is pushed.

<a name='F-Ssit-Pixel-Framework-Input-GameControllerButton-RightStickDown'></a>
### RightStickDown `constants`

##### Summary

Represents the state when the right stick of a game controller is pushed to the down.

<a name='F-Ssit-Pixel-Framework-Input-GameControllerButton-RightStickLeft'></a>
### RightStickLeft `constants`

##### Summary

Represents the state when the right stick of a game controller is pushed to the left.

<a name='F-Ssit-Pixel-Framework-Input-GameControllerButton-RightStickRight'></a>
### RightStickRight `constants`

##### Summary

Represents the state when the right stick of a game controller is pushed to the right.

<a name='F-Ssit-Pixel-Framework-Input-GameControllerButton-RightStickUp'></a>
### RightStickUp `constants`

##### Summary

Represents the state when the right stick of a game controller is pushed to the up.

<a name='F-Ssit-Pixel-Framework-Input-GameControllerButton-RightTrigger'></a>
### RightTrigger `constants`

##### Summary

Represents the state when the right trigger on a game controller is pushed.

<a name='T-Ssit-Pixel-Framework-Content-IContentManager'></a>
## IContentManager `type`

##### Namespace

Ssit.Pixel.Framework.Content

<a name='M-Ssit-Pixel-Framework-Content-IContentManager-Load``1-System-String-'></a>
### Load\`\`1(path) `method`

##### Summary

Loads/gets resource from given path amd returns handle to it.

##### Returns

Disposable handle to the resource instance.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Path to resource file. |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TResource | Type of resource to load. |

##### Remarks

Each handle must be disposed, when no longer in use. When all handles are disposed for a specific resource, it is unloaded and released.

<a name='M-Ssit-Pixel-Framework-Content-IContentManager-RegisterLoader``1-Ssit-Pixel-Framework-Content-LoadResourceDelegate-'></a>
### RegisterLoader\`\`1(loadFunc) `method`

##### Summary

Registers custom resource loader.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| loadFunc | [Ssit.Pixel.Framework.Content.LoadResourceDelegate](#T-Ssit-Pixel-Framework-Content-LoadResourceDelegate 'Ssit.Pixel.Framework.Content.LoadResourceDelegate') | Loading method delegate. |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TResource | Type of resource, which custom loading should be registered. |

<a name='T-Ssit-Pixel-Framework-IO-IFilesProvider'></a>
## IFilesProvider `type`

##### Namespace

Ssit.Pixel.Framework.IO

##### Summary

Provides methods to open files from various locations.

<a name='M-Ssit-Pixel-Framework-IO-IFilesProvider-Open-System-String-'></a>
### Open(path) `method`

##### Summary

Opens a file stream for the given path.

##### Returns

A stream to read the file.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path of the file to open. |

<a name='T-Ssit-Pixel-Framework-Graphics-IFontsManager'></a>
## IFontsManager `type`

##### Namespace

Ssit.Pixel.Framework.Graphics

##### Summary

Service managing fonts in the application.

<a name='P-Ssit-Pixel-Framework-Graphics-IFontsManager-Item-System-String-'></a>
### Item `property`

##### Summary

Gets font with a given name or default.

<a name='M-Ssit-Pixel-Framework-Graphics-IFontsManager-LoadFonts-System-IO-Stream-'></a>
### LoadFonts(jsonStream) `method`

##### Summary

Loads font definitions from a JSON stream and initializes the fonts' collection.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| jsonStream | [System.IO.Stream](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IO.Stream 'System.IO.Stream') | A stream containing the JSON data describing the fonts. |

<a name='T-Ssit-Pixel-Framework-Input-IGameControllers'></a>
## IGameControllers `type`

##### Namespace

Ssit.Pixel.Framework.Input

##### Summary

Service for game controller input handling.

<a name='P-Ssit-Pixel-Framework-Input-IGameControllers-VibrationForce'></a>
### VibrationForce `property`

##### Summary

Gets or sets the vibration force for the game controllers.

##### Remarks

The vibration force is represented as a byte value.

<a name='M-Ssit-Pixel-Framework-Input-IGameControllers-GetAxis-System-Int32,Ssit-Pixel-Framework-Input-GameControllerAxis-'></a>
### GetAxis(player,axis) `method`

##### Summary

Retrieves the value of the specified axis for the given player.

##### Returns

A float representing the axis value, ranging from -1.0 to 1.0.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| player | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The player index to get the axis value for. |
| axis | [Ssit.Pixel.Framework.Input.GameControllerAxis](#T-Ssit-Pixel-Framework-Input-GameControllerAxis 'Ssit.Pixel.Framework.Input.GameControllerAxis') | The specific axis to retrieve the value from. |

<a name='M-Ssit-Pixel-Framework-Input-IGameControllers-GetButton-System-Int32,Ssit-Pixel-Framework-Input-GameControllerButton-'></a>
### GetButton(player,button) `method`

##### Summary

Gets the state of a specified game controller button for a given player.

##### Returns

The state of the specified game controller button.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| player | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The player index (e.g., 0 for the first player). |
| button | [Ssit.Pixel.Framework.Input.GameControllerButton](#T-Ssit-Pixel-Framework-Input-GameControllerButton 'Ssit.Pixel.Framework.Input.GameControllerButton') | The game controller button whose state is to be retrieved. |

<a name='T-Ssit-Pixel-Framework-Input-IInputMapping'></a>
## IInputMapping `type`

##### Namespace

Ssit.Pixel.Framework.Input

##### Summary

Interface representing the mapping of input controls.

<a name='M-Ssit-Pixel-Framework-Input-IInputMapping-GetAxis-System-String-'></a>
### GetAxis(axis) `method`

##### Summary

Retrieves the value of the specified axis for the mapped input controls.

##### Returns

The value of the axis, ranging from -1 to 1.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| axis | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the axis to retrieve the value for. |

<a name='M-Ssit-Pixel-Framework-Input-IInputMapping-GetButton-System-String-'></a>
### GetButton(button) `method`

##### Summary

Gets the current state of the specified button.

##### Returns

A [ButtonState](#T-Ssit-Pixel-Framework-Input-ButtonState 'Ssit.Pixel.Framework.Input.ButtonState') representing the current state of the button.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| button | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the button for which to get the state. |

<a name='T-Ssit-Pixel-Framework-Input-IInputMappings'></a>
## IInputMappings `type`

##### Namespace

Ssit.Pixel.Framework.Input

##### Summary

Service managing input mappings for multiple players.

<a name='P-Ssit-Pixel-Framework-Input-IInputMappings-Item-System-Int32-'></a>
### Item `property`

##### Summary

Gets input mapping for a specified player.

<a name='M-Ssit-Pixel-Framework-Input-IInputMappings-Mapper-System-Int32-'></a>
### Mapper(player) `method`

##### Summary

Retrieves the IMapper instance for a specified player.
Used to define input mappings for a player.

##### Returns

Returns an IMapper instance for the specified player.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| player | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The player index for which to get the IMapper instance. |

<a name='T-Ssit-Pixel-Framework-Input-IKeyboard'></a>
## IKeyboard `type`

##### Namespace

Ssit.Pixel.Framework.Input

##### Summary

Provides methods to query the state of keys on a keyboard.

<a name='M-Ssit-Pixel-Framework-Input-IKeyboard-GetKey-Ssit-Pixel-Framework-Input-Key-'></a>
### GetKey(key) `method`

##### Summary

Retrieves the current state of the specified key.

##### Returns

A [ButtonState](#T-Ssit-Pixel-Framework-Input-ButtonState 'Ssit.Pixel.Framework.Input.ButtonState') indicating the current state of the key.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [Ssit.Pixel.Framework.Input.Key](#T-Ssit-Pixel-Framework-Input-Key 'Ssit.Pixel.Framework.Input.Key') | The key whose state is to be retrieved. |

<a name='T-Ssit-Pixel-Framework-Input-IMapper'></a>
## IMapper `type`

##### Namespace

Ssit.Pixel.Framework.Input

##### Summary

Defines methods for mapping game controller axes and buttons,
as well as keyboard keys, to specific actions or inputs.

<a name='M-Ssit-Pixel-Framework-Input-IMapper-Clear'></a>
### Clear() `method`

##### Summary

Clears all the current input mappings, including axis and button mappings.

##### Returns

The instance of IMapper for chaining further method calls.

##### Parameters

This method has no parameters.

<a name='M-Ssit-Pixel-Framework-Input-IMapper-MapAxis-System-String,Ssit-Pixel-Framework-Input-GameControllerAxis-'></a>
### MapAxis(axisName,axis) `method`

##### Summary

Maps a game controller axis to a specified axis name.

##### Returns

The [IMapper](#T-Ssit-Pixel-Framework-Input-IMapper 'Ssit.Pixel.Framework.Input.IMapper') instance for chaining additional mappings.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| axisName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the axis to map. |
| axis | [Ssit.Pixel.Framework.Input.GameControllerAxis](#T-Ssit-Pixel-Framework-Input-GameControllerAxis 'Ssit.Pixel.Framework.Input.GameControllerAxis') | The [GameControllerAxis](#T-Ssit-Pixel-Framework-Input-GameControllerAxis 'Ssit.Pixel.Framework.Input.GameControllerAxis') to map to the axis name. |

<a name='M-Ssit-Pixel-Framework-Input-IMapper-MapAxis-System-String,Ssit-Pixel-Framework-Input-GameControllerButton,Ssit-Pixel-Framework-Input-GameControllerButton-'></a>
### MapAxis(axisName,negative,positive) `method`

##### Summary

Maps a pair of game controller buttons to an axis, allowing the axis to be controlled by the negative and positive buttons.

##### Returns

An IMapper instance to chain further mapping calls.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| axisName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the axis to map. |
| negative | [Ssit.Pixel.Framework.Input.GameControllerButton](#T-Ssit-Pixel-Framework-Input-GameControllerButton 'Ssit.Pixel.Framework.Input.GameControllerButton') | The button that represents the negative direction of the axis. |
| positive | [Ssit.Pixel.Framework.Input.GameControllerButton](#T-Ssit-Pixel-Framework-Input-GameControllerButton 'Ssit.Pixel.Framework.Input.GameControllerButton') | The button that represents the positive direction of the axis. |

<a name='M-Ssit-Pixel-Framework-Input-IMapper-MapAxis-System-String,Ssit-Pixel-Framework-Input-Key,Ssit-Pixel-Framework-Input-Key-'></a>
### MapAxis(axisName,negative,positive) `method`

##### Summary

Maps an axis to a pair of keyboard keys to act as the negative and positive values of the axis.

##### Returns

An instance of the [IMapper](#T-Ssit-Pixel-Framework-Input-IMapper 'Ssit.Pixel.Framework.Input.IMapper') interface for method chaining.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| axisName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the axis to map. |
| negative | [Ssit.Pixel.Framework.Input.Key](#T-Ssit-Pixel-Framework-Input-Key 'Ssit.Pixel.Framework.Input.Key') | The key representing the negative direction of the axis. |
| positive | [Ssit.Pixel.Framework.Input.Key](#T-Ssit-Pixel-Framework-Input-Key 'Ssit.Pixel.Framework.Input.Key') | The key representing the positive direction of the axis. |

<a name='M-Ssit-Pixel-Framework-Input-IMapper-MapButton-System-String,Ssit-Pixel-Framework-Input-GameControllerButton-'></a>
### MapButton(buttonName,button) `method`

##### Summary

Maps a game controller button to a given button name.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| buttonName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name to map to the game controller button. |
| button | [Ssit.Pixel.Framework.Input.GameControllerButton](#T-Ssit-Pixel-Framework-Input-GameControllerButton 'Ssit.Pixel.Framework.Input.GameControllerButton') | The game controller button to map. |

<a name='M-Ssit-Pixel-Framework-Input-IMapper-MapButton-System-String,Ssit-Pixel-Framework-Input-Key-'></a>
### MapButton(buttonName,key) `method`

##### Summary

Maps a specified key to a button action on the input controller.

##### Returns

The current instance of [IMapper](#T-Ssit-Pixel-Framework-Input-IMapper 'Ssit.Pixel.Framework.Input.IMapper') to allow for method chaining.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| buttonName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the button action to map. |
| key | [Ssit.Pixel.Framework.Input.Key](#T-Ssit-Pixel-Framework-Input-Key 'Ssit.Pixel.Framework.Input.Key') | The key to map to the button action. |

<a name='T-Ssit-Pixel-Framework-Audio-IMusicPlayer'></a>
## IMusicPlayer `type`

##### Namespace

Ssit.Pixel.Framework.Audio

##### Summary

Defines methods for controlling music playback and managing playlists.

<a name='M-Ssit-Pixel-Framework-Audio-IMusicPlayer-ChangePlaylist-System-String,System-Int32,System-Boolean-'></a>
### ChangePlaylist(name,fadeTimeMs,resetProgress) `method`

##### Summary

Changes the current music playlist to the specified one.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the playlist to switch to. |
| fadeTimeMs | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The time it takes to fade out the current playlist and fade in the new one, in milliseconds. |
| resetProgress | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Specifies whether to reset the new playlist's progress to the start. |

<a name='M-Ssit-Pixel-Framework-Audio-IMusicPlayer-NextTrack-System-Int32,System-Boolean-'></a>
### NextTrack(fadeTimeMs,loop) `method`

##### Summary

Advances to the next track in the current playlist.

##### Returns

Returns true if the operation was successful; otherwise, false.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fadeTimeMs | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The time in milliseconds to fade out the current track and fade in the next track. |
| loop | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | A boolean indicating whether to loop back to the start of the playlist when the end is reached. |

<a name='M-Ssit-Pixel-Framework-Audio-IMusicPlayer-PreviousTrack-System-Int32,System-Boolean-'></a>
### PreviousTrack(fadeTimeMs,loop) `method`

##### Summary

Plays the previous track in the current playlist.

##### Returns

True if the previous track was successfully selected; otherwise, false.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| fadeTimeMs | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The time in milliseconds over which to fade the track in and out. |
| loop | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Indicates whether the playlist should loop to the last track when reaching the beginning. |

<a name='M-Ssit-Pixel-Framework-Audio-IMusicPlayer-RegisterPlaylist-System-String,Ssit-Pixel-Framework-Audio-MusicPlaylist-'></a>
### RegisterPlaylist(name,playlist) `method`

##### Summary

Registers a new music playlist with the specified name.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the playlist to register. |
| playlist | [Ssit.Pixel.Framework.Audio.MusicPlaylist](#T-Ssit-Pixel-Framework-Audio-MusicPlaylist 'Ssit.Pixel.Framework.Audio.MusicPlaylist') | The playlist object containing the songs. |

<a name='T-Ssit-Pixel-Framework-Graphics-IRenderTarget'></a>
## IRenderTarget `type`

##### Namespace

Ssit.Pixel.Framework.Graphics

##### Summary

Represents a render target texture.

<a name='T-Ssit-Pixel-Framework-Audio-ISoundEffect'></a>
## ISoundEffect `type`

##### Namespace

Ssit.Pixel.Framework.Audio

##### Summary

Defines the interface for a sound effect. Provides methods to create
instances of the sound effect and play the sound effect.

<a name='M-Ssit-Pixel-Framework-Audio-ISoundEffect-CreateInstance'></a>
### CreateInstance() `method`

##### Summary

Creates a new instance of the sound effect.

##### Returns

New sound effect instance object.

##### Parameters

This method has no parameters.

<a name='M-Ssit-Pixel-Framework-Audio-ISoundEffect-PlayOnce-System-Single-'></a>
### PlayOnce(volume) `method`

##### Summary

Plays the sound effect once with the specified volume.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| volume | [System.Single](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Single 'System.Single') | The volume at which the sound should be played. The default value is 1.0f. |

<a name='T-Ssit-Pixel-Framework-Audio-ISoundEffectInstance'></a>
## ISoundEffectInstance `type`

##### Namespace

Ssit.Pixel.Framework.Audio

##### Summary

Represents an instance of a sound effect, supporting operations like play, stop, pause, and resume.

<a name='P-Ssit-Pixel-Framework-Audio-ISoundEffectInstance-Emitter'></a>
### Emitter `property`

##### Summary

Gets or sets the SoundEmitter associated with a sound effect instance.
The Emitter controls the position and velocity of the sound in a 3D space,
allowing for spatial audio effects.

<a name='P-Ssit-Pixel-Framework-Audio-ISoundEffectInstance-Parameters'></a>
### Parameters `property`

##### Summary

Gets or sets the sound parameters associated with the sound effect instance.

##### Remarks

These parameters define various properties of the sound effect such as volume, pitch, etc.

<a name='M-Ssit-Pixel-Framework-Audio-ISoundEffectInstance-Pause'></a>
### Pause() `method`

##### Summary

Pauses the sound effect instance.

##### Parameters

This method has no parameters.

<a name='M-Ssit-Pixel-Framework-Audio-ISoundEffectInstance-Play'></a>
### Play() `method`

##### Summary

Begins playback of the sound effect instance.

##### Parameters

This method has no parameters.

<a name='M-Ssit-Pixel-Framework-Audio-ISoundEffectInstance-Resume'></a>
### Resume() `method`

##### Summary

Resumes a paused sound effect instance.

##### Parameters

This method has no parameters.

<a name='M-Ssit-Pixel-Framework-Audio-ISoundEffectInstance-Stop'></a>
### Stop() `method`

##### Summary

Stops the playback of the sound effect instance that is currently playing.

##### Parameters

This method has no parameters.

<a name='T-Ssit-Pixel-Framework-Audio-ISoundManager'></a>
## ISoundManager `type`

##### Namespace

Ssit.Pixel.Framework.Audio

##### Summary

Defines the interface for managing sound within the framework.

<a name='P-Ssit-Pixel-Framework-Audio-ISoundManager-Listener'></a>
### Listener `property`

##### Summary

Gets or sets the primary sound listener.
This represents the listener object with its position, velocity and direction.

<a name='P-Ssit-Pixel-Framework-Audio-ISoundManager-MasterVolume'></a>
### MasterVolume `property`

##### Summary

Gets or sets the master volume level for the sound.

##### Remarks

The master volume is a value between 0.0 and 1.0, where 0.0 means no sound and 1.0 means full volume.
Changes to this property will affect the overall volume of all sounds in game/app.

<a name='T-Ssit-Pixel-Framework-Graphics-ITexture'></a>
## ITexture `type`

##### Namespace

Ssit.Pixel.Framework.Graphics

##### Summary

Represents a texture in the graphics framework.

<a name='P-Ssit-Pixel-Framework-Graphics-ITexture-Size'></a>
### Size `property`

##### Summary

Gets the size of the texture as a [Size](#T-Ssit-Pixel-Framework-Size 'Ssit.Pixel.Framework.Size') structure.

<a name='T-Ssit-Pixel-Framework-Utils-ImagesLoader'></a>
## ImagesLoader `type`

##### Namespace

Ssit.Pixel.Framework.Utils

##### Summary

Provides functionality to load images and extract pixel data.

<a name='M-Ssit-Pixel-Framework-Utils-ImagesLoader-LoadImage-System-IO-Stream-'></a>
### LoadImage(stream) `method`

##### Summary

Loads an image from a stream and extracts its pixel data into an array.

##### Returns

A 2-dimensional array of RgbaColor representing the pixel data of the image.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| stream | [System.IO.Stream](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IO.Stream 'System.IO.Stream') | The stream containing the image data. |

<a name='T-Ssit-Pixel-Framework-Core-IoCRegistrar'></a>
## IoCRegistrar `type`

##### Namespace

Ssit.Pixel.Framework.Core

<a name='M-Ssit-Pixel-Framework-Core-IoCRegistrar-WithPixelCore-Ssit-Utils-IoC-IIoCContainerBuilder-'></a>
### WithPixelCore(builder) `method`

##### Summary

Registers the essential Pixel Core components with the IoC Container Builder.

##### Returns

The IoC container builder with registered Pixel Core components.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| builder | [Ssit.Utils.IoC.IIoCContainerBuilder](#T-Ssit-Utils-IoC-IIoCContainerBuilder 'Ssit.Utils.IoC.IIoCContainerBuilder') | The IoC container builder used for registering dependencies. |

<a name='T-Ssit-Pixel-Framework-Input-Key'></a>
## Key `type`

##### Namespace

Ssit.Pixel.Framework.Input

##### Summary

Defines keys that can be used in the input system.

<a name='T-Ssit-Pixel-Framework-Content-LoadResourceDelegate'></a>
## LoadResourceDelegate `type`

##### Namespace

Ssit.Pixel.Framework.Content

##### Summary

Delegate to method, which loads resource from given path.

<a name='T-Ssit-Pixel-Framework-Audio-MusicPlaylist'></a>
## MusicPlaylist `type`

##### Namespace

Ssit.Pixel.Framework.Audio

##### Summary

Represents a music playlist that holds a collection of songs and provides functionality to manage them.

<a name='F-Ssit-Pixel-Framework-Audio-MusicPlaylist-_playlist'></a>
### _playlist `constants`

##### Summary

Represents the list of songs in the music playlist.

<a name='P-Ssit-Pixel-Framework-Audio-MusicPlaylist-CurrentPosition'></a>
### CurrentPosition `property`

##### Summary

Gets or sets the current position in milliseconds of the song being played in the playlist.

<a name='P-Ssit-Pixel-Framework-Audio-MusicPlaylist-CurrentSong'></a>
### CurrentSong `property`

##### Summary

Gets or sets the index of the currently playing song in the playlist.

<a name='P-Ssit-Pixel-Framework-Audio-MusicPlaylist-List'></a>
### List `property`

##### Summary

Gets a read-only list of songs in the music playlist.

<a name='M-Ssit-Pixel-Framework-Audio-MusicPlaylist-Add-Ssit-Pixel-Framework-Audio-Song-'></a>
### Add(song) `method`

##### Summary

Adds the specified song to the playlist.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| song | [Ssit.Pixel.Framework.Audio.Song](#T-Ssit-Pixel-Framework-Audio-Song 'Ssit.Pixel.Framework.Audio.Song') | The song to add to the playlist. |

<a name='M-Ssit-Pixel-Framework-Audio-MusicPlaylist-System#Collections#Generic#IEnumerable{Ssit#Pixel#Framework#Audio#Song}#GetEnumerator'></a>
### System#Collections#Generic#IEnumerable{Ssit#Pixel#Framework#Audio#Song}#GetEnumerator() `method`

##### Summary

Returns an enumerator that iterates through the playlist.

##### Returns

An enumerator for the playlist.

##### Parameters

This method has no parameters.

<a name='M-Ssit-Pixel-Framework-Audio-MusicPlaylist-System#Collections#IEnumerable#GetEnumerator'></a>
### System#Collections#IEnumerable#GetEnumerator() `method`

##### Summary

Returns an enumerator that iterates through the MusicPlaylist.

##### Returns

An enumerator that can be used to iterate through the MusicPlaylist.

##### Parameters

This method has no parameters.

<a name='T-Ssit-Pixel-Framework-Rectangle'></a>
## Rectangle `type`

##### Namespace

Ssit.Pixel.Framework

##### Summary

Represents a rectangle defined by its position and size.

<a name='F-Ssit-Pixel-Framework-Rectangle-Height'></a>
### Height `constants`

##### Summary

Gets the height of the rectangle.

<a name='F-Ssit-Pixel-Framework-Rectangle-Width'></a>
### Width `constants`

##### Summary

Gets the width of the rectangle.

<a name='F-Ssit-Pixel-Framework-Rectangle-X'></a>
### X `constants`

##### Summary

Gets the X coordinate of the rectangle's top-left corner.

<a name='F-Ssit-Pixel-Framework-Rectangle-Y'></a>
### Y `constants`

##### Summary

The Y-coordinate of the top-left corner of the rectangle.

<a name='P-Ssit-Pixel-Framework-Rectangle-Bottom'></a>
### Bottom `property`

##### Summary

Gets the Y coordinate of the rectangle's bottom edge.

<a name='P-Ssit-Pixel-Framework-Rectangle-Center'></a>
### Center `property`

##### Summary

Gets the coordinates of the center of the rectangle.

<a name='P-Ssit-Pixel-Framework-Rectangle-IsEmpty'></a>
### IsEmpty `property`

##### Summary

Gets a value indicating whether the rectangle is empty (has zero width or height).

<a name='P-Ssit-Pixel-Framework-Rectangle-Right'></a>
### Right `property`

##### Summary

Gets the X coordinate of the rectangle's right edge.

<a name='M-Ssit-Pixel-Framework-Rectangle-Inflate-System-Int32-'></a>
### Inflate(i) `method`

##### Summary

Expands or contracts the size of this rectangle by the specified amount.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| i | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The amount by which to inflate the rectangle on each side. |

<a name='M-Ssit-Pixel-Framework-Rectangle-Inflate-Ssit-Pixel-Framework-Size-'></a>
### Inflate(size) `method`

##### Summary

Expands or contracts the rectangle by the specified size.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| size | [Ssit.Pixel.Framework.Size](#T-Ssit-Pixel-Framework-Size 'Ssit.Pixel.Framework.Size') | The size by which to inflate the rectangle. |

<a name='M-Ssit-Pixel-Framework-Rectangle-Intersect-Ssit-Pixel-Framework-Rectangle-'></a>
### Intersect(rectangle) `method`

##### Summary

Computes the intersection of this rectangle with another rectangle.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| rectangle | [Ssit.Pixel.Framework.Rectangle](#T-Ssit-Pixel-Framework-Rectangle 'Ssit.Pixel.Framework.Rectangle') | The rectangle to intersect with. |

<a name='M-Ssit-Pixel-Framework-Rectangle-IsIntersecting-Ssit-Pixel-Framework-Rectangle-'></a>
### IsIntersecting(other) `method`

##### Summary

Determines whether this rectangle intersects with another rectangle.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| other | [Ssit.Pixel.Framework.Rectangle](#T-Ssit-Pixel-Framework-Rectangle 'Ssit.Pixel.Framework.Rectangle') | The rectangle to check for intersection with. |

<a name='T-Ssit-Pixel-Framework-Content-ResourceHandle`1'></a>
## ResourceHandle\`1 `type`

##### Namespace

Ssit.Pixel.Framework.Content

##### Summary

Represents disposable handle to the resource.

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TResource | Type of the underlying resource. |

<a name='P-Ssit-Pixel-Framework-Content-ResourceHandle`1-Name'></a>
### Name `property`

##### Summary

Resource name.

<a name='P-Ssit-Pixel-Framework-Content-ResourceHandle`1-Resource'></a>
### Resource `property`

##### Summary

Resource reference.

<a name='M-Ssit-Pixel-Framework-Content-ResourceHandle`1-Dispose'></a>
### Dispose() `method`

##### Summary

Disposes resource handle.

##### Parameters

This method has no parameters.

<a name='T-Ssit-Pixel-Framework-RgbaColor'></a>
## RgbaColor `type`

##### Namespace

Ssit.Pixel.Framework

##### Summary

Represents a color in the RGBA (Red, Green, Blue, Alpha) color space.

<a name='T-Ssit-Pixel-Framework-Audio-Song'></a>
## Song `type`

##### Namespace

Ssit.Pixel.Framework.Audio

##### Summary

Represents a song with a name and a file path.

<a name='M-Ssit-Pixel-Framework-Audio-Song-#ctor-System-String-'></a>
### #ctor() `constructor`

##### Summary

Constructs a song with a specific file path.

##### Parameters

This constructor has no parameters.

<a name='M-Ssit-Pixel-Framework-Audio-Song-#ctor-System-ValueTuple{System-String,System-String}-'></a>
### #ctor() `constructor`

##### Summary

Constructs a song with a name and file path.

##### Parameters

This constructor has no parameters.

<a name='M-Ssit-Pixel-Framework-Audio-Song-#ctor-System-String,System-String-'></a>
### #ctor() `constructor`

##### Summary

Constructs a song with a name and file path.

##### Parameters

This constructor has no parameters.

<a name='P-Ssit-Pixel-Framework-Audio-Song-Name'></a>
### Name `property`

##### Summary

Gets the name of the song.

<a name='P-Ssit-Pixel-Framework-Audio-Song-Path'></a>
### Path `property`

##### Summary

Gets the file path of the song.

<a name='M-Ssit-Pixel-Framework-Audio-Song-op_Implicit-System-ValueTuple{System-String,System-String}-~Ssit-Pixel-Framework-Audio-Song'></a>
### op_Implicit(song) `method`

##### Summary

Implicitly converts a tuple containing song name and path into a Song object.

##### Returns

A Song object created from the provided tuple.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| song | [System.ValueTuple{System.String,System.String})~Ssit.Pixel.Framework.Audio.Song](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ValueTuple 'System.ValueTuple{System.String,System.String})~Ssit.Pixel.Framework.Audio.Song') | A tuple containing the name and path of the song. |

<a name='M-Ssit-Pixel-Framework-Audio-Song-op_Implicit-System-String-~Ssit-Pixel-Framework-Audio-Song'></a>
### op_Implicit(path) `method`

##### Summary

Implicitly converts a song path into a Song object.

##### Returns

A Song object created from the provided path.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [System.String)~Ssit.Pixel.Framework.Audio.Song](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String)~Ssit.Pixel.Framework.Audio.Song 'System.String)~Ssit.Pixel.Framework.Audio.Song') | A path of the song. |

<a name='T-Ssit-Pixel-Framework-Audio-SoundEmitter'></a>
## SoundEmitter `type`

##### Namespace

Ssit.Pixel.Framework.Audio

##### Summary

The SoundEmitter struct is used to represent the position and velocity of a sound source in 3D space.
It is primarily used in conjunction with sound effect instances to provide spatial audio effects.

<a name='M-Ssit-Pixel-Framework-Audio-SoundEmitter-#ctor'></a>
### #ctor() `constructor`

##### Parameters

This constructor has no parameters.

<a name='P-Ssit-Pixel-Framework-Audio-SoundEmitter-Position'></a>
### Position `property`

##### Summary

Gets or sets the position of the sound emitter in 3D space.

<a name='P-Ssit-Pixel-Framework-Audio-SoundEmitter-Velocity'></a>
### Velocity `property`

##### Summary

Gets or sets the velocity of the sound emitter.
The velocity determines the speed and direction at which the emitter is moving.

<a name='T-Ssit-Pixel-Framework-Audio-SoundListener'></a>
## SoundListener `type`

##### Namespace

Ssit.Pixel.Framework.Audio

##### Summary

Represents a sound listener within the audio framework.
This struct is used to manage and interact with sound sources in 3D space.

<a name='M-Ssit-Pixel-Framework-Audio-SoundListener-#ctor'></a>
### #ctor() `constructor`

##### Summary

Represents a sound listener in the audio framework.

##### Parameters

This constructor has no parameters.

<a name='P-Ssit-Pixel-Framework-Audio-SoundListener-Direction'></a>
### Direction `property`

##### Summary

Gets or sets the direction the listener is facing.
The direction is represented as a Vector3 where the default value is (0, 0, -1), indicating forward.

<a name='P-Ssit-Pixel-Framework-Audio-SoundListener-Position'></a>
### Position `property`

##### Summary

Gets or sets the position of the sound listener in 3D space.
The default position is initialized to (0, 0, 10).

<a name='P-Ssit-Pixel-Framework-Audio-SoundListener-Velocity'></a>
### Velocity `property`

##### Summary

Represents the velocity of the sound listener in 3D space.

<a name='T-Ssit-Pixel-Framework-Audio-SoundParameters'></a>
## SoundParameters `type`

##### Namespace

Ssit.Pixel.Framework.Audio

##### Summary

SoundParameters struct holds various properties and parameters related to sound effects.
It is used in conjunction with sound emitting structures and interfaces to provide detailed control over sound playback characteristics.

<a name='M-Ssit-Pixel-Framework-Audio-SoundParameters-#ctor'></a>
### #ctor() `constructor`

##### Summary

Represents the parameters related to sound configuration.

##### Parameters

This constructor has no parameters.

<a name='P-Ssit-Pixel-Framework-Audio-SoundParameters-Pan'></a>
### Pan `property`

##### Summary

Gets or sets the panning value of the sound.
The panning value ranges from -1.0 (full left) to 1.0 (full right),
with 0.0 representing the center (default).

<a name='P-Ssit-Pixel-Framework-Audio-SoundParameters-Pitch'></a>
### Pitch `property`

##### Summary

Gets or sets the pitch of the sound.
The pitch determines how high or low the sound is perceived.
A value of 1.0 represents the original pitch, values greater than 1.0 increase the pitch, and values less than 1.0 decrease the pitch.

<a name='P-Ssit-Pixel-Framework-Audio-SoundParameters-Volume'></a>
### Volume `property`

##### Summary

Gets or sets the volume level for the sound.
The volume is a float value where 0 represents silence and 1 represents the maximum volume.
Default value is 1.

<a name='T-Ssit-Pixel-Framework-Input-Vibration'></a>
## Vibration `type`

##### Namespace

Ssit.Pixel.Framework.Input

##### Summary

Defines the levels of vibration intensity for a game controller.
