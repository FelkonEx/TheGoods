function Show-Divider {
    Write-Host ""
    Write-Host "=============================="
    Write-Host ""
}

function Convert-String {
    param (
        [String]$Name
    )

    switch ($Name) {
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

function Invoke-Downloads {
    #get URL to download

    $clipRangeValue = Read-Host "Enter Range to search clips (24hr, 7d, 30d, all)"
    
    $clipRange = @('24hr', '7d', '30d', 'all')

    if (-not $clipRange.Contains($clipRangeValue)) {
        Write-Host "Invalid Clip Range Specified, defaulting to 24 hours"
        $clipRangeValue = '24hr'
    }

    $clips_url = "https://www.twitch.tv/felkonex/clips?filter=clips&range=$($clipRangeValue)"

    # Fetching clip info for folder set-up & converting to object
    $clipRawInfo = .\yt-dlp.exe $clips_url -s -J | Out-String 
    $clipObject = ConvertFrom-Json $clipRawInfo
    $clipList = $clipObject.entries

    $clipCountTotal = ($clipList).Count

    $clipCountDownloaded = 0
    $clipCountAlreadyDownloaded = 0
    $clipCountIgnore = 0

    $clipNameDownloadedArr = @()
    $clipNameAlreadyDownloadedArr = @()
    $clipNameIgnoredArr = @()

    Show-Divider

    Write-Host "Initiating Download of Clips:" -ForegroundColor Magenta
    Write-Host " - Channel: FelkonEx" -ForegroundColor Magenta
    Write-Host " - Timeframe: $($clipRangeValue)" -ForegroundColor Magenta
    Write-Host " - Clip Count: $($clipCountTotal)" -ForegroundColor Magenta

    foreach ($clip in $clipList) {
        Show-Divider

        # setup variables to use
        $title = $clip.fulltitle
        $username = $clip.uploader
        $date = $clip.upload_date
        $category = $clip.category
        $ext = $clip.ext
        $url = $clip.original_url
        $id = $clip.id

        $date_year = $date.Substring(0, 4)
        $date_month = $date.Substring(4, 2)
        $date_day = $date.Substring(6, 2)

        # $clipRawInfo | Out-File .\debug.json

        # Create folder for Month
        $month_folder_path = "$($date_year)-$($date_month)"
        $day_folder_path = "$($date_year)-$($date_month)-$($date_day)_$($category)"
        $fileName = "$($title)__$($username)__$($id).$($ext)"

        [System.IO.Path]::GetInvalidFileNameChars() | ForEach-Object { $fileName = $fileName.replace("$($_)", "$(Convert-String -Name $($_))") }
        
        #If the .ignore file exists
        if (Test-Path -Path "$($month_folder_path)\$($day_folder_path)\.ignore" -PathType Leaf) {
            $clipCountIgnore += 1
            $clipNameIgnoredArr += "$($day_folder_path)\$($fileName)"
            Write-Host "Ignored directory:" -ForegroundColor Red
            Write-Host "$($month_folder_path)\$($day_folder_path)" -ForegroundColor Red
            Write-Host "Skipping..." -ForegroundColor Red
        }
        else {
            if (Test-Path -Path "$($month_folder_path)\$($day_folder_path)\$($fileName)" -PathType Leaf) {
                $clipCountAlreadyDownloaded += 1
                $clipNameAlreadyDownloadedArr += "$($day_folder_path)\$($fileName)"
                Write-Host "Already downloaded clip:" -ForegroundColor Yellow
                Write-Host "$($day_folder_path)\$($fileName)" -ForegroundColor Yellow
                Write-Host "Skipping..." -ForegroundColor Yellow
            }
            else {
                # Create path for Month folder, and Day folder with , or hide error if already exists
                $null = New-Item -Path $month_folder_path -ItemType Directory -ErrorAction SilentlyContinue
                $null = New-Item -Path "$($month_folder_path)\$($day_folder_path)" -ItemType Directory -ErrorAction SilentlyContinue
				
                Write-Host "Downloading Clip #: $($clipCountDownloaded + 1):" -ForegroundColor Yellow
                Write-Host ""
				
                # download clip into folder
                .\yt-dlp.exe $url -o "$($month_folder_path)\$($day_folder_path)\$($fileName)" | Write-Host -ForegroundColor DarkCyan 
				
                $clipCountDownloaded += 1
                Write-Host ""
                Write-Host "Clip Successfully Downloaded!" -ForegroundColor Green
                $clipNameDownloadedArr += "$($day_folder_path)\$($fileName)"
            }
        }
    }

    $sortedClipDownloadedName = $clipNameDownloadedArr | Sort-Object
    $sortedClipAlreadyDownloadedName = $clipNameAlreadyDownloadedArr | Sort-Object
    $sortedClipIgnoredName = $clipNameIgnoredArr | Sort-Object

    Show-Divider

    Write-Host "Processed $($clipCountTotal) clip(s)." -ForegroundColor Magenta
    Write-Host ""
    Write-Host "Downloaded $($clipCountDownloaded) new clip(s)" -ForegroundColor Green
    Write-Host "Already Downloaded $($clipCountAlreadyDownloaded) clip(s)" -ForegroundColor Yellow
    Write-Host "Ignored $($clipCountIgnore) clip(s)" -ForegroundColor Red
    Write-Host ""
    
    foreach ($clipName in $sortedClipDownloadedName) {
        Write-Host " - $($clipName)" -ForegroundColor Green
    }
    foreach ($clipName in $sortedClipAlreadyDownloadedName) {
        Write-Host " - $($clipName)" -ForegroundColor Yellow
    }
    foreach ($clipName in $sortedClipIgnoredName) {
        Write-Host " - $($clipName)" -ForegroundColor Red
    }

    Show-Divider
}

Invoke-Downloads
Write-Host "Downloads Finished, press enter to close"
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown');
