using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalIMCSPA.Enums
{
    public enum DietaryFlags : byte
    {
        [Display(Name = "Vegan")]
        Vegan = 0,
        [Display(Name = "Lactose Free")]
        LactoseFree = 1,
        [Display(Name = "Low Fat")]
        LowFat = 2,
    }
}
