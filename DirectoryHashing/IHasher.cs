using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DirectoryHasher
{
    public interface IHasher
    {
        Task<string> GetDirectoryHash(
            string path,
            HashAlgorithm algorithm,
            CancellationToken ct = default);
    }
}
