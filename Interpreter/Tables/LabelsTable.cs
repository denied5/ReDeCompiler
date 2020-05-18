using System;
using System.Collections.Generic;
using System.Text;

namespace DenysRedkoParser.Interpreter.Tables
{
    public class LabelsTable
    {
        public Dictionary<Label, int> Labels { get; set; }

        public void Add(Label label, int adress)
        {
            if (GetAdress(label) != null)
            {
                throw new Exception($"{label.Name} already exist in table");
            }
            Labels[label] = adress;
        }

        public int? GetAdress(Label label)
        {
            foreach (var item in Labels)
            {
                if (label.Name == item.Key.Name)
                {
                    return item.Value;
                }
            }
            return null;
        }

        public int getAdressOrFail(Label label)
        {
            var adress = GetAdress(label);
            if (adress != null)
            {
                return adress.GetValueOrDefault();
            }
            throw new Exception($" Fail to find adress of {label.Name}");
        }
    }
}
