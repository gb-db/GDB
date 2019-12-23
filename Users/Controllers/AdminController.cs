using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Users.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Models.Users;
using Models.ViewModels;
using DataLibrary.Context;

namespace Users.Controllers
{

    [Authorize(Roles = "Admins")]
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;
        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;
        private AppIdentityDbContext context;

        public AdminController(UserManager<AppUser> usrMgr,
                IUserValidator<AppUser> userValid,
                IPasswordValidator<AppUser> passValid,
                IPasswordHasher<AppUser> passwordHash,
                AppIdentityDbContext ctx)
        {
            userManager = usrMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;
            context = ctx;
        }

        public ViewResult Index() => View(userManager.Users);

        [AllowAnonymous]
        public ViewResult Create() => View(new CreateModel());

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            //model.success = true;
            //model.message = "AAAAAAAAAAAAAAAAAA";
            if (ModelState.IsValid)
            {
                int codeId_ = 0;
                int.TryParse(model.CodeId?.Trim(), out codeId_);
                string msg = CheckProvidedCode(codeId_, model.Code ?? "");
                if (msg == "")
                {
                    AppUser user = new AppUser
                    {
                        UserName = model.Name,
                        Email = model.Email
                    };
                    IdentityResult result
                        = await userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        if(!UpdateUser_CodeStatus(user,codeId_, model.Code))
                        {
                            model.message = "There was problem to update Code table!";
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    model.message = msg;
                }


            }
            if (model.success)
            {
                return Json(new { success = model.success, message = model.message });
            }
            else
            {
                return PartialView("Create", model);
            }
        }

        private bool UpdateUser_CodeStatus(AppUser user,int codeId,string codeStr)
        {
            bool retVal = false;
            bool retValC = false;
            bool retValU = false;
            Code code = context.Codes.Where(t => t.Id == codeId).FirstOrDefault();
            if (code != null)
            {
                code.status = true;
                context.SaveChanges();
                retValC = true;
            }

            AppUser userObj = userManager.Users.Where(t => t.Id == user.Id).FirstOrDefault();
            if (userObj != null)
            {
                userObj.UserCode = codeStr;
                IdentityResult res =  userManager.UpdateAsync(userObj).Result;
                retValU = true;
            }

            if(retValC && retValU)
            {

                retVal = true;
            }

            return retVal;
        }

        private string CheckProvidedCode(int codeId, string code)
        {
            string retStr = "";
            Code codeF = context.Codes.Where(t => t.UserCode == code.Trim() && t.Id == codeId).FirstOrDefault();
            if (codeF != null && codeF.status == false)
            {
            }
            else
            {
                retStr = "There was problem with supplied code!";
            }

            //var v = (from u in userManager.Users
            //         join c in context.Codes on u.UserCode equals c.UserCode
            //         select c).ToList();

            return retStr;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Index", userManager.Users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail
                    = await userValidator.ValidateAsync(userManager, user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager,
                        user, password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user,
                            password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }
                if ((validEmail.Succeeded && validPass == null)
                        || (validEmail.Succeeded
                        && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

    }
}
