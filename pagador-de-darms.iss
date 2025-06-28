[Setup]
AppName=Pagador de DARMs
AppVersion=1.0
DefaultDirName={pf}\PagadorDeDARMs
DefaultGroupName=Pagador de DARMs
OutputDir=.
OutputBaseFilename=PagadorDeDARMs-Setup
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64

[Files]
Source: "bin\Release\net8.0-windows\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Pagador de DARMs"; Filename: "{app}\pagador-de-darms.exe"
Name: "{commondesktop}\Pagador de DARMs"; Filename: "{app}\pagador-de-darms.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Criar atalho na área de trabalho"; GroupDescription: "Opções adicionais:" 