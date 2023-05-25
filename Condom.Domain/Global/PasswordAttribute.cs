using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Condom.Domain.Global.CondEnum;

namespace Condom.Domain.Global
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PasswordAttribute : Attribute
    {
        public PasswordAttribute(int minLen, int maxLen)
        {
            MinLen = minLen;
            MaxLen = maxLen;
        }

        protected int MinLen { get; set; }
        protected int MaxLen { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsValid(object? v)
        {
            string? value = (string?)v;

            if(value == null) return false;

            if (string.IsNullOrEmpty(value)) return false;

            if(value.Length < MinLen) return false;

            if(value.Length > MaxLen) return false;

            if (!value.Any(char.IsDigit)) return false;

            if (!value.Any(char.IsLetter)) return false;

            if (!value.Any(ch => !char.IsLetterOrDigit(ch))) return false;

            return true;
        }
    }
}
