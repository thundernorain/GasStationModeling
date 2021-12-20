
namespace GasStationModeling.core.models
{
    public class Cashbox : IGasStationElement
    {
        public string Image { get; }

        public int LimitCash { get; set; }

        public int CurrentCash { get; set; }
    }
}
