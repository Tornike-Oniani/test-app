using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiDesktopApp2.DataAccess.Entities
{
    /// <summary>
    /// რამდენი დრო დახარჯა თითო სურათის ვარიანტზე
    /// რამდენი დრო დაიხარჯა სურათის ნაკრებზე
    /// ამოუცნო თუ არა სურათი
    /// რამდენჯერ გამოტოვა სურათის ვარიანტი
    /// </summary>
    public class Result
    {
        [Key]
        public int Id { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        public List<ResultImageSetTime> ImageSetTimes { get; set; }
        public List<ResultImageVariantTime> ImageVariantTimes { get; set; }
    }
}
