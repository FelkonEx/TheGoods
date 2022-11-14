function Show-Divider {
    Write-Host ""
    Write-Host "=============================="
    Write-Host ""
}

function Convert-String($name) {
    switch ($name) {
        "\" { "(backward_slash)" }
        "/" { "(forward_slash)" }
        "?" { "(question_mark)" }
        "*" { "(asterisk)" }
        "<" { "(less_than)" }
        ">" { "(greater_than)" }
        "|" { "(pipe_symbol)" }
        '"' { "(quotation_mark)" }
        Default { "__INVALID___" }
    }
}

function Format-Date($date) {
    $year = $date.Substring(0, 4);
    $month = $date.Substring(4, 2);
    $day = $date.Substring(6, 2);

    return "$($year)-$($month)-$($day)"
}

function Format-Date-Month($date) {
    $year = $date.Substring(0, 4);
    $month = $date.Substring(4, 2);

    return "$($year)-$($month)"
}

function Invoke-Chat-Downloader($filename, $month) {
    $outputHTML = "$($filename).html"
    $outputTXT = "$($filename).txt"
    $folderPath = "vod_chats\$($formattedMonth)"

    if ( -not(Test-Path -Path "$($folderPath)" -PathType Leaf)) {
        $null = New-Item -Path $folderPath -ItemType Directory -ErrorAction SilentlyContinue
    }

    if (Test-Path -Path "$($folderPath)\$($outputHTML)" -PathType Leaf) {
        Write-Host "HTML Chat Already Downloaded" -ForegroundColor Green
    }
    else {
        Write-Host "Downloading HTML Chat" -ForegroundColor Yellow
        .\TwitchDownloaderCLI.exe --mode ChatDownload --id 1642176464 --embed-emotes --bttv=true --ffz=true --stv=true --output "$($folderPath)\$($outputHTML)"
        Write-Host "HTML Chat Downloaded!" -ForegroundColor Green
        Write-Host ""
    }


    if (Test-Path -Path "$($folderPath)\$($outputTXT)" -PathType Leaf) {
        Write-Host "Text Chat Already Downloaded" -ForegroundColor Green
    }
    else {
        Write-Host "Downloading Text Chat" -ForegroundColor Yellow
        .\TwitchDownloaderCLI.exe --mode ChatDownload --id 1642176464 --output "$($folderPath)\$($outputTXT)"
        Write-Host ""
        Write-Host "Text Chat Downloaded!" -ForegroundColor Green
    }
}

function Invoke-Main {
    if (-not(Test-Path -Path "yt-dlp.exe" -PathType Leaf)) {
        Write-Host "yt-dlp.exe not found" -ForegroundColor Red
        exit
    }
    elseif (-not(Test-Path -Path "TwitchDownloaderCLI.exe" -PathType Leaf)) {
        Write-Host "TwitchDownloaderCLI.exe not found" -ForegroundColor Red
        exit
    }
    else {
        Write-Host "" -ForegroundColor Yellow
        Write-Host "Fetching past stream data..." -ForegroundColor Yellow
        $vodsRawInfo = .\yt-dlp.exe 'https://www.twitch.tv/felkonEx/videos?filter=archives&sort=time' -s -J | Out-String 
        $vodsObject = ConvertFrom-Json $vodsRawInfo
        $vodsList = $vodsObject.entries
        Write-Host "VODs Successfully Fetched! VOD count: $($($vodsList).Count)" -ForegroundColor Green
        Write-Host "VODs Dated From $(Format-Date $vodsList[($vodsList).Count - 1].upload_date) to $(Format-Date $vodsList[0].upload_date)" -ForegroundColor Green

        Show-Divider

        foreach ($vod in $vodsList) {
            #Fetch all variables for filename
            $title = $vod.fulltitle
            $username = $vod.uploader
            $date = $vod.upload_date
            $id = $vod.webpage_url_basename

            #Set formatted date properly
            $formattedDate = Format-Date $date
            $formattedMonth = Format-Date-Month $date

            $filename = "$($formattedDate)_$($username)_$($id)"
            [System.IO.Path]::GetInvalidFileNameChars() | ForEach-Object { $fileName = $fileName.replace("$($_)", "$(Convert-String -Name $($_))") }

            Write-Host "Downloading chat for Vod:" -ForegroundColor Magenta
            Write-Host " - Title: $($title)" -ForegroundColor Magenta
            Write-Host " - Date: $($formattedDate)" -ForegroundColor Magenta
            Write-Host ""

            Invoke-Chat-Downloader $filename $formattedMonth

            Show-Divider
        }
    }
}

Invoke-Main
Write-Host "VOD chat's have been exported successfully, press enter to close"
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');