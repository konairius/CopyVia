using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hashing
{
    public class HashInfo
    {
        [Key]
        public Int32 HashId { get; set; }
        [Required]
        public String Algorithm { get; set; }
        [Required]
        public UInt64 Size { get; set; }
        [Required]
        public DateTime MTime { get; set; }
        [Required]
        public Byte[] PathHash { get; set; }
        [Required]
        public Byte[] Hash { get; set; }


        public static HashInfo FromFile(String path)
        {
            HashInfo info = new HashInfo();
            using (HashAlgorithm algorithm = SHA256.Create())
            {
                info.Algorithm = algorithm.ToString();
                info.PathHash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(path));
                FileInfo fieInfo = new FileInfo(path);
                info.MTime = fieInfo.LastWriteTime;
                info.Size = (UInt64) fieInfo.Length;
            }

            using (FileStream file = File.OpenRead(path))
            using (HashAlgorithm algorithm = SHA256.Create())
            {
                info.Hash = algorithm.ComputeHash(file);
            }

            return info;
        }
    }
}
