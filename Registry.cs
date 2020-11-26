using Microsoft.Win32;

namespace RA34GBPatchInstaller
{
    class Registry
    {
        public static string GetRA3Path()
        {
            using var view32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            using var ra3 = view32.OpenSubKey("Software\\Electronic Arts\\Electronic Arts\\Red Alert 3");
            return ra3?.GetValue("Install Dir") as string ?? string.Empty;
        }

        public static void SetRA3Path(string path)
        {
            using var view32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            using var ra3 = view32.OpenSubKey("Software\\Electronic Arts\\Electronic Arts\\Red Alert 3", writable: true);
            if (ra3 == null)
            {
                using var newra3 = view32.CreateSubKey("Software\\Electronic Arts\\Electronic Arts\\Red Alert 3", writable: true);
                newra3.SetValue("Install Dir", path, RegistryValueKind.String);
                newra3.SetValue("UseLocalUserMap", 0, RegistryValueKind.DWord);
                return;
            }
            ra3.SetValue("Install Dir", path);
        }
    }
}
