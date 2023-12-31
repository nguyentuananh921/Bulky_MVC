﻿using BulkyBook.Models.Models;
using BulkyBook.Models.Models.Product;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.ViewModel
{
    public class ProductVM
    {
        public Product Product { get; set; }

        //Need Package <PackageReference Include="Microsoft.AspNetCore.Mvc.ViewFeatures" Version="2.2.0" />
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

    }
}
