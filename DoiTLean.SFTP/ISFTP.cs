using DoiTLean.SFTP.Structures;
using OutSystems.ExternalLibraries.SDK;
using System.Collections.Generic;

namespace DoiTLean.SFTP
{
    /// <summary>
    /// SSH File Transfer Protocol (SFTP) actions that you can use in your applications.
    /// </summary>
    [OSInterface(Description = "SSH File Transfer Protocol (SFTP) that you can use in your applications",IconResourceName = "DoiTLean.SFTP.resources.odc-sftp.png", Name = "SFTP")]
    public interface ISFTP
    {

        /// <summary>
        /// Downloads a remote file, authenticating with a plain-text password.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">Remote file path to download.</param>
        /// <param name="Data">Downloaded file content.</param>
        [OSAction]
        void Get(string IP, int Port, string Username, string Paword, string Path, out byte[] Data);

        /// <summary>
        /// Uploads a file to a remote path, authenticating with a plain-text password.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">Remote destination path.</param>
        /// <param name="Data">File content to upload.</param>
        [OSAction]
        void Put(string IP, int Port, string Username, string Paword, string Path, byte[] Data);

        /// <summary>
        /// Lists the contents of a remote directory, authenticating with a plain-text password.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">default: &quot;/&quot;</param>
        /// <param name="NumberOfFiles">Maximum number of entries to return. 0 returns all entries.
        /// </param>
        /// <param name="List">Directory entries found.</param>
        [OSAction]
        void List(string IP, int Port, string Username, string Paword, string Path, int NumberOfFiles, out List<RemoteItem> List);

        /// <summary>
        /// Moves/renames a remote file, authenticating with a plain-text password.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Source">Source remote path.</param>
        /// <param name="Target">Destination remote path.</param>
        [OSAction]
        void Move(string IP, int Port, string Username, string Paword, string Source, string Target);

        /// <summary>
        /// Deletes a remote file, authenticating with a plain-text password.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">Remote file path to delete.</param>
        [OSAction]
        void Delete(string IP, int Port, string Username, string Paword, string Path);

        /// <summary>
        /// Creates a remote directory, authenticating with a plain-text password.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">Remote directory path to create.</param>
        [OSAction]
        void Mkdir(string IP, int Port, string Username, string Paword, string Path);

        /// <summary>
        /// Removes a remote directory, authenticating with a plain-text password.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">Remote directory path to remove.</param>
        [OSAction]
        void Rmdir(string IP, int Port, string Username, string Paword, string Path);

        /// <summary>
        /// Searches a remote directory for an entry by exact name, authenticating with a plain-text password.
        /// </summary>
        /// <remarks>
        /// When no match is found, File is returned as a default/empty RemoteItem. There is no way to
        /// distinguish "not found" from a legitimate all-empty entry. Use SearchWithStatus for an
        /// explicit found/not-found result.
        /// </remarks>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">Remote directory path to search.</param>
        /// <param name="FileName">File name to search</param>
        /// <param name="File">Matching entry, or a default RemoteItem when not found.</param>
        [OSAction]
        void Search(string IP, int Port, string Username, string Paword, string Path, string FileName, out RemoteItem File);

        /// <summary>
        /// Searches a remote directory for an entry by exact name, authenticating with a plain-text password.
        /// Unlike Search, this explicitly reports whether a match was found.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">Remote directory path to search.</param>
        /// <param name="FileName">File name to search</param>
        /// <param name="File">Matching entry, or a default RemoteItem when not found.</param>
        /// <param name="Found">True when an entry matching FileName was found.</param>
        [OSAction]
        void SearchWithStatus(string IP, int Port, string Username, string Paword, string Path, string FileName, out RemoteItem File, out bool Found);

        /// <summary>
        /// Checks whether a remote path exists, authenticating with a plain-text password.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">Remote path to check.</param>
        /// <param name="Exists">True when the path exists.</param>
        [OSAction]
        void Exists(string IP, int Port, string Username, string Paword, string Path, out bool Exists);

        /// <summary>
        /// Deletes a remote file, authenticating with a private key.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Path">Remote file path to delete.</param>
        [OSAction]
        void Delete_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path);

        /// <summary>
        /// Checks whether a remote path exists, authenticating with a private key.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Path">Remote path to check.</param>
        /// <param name="Exists">True when the path exists.</param>
        [OSAction]
        void Exists_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path, out bool Exists);

        /// <summary>
        /// Downloads a remote file, authenticating with a private key.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Path">Remote file path to download.</param>
        /// <param name="Data">Downloaded file content.</param>
        [OSAction]
        void Get_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path, out byte[] Data);

        /// <summary>
        /// Lists the contents of a remote directory, authenticating with a private key.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Path">Remote directory path to list.</param>
        /// <param name="NumberOfFiles">Maximum number of entries to return. 0 returns all entries.</param>
        /// <param name="List">Directory entries found.</param>
        [OSAction]
        void List_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path, int NumberOfFiles, out List<RemoteItem> List);

        /// <summary>
        /// Creates a remote directory, authenticating with a private key.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Path">Remote directory path to create.</param>
        [OSAction]
        void Mkdir_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path);

        /// <summary>
        /// Moves/renames a remote file, authenticating with a private key.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Source">Source remote path.</param>
        /// <param name="Target">Destination remote path.</param>
        [OSAction]
        void Move_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Source, string Target);

        /// <summary>
        /// Uploads a file to a remote path, authenticating with a private key.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Path">Remote destination path.</param>
        /// <param name="Data">File content to upload.</param>
        [OSAction]
        void Put_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path, byte[] Data);

        /// <summary>
        /// Removes a remote directory, authenticating with a private key.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Path">Remote directory path to remove.</param>
        [OSAction]
        void Rmdir_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path);

        /// <summary>
        /// Searches a remote directory for an entry by exact name, authenticating with a private key.
        /// </summary>
        /// <remarks>
        /// When no match is found, File is returned as a default/empty RemoteItem. There is no way to
        /// distinguish "not found" from a legitimate all-empty entry. Use SearchWithStatus_PrivateKey for
        /// an explicit found/not-found result.
        /// </remarks>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Path">Remote directory path to search.</param>
        /// <param name="FileName">File name to search</param>
        /// <param name="File">Matching entry, or a default RemoteItem when not found.</param>
        [OSAction]
        void Search_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path, string FileName, out RemoteItem File);

        /// <summary>
        /// Searches a remote directory for an entry by exact name, authenticating with a private key.
        /// Unlike Search_PrivateKey, this explicitly reports whether a match was found.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Path">Remote directory path to search.</param>
        /// <param name="FileName">File name to search</param>
        /// <param name="File">Matching entry, or a default RemoteItem when not found.</param>
        /// <param name="Found">True when an entry matching FileName was found.</param>
        [OSAction]
        void SearchWithStatus_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path, string FileName, out RemoteItem File, out bool Found);


    }
}
