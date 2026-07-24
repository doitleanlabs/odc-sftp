# odc-sftp

Source code for an OutSystems Developer Cloud (ODC) External Library that exposes SSH File
Transfer Protocol (SFTP) actions to OutSystems applications: `Get`, `Put`, `List`, `Move`,
`Delete`, `Mkdir`, `Rmdir`, `Search`, `SearchWithStatus`, and `Exists` — each available in a
plain-text-password variant and a private-key (`_PrivateKey` suffix) variant.

This README is for whoever builds, tests, and maintains the `.cs` source. For the OutSystems
consumer-facing documentation (inputs/outputs, JSON examples, error handling), see
[`DOC.md`](DOC.md).

## Requirements

- .NET SDK 10.0 (`dotnet --list-sdks` should show a `10.0.x` entry)
- NuGet access to restore `OutSystems.ExternalLibraries.SDK` and `SSH.NET`
- On Windows, PowerShell to run `generate_upload_package.ps1` (see Packaging below); on
  Linux/macOS, run the equivalent `dotnet publish` + zip commands manually

## Folder structure

```
DoiTLean.SFTP/                    Library project (net10.0)
  DoiTLean.SFTP.csproj
  DoiTLean.SFTP.sln
  ISFTP.cs                        Public interface — OutSystems action definitions
  SFTP.cs                         Implementation of ISFTP
  Structures/RemoteItem.cs        OSStructure returned by List/Search actions
  resources/odc-sftp.png          Icon embedded in the compiled library, referenced by [OSInterface]
  generate_upload_package.ps1     Packaging script (publish + zip)
DoiTLean.SFTP.UnitTests/          NUnit test project (net10.0)
Dist/                             Packaging output (zip), not committed to git — see Packaging
```

## Build & test

```bash
dotnet build DoiTLean.SFTP/DoiTLean.SFTP.sln -c Release
dotnet test DoiTLean.SFTP/DoiTLean.SFTP.sln -c Release
```

Both should complete with 0 warnings and all tests passing.

## Packaging / publishing

The library ships to OutSystems as a self-contained-false, linux-x64 publish output, zipped.

On Windows/PowerShell, from `DoiTLean.SFTP/`:

```powershell
./generate_upload_package.ps1
```

This runs `dotnet publish -c Release -r linux-x64 --self-contained false` and zips
`bin/Release/net10.0/linux-x64/publish/*` into `../Dist/ODC-SFTP.zip`.

On Linux/macOS (no PowerShell), the equivalent is:

```bash
cd DoiTLean.SFTP
dotnet publish -c Release -r linux-x64 --self-contained false
cd ..
mkdir -p Dist
(cd DoiTLean.SFTP/bin/Release/net10.0/linux-x64/publish && zip -r ../../../../../../Dist/ODC-SFTP.zip .)
```

The resulting `Dist/ODC-SFTP.zip` is what gets uploaded as an External Library in ODC and
attached as a GitHub release asset.

## Main concepts

- **`ISFTP`** — the public contract. Every method is annotated `[OSAction]` so OutSystems ODC
  discovers it as a callable action when the library is imported. Method/parameter names become
  the action/input/output names shown in OutSystems Service Studio, so renaming any of them is a
  breaking change for existing consumers.
- **`SFTP`** — the implementation, backed by `Renci.SshNet.SftpClient` (the `SSH.NET` package).
  Each action opens its own connection, performs one operation, and disconnects — there is no
  persistent/pooled connection across calls.
- **`RemoteItem`** (`[OSStructure]`) — the record type returned by `List`/`Search`/
  `SearchWithStatus` actions. New fields added to this structure must be marked
  `IsMandatory = false` with a sensible default, so that existing OutSystems entities/mappings
  built against the previous structure shape keep working after an upgrade.
- **Two auth styles per operation** — most operations exist twice: once taking a plain-text
  `Paword` (password), once taking a `PrivateKey` byte array (`.pem` file content), suffixed
  `_PrivateKey`. Both call the same underlying `SftpClient` logic, just with a different
  `SftpClient` constructor overload.

## Known limitations

- **`ssSizeInBytes` clamps at 2 GB.** It's an `Int32` OutSystems field kept for backward
  compatibility; files above `int.MaxValue` bytes report `int.MaxValue` there. Use the newer
  `ssSizeInBytesLong` field (`Int64`) for the exact size.
- **`Search`/`Search_PrivateKey` can't distinguish "not found" from "found an empty entry."**
  When no file matches, the output is a zeroed/empty `RemoteItem`, with no explicit flag. Use
  `SearchWithStatus`/`SearchWithStatus_PrivateKey` instead, which add an explicit `Found` output.
- **No connection pooling/reuse.** Every action call opens and tears down its own SSH
  connection; there is no way to batch multiple operations over one session.
- **Exceptions cross the OutSystems boundary unwrapped.** Any `SftpClient`/`SSH.NET` exception
  (auth failure, timeout, path errors, etc.) propagates as-is; there's no OutSystems-specific
  error classification layer.
- **No integration tests for the SSH-connected methods.** `Renci.SshNet.SftpClient` has no
  seam for mocking a remote server, so `Get`/`Put`/`List`/`Search`/`Move`/`Delete`/`Mkdir`/
  `Rmdir`/`Exists` are covered only by manual/OutSystems-side testing against a real SFTP
  server. Unit tests in this repo cover `RemoteItem` and the size-clamping logic only.

## OutSystems links

- [ODC Documentation](https://success.outsystems.com/documentation/outsystems_developer_cloud/)
- [ODC External Logic](https://success.outsystems.com/documentation/outsystems_developer_cloud/building_apps/extend_your_apps_with_external_logic/)
- [ODC External Libraries SDK](https://success.outsystems.com/documentation/outsystems_developer_cloud/building_apps/extend_your_apps_with_external_logic/external_libraries_sdk_readme/)
