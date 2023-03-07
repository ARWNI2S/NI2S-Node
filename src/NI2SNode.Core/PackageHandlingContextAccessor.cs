﻿using NI2S.Node;

namespace SuperSocket.Server
{


    public class PackageHandlingContextAccessor<TPackageInfo> : IPackageHandlingContextAccessor<TPackageInfo>
    {
        private static AsyncLocal<PackageHandlingContextHolder> AppSessionCurrent { get; set; } = new AsyncLocal<PackageHandlingContextHolder>();

        /// <inheritdoc/>
        PackageHandlingContext<IAppSession, TPackageInfo>? IPackageHandlingContextAccessor<TPackageInfo>.PackageHandlingContext
        {
            get
            {
                return AppSessionCurrent.Value?.Context;
            }
            set
            {
                var holder = AppSessionCurrent.Value;
                if (holder != null)
                {
                    holder.Context = null;
                }

                if (value != null)
                {
                    AppSessionCurrent.Value = new PackageHandlingContextHolder { Context = value };
                }
            }
        }

        private class PackageHandlingContextHolder
        {
            public PackageHandlingContext<IAppSession, TPackageInfo>? Context { get; set; }
        }
    }


}
