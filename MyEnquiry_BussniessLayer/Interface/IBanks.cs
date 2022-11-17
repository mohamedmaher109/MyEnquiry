﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyEnquiry_BussniessLayer.ViewModel;
using MyEnquiry_DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Interface
{
    public interface IBanks
    {
       
        dynamic Get(ModelStateDictionary modelState);
        dynamic GetById(ModelStateDictionary modelState,int Id);
        Task<dynamic> Add(ModelStateDictionary modelState, Banks model);
        Task<dynamic> Edit(ModelStateDictionary modelState, Banks model);
        Task<dynamic> Delete(ModelStateDictionary modelState, int Id);
		Companies GetCompany(int? formId, int? companyId, int? caseTypeId);
	}
}
