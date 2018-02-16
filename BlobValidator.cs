using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Helium24
{
    internal class BlobValidator
    {
        private const string BlobsFolder = "./Blobs";

        internal static bool ValidateHash(string hash)
        {
            using (SHA1 hashFunction = SHA1.Create())
            {
                byte[] conglomerateHashes = BlobValidator.GetHashValue(BlobValidator.BlobsFolder, hashFunction);
                string computedHash = Convert.ToBase64String(hashFunction.ComputeHash(conglomerateHashes));

                Global.Log($"Computed blob hash {computedHash}");
                Global.Log($"Provided blob hash {hash}");
                return computedHash.Equals(hash);
            }
        }

        private static byte[] GetHashValue(string folder, SHA1 hashFunction)
        {
            byte[] combinedHashes = new byte[0];
            foreach (string directory in Directory.GetDirectories(folder).OrderBy(dir => dir))
            {
                byte[] directoryHashes = GetHashValue(directory, hashFunction);
                Array.Resize(ref combinedHashes, combinedHashes.Length + directoryHashes.Length);
                Array.Copy(directoryHashes, 0, combinedHashes, combinedHashes.Length - directoryHashes.Length, directoryHashes.Length);
            }

            foreach (string file in Directory.GetFiles(folder).OrderBy(file => file))
            {
                byte[] hashValue = hashFunction.ComputeHash(File.ReadAllBytes(file));
                Array.Resize(ref combinedHashes, combinedHashes.Length + hashValue.Length);
                Array.Copy(hashValue, 0, combinedHashes, combinedHashes.Length - hashValue.Length, hashValue.Length);
            }

            return combinedHashes;
        }
    }
}