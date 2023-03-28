@ECHO OFF
PUSHD .
FOR /R %%d IN (.) DO (
cd "%%d"
IF EXIST *.cs (
REN *.cs *.old.cs
)
)
POPD