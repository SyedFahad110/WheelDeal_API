using AutoMapper;
using BookPlazaAPI.AppClasses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Cryptography;
using NETCore.MailKit.Core;
using System.Net;
using System.Runtime.CompilerServices;
using WheelDeal_API.DbContexts;
using WheelDeal_API.Models;
using WheelDeal_API.Repositories;
using WheelDeal_API.Repositories.DTO;
using WheelDeal_API.Repositories.Interface;
using WheelDeal_API.Utilitties;

namespace WheelDeal_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HomeController : ControllerBase
    {
        private readonly ISignIn _ISigninrepo;
        private readonly ISignUp _ISignUpRepository;
        private readonly IBodyType _IBodyTypeRepo;
        private readonly IBrand _IBrandRepo;
        private readonly IDriveType _IDriveTypeRepo;
        private readonly IFuelType _IFuelTypeRepo;
        private readonly IModels _IModelsRepo;
        private readonly IYear _IYearRepo;
        private readonly ICondition _IConditionRepo;
        private readonly ICarColors _ICarColorsRepo;
        private readonly ICarCylinders _ICarCylindersRepo;
        private readonly ITransmission _ITransmissionRepo;
        private readonly IFeatures _IFeaturesRepo;
        

        //private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        //private readonly IEmailService _emailService;
        public HomeController(ISignIn ISigninrepo, ISignUp signupRepo, IBodyType IBodyTypeRepo, IBrand IBrandRepo, IDriveType IDriveTypeRepo, IFuelType IFuelTypeRepo, IModels IModelsRepo, IMapper mapper, IYear IYearRepo, ICondition IConditionRepo, ICarColors ICarColorsRepo, ICarCylinders ICarCylindersRepo, ITransmission ITransmissionRepo, IFeatures IFeaturesRepo)
        {
            _ISigninrepo = ISigninrepo;
            _ISignUpRepository = signupRepo;
            _IBodyTypeRepo  = IBodyTypeRepo;
            _IBrandRepo = IBrandRepo;
            _IDriveTypeRepo = IDriveTypeRepo;
            _IFuelTypeRepo = IFuelTypeRepo;
            _IModelsRepo = IModelsRepo;
            _IYearRepo = IYearRepo;
            _IConditionRepo = IConditionRepo;
            _ICarColorsRepo = ICarColorsRepo;
            _ICarCylindersRepo = ICarCylindersRepo;
            _ITransmissionRepo = ITransmissionRepo;
            _IFeaturesRepo = IFeaturesRepo;
            _mapper = mapper;
            //_emailService = EService;
        }

        [HttpPost("login")]

        public async Task<APIResponse> Login([FromBody] SignInModel signInModel)
        {
            var response = new APIResponse();

            try
            {
                var encryptedEmails = await _ISignUpRepository.GetAllEncryptedEmailAsync();

                foreach (var encryptedEmail in encryptedEmails)
                {
                    string decryptedEmail = null;

                    if (!GeneralClass.IsHexString(encryptedEmail))
                    {
                        decryptedEmail = await GeneralClass.DecryptAsync(GeneralClass.HexStringToByteArray(encryptedEmail));
                    }
                    else
                    {
                        decryptedEmail = encryptedEmail; // already decrypted;
                    }

                    if (!string.IsNullOrEmpty(decryptedEmail) &&
                        decryptedEmail.Equals(signInModel.Email, StringComparison.OrdinalIgnoreCase))
                    {
                        var user = await _ISignUpRepository.GetUserByEmail(encryptedEmail); // Note: encryptedEmail used here
                        if (user == null)
                        {
                            response.StatusCode = HttpStatusCode.NotFound;
                            response.IsSuccess = false;
                            response.ErrorMessages = new List<string> { "User not found." };
                            return response;
                        }

                        string hashPass = GeneralClass.HashPassword(signInModel.Password);

                        if (hashPass != user.PasswordHash)
                        {
                            response.StatusCode = HttpStatusCode.Unauthorized;
                            response.IsSuccess = false;
                            response.ErrorMessages = new List<string> { "Invalid password." };
                            return response;
                        }

                        string token = GeneralClass.GenerateJwtToken();
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                        response.Result = new { user, decryptedEmail, jwtToken = token };
                        response.Messages = new List<string> { "Login successful." };
                        return response;
                    }
                }

                response.StatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { "No account found for this email." };
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { ex.Message };
                return response;
            }
        }

        //null araha hai

        [HttpPost("UserSignup")]
        public async Task<ActionResult<APIResponse>> Signup(DTOSignUp dtoSignUp)
        {
            var response = new APIResponse();

            try
            {
                if (dtoSignUp == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    response.ErrorMessages = new List<string> { "Invalid data" };
                    return BadRequest(response);
                }

                //** Existting Email ki condition laga dena yaha
                var existingEmail = await _ISignUpRepository.GetAllEncryptedEmailAsync();
                foreach (var encrypted in existingEmail)
                {
                    var decryptedEmail = await GeneralClass.DecryptAsync(GeneralClass.HexStringToByteArray(encrypted));
                    if (decryptedEmail.Equals(dtoSignUp.Email, StringComparison.OrdinalIgnoreCase))
                    {
                        response.StatusCode = HttpStatusCode.Conflict; // 409 Conflict
                        response.IsSuccess = false;
                        response.ErrorMessages = new List<string> { "Account Already Exist. Please use a different email." };
                        return Conflict(response);
                    }
                }
                //***************OK

                var existingPhone = await _ISignUpRepository.GetAllEncryptedPhoneAsync();
                foreach (var encrypted in existingPhone)
                {
                    var decryptedEmail = await GeneralClass.DecryptAsync(GeneralClass.HexStringToByteArray(encrypted));
                    if (decryptedEmail.Equals(dtoSignUp.Phone, StringComparison.OrdinalIgnoreCase))
                    {
                        response.StatusCode = HttpStatusCode.Conflict; // 409 Conflict
                        response.IsSuccess = false;
                        response.ErrorMessages = new List<string> { "Account Already Exist. Please use a different phone." };
                        return Conflict(response);
                    }
                }

                // Map DTO to entity
                var userSignUP = _mapper.Map<SignUp>(dtoSignUp);

                //userSignUP.Name = string.IsNullOrWhiteSpace(dtoSignUp.Name) ? "" : dtoSignUp.Name; ye 


                var result = await _ISignUpRepository.AddAsync(userSignUP);

                //Chatgpt sy achasa koi format bnwa lena okye wali hi email dena or bolna replace krdy
                //var subject = "Account Verification Pending";
                //var message = $@"Dear User,
                //        <br />
                //        <br />
                //        Thank you for registering as a seller with True Readers!
                //        <br />
                //        <br />
                //        We’ve received your registration, and your seller account is currently under review. Our team is working on verifying your information, and once approved, you’ll have full access to all the features on our platform.
                //        <br />
                //        <br />
                //        Please allow some time for the verification process to be completed. You’ll be notified once your account has been successfully approved.
                //        <br />
                //        <br />
                //        If you have any questions or need assistance, feel free to reach out to our support team. We’re here to help!
                //        <br />
                //       <br />
                //        Thank you for your patience, and we look forward to working with you!
                //        <br />
                //        <br />
                //        <h4>Best Regards</h4>,
                //        The True Readers Team";

                //await _emailService.SendAsync(dtoSignUp.Email, subject, message);


                if (result == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMessages = new List<string> { "Failed to insert user" };
                    return StatusCode((int)HttpStatusCode.InternalServerError, response);
                }

                response.IsSuccess = true;
                response.Result = result;
                response.Messages = new List<string> { "Account Created successfully" };
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages = new List<string> { ex.Message };
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }

            return Ok(response);
        }

        [HttpGet("GetBodyType")] 
        public async Task<IActionResult> GetAllBodyTypes()
        {
            var bodytypes = await _IBodyTypeRepo.GetBodyTypesAsync();
            return Ok(bodytypes);

        }

        [HttpGet("GetBrand")]

        public async Task<IActionResult> GetAllBrand()
        {
            var brand = await _IBrandRepo.GetAllBrandAsync();

            return Ok(brand);
        }

        [HttpGet("GetDriveType")]
        public async Task<IActionResult> GetAllDriveTypes()
        {
            var drivetype = await _IDriveTypeRepo.GetDriveTypes();

            return Ok(drivetype);
        }

        [HttpGet("GetFuelType")]

        public async Task<IActionResult> GetAllFuelTypes()
        {
            var fueltype = await _IFuelTypeRepo.GetAllFuelType();

            return Ok(fueltype);
        }

        [HttpGet("GetCarModels")]

        public async Task<IActionResult> GetAllModels()
        {
            var models = await _IModelsRepo.GetAllModel();

            return Ok(models);
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {

            var user = await _ISignUpRepository.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

                return Ok(user);
        }


        [HttpGet("GetCondition")]

        public async Task<IActionResult> GetCondition()
        {
            var condition = await _IConditionRepo.GetConditionsAsync();
            return Ok(condition);

               
        }

        [HttpGet("GetAllYear")]

        public async Task<IActionResult> GetAllYear()
        {
            var Year = await _IYearRepo.GetAllYearAsync();
            return Ok(Year);
        }

        [HttpGet("GetAllCarColors")]

        public async Task<IActionResult> GetCarColors()
        {
            var colors = await _ICarColorsRepo.GetAllCarColorsAsync();

            return Ok(colors);
        }


        [HttpGet("GetAllCarCylinder")]

        public async Task<IActionResult> GetAllCylinders()
        {
            var cylinders = await _ICarCylindersRepo.GetAllCarCylinders();
            return Ok(cylinders);
        }

        [HttpGet("GetAllFeatures")]

        public async Task<IActionResult> GetAllFeature()
        {
            var features = await _IFeaturesRepo.GetAllFeatures();
                return Ok(features);
          
        }

        [HttpGet("GetTransmission")]

        public async Task<IActionResult> GetAllTransmission()
        {
            var transmission = await _ITransmissionRepo.GetTransmissionsAsync();

            return Ok(transmission);
        }

        [HttpDelete("DeleteUser")]
        public async Task<ActionResult<APIResponse>> DeleteUser(int id, string password)
        {
            var response = new APIResponse();

            try
            {
                string hasPass = GeneralClass.HashPassword(password);

                var result = await _ISignUpRepository.DeleteAsync(id, hasPass);

                if (result == null)
                {
                    response.StatusCode = HttpStatusCode.Conflict; 
                    response.IsSuccess = false;
                    response.ErrorMessages = new List<string> { "User not found or password incorrect." };
                    return Conflict(response);
                }

                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;
                response.ErrorMessages = new List<string> { "User marked as deleted successfully." };
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages = new List<string> { ex.Message };
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }

            return Ok(response);
        }
    }
    
}
