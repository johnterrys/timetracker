using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TimeCats.Session;
using TimeCats.Models;
using TimeCats.Services;
using TimeCats.DTOs;

namespace TimeCats.Controllers
{
    public class EvalController : HomeController
    {
        public EvalController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {}

        [HttpPost]
        public IActionResult CreateTemplateQuestion([FromBody] EvalTemplateQuestionCategory json)
        {
            var JsonString = json.ToString();
            var evalTemplateQuestionCategory = JsonConvert.DeserializeObject<EvalTemplateQuestionCategory>(JsonString);

            if (IsInstructorForEval(evalTemplateQuestionCategory.evalTemplateID) || IsAdmin())
            {
                var questionID = _evalService.CreateTemplateQuestion(
                    evalTemplateQuestionCategory.evalTemplateQuestionCategoryID,
                    evalTemplateQuestionCategory.evalTemplateID);
                if (questionID > 0) return Ok(questionID);
                return StatusCode(500);
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult CreateTemplate([FromBody] object json)
        {
            var JsonString = json.ToString();
            var user = JsonConvert.DeserializeObject<User>(JsonString);

            if (IsAdmin() || GetUserType() == 'I' && user.userID == GetUserID())
                return Ok(_evalService.CreateTemplate(user.userID));
            return Unauthorized();
        }

        [HttpPost]
        public IActionResult SaveTemplateName([FromBody] object json)
        {
            var JsonString = json.ToString();
            var evalTemplate = JsonConvert.DeserializeObject<EvalTemplate>(JsonString);

            if (GetUserType() == 'I' || IsAdmin()) return Ok(_evalService.SaveTemplateName(evalTemplate));
            return Unauthorized();
        }

        [HttpPost]
        public IActionResult SaveEval([FromBody] object json)
        {
            var JsonString = json.ToString();
            var eval = JsonConvert.DeserializeObject<AdminEval>(JsonString);

            if (IsAdmin()) return Ok(_evalService.SaveEval(eval));
            return Unauthorized();
        }

        [HttpPost]
        public IActionResult CreateTemplateCopy([FromBody] object json)
        {
            var JsonString = json.ToString();
            var evalTemplate = JsonConvert.DeserializeObject<EvalTemplate>(JsonString);
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (GetUserType() == 'I' || IsAdmin())
            {
                if (_evalService.CreateTemplateCopy(user.userID, evalTemplate.evalTemplateID)) return Ok();
                return StatusCode(500);
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] object json)
        {
            var JsonString = json.ToString();
            var evalTemplate = JsonConvert.DeserializeObject<EvalTemplate>(JsonString);

            if (IsInstructorForEval(evalTemplate.evalTemplateID) || IsAdmin())
            {
                var categoryID = _evalService.CreateCategory(evalTemplate.evalTemplateID);
                if (categoryID > 0) return Ok(categoryID);
                return StatusCode(500);
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult EvalResponses([FromBody] object json)
        {
            var JsonString = json.ToString();
            var eval = JsonConvert.DeserializeObject<Eval>(JsonString);
            var evals = new List<Eval>();

            if (IsAdmin() || IsInstructorForCourse(GetCourseForGroup(eval.groupID)))
            {
                evals = _evalService.EvalResponsesA(eval.groupID, eval.userID);
                return Ok(evals);
            }

            if (eval.userID == GetUserID())
            {
                evals = _evalService.EvalResponses(eval.groupID, eval.userID);
                return Ok(evals);
            }

            return Unauthorized();
        }

        /// <summary>
        ///     Updates a Category
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveCategory([FromBody] object json)
        {
            var JsonString = json.ToString();

            var category = JsonConvert.DeserializeObject<EvalTemplateQuestionCategory>(JsonString);

            if (IsAdmin() || IsInstructorForEval(category.evalTemplateID))
            {
                if (_evalService.SaveCategory(category)) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized(); //Not an Admin or the Instructor for the course, Unauthorized (401)
        }

        /// <summary>
        ///     Delete a Category
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteCategory([FromBody] object json)
        {
            var JsonString = json.ToString();
            var category = JsonConvert.DeserializeObject<EvalTemplateQuestionCategory>(JsonString);
            if (IsAdmin() || IsInstructorForEval(category.evalTemplateID))
            {
                if (_evalService.DeleteCategory(category.evalTemplateQuestionCategoryID)) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized(); //Not an Admin or the Instructor for the course, Unauthorized (401)
        }

        /// <summary>
        ///     Updates a Question
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveQuestion([FromBody] object json)
        {
            var JsonString = json.ToString();

            var question = JsonConvert.DeserializeObject<EvalTemplateQuestion>(JsonString);

            if (IsAdmin() || IsInstructorForEval(question.evalTemplateID))
            {
                if (_evalService.SaveQuestion(question)) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized(); //Not an Admin or the Instructor for the course, Unauthorized (401)
        }

        /// <summary>
        ///     Delete a Question
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteQuestion([FromBody] object json)
        {
            var JsonString = json.ToString();
            var question = JsonConvert.DeserializeObject<EvalTemplateQuestion>(JsonString);
            if (IsAdmin() || IsInstructorForEval(question.evalTemplateID))
            {
                if (_evalService.DeleteQuestion(question.evalTemplateQuestionID)) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized(); //Not an Admin or the Instructor for the course, Unauthorized (401)
        }

        [HttpPost]
        public IActionResult GetTemplates([FromBody] object json)
        {
            var JsonString = json.ToString();

            var course = JsonConvert.DeserializeObject<Course>(JsonString);
            var templates = _evalService.GetTemplates(_courseService.GetInstructorForCourse(course.courseID));

            if (templates.Count > 0) return Ok(templates);
            return NoContent();
        }

        [HttpPost]
        public IActionResult GetTemplatesForInstructor([FromBody] object json)
        {
            var JsonString = json.ToString();
            var user = JsonConvert.DeserializeObject<User>(JsonString);

            if (IsAdmin() || GetUserID() == user.userID)
            {
                var templates = _evalService.GetFullTemplatesForInstructor(user.userID);
                if (templates.Count > 0) return Ok(templates);
            }

            return NoContent();
        }
        
        [HttpPost]
        public IActionResult AssignEvals([FromBody] object json)
        {
            var JsonString = json.ToString();

            var assignEvals = JsonConvert.DeserializeObject<AssignEvals>(JsonString);

            //call and set the inUse flag with another query

            if (_evalService.AssignEvals(assignEvals.projectIDs, assignEvals.evalTemplateID))
            {
                _evalService.SetInUse(assignEvals.evalTemplateID);
                return Ok();
            }

            return StatusCode(500);
        }

        [HttpPost]
        public IActionResult GetEvaluation([FromBody] object json)
        {
            var JsonString = json.ToString();
            var eval = JsonConvert.DeserializeObject<Eval>(JsonString);
            if (IsAdmin()) return Ok(_evalService.GetEvaluation(eval.evalID));
            return Unauthorized();
        }

        [HttpPost]
        public IActionResult GetLatestIncompleteEvaluation([FromBody] object json)
        {
            var JsonString = json.ToString();
            var group = JsonConvert.DeserializeObject<Group>(JsonString);

            var evalID = _evalService.GetLatestIncompleteEvaluationID(group.groupID, GetUserID());
            if (evalID > 0) return Ok(_evalService.GetEvaluation(evalID));
            return NoContent();
        }

        [HttpPost]
        public IActionResult CompleteEvaluation([FromBody] object json)
        {
            var JsonString = json.ToString();
            var responses = JsonConvert.DeserializeObject<List<EvalResponse>>(JsonString);
            //Evals eval = JsonConvert.DeserializeObject<Evals>(JsonString);
            var failed = false;
            var evalID = 0;
            //if (GetUserID() == eval.userID)
            //{
            foreach (var response in responses)
            {
                if (evalID == 0) evalID = response.evalID;
                if (!_evalService.SaveResponse(response.userID, response.evalID, response.evalTemplateQuestionID,
                    response.response)) failed = true;
            }

            //}
            if (failed) return StatusCode(500);

            if (!_evalService.CompleteEval(evalID)) return StatusCode(500);

            return Ok();
        }

        [HttpPost]
        public IActionResult GetAllCompleteEvaluations([FromBody] object json)
        {
            var JsonString = json.ToString();

            var group = JsonConvert.DeserializeObject<UserGroup>(JsonString);

            if (IsActiveStudentInGroup(group.groupID))
                //Use logged in users ID if they are a student
                return Ok(_evalService.RandomizeEvaluations(group.groupID, group.userID));
            if (IsAdmin() || IsInstructorForCourse(GetCourseForGroup(group.groupID)))
                //Get passed userID if they are an Admin/Instructor
                return Ok(_evalService.EvalResponsesA(group.groupID, group.userID));
            return Unauthorized();
        }

        [HttpGet]
        public IActionResult GetAllEvaluations()
        {
            if (IsAdmin()) return Ok(_evalService.GetAllEvals());
            return Unauthorized();
        }
    }
}
