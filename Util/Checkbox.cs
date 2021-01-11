namespace ems.Util
{
    public class Checkbox
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }

        public bool IsSelected
        {
            get
            {
                return Selected;
            }
        }
    }
}