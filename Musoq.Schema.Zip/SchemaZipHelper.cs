﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Musoq.Schema.DataSources;

namespace Musoq.Schema.Zip
{
    public static class SchemaZipHelper
    {
        public static readonly IDictionary<string, int> NameToIndexMap;
        public static readonly IDictionary<int, Func<ZipArchiveEntry, object>> IndexToMethodAccessMap;
        public static readonly ISchemaColumn[] SchemaColumns;

        static SchemaZipHelper()
        {
            NameToIndexMap = new Dictionary<string, int>
            {
                {nameof(ZipArchiveEntry.Name), 0},
                {nameof(ZipArchiveEntry.FullName), 1},
                {nameof(ZipArchiveEntry.CompressedLength), 2},
                {nameof(ZipArchiveEntry.LastWriteTime), 3},
                {nameof(ZipArchiveEntry.Length), 4},
                {nameof(ZipArchiveEntry), 5},
                {nameof(FileInfo), 6},
                {"IsDirectory", 7}
            };

            IndexToMethodAccessMap = new Dictionary<int, Func<ZipArchiveEntry, object>>
            {
                {0, info => info.Name},
                {1, info => info.FullName},
                {2, info => info.CompressedLength},
                {3, info => info.LastWriteTime},
                {4, info => info.Length},
                {5, info => info},
                {6,
                    info =>
                    {
                        //using (var mmf = MemoryMappedFile.CreateNew(null, info.CompressedLength,
                        //    MemoryMappedFileAccess.Write))
                        //{

                        //}
                        var extractPath = Path.Combine(Path.GetTempPath(), info.Name);
                        info.ExtractToFile(extractPath, true);

                        return new FileInfo(extractPath);
                    }
                },
                {7, info => info.Name == string.Empty}
            };

            SchemaColumns = new ISchemaColumn[]
            {
                new SchemaColumn(nameof(ZipArchiveEntry.Name), 0, typeof(string)),
                new SchemaColumn(nameof(ZipArchiveEntry.FullName), 1, typeof(string)),
                new SchemaColumn(nameof(ZipArchiveEntry.CompressedLength), 2, typeof(long)),
                new SchemaColumn(nameof(ZipArchiveEntry.LastWriteTime), 3, typeof(DateTimeOffset)),
                new SchemaColumn(nameof(ZipArchiveEntry.Length), 4, typeof(long)),
                new SchemaColumn(nameof(ZipArchiveEntry), 5, typeof(ZipArchiveEntry)),
                new SchemaColumn(nameof(FileInfo), 6, typeof(FileInfo)),
                new SchemaColumn(nameof(Boolean), 7, typeof(bool)) 
            };
        }
    }
}