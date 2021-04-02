using System.ComponentModel.DataAnnotations;

namespace TimeCats.Models
{
    public class EvalResponse
    {
        [Key]
        public int evalResponseID { get; set; }
        [Required]
        public int evalID { get; set; }
        [Required]
        public int evalTemplateQuestionID { get; set; }
        [Required]
        public int userID { get; set; }

        public string response { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public int evalNumber { get; set; }

        public int questionNumber { get; set; }

        /**Jamison Edit**/
        public int columTotal { get; set; }

        /**End Edit**/
        public double userAvgerage { get; set; } //  Using this for AVG now

        public string questionText { get; set; }
        public string categoryName { get; set; }
    }
}