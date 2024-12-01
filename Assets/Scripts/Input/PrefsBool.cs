namespace UnderworldExporter.Game
{
    public class PrefsBool : PrefsValue<bool?>
    {
        public PrefsBool(string prefsKey) : base(prefsKey)
        {
        }

        protected override bool? GetSharedPrefsValue()
        {
            return PlayerPrefsExtensions.GetBool(_prefsKey, false);
        }

        protected override void SetSharedPrefsValue(bool? value)
        {
            PlayerPrefsExtensions.SetBool(_prefsKey, value.Value);
        }
    }
}