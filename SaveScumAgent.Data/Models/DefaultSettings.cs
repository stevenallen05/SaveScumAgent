﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Data.Models
{
    public class DefaultSettings : GameSettings
    {
        [DefaultValue(false)]
        public bool IsDefault { get; internal set; }

        public DefaultSettings()
        {
            IsDefault = true;
        }

        //TODO: Flesh this out a lot more. Needs to have <Default> and <Current global>
    }
}
