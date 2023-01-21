; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Typing game"
#define MyAppVersion "1.0"
#define MyAppPublisher "Gabriel Kaszewski"
#define MyAppURL "https://gabrielkaszewski.pl/"
#define MyAppExeName "typing-game.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{F4EBDBB2-FF4B-4057-BF52-DE494BAD09D7}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=commandline
OutputDir=E:\Dev\Prototypes\typing-game\Builds\Windows 64 bit - Copy
OutputBaseFilename=Typing Game Installer
SetupIconFile=E:\Dev\Content\Typing Game Icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "E:\Dev\Prototypes\typing-game\Builds\Windows 64 bit - Copy\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Prototypes\typing-game\Builds\Windows 64 bit - Copy\typing-game.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Prototypes\typing-game\Builds\Windows 64 bit - Copy\UnityCrashHandler64.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Prototypes\typing-game\Builds\Windows 64 bit - Copy\UnityPlayer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\Dev\Prototypes\typing-game\Builds\Windows 64 bit - Copy\MonoBleedingEdge\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "E:\Dev\Prototypes\typing-game\Builds\Windows 64 bit - Copy\typing-game_Data\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
