using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ByteSizeLib;

namespace CopyVia
{
    [Cmdlet(VerbsCommon.Copy, "File")]
    [OutputType(typeof(CopyResult))]
    public class CopyViaCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public DirectoryInfo Via { get; set; }
        [Parameter(Mandatory = true)]
        public DirectoryInfo Target { get; set; }
        [Parameter(Mandatory = true)]
        public DirectoryInfo Source { get; set; }

        [Parameter]
        public Boolean Move { get; set; }

        private ByteSize Threshold { get; set; } = ByteSize.Parse("10MB");


        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            ProcessStep(Source, Via, Target);
        }


        private void ProcessStep(DirectoryInfo source, DirectoryInfo via, DirectoryInfo target)
        {
            Console.WriteLine("Process Directory: {0} -> {1}", source.Name, target.Name);
            foreach (DirectoryInfo directory in source.GetDirectories())
            {
                target.CreateSubdirectory(directory.Name);
                ProcessStep(directory, via, target.GetDirectories(directory.Name).Single());
            }

            foreach (FileInfo file in source.GetFiles())
            {
                Console.WriteLine("File: {0} ({1})", file.Name, ByteSize.FromBytes(file.Length));
                if (file.Length < Threshold.Bytes)
                {
                    file.CopyTo(Path.Combine(target.FullName, file.Name));
                    if (Move)
                    {
                        file.Delete();
                    }
                }
                else
                {
                    String tmpPath = Path.Combine(via.FullName, file.Name);
                    file.CopyTo(tmpPath);
                    FileInfo tmpFile = new FileInfo(tmpPath);
                    tmpFile.CopyTo(Path.Combine(target.FullName, file.Name));
                    tmpFile.Delete();
                    if(Move)file.Delete();
                }
            }
        }

    }

    public class CopyResult
    {
        public UInt32 FileCount { get; set; }
        public TimeSpan Duration { get; set; }
        public UInt64 TotalBytes { get; set; }

    }
}
