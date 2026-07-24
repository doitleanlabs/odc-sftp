using OutSystems.ExternalLibraries.SDK;
using System;

namespace DoiTLean.SFTP.Structures
{
    /// <summary>
    /// Represents a single remote file or directory entry returned by List/Search actions.
    /// </summary>
    [OSStructure(Description = "Remote Item")]
    public struct RemoteItem
    {

        [OSStructureField(DataType = OSDataType.Text, Description = "File or directory name.")]
        public string ssFilename;

        /// <remarks>
        /// Kept as Int32 for backward compatibility with existing OutSystems consumers.
        /// Values above int.MaxValue are clamped here; use <see cref="ssSizeInBytesLong"/> for the exact size.
        /// </remarks>
        [OSStructureField(DataType = OSDataType.Integer, Description = "File size in bytes, clamped to Int32.MaxValue for files larger than 2 GB. See ssSizeInBytesLong for the exact value.")]
        public int ssSizeInBytes;

        [OSStructureField(DataType = OSDataType.Boolean, Description = "True when the entry is a directory.")]
        public bool ssIsDir;

        [OSStructureField(DataType = OSDataType.Boolean, Description = "True when the entry is a symbolic link.")]
        public bool ssIsLink;

        [OSStructureField(DataType = OSDataType.DateTime, Description = "Last access time reported by the SFTP server.")]
        public DateTime ssCreated;

        [OSStructureField(DataType = OSDataType.DateTime, Description = "Last modification time reported by the SFTP server.")]
        public DateTime ssModified;

        /// <summary>
        /// Exact file size in bytes. Added to support files larger than 2 GB, which <see cref="ssSizeInBytes"/> clamps.
        /// Optional/non-mandatory so existing consumers built against the previous structure keep working unchanged.
        /// </summary>
        [OSStructureField(DataType = OSDataType.LongInteger, IsMandatory = false, DefaultValue = "0", Description = "Exact file size in bytes (not clamped). Use this instead of ssSizeInBytes for files larger than 2 GB.")]
        public long ssSizeInBytesLong;

        /// <summary>
        /// Constructs a RemoteItem.
        /// </summary>
        public RemoteItem(string Filename, int SizeInBytes, bool IsDir, bool IsLink, DateTime Created, DateTime Modified, long SizeInBytesLong = 0) : this()
        {
            ssFilename = Filename ?? string.Empty;
            ssSizeInBytes = SizeInBytes;
            ssIsDir = IsDir;
            ssIsLink = IsLink;
            ssCreated = Created;
            ssModified = Modified;
            ssSizeInBytesLong = SizeInBytesLong;
        }
    }

}


