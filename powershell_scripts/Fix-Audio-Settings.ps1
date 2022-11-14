# Get-AudioDevice -ID "{0.0.0.00000000}.{95312096-1706-4fa7-8aa9-3be0c957a2f9}"
# Get-AudioDevice -List
Write-Host ""
Write-Host -ForegroundColor DarkYellow "--------------------------------------------------------------------------------"

$scarletMic = Set-AudioDevice -ID "{0.0.1.00000000}.{5ac24442-ee7e-41e9-b55f-29f3de793059}" #Microphone (3- Scarlett Solo USB)
Set-AudioDevice -RecordingVolume 50

Write-Host -ForegroundColor DarkGreen "Default " -NoNewline
Write-Host -ForegroundColor DarkCyan "$($scarletMic.type)" -NoNewline
Write-Host -ForegroundColor DarkGreen " device set to " -NoNewline
Write-Host -ForegroundColor DarkMagenta "$($scarletMic.Name)"

Write-Host -ForegroundColor DarkGreen "Changed volume of " -NoNewline
Write-Host -ForegroundColor DarkCyan "$($scarletMic.type)" -NoNewline
Write-Host -ForegroundColor DarkGreen " device " -NoNewline
Write-Host -ForegroundColor DarkMagenta "$($scarletMic.Name)" -NoNewline
Write-Host -ForegroundColor DarkGreen " to " -NoNewline
Write-Host -ForegroundColor DarkRed "50%"

$newMic = Set-AudioDevice -ID "{0.0.1.00000000}.{56d639c6-8dd7-4c3a-9539-d9b5c7aa6143}" #Voicemeeter Output'
Write-Host -ForegroundColor DarkGreen "Default " -NoNewline
Write-Host -ForegroundColor DarkCyan "$($newMic.type)" -NoNewline
Write-Host -ForegroundColor DarkGreen " device set to " -NoNewline
Write-Host -ForegroundColor DarkMagenta "$($newMic.Name)"

$newSpeaker = Set-AudioDevice -ID "{0.0.0.00000000}.{5bef2079-f6f9-410f-9ae0-29c2bab92724}" #Voicemeeter Input
Write-Host -ForegroundColor DarkGreen "Default " -NoNewline
Write-Host -ForegroundColor DarkCyan "$($newSpeaker.type)" -NoNewline
Write-Host -ForegroundColor DarkGreen " device set to " -NoNewline
Write-Host -ForegroundColor DarkMagenta "$($newSpeaker.Name)"

Write-Host -ForegroundColor DarkYellow "--------------------------------------------------------------------------------"
Write-Host ""
Write-Host -ForegroundColor DarkMagenta "Press any key to continue...";
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');