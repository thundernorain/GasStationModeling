using GasStationModeling.core.models;
using GasStationModeling.settings_screen.model;
using System.Collections.Generic;

namespace GasStationModeling.settings_screen.mapper
{
    class FuelDispenserToFuelDispenserComboBoxItemMapper
    {
        public List<FuelDispenserComboBoxItem> MapList(List<FuelDispenser> fuels)
        {
            var mappedList = new List<FuelDispenserComboBoxItem>();

            foreach (var item in fuels)
            {
                mappedList.Add(Map(item));
            }

            return mappedList;
        }

        public FuelDispenserComboBoxItem Map(FuelDispenser fuel)
            => new FuelDispenserComboBoxItem(fuel.Id, fuel.Name);
    }
}
