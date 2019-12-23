using DataLibrary.Context;
using Models.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Users.Models;

namespace Users.Maintennce
{
    public class CodeGenerator
    {
        AppIdentityDbContext context = null;

        public CodeGenerator(AppIdentityDbContext cx)
        {
            context = cx;
        }

        public int GenerateCodes(int num)
        {
            int n = 0;
            for (int i = 0; i < 5000; i++)
            {
                string codeStr = Get8CharacterRandomString();
                if (!string.IsNullOrWhiteSpace(codeStr))
                {
                    Code code = new Code();
                    code.status = false;
                    code.UserCode = codeStr;

                    context.Codes.Add(code);
                    n += 1;
                }

            }

            int e = 0;

            context.SaveChanges();

            return n;
        }


        public string Get8CharacterRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); // Remove period.
            return path.Substring(0, 8);  // Return 8 character string
        }
    }
}
