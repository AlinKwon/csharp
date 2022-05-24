
# Electron Update Server use IIS

1. Create new web site 
2. Target directory : add IIS_IUSRS (read only) 
3. 처리기 매핑 : 정렬된 목록 보기 -> StaticFile을 최상단으로
4. MINE 형식 : add ".yml" (text/yml) , add ".blockmap"(application/octet-stream) 
5. Copy file "latest.yml", "*{version}.exe", "*{version}.exe.blockmap" from Electron buil target directroy   
