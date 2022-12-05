# Get-AudioDevice -ID "{0.0.0.00000000}.{95312096-1706-4fa7-8aa9-3be0c957a2f9}"
# Get-AudioDevice -List
clear
Write-Host "";
Write-Host -ForegroundColor DarkYellow "--------------------------------------------------------------------------------";

$scarletMic = Set-AudioDevice -ID "{0.0.1.00000000}.{5ac24442-ee7e-41e9-b55f-29f3de793059}"; #Microphone (3- Scarlett Solo USB)
Set-AudioDevice -RecordingVolume 50;

Write-Host -ForegroundColor Green "Default " -NoNewline;
Write-Host -ForegroundColor Cyan $scarletMic.type -NoNewline;
Write-Host -ForegroundColor Green " device set to " -NoNewline;
Write-Host -ForegroundColor Magenta $scarletMic.Name;

Write-Host -ForegroundColor Green "Changed volume of " -NoNewline;
Write-Host -ForegroundColor Cyan $scarletMic.type -NoNewline;
Write-Host -ForegroundColor Green " device " -NoNewline;
Write-Host -ForegroundColor Magenta $scarletMic.Name -NoNewline;
Write-Host -ForegroundColor Green " to " -NoNewline;
Write-Host -ForegroundColor Red "50%";

$newMic = Set-AudioDevice -ID "{0.0.1.00000000}.{56d639c6-8dd7-4c3a-9539-d9b5c7aa6143}"; #Voicemeeter Output'
Write-Host -ForegroundColor Green "Default " -NoNewline;
Write-Host -ForegroundColor Cyan $newMic.type -NoNewline;
Write-Host -ForegroundColor Green " device set to " -NoNewline;
Write-Host -ForegroundColor Magenta $newMic.Name;

$newSpeaker = Set-AudioDevice -ID "{0.0.0.00000000}.{5bef2079-f6f9-410f-9ae0-29c2bab92724}"; #Voicemeeter Input
Write-Host -ForegroundColor Green "Default " -NoNewline;
Write-Host -ForegroundColor Cyan $newSpeaker.type -NoNewline;
Write-Host -ForegroundColor Green " device set to " -NoNewline;
Write-Host -ForegroundColor Magenta $newSpeaker.Name;

Write-Host -ForegroundColor DarkYellow "--------------------------------------------------------------------------------";
Write-Host "";
Write-Host -ForegroundColor Magenta "Press any key to continue...";
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');