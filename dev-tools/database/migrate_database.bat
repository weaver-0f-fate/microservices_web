@echo off
setlocal

rem Execute Liquibase commands in the core directory first
cd core
Liquibase status
liquibase update-sql
liquibase update
cd ..

rem Loop through each subdirectory except core
for /D %%d in (*) do (
    rem Skip core directory since it's already executed
    if /I "%%d" neq "core" (
	
        cd "%%d"
		
		liquibase status
        echo Executing Liquibase update-sql in directory %%d
        liquibase update-sql

        echo Executing Liquibase update in directory %%d
        liquibase update

        cd ..
    )
)

cmd /k
endlocal

