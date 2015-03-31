﻿using System.Environment;
using System.IO;
using LibGit2Sharp;

namespace SaveScumAgent
{
    public class GitWrapper
    {


        public static string CloneRepo(string localRepo, string remoteRepo, bool overwrite = false)
        {
            if (Directory.Exists(localRepo) && overwrite)
                ForceDeleteDirectory(localRepo);

            var val = Repository.Clone(remoteRepo, localRepo);
            return (val);
        }

        public static void ForceDeleteDirectory(string path)
        {
            if (!Directory.Exists(path))
                return;

            var directory = new DirectoryInfo(path) { Attributes = FileAttributes.Normal };

            foreach (var info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
            {
                info.Attributes = FileAttributes.Normal;
            }

            directory.Delete(true);
        }

        public static string CloneProfilesRepo()
        {
            try
            {
                using (var repo = new Repository(ProfilesFolder))
                {
                    return ForcePull(ProfilesFolder);
                }
            }
            catch (RepositoryNotFoundException)
            {
                return CloneRepo(ProfilesFolder, Settings.Default.ProfilesRepo);
            }


        }


        public static string ProfilesFolder
        {
            get
            {
                var appDataPath = GetFolderPath(SpecialFolder.ApplicationData);

                var specificFolder = Path.Combine(appDataPath, Settings.Default.ApplicationName);

                var localRepoFolder = Path.Combine(specificFolder, Settings.Default.ProfilesRepoName);

                return localRepoFolder;
            }
        }

        public static string ForcePull(string localRepo)
        {
            MergeResult retval;

            using (var repo = new Repository(localRepo))
            {
                repo.Network.Fetch(repo.Head.Remote);
                //retval = repo.Network.Pull(Sig, ForcedPullOptions);
            }
            return localRepo;
        }

        private static readonly PullOptions ForcedPullOptions = new PullOptions
        {
            MergeOptions = new MergeOptions
            {
                FastForwardStrategy = FastForwardStrategy.FastForwardOnly,
                FileConflictStrategy = CheckoutFileConflictStrategy.Theirs
            }
        };

    }
}