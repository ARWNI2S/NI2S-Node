
namespace ARWNI2S.Infrastructure.Collections.Rendering
{
    public class SelectListItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }
    }

    public class SelectList : List<SelectListItem>
    {
        public SelectList(IEnumerable<object> values, string valuePropertyName, string textPropertyName, object selectedValue = null)
        {
            foreach (var item in values)
            {
                var valueProperty = item.GetType().GetProperty(valuePropertyName);
                var textProperty = item.GetType().GetProperty(textPropertyName);

                if (valueProperty == null || textProperty == null)
                    throw new ArgumentException("Invalid property names for value or text.");

                var value = valueProperty.GetValue(item)?.ToString();
                var text = textProperty.GetValue(item)?.ToString();

                Add(new SelectListItem
                {
                    Value = value,
                    Text = text,
                    Selected = selectedValue != null && selectedValue.Equals(value)
                });
            }
        }

        public int SelectedIndex
        {
            get { return FindIndex(0, item => item.Selected); }
            set
            {
                if (value < 0 || value >= this.Count)
                    throw new ArgumentOutOfRangeException(nameof(value));

                foreach (var item in this)
                {
                    item.Selected = false;
                }

                this[value].Selected = true;
            }
        }
    }

}
