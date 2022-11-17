using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyEnquiry_BussniessLayer.ViewModel.ResponseModels;

namespace MyEnquiry_BussniessLayer.Helper
{
    public class CustomBadRequest
    {
        public static BadRequestObjectResult CustomErrorResponse(ActionContext actionContext)
        {
            ErrorResponse ErrorResponse = new ErrorResponse() { Code = 101, Message = "Validats error", details = new List<DetailsResponse>() };
            foreach (var keyModelStatePair in actionContext.ModelState)
            {
                var key = keyModelStatePair.Key;
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    DetailsResponse detailsResponse = new DetailsResponse() { Key = key };
                    foreach (var error in errors)
                    {
                        detailsResponse.Value += error.ErrorMessage + " ";
                    }
                    ErrorResponse.details.Add(detailsResponse);
                }
            }
            return new BadRequestObjectResult(ErrorResponse);
        }

        public static BadRequestObjectResult CustomExErrorResponse(Exception ex)
        {

            ErrorResponse ErrorResponse = new() { Code = 102, Message = ex.Message };
            ErrorResponse.details = new List<DetailsResponse>
                            {
                               new DetailsResponse
                               {
                                    Key = string.Join(",", "Message"),
                                    Value = string.Join(",", ex.Message)
                               },new DetailsResponse
                               {
                                    Key = string.Join(",", "InnerException"),
                                    Value = string.Join(",", ex.InnerException)
                               },new DetailsResponse
                               {
                                    Key = string.Join(",", "StackTrace"),
                                    Value = string.Join(",", ex.StackTrace)
                               }
                            };
            return new BadRequestObjectResult(ErrorResponse);
        }

        public static BadRequestObjectResult CustomModelStateErrorResponse(ModelStateDictionary ModelState)
        {
            ErrorResponse ErrorResponse = new ErrorResponse() { Code = 103, Message = "Code error" };
            foreach (var modelState in ModelState)
            {
                foreach (var error in modelState.Value.Errors)
                {
                    ErrorResponse.Message = error.ErrorMessage;
                    ErrorResponse.details = new List<DetailsResponse>
                            {
                               new DetailsResponse
                               {
                                    Key = modelState.Key,
                                    Value = error.ErrorMessage
                               }
                            };
                }
            }


            
            return new BadRequestObjectResult(ErrorResponse);
        }





        public static BadRequestObjectResult UnAutrize()
        {
            ErrorResponse ErrorResponse = new ErrorResponse() { Code = 401, Message = "User Not Found" };
          
            return new BadRequestObjectResult(ErrorResponse);
        }




    }
}
