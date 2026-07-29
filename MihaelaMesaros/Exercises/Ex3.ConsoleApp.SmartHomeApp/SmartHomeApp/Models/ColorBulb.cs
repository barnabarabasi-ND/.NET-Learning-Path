using SmartHomeApp.Models.Interfaces;

namespace SmartHomeApp.Models
{
    internal class ColorBulb : LightBulb, IDimmable, IColorControl
    {
        public string Color { get; private set; } = "White";

        public void SetColor(string color)
        {
            Color = color;
        }

        public override string GetDetails()
        {
            return $"{base.GetDetails()}, Color: {Color}";
        }

    }
}
