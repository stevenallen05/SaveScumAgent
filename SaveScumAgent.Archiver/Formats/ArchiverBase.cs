﻿using System;
using System.ComponentModel;
using SaveScumAgent.UtilityClasses;

namespace SaveScumAgent.Archiver.Formats
{
    public abstract class ArchiverBase : IDisposable
    {
        public string ArchiveIdentifier { get; protected set; }
        public bool IsArchiving { get; protected set; }
        public PathString DirectoryToArchive { get; set; }
        public PathString ArchivesLocation { get; set; }
        public string GameTitle { get; set; }

        public virtual void Dispose()
        {
        }

        public abstract void Abort();
        public abstract void StartArchivingAsync();
        public event EventHandler<ArchivingEventArgs> ArchiveProgress;
        public event EventHandler<ArchivingEventArgs> ArchivingDone;
        public event EventHandler<ArchivingInterruptedEventArgs> ArchivingError;

        protected virtual void OnArchiveProgress(object sender, ArchivingEventArgs archivingEventArgs)
        {
            ArchiveProgress?.Invoke(this, archivingEventArgs);
        }

        protected virtual void OnArchivingDone(ArchivingEventArgs e)
        {
            IsArchiving = false;
            ArchivingDone?.Invoke(this, e);
        }

        protected virtual void OnArchivingError(ArchivingInterruptedEventArgs e)
        {
            IsArchiving = false;
            ArchivingError?.Invoke(this, e);
        }

        public static ArchiverBase FromFormatEnum(ArchiveFormat type)
        {
            var t = type.GetAttributeOfType<ArchiveFormatAttribute>();
            var instance = (ArchiverBase) Activator.CreateInstance(t.FormatType);
            return instance;
        }
    }

    public enum ArchiveFormat
    {
        [ArchiveFormat(typeof (ZipArchiver))] [Description("Zip")] Zip,
        [ArchiveFormat(typeof (SevenZipArchiver))] [Description("7zip")] SevenZip,
        [ArchiveFormat(typeof (GitArchiver))] [Description("Git")] Git
    }
}