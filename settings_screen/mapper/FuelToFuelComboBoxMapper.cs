using GasStationModeling.core.models;
using GasStationModeling.settings_screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStationModeling.settings_screen.mapper
{
    class FuelToFuelComboBoxMapper
    {
        public List<FuelComboBoxItem> MapList(List<Fuel> fuels)
        {
            var mappedList = new List<FuelComboBoxItem>();

            foreach (var item in fuels)
            {
                mappedList.Add(Map(item));
            }

            return mappedList;
        }

        public FuelComboBoxItem Map(Fuel fuel) 
            => new FuelComboBoxItem(fuel.Id, fuel.Name);
    }
}
