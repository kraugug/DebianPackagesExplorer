;
; Copyright(C) 2019 Michal Heczko
; All rights reserved.
;
; This software may be modified and distributed under the terms
; of the BSD license.  See the LICENSE file for details.
;
#define AppId "{F695E440-6561-457C-A81E-F1EEA1372FBC}"
#define AppName "Debian Packages Explorer"
#define AppVersion "1.0.0.0"  
#define AppPublisher "Michal Heczko"
#define AppURL "http://dpe.kraugug.net"
#define AppExeName "DebianPackagesExplorer.exe"

#define PathBinary SourcePath + "\DebianPackagesExplorer\Bin\Release\"

[Setup]
AppId={{#AppId}
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
AppSupportURL={#AppURL}
AppUpdatesURL={#AppURL}
ArchitecturesInstallIn64BitMode=x64
CloseApplications=yes
Compression=lzma
DefaultDirName={pf}\{#AppName}
DefaultGroupName={#AppName}
DisableProgramGroupPage=yes
InfoBeforeFile={#SourcePath}\Changelog.txt
LicenseFile={#SourcePath}\LICENSE
OutputDir=.\
OutputBaseFilename={#AppName} {#AppVersion}
SetupIconFile={#SourcePath}\DebianPackagesExplorer\DebianPackagesExplorer.ico
SolidCompression=yes
Uninstallable=yes
UninstallDisplayIcon={#SourcePath}\DebianPackagesExplorer\DebianPackagesExplorer.ico

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}";

[Components]
Name: "src"; Description: "Source Codes";

[Files]
; Files...
Source: "{#PathBinary}\*"; DestDir: "{app}\Sources\TrayRadio"; Components: "src"; Flags: recursesubdirs; Excludes: "*.pdb";
Source: "{#SourcePath}\Changelog.txt"; DestDir: "{app}";
Source: "{#SourcePath}\LICENSE"; DestDir: "{app}"; DestName: "License.txt";

[Icons]
Name: {group}\{#AppName}; Filename: "{app}\{#AppExeName}"; Flags: foldershortcut;
Name: {group}\Changelog; Filename: {app}\Changelog.txt
Name: {group}\License; Filename: {app}\License.txt
Name: {group}\{cm:UninstallProgram,{#AppName} v{#AppVersion}}; Filename: {uninstallexe}
Name: {commondesktop}\{#AppName}; Filename: "{app}\{#AppExeName}"; Tasks: desktopicon;

[Registry]
Root: HKCU; SubKey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueName: {#AppName}; Flags: dontcreatekey uninsdeletevalue;

[Run]
Filename: "{app}\{#AppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(AppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent;

[UninstallDelete]
Type: dirifempty; Name: "{group}";
Type: filesandordirs; Name: "{app}";
Type: filesandordirs; Name: "{localappdata}\Michal_Heczko";

[Code]
function InitializeSetup: Boolean;
var
	Uninstaller: String;
	ErrorCode: Integer;
begin
    Result := not RegQueryStringValue(HKLM, ExpandConstant('SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{{#AppId}_is1\'), 'UninstallString', Uninstaller);
	if not Result then
		if (MsgBox(ExpandConstant('{#AppName} is already installed, you must uninstall it first! Do you wish to uninstall it now?'), mbConfirmation, MB_YESNO) = IDYES) then begin
			Result := ShellExec('open', Uninstaller, '/SILENT', '', SW_SHOW, ewWaitUntilTerminated, ErrorCode)
			if not Result then
				MsgBox(SysErrorMessage(ErrorCode), mbError, MB_OK)
		end else
			Result := False;
end;

procedure RegisterExtraCloseApplicationsResources;
begin
	RegisterExtraCloseApplicationsResource(true, ExpandConstant('{app}\DebianPackagesExplorer.exe'));
end;
