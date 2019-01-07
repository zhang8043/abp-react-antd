namespace Precise.Configuration.Dto
{
    public class ThemeHeaderSettingsDto
    {
        /// <summary>
        /// 内容区域宽度
        /// </summary>
        public string ContentWidth { get; set; }
        /// <summary>
        /// 固定 Header
        /// </summary>
        public bool FixedHeader { get; set; }
        /// <summary>
        /// 下滑时隐藏 Header
        /// </summary>
        public bool SlidingHiddenHeader { get; set; }
    }
}