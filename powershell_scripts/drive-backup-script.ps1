# C Drive Files - OS (Samsung SSD [1TB])
robocopy C:\BACKUPS     Q:\C\BACKUPS /mir /R:0 /W:3
robocopy C:\GAMES       Q:\C\GAMES /mir /R:0 /W:3
robocopy "C:\Program Files" "Q:\C\Program Files" /mir /R:0 /W:3
robocopy "C:\Program Files (x86)" "Q:\C\Program Files (x86)" /mir /R:0 /W:3
robocopy C:\RIPs        Q:\C\RIPs /mir /R:0 /W:3
robocopy C:\TEMP        Q:\C\TEMP /mir /R:0 /W:3
# robocopy C:\Users       Q:\C\Users /mir /R:0 /W:3

# D Drive Files - Data (WD blue SSD [4TB]) /R:0 /W:3
robocopy D:\Backups Q:\D\Backups /mir /R:0 /W:3
robocopy D:\Games Q:\D\Games /mir /R:0 /W:3
robocopy D:\GIT Q:\D\GIT /mir /R:0 /W:3
robocopy D:\Google-Drive Q:\D\Google-Drive /mir /R:0 /W:3
robocopy D:\Media Q:\D\Media /mir /R:0 /W:3
robocopy D:\OneDrive Q:\D\OneDrive /mir /R:0 /W:3
robocopy D:\OTHER Q:\D\OTHER /mir /R:0 /W:3
robocopy "D:\Program Files" "Q:\D\Program Files" /mir /R:0 /W:3
robocopy "D:\Program Files (x86)" "Q:\D\Program Files (x86)" /mir /R:0 /W:3
robocopy D:\Recordings Q:\D\Recordings /mir /R:0 /W:3
robocopy "D:\Standalone Programs" "Q:\D\Standalone Programs" /mir /R:0 /W:3
robocopy D:\Temp Q:\D\Temp /mir /R:0 /W:3
robocopy D:\Tim Q:\D\Tim /mir /R:0 /W:3

# F Drive Files - Emulation (Samsung SSD [1TB])
Robocopy F:\Emulation Q:\F\Emulation /mir

# G Drive Files - Recordings (Samsung SSD [1TB])
Robocopy G:\ Q:\G /mir

# S Drive Files - temp (Samsung SSD [1TB])
Robocopy S:\temp Q:\S\temp /mir
