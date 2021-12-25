using GasStationModeling.core.models;
using GasStationModeling.settings_screen.model;
using System.Collections.Generic;

namespace GasStationModeling.settings_screen.mapper
{
    class TankToFuelTankComboBoxItemMapper
    {
        public List<FuelTankComboBoxItem> MapList(List<Tank> fuels)
        {
            var mappedList = new List<FuelTankComboBoxItem>();

            foreach (var item in fuels)
            {
                mappedList.Add(Map(item));
            }

            return mappedList;
        }

        public FuelTankComboBoxItem Map(Tank fuel)
            => new FuelTankComboBoxItem(fuel.Id, fuel.Name);
    }
}
