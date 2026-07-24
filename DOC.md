# SFTP External Library — OutSystems ODC consumer guide

This document is for OutSystems developers consuming the **SFTP** External Library inside
OutSystems Developer Cloud (ODC). It describes what each action does, its inputs/outputs, and
how to handle errors — from Service Studio's point of view, not the `.cs` source. For build/
publish instructions, see [`README.md`](README.md).

## Installing the library

1. In ODC Portal, go to your application (or a shared module) and open **Extensibility >
   External Libraries** (or **Dependencies** depending on your ODC version).
2. Upload the `ODC-SFTP.zip` produced by this repo's release (see the repo's GitHub Releases
   page) or built via `generate_upload_package.ps1`.
3. Once imported, the library appears as **SFTP** in Service Studio, with all actions below
   listed under it.
4. Add **SFTP** as a dependency to any module that needs it, then drag the desired action into
   a Screen Action / Server Action flow like any other action.

## Common concepts

- **`IP`** — SFTP server hostname or IP address, e.g. `"127.0.0.1"` or `"sftp.example.com"`.
- **`Port`** — SFTP port, typically `22`.
- **`Username`** — SSH username to authenticate with.
- **Two authentication styles** — every action exists in a password variant (`Paword` input,
  plain text) and a private-key variant (`PrivateKey` input, suffixed `_PrivateKey`, expects the
  raw bytes of a `.pem` file — read it with `Get`/upload it as a Binary Data input).
- **`Path`** — remote absolute path, e.g. `"/home/user/file.txt"` or `"/home/user/folder"`.
- **Errors** — every action can raise an exception (connection refused, auth failure, path not
  found, permission denied, etc.). Wrap calls in an **Exception Handler** in your OutSystems
  logic; the exception message comes directly from the underlying SSH library and will vary by
  failure type — do not pattern-match on exact message text, only on whether it failed.

## RemoteItem structure

Returned by `List`, `Search`, `SearchWithStatus`, and their `_PrivateKey` variants.

| Field              | Type      | Description |
|--------------------|-----------|--------------|
| `ssFilename`       | Text      | File or directory name (not full path). |
| `ssSizeInBytes`    | Integer   | Size in bytes, **clamped to 2,147,483,647 (2 GB − 1) for larger files**. |
| `ssIsDir`          | Boolean   | `True` if the entry is a directory. |
| `ssIsLink`         | Boolean   | `True` if the entry is a symbolic link. |
| `ssCreated`        | DateTime  | Last access time reported by the server (not true creation time — SFTP has no creation timestamp). |
| `ssModified`       | DateTime  | Last modification time. |
| `ssSizeInBytesLong`| LongInteger | Exact size in bytes, not clamped. Use this instead of `ssSizeInBytes` for files ≥ 2 GB. Added in this release; defaults to `0` if you're on an older library version's cached entity, so re-fetch after upgrading. |

Example JSON as it would appear in an OutSystems REST/JSON export:

```json
{
  "ssFilename": "report.csv",
  "ssSizeInBytes": 15234,
  "ssIsDir": false,
  "ssIsLink": false,
  "ssCreated": "2026-07-01T10:15:00Z",
  "ssModified": "2026-07-20T08:42:11Z",
  "ssSizeInBytesLong": 15234
}
```

## Actions

### Get / Get_PrivateKey

Downloads a remote file's content.

**Inputs:** `IP` (Text), `Port` (Integer), `Username` (Text), `Paword` (Text) or `PrivateKey`
(Binary Data), `Path` (Text)
**Outputs:** `Data` (Binary Data) — the file's raw bytes.

```
Get("10.0.0.5", 22, "svc-user", "s3cr3t", "/data/export.csv", Data)
```

### Put / Put_PrivateKey

Uploads bytes to a remote path, overwriting any existing file at that path.

**Inputs:** `IP`, `Port`, `Username`, `Paword`/`PrivateKey`, `Path`, `Data` (Binary Data)
**Outputs:** none.

```
Put("10.0.0.5", 22, "svc-user", "s3cr3t", "/data/upload.csv", FileContent)
```

### List / List_PrivateKey

Lists entries in a remote directory.

