import xml.etree.ElementTree
import datetime
import time
import asyncio
pattern = "alpha"
filename = "SpectruMineAPI.csproj"
file = xml.etree.ElementTree.parse(filename)
publishdir = "./bin/Publish"


version = file.getroot().find("PropertyGroup").find("Version")
print("Generating version")
print("Pattern: maj.min.[DAY].[MOUTH][LASTHOUR][MINUTES]")
print("0.1* alpha, 0.2* beta, 1.* release")
print("selected pattern: " + pattern)

MAJ: int
MIN: int
DAYBUILD: int
if pattern == "alpha":
    MAJ = 0
    MIN = 1

if pattern == "beta":
    MAJ = 0
    MIN = 2

if pattern == "release":
    MAJ = 1
    MIN = 0
DAYBUILD = datetime.datetime.now().day
LATESTINFO = str(datetime.datetime.now().month) + str(datetime.datetime.now().hour)[1:] + str(datetime.datetime.now().minute)
ALLVERSION = str(MAJ) + "." + str(MIN) + "." + str(DAYBUILD) + "." + LATESTINFO
print(ALLVERSION)
version.text = ALLVERSION
file.write(filename)
print("[DONE]")
async def Proc():
    print("Publishing at " + publishdir + "... Please wait...")
    time1 = time.time()
    proc = await asyncio.create_subprocess_shell(
    "dotnet build --configuration Release",
    stdout=asyncio.subprocess.PIPE,
    stderr=asyncio.subprocess.PIPE)
    stdout, stderr = await proc.communicate()
    print(f'[{proc!r} exited with {proc.returncode}]')
    if stdout:
        print(f'\n{stdout.decode()}')
    if stderr:
        print(f'\n{stderr.decode()}')
    elapsed = str(round(time.time() - time1, 2)) 
    if proc.returncode != 0:
        print("Build failed at " + elapsed + "s. returning...")
        return
    print("Build Successful at " + elapsed + "s. Publishing...")
    proc1 = await asyncio.create_subprocess_shell(
    "dotnet publish -c Release -r ubuntu.22.04-x64 --output " + publishdir,
    stdout=asyncio.subprocess.PIPE,
    stderr=asyncio.subprocess.PIPE)
    stdout, stderr = await proc1.communicate()
    if stdout:
        print(f'\n{stdout.decode()}')
    if stderr:
        print(f'\n{stderr.decode()}')
    elapsed = str(round(time.time() - time1, 2)) 
    print('[DONE]\nTime spent ' + elapsed + 's.') 
asyncio.run(Proc())