Tools\mysql\bin\mysqld --defaults-file=Tools\mysql\bin\my.ini --standalone

if errorlevel 1 goto error
goto finish

:error
echo.
echo MySQL could not be started
pause

:finish