**Inputs:** `IP`, `Port`, `Username`, `Paword`/`PrivateKey`, `Path` (e.g. `"/data"`),
`NumberOfFiles` (Integer) — maximum entries to return; **`0` returns all entries**.
**Outputs:** `List` (List of RemoteItem).

```
List("10.0.0.5", 22, "svc-user", "s3cr3t", "/data", 0, Files)
```

### Move / Move_PrivateKey

Renames/moves a remote file.

**Inputs:** `IP`, `Port`, `Username`, `Paword`/`PrivateKey`, `Source` (Text), `Target` (Text)
**Outputs:** none.

### Delete / Delete_PrivateKey

Deletes a remote file (not a directory — use `Rmdir` for that).

**Inputs:** `IP`, `Port`, `Username`, `Paword`/`PrivateKey`, `Path`
**Outputs:** none.

### Mkdir / Mkdir_PrivateKey

Creates a remote directory. Fails if the parent directory doesn't exist or the directory
already exists (server-dependent — check the raised exception).

**Inputs:** `IP`, `Port`, `Username`, `Paword`/`PrivateKey`, `Path`
**Outputs:** none.

### Rmdir / Rmdir_PrivateKey

Removes a remote directory. Typically fails if the directory is not empty.

**Inputs:** `IP`, `Port`, `Username`, `Paword`/`PrivateKey`, `Path`
**Outputs:** none.

### Exists / Exists_PrivateKey

Checks whether a remote path (file or directory) exists.

**Inputs:** `IP`, `Port`, `Username`, `Paword`/`PrivateKey`, `Path`
**Outputs:** `Exists` (Boolean).

### Search / Search_PrivateKey

Looks for an entry with an exact `FileName` match inside `Path` (non-recursive — only that
directory's direct children).

**Inputs:** `IP`, `Port`, `Username`, `Paword`/`PrivateKey`, `Path`, `FileName` (Text)
**Outputs:** `File` (RemoteItem).

> **Caveat:** if no entry matches `FileName`, `File` comes back as an all-default/empty
> `RemoteItem` (`ssFilename = ""`, sizes `0`, dates at their minimum value) — there is **no
> explicit signal that nothing was found**. Prefer `SearchWithStatus` below when you need to
> reliably branch on "found vs not found."

### SearchWithStatus / SearchWithStatus_PrivateKey

Same lookup as `Search`, but with an explicit `Found` output so you don't have to infer
"not found" from an empty structure.

**Inputs:** `IP`, `Port`, `Username`, `Paword`/`PrivateKey`, `Path`, `FileName`
**Outputs:** `File` (RemoteItem), `Found` (Boolean) — `True` when a match was found; `File` is
only meaningful when `Found = True`.

```
SearchWithStatus("10.0.0.5", 22, "svc-user", "s3cr3t", "/data", "report.csv", Item, Found)
If (Found) {
    // use Item
}
```

## Error handling

All actions can throw. Typical failure causes and how they surface:

| Cause                          | What you'll see |
|--------------------------------|------------------|
| Wrong `IP`/`Port`, network down| Connection/socket exception, e.g. timeout or connection refused. |
| Wrong `Username`/`Paword`/`PrivateKey` | Authentication exception. |
| `Path` doesn't exist (`Get`, `Delete`, `Move` source, `Rmdir`) | "No such file" style exception from the SSH library. |
| No permission on the server    | Permission-denied exception. |
| `Mkdir` on an existing path / `Rmdir` on a non-empty directory | Server-dependent exception. |

Since these are raised as regular .NET exceptions crossing into OutSystems, wrap every call in
an **Exception Handler** block and branch on whether an exception occurred — do not rely on
specific exception message text, as it comes from the underlying SSH library and can vary
across server implementations.

## Known limitations

- `ssSizeInBytes` clamps at 2 GB — use `ssSizeInBytesLong` for exact sizes on larger files.
- `Search`/`Search_PrivateKey` cannot distinguish "not found" from "found an empty entry" — use
  `SearchWithStatus`/`SearchWithStatus_PrivateKey` instead.
- No connection reuse across actions — each call opens a fresh SSH session.
