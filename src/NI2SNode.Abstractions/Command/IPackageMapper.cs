﻿using System;
using System.Threading.Tasks;

namespace NI2S.Node.Command
{
    public interface IPackageMapper<PackageFrom, PackageTo>
    {
        PackageTo Map(PackageFrom package);
    }
}
