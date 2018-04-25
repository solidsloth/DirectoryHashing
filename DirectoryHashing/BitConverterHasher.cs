using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DirectoryHasher
{
    public class BitConverterHasher : IHasher
    {
        /// <summary>
        /// Gets a cumulative hash of all paths and files within a directory.
        /// </summary>
        /// <param name="path">A path to the directory to hash.</param>
        /// <param name="algorithm">A hashing algorithm implementation.</param>
        /// <param name="ct">A token for cancelling the asynchronous operation.</param>
        /// <returns>An awaitable task that returns a lower case string hash of</returns>
        public async Task<string> GetDirectoryHash(
            string path,
            HashAlgorithm algorithm,
            CancellationToken ct = default)
        {
            foreach (string filePath in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories)
                .OrderBy(p => p))
            {
                string relativePath = Path.GetRelativePath(path, filePath);

                // Hash the relative path since file names are significant.
                byte[] pathBytes = Encoding.UTF8.GetBytes(relativePath);
                algorithm.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);

                byte[] fileBytes = await File.ReadAllBytesAsync(filePath, ct);
                algorithm.TransformBlock(fileBytes, 0, fileBytes.Length, fileBytes, 0);
            }

            algorithm.TransformFinalBlock(Array.Empty<byte>(), 0, 0);

            return BitConverter.ToString(algorithm.Hash).Replace("-", "").ToLower();
        }
    }
}
