using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AlinKwon.Helpers.StringUtil
{
    internal class MangedPtr : IDisposable
    {
        public GCHandle Address { get; set; } = default(GCHandle);
        public int Length { get; set; } = 0;

        public void Dispose()
        {
            if (Address != default(GCHandle))
            {
                Address.Free();
                Address = default(GCHandle);
            }
            Length = 0;
        }
    }

    public interface ISafeStringManager
    {
        void ClearAllRegisterString();
        void ClearStringToX(string managedString);
        void Register(string managedString);
    }

    /// <summary>
    /// ex:
    /// App.ResolveObject<ISafeStringManager>().ClearStringToX(scurityNumber) ;
    /// </summary>
    public class SafeStringManager : ISafeStringManager
    {
        public SafeStringManager()
        {
            Targets = new List<MangedPtr>();
        }

        List<MangedPtr> Targets;
        
        void ISafeStringManager.ClearAllRegisterString()
        {
            if(Targets.Count <= 0) { return; }
            try
            {
                foreach (var mp in Targets)
                {
                    try
                    {
                        if (mp.Address != default(GCHandle) && mp.Length > 0)
                        {
                            for (int i = 0; i < mp.Length; i++)
                            {
                                Marshal.WriteInt16(mp.Address.AddrOfPinnedObject(), i * 2, '*');
                                Marshal.WriteInt16(mp.Address.AddrOfPinnedObject(), i * 2, 'X');
                                Marshal.WriteInt16(mp.Address.AddrOfPinnedObject(), i * 2, '*');
                            }
                        }
                    }
                    finally
                    {
                        mp.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                if(Debugger.IsAttached)Debugger.Break();
            }
            finally
            {
                Targets.Clear();
            }
        }

        void ISafeStringManager.ClearStringToX(string managedString)
        {
            if (string.IsNullOrEmpty(managedString)) { return; }
            var address = GCHandle.Alloc(managedString, GCHandleType.Pinned);
            try
            {
                int length = managedString.Length;
                if (address != default(GCHandle) && length > 0)
                {
                    for (int i = 0; i < length; i++)
                    {
                        Marshal.WriteInt16(address.AddrOfPinnedObject(), i * 2, '*');
                        Marshal.WriteInt16(address.AddrOfPinnedObject(), i * 2, 'X');
                        Marshal.WriteInt16(address.AddrOfPinnedObject(), i * 2, '*');
                    }
                }
            }
            catch (Exception)
            {
                if(Debugger.IsAttached)Debugger.Break();
            }
            finally
            {
                address.Free();
            }
        }

        void ISafeStringManager.Register(string managedString)
        {
            if (string.IsNullOrEmpty(managedString)) { return; }
            try
            {
                var address = GCHandle.Alloc(managedString, GCHandleType.Pinned);

                //CHECK aready in..
                foreach(var mp in Targets)
                {
                    if(mp.Address.Equals(address))
                    {
                        address.Free();
                        return;
                    }
                }

                Targets.Add(new MangedPtr()
                {
                    Address = address,
                    Length = managedString.Length,
                });
            }
            catch (Exception)
            {
                if(Debugger.IsAttached)Debugger.Break();
            }
        }
    }
}