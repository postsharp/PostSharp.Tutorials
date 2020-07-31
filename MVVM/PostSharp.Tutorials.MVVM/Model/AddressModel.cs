using System.Text;
using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;

namespace PostSharp.Tutorials.MVVM.Model
{
    public class AddressModel : ModelBase
    {
        [Required]
        public string Line1 { get; set; }

        public string Line2 { get; set; }

        [Required]
        public string Town { get; set; }

        public string Country { get; set; }

        [SafeForDependencyAnalysis]
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