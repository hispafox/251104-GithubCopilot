namespace DemoWpf.Properties
{
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase
    {
   private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));

        public static Settings Default
    {
      get
  {
     return defaultInstance;
  }
      }

   [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool IsDarkMode
        {
            get
            {
return ((bool)(this["IsDarkMode"]));
  }
            set
            {
      this["IsDarkMode"] = value;
}
        }
    }
}
