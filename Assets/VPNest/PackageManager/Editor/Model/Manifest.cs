using System;
using System.Collections.Generic;

namespace NestPackageManager.Model
{
    [Serializable]
    public class Manifest
    {
        public List<BundlePackage> bundles;
    }

    [Serializable]
    public class BundlePackage
    {
        public string bundle_id;
        public string last_modified;
        public string size;
    }

    [Serializable]
    public class PackageData
    {
        public string name;
        public string source;
        public string version;
        public string fullName;
    }

    [Serializable]
    public class PostData
    {
        public string bundle_name;
    }

    [Serializable]
    public class DownlandURL
    {
        public string download_url;
    }
}