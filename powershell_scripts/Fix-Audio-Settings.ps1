# Get-AudioDevice -ID "{0.0.0.00000000}.{95312096-1706-4fa7-8aa9-3be0c957a2f9}"
# Get-AudioDevice -List

function Write-DefaultDeviceInfo ($deviceType, $deviceName) {
    Write-Host -ForegroundColor Green "-> Default " -NoNewline;
    Write-Host -ForegroundColor Cyan $deviceType -NoNewline;
    Write-Host -ForegroundColor Green " device set to " -NoNewline;
    Write-Host -ForegroundColor Magenta $deviceName;
}

function Write-DefaultDeviceVolumeInfo ($deviceType, $deviceName, $value) {
    Write-Host -ForegroundColor Green "-> Changed volume of " -NoNewline;
    Write-Host -ForegroundColor Cyan $deviceType -NoNewline;
    Write-Host -ForegroundColor Green " device " -NoNewline;
    Write-Host -ForegroundColor Magenta $deviceName -NoNewline;
    Write-Host -ForegroundColor Green " to " -NoNewline;
    Write-Host -ForegroundColor Red $value%;
}

function Set-PCSpeakerInfo {
    Write-Host;
    Write-Host -ForegroundColor Yellow "Setting to PC Speakers (Default)";

    $voicemeeterSpeakerID = "{0.0.0.00000000}.{5bef2079-f6f9-410f-9ae0-29c2bab92724}"; #Voicemeeter Input
    
    $newSpeaker = Set-AudioDevice -ID $voicemeeterSpeakerID;
    Write-DefaultDeviceInfo $newSpeaker.type $newSpeaker.Name
}

function Set-PCMicInfo {
    Write-Host;
    Write-Host -ForegroundColor Yellow "Setting PC Mic Info (Default)";

    $scarletMicID = "{0.0.1.00000000}.{5ac24442-ee7e-41e9-b55f-29f3de793059}"; #Microphone (3- Scarlett Solo USB)
    $voicemeeterMicID = "{0.0.1.00000000}.{56d639c6-8dd7-4c3a-9539-d9b5c7aa6143}"; #Voicemeeter Output'

    $scarletMic = Set-AudioDevice -ID $scarletMicID;
    Write-DefaultDeviceInfo $scarletMic.type $scarletMic.Name;

    Set-AudioDevice -RecordingVolume 50;
    Write-DefaultDeviceVolumeInfo $scarletMic.type $scarletMic.Name 50;

    $newMic = Set-AudioDevice -ID $voicemeeterMicID;
    Write-DefaultDeviceInfo $newMic.type $newMic.Name;
}

function Set-TVSpeakerInfo {
    Write-Host;
    Write-Host -ForegroundColor Yellow "Setting to TV Speakers";

    $tvSpeakerID = "{0.0.0.00000000}.{9be4134e-9533-4bc3-a8c4-bcdd8acc095a}"; #Sony TV (2- NVIDIA High Definition Audio)
    
    $newSpeaker = Set-AudioDevice -ID $tvSpeakerID;
    Write-DefaultDeviceInfo $newSpeaker.type $newSpeaker.Name
}

function Check-Keypress ($sleepSeconds = 5) {
    $timeout = New-TimeSpan -Seconds $sleepSeconds
    $stopWatch = [Diagnostics.Stopwatch]::StartNew()
    $interrupted = $false

    while ($stopWatch.Elapsed -lt $timeout) {
        if ($Host.UI.RawUI.KeyAvailable) {
            $keyPressed = $Host.UI.RawUI.ReadKey("NoEcho, IncludeKeyUp, IncludeKeyDown")
            if ($keyPressed.KeyDown -eq "True") { 
                $interrupted = $true
                break          
            } 
        }
    }

    return $interrupted
}

function Set-DefaultOptions {
    Set-PCSpeakerInfo;
    Set-PCMicInfo;
}

function menu {
    Write-Host -ForegroundColor White "";
    Write-Host -ForegroundColor White "    ##################################";
    Write-Host -ForegroundColor White "    #                                #";
    Write-Host -ForegroundColor White "    #" -NoNewline; Write-Host -ForegroundColor Green "  Switch Audio Drivers Utility  " -NoNewline; Write-Host -ForegroundColor White "#";
    Write-Host -ForegroundColor White "    #                                #";
    Write-Host -ForegroundColor White "    ##################################";
    Write-Host -ForegroundColor White "    #                                #";
    Write-Host -ForegroundColor White "    #" -NoNewline; Write-Host -ForegroundColor Cyan "  1) Switch to PC + Fix Mic     " -NoNewline; Write-Host -ForegroundColor White "#";
    Write-Host -ForegroundColor White "    #" -NoNewline; Write-Host -ForegroundColor Cyan "  2) Switch to PC               " -NoNewline; Write-Host -ForegroundColor White "#";
    Write-Host -ForegroundColor White "    #" -NoNewline; Write-Host -ForegroundColor Cyan "  3) Fix Mic                    " -NoNewline; Write-Host -ForegroundColor White "#";
    Write-Host -ForegroundColor White "    #" -NoNewline; Write-Host -ForegroundColor Cyan "  4) Switch to TV               " -NoNewline; Write-Host -ForegroundColor White "#";
    Write-Host -ForegroundColor White "    #" -NoNewline; Write-Host -ForegroundColor Cyan "  5) Exit                       " -NoNewline; Write-Host -ForegroundColor White "#";
    Write-Host -ForegroundColor White "    #                                #";
    Write-Host -ForegroundColor White "    ##################################";
    Write-Host -ForegroundColor White "";

    Write-Host -ForegroundColor Cyan "Will set to default in 5s, press any key for menu..."
    $valid = $false;
    if (Check-Keypress) {
        while (-not $valid) {
            $selection = Read-Host -Prompt 'Please make your selection'
    
            switch ($selection) {
                1 { 
                    Set-DefaultOptions; 
                    $valid = $true;
                }
                2 { 
                    Set-PCSpeakerInfo; 
                    $valid = $true;
                }
                3 { 
                    Set-PCMicInfo; 
                    $valid = $true;
                }
                4 { 
                    Set-TVSpeakerInfo; 
                    $valid = $true;
                }
                5 { 
                    $valid = $true;
                    exit;
                }
                default { 
                    Write-Host -ForegroundColor Red "Invalid Option... cringe";
                }
            }
        }
    }
    else {
        Set-DefaultOptions;
    }

    Write-Host;
    Write-Host -ForegroundColor Magenta "Press any key to continue...";
    $null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
}

Clear-Host
menu