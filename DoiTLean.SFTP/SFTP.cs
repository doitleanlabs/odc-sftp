using System;
using System.IO;
using DoiTLean.SFTP.Structures;
using System.Collections.Generic;
using Renci.SshNet;

namespace DoiTLean.SFTP {
    /// <summary>
    /// Implements SSH File Transfer Protocol (SFTP) actions exposed to OutSystems Developer Cloud.
    /// </summary>
    public class SFTP : ISFTP {

        /// <summary>
        /// Deletes a remote file, authenticating with a private key.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Path">Remote file path to delete.</param>
        public void Delete_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path)
        {
            using (Stream s = new MemoryStream(PrivateKey))
            {
                var keyFile = new PrivateKeyFile(s);
                var keyFiles = new[] { keyFile };
                using (var sftp = new SftpClient(IP, Port, Username, keyFiles))
                {
                    try
                    {
                        sftp.Connect();
                        sftp.DeleteFile(Path);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        sftp.Disconnect();
                    }
                }
            }
        } // Delete_PrivateKey

        /// <summary>
        /// Checks whether a remote path exists, authenticating with a private key.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Path">Remote path to check.</param>
        /// <param name="Exists">True when the path exists.</param>
        public void Exists_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path, out bool Exists)
        {
            Exists = false;

            using (Stream s = new MemoryStream(PrivateKey))
            {
                var keyFile = new PrivateKeyFile(s);
                var keyFiles = new[] { keyFile };
                using (var sftp = new SftpClient(IP, Port, Username, keyFiles))
                {
                    try
                    {
                        sftp.Connect();
                        Exists = sftp.Exists(Path);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        sftp.Disconnect();
                    }
                }
            }
        } // Exists_PrivateKey

        /// <summary>
        /// Downloads a remote file, authenticating with a private key.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Path">Remote file path to download.</param>
        /// <param name="Data">Downloaded file content.</param>
        public void Get_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path, out byte[] Data)
        {
            using (Stream s = new MemoryStream(PrivateKey))
            {
                var keyFile = new PrivateKeyFile(s);
                var keyFiles = new[] { keyFile };
                using (var sftp = new SftpClient(IP, Port, Username, keyFiles))
                {
                    try
                    {
                        sftp.Connect();
                        using (var buffer = new MemoryStream())
                        {
                            sftp.DownloadFile(Path, buffer);
                            Data = buffer.ToArray();
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        sftp.Disconnect();
                    }
                }
            }
        } // Get_PrivateKey

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
        public void List_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path, int NumberOfFiles, out List<RemoteItem> List)
        {
            List = new List<RemoteItem>();

            using (Stream s = new MemoryStream(PrivateKey))
            {
                var keyFile = new PrivateKeyFile(s);
                var keyFiles = new[] { keyFile };
                using (var sftp = new SftpClient(IP, Port, Username, keyFiles))
                {
                    try
                    {
                        sftp.Connect();
                        int idx = 0;
                        foreach (var file in sftp.ListDirectory(Path))
                        {
                            if (idx >= NumberOfFiles && NumberOfFiles > 0)
                                break;

                            List.Add(ToRemoteItem(file));

                            idx++;
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        sftp.Disconnect();
                    }
                }
            }
        } // List_PrivateKey

        /// <summary>
        /// Creates a remote directory, authenticating with a private key.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Path">Remote directory path to create.</param>
        public void Mkdir_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path)
        {
            using (Stream s = new MemoryStream(PrivateKey))
            {
                var keyFile = new PrivateKeyFile(s);
                var keyFiles = new[] { keyFile };
                using (var sftp = new SftpClient(IP, Port, Username, keyFiles))
                {
                    try
                    {
                        sftp.Connect();
                        sftp.CreateDirectory(Path);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        sftp.Disconnect();
                    }
                }
            }
        } // Mkdir_PrivateKey

        /// <summary>
        /// Moves/renames a remote file, authenticating with a private key.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Source">Source remote path.</param>
        /// <param name="Target">Destination remote path.</param>
        public void Move_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Source, string Target)
        {
            using (Stream s = new MemoryStream(PrivateKey))
            {
                var keyFile = new PrivateKeyFile(s);
                var keyFiles = new[] { keyFile };
                using (var sftp = new SftpClient(IP, Port, Username, keyFiles))
                {
                    try
                    {
                        sftp.Connect();
                        sftp.RenameFile(Source, Target);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        sftp.Disconnect();
                    }
                }
            }
        } // Move_PrivateKey

        /// <summary>
        /// Uploads a file to a remote path, authenticating with a private key.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Path">Remote destination path.</param>
        /// <param name="Data">File content to upload.</param>
        public void Put_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path, byte[] Data)
        {
            using (Stream s = new MemoryStream(PrivateKey))
            {
                var keyFile = new PrivateKeyFile(s);
                var keyFiles = new[] { keyFile };
                using (var sftp = new SftpClient(IP, Port, Username, keyFiles))
                {
                    try
                    {
                        sftp.Connect();
                        using (var source = new MemoryStream(Data))
                        {
                            sftp.UploadFile(source, Path); // Optional: canOverride
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        sftp.Disconnect();
                    }
                }
            }
        } // Put_PrivateKey

        /// <summary>
        /// Removes a remote directory, authenticating with a private key.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Path">Remote directory path to remove.</param>
        public void Rmdir_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path)
        {
            using (Stream s = new MemoryStream(PrivateKey))
            {
                var keyFile = new PrivateKeyFile(s);
                var keyFiles = new[] { keyFile };
                using (var sftp = new SftpClient(IP, Port, Username, keyFiles))
                {
                    try
                    {
                        sftp.Connect();
                        sftp.DeleteDirectory(Path);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        sftp.Disconnect();
                    }
                }
            }
        } // Rmdir_PrivateKey

        /// <summary>
        /// Searches a remote directory for an entry by exact name, authenticating with a private key.
        /// </summary>
        /// <remarks>
        /// When no match is found, <paramref name="File"/> is returned as a default/empty RemoteItem.
        /// There is no way to distinguish "not found" from a legitimate all-empty entry.
        /// Use <see cref="SearchWithStatus_PrivateKey"/> for an explicit found/not-found result.
        /// </remarks>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Path">Remote directory path to search.</param>
        /// <param name="FileName">File name to search</param>
        /// <param name="File">Matching entry, or a default RemoteItem when not found.</param>
        public void Search_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path, string FileName, out RemoteItem File)
        {
            using (Stream s = new MemoryStream(PrivateKey))
            {
                var keyFile = new PrivateKeyFile(s);
                var keyFiles = new[] { keyFile };
                using (var sftp = new SftpClient(IP, Port, Username, keyFiles))
                {
                    try
                    {
                        sftp.Connect();
                        FindByName(sftp, Path, FileName, out File, out _);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        sftp.Disconnect();
                    }
                }
            }
        } // Search_PrivateKey

        /// <summary>
        /// Searches a remote directory for an entry by exact name, authenticating with a private key.
        /// Unlike <see cref="Search_PrivateKey"/>, this explicitly reports whether a match was found.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="PrivateKey">.pem file content</param>
        /// <param name="Path">Remote directory path to search.</param>
        /// <param name="FileName">File name to search</param>
        /// <param name="File">Matching entry, or a default RemoteItem when not found.</param>
        /// <param name="Found">True when an entry matching FileName was found.</param>
        public void SearchWithStatus_PrivateKey(string IP, int Port, string Username, byte[] PrivateKey, string Path, string FileName, out RemoteItem File, out bool Found)
        {
            using (Stream s = new MemoryStream(PrivateKey))
            {
                var keyFile = new PrivateKeyFile(s);
                var keyFiles = new[] { keyFile };
                using (var sftp = new SftpClient(IP, Port, Username, keyFiles))
                {
                    try
                    {
                        sftp.Connect();
                        FindByName(sftp, Path, FileName, out File, out Found);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        sftp.Disconnect();
                    }
                }
            }
        } // SearchWithStatus_PrivateKey

        /// <summary>
        /// Checks whether a remote path exists, authenticating with a plain-text password.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">Remote path to check.</param>
        /// <param name="Exists">True when the path exists.</param>
        public void Exists(string IP, int Port, string Username, string Paword, string Path, out bool Exists)
        {
            Exists = false;

            using (var sftp = new SftpClient(IP, Port, Username, Paword))
            {
                try
                {
                    sftp.Connect();
                    Exists = sftp.Exists(Path);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    sftp.Disconnect();
                }
            }
        } // Exists

        /// <summary>
        /// Searches a remote directory for an entry by exact name, authenticating with a plain-text password.
        /// </summary>
        /// <remarks>
        /// When no match is found, <paramref name="File"/> is returned as a default/empty RemoteItem.
        /// There is no way to distinguish "not found" from a legitimate all-empty entry.
        /// Use <see cref="SearchWithStatus"/> for an explicit found/not-found result.
        /// </remarks>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">Remote directory path to search.</param>
        /// <param name="FileName">File name to search</param>
        /// <param name="File">Matching entry, or a default RemoteItem when not found.</param>
        public void Search(string IP, int Port, string Username, string Paword, string Path, string FileName, out RemoteItem File)
        {
            using (var sftp = new SftpClient(IP, Port, Username, Paword))
            {
                try
                {
                    sftp.Connect();
                    FindByName(sftp, Path, FileName, out File, out _);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    sftp.Disconnect();
                }
            }
        } // Search

        /// <summary>
        /// Searches a remote directory for an entry by exact name, authenticating with a plain-text password.
        /// Unlike <see cref="Search"/>, this explicitly reports whether a match was found.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">Remote directory path to search.</param>
        /// <param name="FileName">File name to search</param>
        /// <param name="File">Matching entry, or a default RemoteItem when not found.</param>
        /// <param name="Found">True when an entry matching FileName was found.</param>
        public void SearchWithStatus(string IP, int Port, string Username, string Paword, string Path, string FileName, out RemoteItem File, out bool Found)
        {
            using (var sftp = new SftpClient(IP, Port, Username, Paword))
            {
                try
                {
                    sftp.Connect();
                    FindByName(sftp, Path, FileName, out File, out Found);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    sftp.Disconnect();
                }
            }
        } // SearchWithStatus

        /// <summary>
        /// Removes a remote directory, authenticating with a plain-text password.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">Remote directory path to remove.</param>
        public void Rmdir(string IP, int Port, string Username, string Paword, string Path)
        {
            using (var sftp = new SftpClient(IP, Port, Username, Paword))
            {
                try
                {
                    sftp.Connect();
                    sftp.DeleteDirectory(Path);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    sftp.Disconnect();
                }
            }
        } // Rmdir

        /// <summary>
        /// Creates a remote directory, authenticating with a plain-text password.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">Remote directory path to create.</param>
        public void Mkdir(string IP, int Port, string Username, string Paword, string Path)
        {
            using (var sftp = new SftpClient(IP, Port, Username, Paword))
            {
                try
                {
                    sftp.Connect();
                    sftp.CreateDirectory(Path);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    sftp.Disconnect();
                }
            }
        } // Mkdir

        /// <summary>
        /// Deletes a remote file, authenticating with a plain-text password.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">Remote file path to delete.</param>
        public void Delete(string IP, int Port, string Username, string Paword, string Path)
        {
            using (var sftp = new SftpClient(IP, Port, Username, Paword))
            {
                try
                {
                    sftp.Connect();
                    sftp.DeleteFile(Path);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    sftp.Disconnect();
                }
            }
        }

        /// <summary>
        /// Moves/renames a remote file, authenticating with a plain-text password.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Source">Source remote path.</param>
        /// <param name="Target">Destination remote path.</param>
        public void Move(string IP, int Port, string Username, string Paword, string Source, string Target)
        {
            using (var sftp = new SftpClient(IP, Port, Username, Paword))
            {
                try
                {
                    sftp.Connect();
                    sftp.RenameFile(Source, Target);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    sftp.Disconnect();
                }
            }
        }

        /// <summary>
        /// Lists the contents of a remote directory, authenticating with a plain-text password.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">default: &quot;/&quot;</param>
        /// <param name="NumberOfFiles">Maximum number of entries to return. 0 returns all entries.</param>
        /// <param name="List">Directory entries found.</param>
        public void List(string IP, int Port, string Username, string Paword, string Path, int NumberOfFiles, out List<RemoteItem> List)
        {
            List = new List<RemoteItem>();

            using (var sftp = new SftpClient(IP, Port, Username, Paword))
            {
                try
                {
                    sftp.Connect();
                    int idx = 0;
                    foreach (var file in sftp.ListDirectory(Path))
                    {
                        if (idx >= NumberOfFiles && NumberOfFiles > 0)
                            break;

                        List.Add(ToRemoteItem(file));

                        idx++;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    sftp.Disconnect();
                }
            }
        }

        /// <summary>
        /// Uploads a file to a remote path, authenticating with a plain-text password.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">Remote destination path.</param>
        /// <param name="Data">File content to upload.</param>
        public void Put(string IP, int Port, string Username, string Paword, string Path, byte[] Data)
        {
            using (var sftp = new SftpClient(IP, Port, Username, Paword))
            {
                try
                {
                    sftp.Connect();
                    using (var source = new MemoryStream(Data))
                    {
                        sftp.UploadFile(source, Path); // Optional: canOverride
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    sftp.Disconnect();
                }
            }
        }

        /// <summary>
        /// Downloads a remote file, authenticating with a plain-text password.
        /// </summary>
        /// <param name="IP">host (e.g. &quot;127.0.0.1&quot;)</param>
        /// <param name="Port">default: 22</param>
        /// <param name="Username">SSH username.</param>
        /// <param name="Paword">Plain text password.</param>
        /// <param name="Path">Remote file path to download.</param>
        /// <param name="Data">Downloaded file content.</param>
        public void Get(string IP, int Port, string Username, string Paword, string Path, out byte[] Data)
        {
            using (var sftp = new SftpClient(IP, Port, Username, Paword))
            {
                try
                {
                    sftp.Connect();
                    using (var buffer = new MemoryStream())
                    {
                        sftp.DownloadFile(Path, buffer);
                        Data = buffer.ToArray();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    sftp.Disconnect();
                }
            }
        }

        /// <summary>
        /// Finds a directory entry by exact name within Path, without throwing when it is absent.
        /// </summary>
        private static void FindByName(SftpClient sftp, string path, string fileName, out RemoteItem file, out bool found)
        {
            file = new RemoteItem();
            found = false;

            foreach (var entry in sftp.ListDirectory(path))
            {
                if (entry.Name == fileName)
                {
                    file = ToRemoteItem(entry);
                    found = true;
                    break;
                }
            }
        }

        private static RemoteItem ToRemoteItem(Renci.SshNet.Sftp.ISftpFile file)
        {
            return new RemoteItem(
                file.Name,
                Long2Int(file.Attributes.Size),
                file.IsDirectory,
                file.IsSymbolicLink,
                file.LastAccessTime,
                file.LastWriteTime,
                file.Attributes.Size);
        }

        /// <summary>
        /// Clamps a 64-bit size to Int32 range for the legacy ssSizeInBytes field.
        /// Files larger than 2 GB report int.MaxValue here; use ssSizeInBytesLong for the exact value.
        /// </summary>
        private static int Long2Int(long v)
        {
            if (v >= int.MaxValue) return int.MaxValue;
            else return Convert.ToInt32(v);
        }
    }
}
