using AutoMapper;
using BookPlazaAPI.AppClasses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Net;
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

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        //private readonly IEmailService _emailService;
        public HomeController(ISignIn ISigninrepo, ISignUp signupRepo, IMapper mapper)
        {
            _ISigninrepo = ISigninrepo;
            _ISignUpRepository = signupRepo;
            _mapper = mapper;
            //_emailService = EService;
        }
        [AllowAnonymous]
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


                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                        response.Result = new { user, decryptedEmail };
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

        [AllowAnonymous]
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

                userSignUP.Name = string.IsNullOrWhiteSpace(dtoSignUp.Name) ? "" : dtoSignUp.Name;


                userSignUP.PasswordHash = dtoSignUp.Password;

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
    }
}
