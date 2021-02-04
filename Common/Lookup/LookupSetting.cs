namespace Sphyrnidae.Common.Lookup
{
    /// <summary>
    /// Required fields that any lookup object should have
    /// </summary>
    public class LookupSetting
    {
        /// <summary>
        /// The primary key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// String value of the key
        /// </summary>
        public string Value { get; set; }
    }
}