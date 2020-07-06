using System;
using System.ComponentModel;
using System.Text;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;

namespace PostSharp.Tutorials.MVVM.Model
{
    public class AddressModel : ModelBase
    {
        [DisplayName("Address Line 1")]
        [Required]
        public string Line1 { get; set; }

        [DisplayName("Address Line 2")]
        public string Line2 { get; set; }

        [Required]
        public string Town { get; set; }

        public string Country { get; set; }

        public string FullAddress
        {
            get
            {
                var stringBuilder = new StringBuilder();
                if (Line1 != null)
                {
                    stringBuilder.Append(Line1);
                }

                if (Line2 != null)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append("; ");
                    }

                    stringBuilder.Append(Line2);
                }
                if (Town != null)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append("; ");
                    }

                    stringBuilder.Append(Town);
                }
                if (Country != null)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append("; ");
                    }

                    stringBuilder.Append(Country);
                }


                return stringBuilder.ToString();
            }
        }
    }
}