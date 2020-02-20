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

namespace TimeCats.Controllers
{
    public class HomeController : Controller
    {
        private readonly StudentTimeTrackerService _timeTrackerService;
        private readonly CourseService _courseService;
        private readonly ProjectService _projectService;
        private readonly UserService _userService;

        public HomeController(IServiceProvider serviceProvider)
        {
            _timeTrackerService = serviceProvider.GetRequiredService<StudentTimeTrackerService>();
            _courseService = serviceProvider.GetRequiredService<CourseService>();
            _projectService = serviceProvider.GetRequiredService<ProjectService>();
            _userService = serviceProvider.GetRequiredService<UserService>();
        }
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Helper Functions

        /// <summary>
        ///     Returns a hashed version of the passed password
        /// </summary>
        /// <returns></returns>
        public static string GenerateHash(string password)
        {
            var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        ///     Returns the courseID for the passed groupID
        /// </summary>
        /// <returns></returns>
        public int GetCourseForGroup(int groupID)
        {
            return DBHelper.GetCourseForGroup(groupID);
        }

        /// <summary>
        ///     Returns the courseID for the passed projectID
        /// </summary>
        /// <returns></returns>
        public int GetCourseForProject(int projectID)
        {
            var courseId = _projectService.GetProjectById(projectID).CourseID;
            return courseId;
        }

        /// <summary>
        ///     Returns the currently logged in user's userID.
        /// </summary>
        /// <returns></returns>
        public int GetUserID()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return user.userID;

            return 0;
        }

        /// <summary>
        ///     Returns the currently logged in user's type. Otherwise returns null character.
        /// </summary>
        /// <returns></returns>
        public char GetUserType()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return user.type;

            return '\0'; //Null character literal
        }

        /// <summary>
        ///     Returns true if the currently logged in user is an Admin
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return user.type == 'A';

            return false;
        }

        /// <summary>
        ///     Returns true if the logged in user is the instructor for the passed courseID
        /// </summary>
        /// <returns></returns>
        public bool IsInstructorForCourse(int courseID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");
            var courses = _courseService.GetCoursesByUser(user);

            if (courses.Any(c => c.courseID == courseID))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Returns true if the logged in user is the instructor for the passed evalTemplateID
        /// </summary>
        /// <returns></returns>
        public bool IsInstructorForEval(int evalTemplateID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return user.userID == DBHelper.GetInstructorForEval(evalTemplateID);

            return false;
        }

        /// <summary>
        ///     Returns true if the logged in user is a student for the passed courseID
        /// </summary>
        /// <returns></returns>
        public bool IsStudentInCourse(int courseID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");
            var courses = _courseService.GetCourses();

            return courses.Select(c => c.users)
                .Any(u => u.Any(u => u.userID == user.userID));
        }

        /// <summary>
        ///     Returns true if the logged in user is a student for the passed courseID
        /// </summary>
        /// save
        /// <returns></returns>
        public bool UserIsStudentInCourse(int userID, int courseID)
        {
            if (userID != 0 && courseID != 0) return DBHelper.UserIsInCourse(courseID, userID);

            return false;
        }

        /// <summary>
        ///     Returns true if the user is already in a group for the project associated with the given group, ACTIVE OR INACTIVE
        /// </summary>
        /// <returns></returns>
        public bool IsStudentInGroup(int groupID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return DBHelper.IsUserInGroup(user.userID, groupID);
            return false;
        }

        /// <summary>
        ///     Returns true if the user is already in a group for the project associated with the given group
        /// </summary>
        /// <returns></returns>
        public bool IsActiveStudentInGroup(int groupID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return DBHelper.IsActiveUserInGroup(user.userID, groupID);
            return false;
        }

        /// <summary>
        ///     Returns true if the user is already in a group for the project associated with the given group
        /// </summary>
        /// <returns></returns>
        public bool IsStudentInOtherGroup(int groupID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return DBHelper.IsUserInOtherGroup(user.userID, groupID);
            return false;
        }

        /// <summary>
        ///     Returns true if the user is already in a group for the given project
        /// </summary>
        /// <returns></returns>
        public bool IsStudentInGroupForProject(int projectID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return DBHelper.IsUserInGroupForProject(user.userID, projectID);
            return false;
        }

        /// <summary>
        ///     Returns true if the logged in user has timecard entries for the passed group
        /// </summary>
        /// <returns></returns>
        public bool UserHasTimeInGroup(int groupID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return DBHelper.UserHasTimeInGroup(user.userID, groupID);

            return false;
        }

        #endregion

        #region Endpoints

        /// <summary>
        ///     Add a course. Returns the course ID
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddCourse([FromBody] object json)
        {
            var JsonString = json.ToString();
            
            if (GetUserType() == 'I' || IsAdmin())
            {
                var course = _courseService.AddCourse(new Course()
                {
                    courseName = "New Course",
                    InstructorId = 2,
                    isActive = true,
                    description = ""
                });
    
                if (course.courseID > 0)
                    return Ok(course.courseID);
                else
                    return StatusCode(500);
            }
            
            return Unauthorized();
        }

        /// <summary>
        ///     Add a project for the passed course. Returns the projectID
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddProject([FromBody] object json)
        {
            var JsonString = json.ToString();
            var course = JsonConvert.DeserializeObject<Course>(JsonString);

            if (IsInstructorForCourse(course.courseID) || IsAdmin())
            {
                var project = new Project()
                {
                    projectName = "Default Project",
                    description = "This is the default project template.",
                                                        isActive = true,
                    CourseID = course.courseID
                };

                var p = _projectService.AddProject(project);
                if (project.projectID > 0) return Ok(project.projectID);
                return StatusCode(500); //Query Error
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult CreateTemplateQuestion([FromBody] object json)
        {
            var JsonString = json.ToString();
            var evalTemplateQuestionCategory = JsonConvert.DeserializeObject<EvalTemplateQuestionCategory>(JsonString);

            if (IsInstructorForEval(evalTemplateQuestionCategory.evalTemplateID) || IsAdmin())
            {
                var questionID = DBHelper.CreateTemplateQuestion(
                    evalTemplateQuestionCategory.evalTemplateQuestionCategoryID,
                    evalTemplateQuestionCategory.evalTemplateID);
                if (questionID > 0) return Ok(questionID);
                return StatusCode(500);
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult ChangePassword([FromBody] object json)
        {
            var JsonString = json.ToString();
            var user = JsonConvert.DeserializeObject<User>(JsonString);
            user.password = GenerateHash(user.password);
            user.newPassword = GenerateHash(user.newPassword);

            if (IsAdmin())
            {
                if (DBHelper.ChangePasswordA(user)) return Ok();
                return StatusCode(500); //Query failed
            }

            if (user.userID == GetUserID())
            {
                if (DBHelper.ChangePassword(user)) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized(); //Not an Admin or the current user, Unathorized (401)
        }

        /// <summary>
        ///     Updates the passed user in the database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ChangeUser([FromBody] object json)
        {
            var JsonString = json.ToString();
            var user = JsonConvert.DeserializeObject<User>(JsonString);
            if (user.username == null || user.username.Length < 1)
                return StatusCode(400); //Didn't pass a valid username, Bad Request (400)
            user.username = user.username.ToLower();
            var checkUser = DBHelper.GetUser(user.username);
            if (checkUser != null && checkUser.userID != user.userID)
                return StatusCode(403); //Username already exists, Forbidden (403)

            if (IsAdmin())
            {
                if (DBHelper.ChangeUserA(user)) return Ok();
                return StatusCode(500); //Query failed
            }

            if (user.userID == GetUserID())
            {
                if (DBHelper.ChangeUser(user)) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized(); //Not an Admin or the current user, Unauthorized (401)
        }

        [HttpPost]
        public IActionResult CreateTemplate([FromBody] object json)
        {
            var JsonString = json.ToString();
            var user = JsonConvert.DeserializeObject<User>(JsonString);

            if (IsAdmin() || GetUserType() == 'I' && user.userID == GetUserID())
                return Ok(DBHelper.CreateTemplate(user.userID));
            return Unauthorized();
        }

        [HttpPost]
        public IActionResult SaveTemplateName([FromBody] object json)
        {
            var JsonString = json.ToString();
            var evalTemplate = JsonConvert.DeserializeObject<EvalTemplate>(JsonString);

            if (GetUserType() == 'I' || IsAdmin()) return Ok(DBHelper.SaveTemplateName(evalTemplate));
            return Unauthorized();
        }

        [HttpPost]
        public IActionResult SaveEval([FromBody] object json)
        {
            var JsonString = json.ToString();
            var eval = JsonConvert.DeserializeObject<AdminEval>(JsonString);

            if (IsAdmin()) return Ok(DBHelper.SaveEval(eval));
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
                if (DBHelper.CreateTemplateCopy(user.userID, evalTemplate.evalTemplateID)) return Ok();
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
                var categoryID = DBHelper.CreateCategory(evalTemplate.evalTemplateID);
                if (categoryID > 0) return Ok(categoryID);
                return StatusCode(500);
            }

            return Unauthorized();
        }

        /// <summary>
        ///     Returns the logged in user if there is one.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CheckSession()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");
            if (user != null)
                return Ok(user);
            return Unauthorized(); //Unauthorized (401) if there isn't a user in the session
        }

        public IActionResult CreateGroup([FromBody] object json)
        {
            var JsonString = json.ToString();

            var groupID = 0;
            var project = JsonConvert.DeserializeObject<Project>(JsonString);
            var courseID = GetCourseForProject(project.projectID);
            
            

            if (IsAdmin() || IsInstructorForCourse(courseID) || IsStudentInCourse(courseID))
            {
                if (GetUserType() == 'S')
                {
                    if (!IsStudentInGroupForProject(project.projectID))
                    {
                        groupID = (int) DBHelper.CreateGroup(new Group());
                        if (groupID > 0) DBHelper.JoinGroup(GetUserID(), groupID);
                    }
                    else
                    {
                        return StatusCode(403); //Student already part of group, unable to create a new one.
                    }
                }
                else
                {
                    groupID = (int) DBHelper.CreateGroup(new Group());
                }

                if (groupID > 0) return Ok(groupID);
                return StatusCode(500); //Failed Query
            }

            return Unauthorized();
        }


        /// <summary>
        ///     Creates a TimeCard and returns the timeSlotID
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IActionResult CreateTimeCard([FromBody] object json)
        {
            var JsonString = json.ToString();

            var timeCard = JsonConvert.DeserializeObject<TimeCard>(JsonString);
            var courseID = GetCourseForGroup(timeCard.groupID);

            if (IsAdmin() || IsInstructorForCourse(courseID) || IsStudentInCourse(courseID))
            {
                if (GetUserType() == 'S' && GetUserID() == timeCard.userID)
                    timeCard.timeslotID = (int) DBHelper.CreateTimeCard(timeCard);
                else
                    timeCard.timeslotID = (int) DBHelper.CreateTimeCard(timeCard);
                if (timeCard.timeslotID > 0) return Ok(timeCard.timeslotID);
                return StatusCode(500);
            }

            return Unauthorized();
        }


        /*********************************************   Jamison Edit *************************************/

        public IActionResult DeleteTimeCard([FromBody] object json)
        {
            var JsonString = json.ToString();

            var timeCard = JsonConvert.DeserializeObject<TimeCard>(JsonString);
            var courseID = GetCourseForGroup(timeCard.groupID);

            if (IsAdmin() || IsInstructorForCourse(courseID) || IsStudentInCourse(courseID))
            {
                if (GetUserType() == 'S' && GetUserID() == timeCard.userID)
                {
                    /*Changed to DELETE*/
                    timeCard.timeslotID = (int) DBHelper.DeleteTimeCard(timeCard);
                    return StatusCode(200);
                }

                /*Changed to DELETE*/
                timeCard.timeslotID = (int) DBHelper.DeleteTimeCard(timeCard);
                return StatusCode(200);
            }

            return Unauthorized();
        }

        /******************************************** End Jamison Edit *************************************/

        /// <summary>
        ///     Creates a project given a project object. Returns the project ID
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateProject([FromBody] object json)
        {
            var JsonString = json.ToString();

            var course = JsonConvert.DeserializeObject<Course>(JsonString);

            //TODO
            //String DBProjectID = DataAccess.AddProject(course._id);
            var DBProjectID = 0;

            return Ok(DBProjectID);
        }

        /// <summary>
        ///     Deletes the passed user and course association
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteUserCourse([FromBody] object json)
        {
            var JsonString = json.ToString();
            var uCourse = JsonConvert.DeserializeObject<UserCourse>(JsonString);
            if (IsAdmin() || IsInstructorForCourse(uCourse.courseID))
            {
                if (DBHelper.DeleteUserCourse(uCourse.userID, uCourse.courseID)) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized(); //Not an Admin or the Instructor for the course, Unauthorized (401)
        }

        [HttpPost]
        public IActionResult EvalResponses([FromBody] object json)
        {
            var JsonString = json.ToString();
            var eval = JsonConvert.DeserializeObject<Eval>(JsonString);
            var evals = new List<Eval>();

            if (IsAdmin() || IsInstructorForCourse(GetCourseForGroup(eval.groupID)))
            {
                evals = DBHelper.EvalResponsesA(eval.groupID, eval.userID);
                return Ok(evals);
            }

            if (eval.userID == GetUserID())
            {
                evals = DBHelper.EvalResponses(eval.groupID, eval.userID);
                return Ok(evals);
            }

            return Unauthorized();
        }

        /// <summary>
        ///     Get a course and its projects and users
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetCourse([FromBody] object json)
        {
            var JsonString = json.ToString();
            var course = JsonConvert.DeserializeObject<Course>(JsonString);

            var retrievedCourse = _courseService.GetCourse(course.courseID);

            return Ok(retrievedCourse);
        }

        /// <summary>
        ///     Get a list of all the courses
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCourses()
        {
            var allCourses = _courseService.GetCourses();
            return Ok(allCourses);
        }

        /// <summary>
        ///     Get a list of this user's courses if they are an instructor
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetInstructorCourses()
        {
            var allCourses = _courseService.GetCourses();
            var userCourses = new List<Course>();

            foreach (var c in allCourses)
                if (IsInstructorForCourse(c.courseID))
                    userCourses.Add(c);
            return Ok(userCourses);
        }

        [HttpPost]
        public IActionResult GetInactiveStudentsInCourse([FromBody] object json)
        {
            var course = JsonConvert.DeserializeObject<Course>(json.ToString());
            var inactiveUsers = new List<User>();
            inactiveUsers = DBHelper.GetInactiveUsersForCourse(course.courseID);
            return Ok(inactiveUsers);
        }

        /// <summary>
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetGroup([FromBody] object json)
        {
            var JsonString = json.ToString();
            var requestedGroup = JsonConvert.DeserializeObject<Group>(JsonString);
            //Group requestedGroup = new Group();
            //requestedGroup.groupID = Int32.Parse(requestedGroupStr);

            //Make sure that the user is part of the groups course
            var courseID = GetCourseForGroup(requestedGroup.groupID);
            if (IsStudentInCourse(courseID) || IsAdmin() || IsInstructorForCourse(courseID))
            {
                requestedGroup = DBHelper.GetGroup(requestedGroup.groupID);
                return Ok(requestedGroup);
            }

            return Unauthorized(); //Not allowed to view the group.
        }

        /// <summary>
        ///     Return a Project based on the ID. Returns a Project if successful 204 otherwise
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetProject([FromBody] object json)
        {
            var JsonString = json.ToString();
            var project = JsonConvert.DeserializeObject<Project>(JsonString);
            var courseID = GetCourseForProject(project.projectID);

            if (IsAdmin() || IsInstructorForCourse(courseID) || IsStudentInCourse(courseID))
            {
                project = _projectService.GetProjectById(project.projectID);
                return Ok(project);
            }

            return Unauthorized();
        }

        /// <summary>
        ///     Returns OK if admmin or ID's match
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetUser([FromBody] object json)
        {
            var JsonString = json.ToString();
            var sentUser = JsonConvert.DeserializeObject<User>(JsonString);
            var currentUser = HttpContext.Session.GetObjectFromJson<User>("user");
            if (currentUser.type == 'A' || currentUser.userID == sentUser.userID)
            {
                var dbUser = DBHelper.GetUserByID(sentUser.userID);
                return Ok(dbUser);
            }

            return NoContent();
        }

        /// <summary>
        ///     Returns dashboard for a given userID
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetDashboard()
        {
            var dashboard = DBHelper.GetDashboard(GetUserID());
            return Ok(dashboard);
        }

        /// <summary>
        ///     Returns all users in the system after verifying access.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUsers()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            //checks if user is admin
            if (IsAdmin())
            {
                var users = _timeTrackerService.GetUsers();
                return Ok(users);
            }

            return NoContent();
        }

        /// <summary>
        ///     Returns all users in the passed group.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetUsersForGroup([FromBody] object json)
        {
            var JsonString = json.ToString();
            var group = JsonConvert.DeserializeObject<Group>(JsonString);
            if (IsStudentInGroup(group.groupID) || IsAdmin() || IsInstructorForCourse(GetCourseForGroup(group.groupID)))
            {
                var users = DBHelper.GetUsersForGroup(group.groupID);
                return Ok(users);
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult GetUsersForCourse([FromBody] object json)
        {
            var JsonString = json.ToString();
            var course = JsonConvert.DeserializeObject<Course>(JsonString);

            var users = _userService.GetUsersForCourse(course.courseID);
            return Ok(users);
        }

        [HttpPost]
        public IActionResult JoinCourse([FromBody] object json)
        {
            var JsonString = json.ToString();
            var course = JsonConvert.DeserializeObject<Course>(JsonString);

            var recordsUpdated = _courseService.RequestJoinCourse(GetUserID(), course.courseID);

            if (recordsUpdated > 0)
            {
                return Ok();
            }
            
            return StatusCode(500); //Query failed
        }

        [HttpPost]
        public IActionResult LeaveCourse([FromBody] object json)
        {
            var JsonString = json.ToString();
            var course = JsonConvert.DeserializeObject<Course>(JsonString);

            if (IsStudentInCourse(course.courseID))
            {
                if (DBHelper.LeaveCourse(course.courseID, GetUserID())) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult SaveUserInCourse([FromBody] object json)
        {
            var JsonString = json.ToString();
            var uCourse = JsonConvert.DeserializeObject<UserCourse>(JsonString);

            if ((IsAdmin() || IsInstructorForCourse(uCourse.courseID)) &&
                UserIsStudentInCourse(uCourse.userID, uCourse.courseID))
            {
                if (DBHelper.SaveUserInCourse(uCourse)) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult DeleteUserFromCourse([FromBody] object json)
        {
            var JsonString = json.ToString();
            var uCourse = JsonConvert.DeserializeObject<UserCourse>(JsonString);

            if ((IsAdmin() || IsInstructorForCourse(uCourse.courseID)) &&
                UserIsStudentInCourse(uCourse.userID, uCourse.courseID))
            {
                if (DBHelper.DeleteFromCourse(uCourse.courseID, uCourse.userID)) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult JoinGroup([FromBody] object json)
        {
            var JsonString = json.ToString();
            var uGroups = JsonConvert.DeserializeObject<UserGroup>(JsonString);

            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (IsStudentInOtherGroup(uGroups.groupID)) return StatusCode(403);

            if (IsStudentInCourse(GetCourseForGroup(uGroups.groupID)))
            {
                if (IsStudentInGroup(uGroups.groupID))
                {
                    if (DBHelper.ReJoinGroup(user.userID, uGroups.groupID)) return NoContent();
                    return StatusCode(500); //Query failed
                }

                var groupID = DBHelper.JoinGroup(user.userID, uGroups.groupID);
                if (groupID > 0) return Ok(groupID);
                return StatusCode(500); //Query failed
            }

            return Unauthorized(); //User not in Course
        }

        [HttpPost]
        public IActionResult LeaveGroup([FromBody] object json)
        {
            var JsonString = json.ToString();
            var group = JsonConvert.DeserializeObject<Group>(JsonString);

            if (IsActiveStudentInGroup(group.groupID))
            {
                if (UserHasTimeInGroup(group.groupID))
                {
                    //Mark the user as inactive in the group if they have existing time entries
                    if (DBHelper.LeaveGroup(GetUserID(), group.groupID)) return Ok();
                    return StatusCode(500); //Query failed
                }

                //Actually remove the user from the group if they don't have any time entries yet.
                if (DBHelper.DeleteFromGroup(GetUserID(), group.groupID)) return NoContent();
                return StatusCode(500); //Query failed
            }

            return Unauthorized();
        }

        /// <summary>
        ///     Allows a user to log in. Returns an OK (200) if successful, No Content (204) if the
        ///     username doesn't exist, and Unauthorized (401) if the password is incorrect
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LoginUser([FromBody] object json)
        {
            var JsonString = json.ToString();
            //Username and Password must be here, everything else can be empty
            var loginUser = JsonConvert.DeserializeObject<User>(JsonString);

            //Check database for User and create a session
            var user = _timeTrackerService.GetUserByUsername(loginUser.username);
            var crypto = new CryptographyService();

            if (crypto.Verify(user.password, user.Salt, loginUser.password))
            {
                if (!user.isActive) return StatusCode(403); //return Forbidden (403) if the user's account isn't active
                
                // We found a user! Send them to the Dashboard and save their Session
                HttpContext.Session.SetObjectAsJson("user", user);
                return Ok();
            }

            //return Unauthorized (401) if the username or password is wrong
            return Unauthorized();
        }

        /// <summary>
        ///     Returns OK if a users session succesfully ended. 204 otherwise
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.DestroySession<User>("user");

            var user = HttpContext.Session.GetObjectFromJson<User>("user");
            if (user == null)
                return Ok();
            return null;
        }

        /// <summary>
        ///     Registers a User, returns a 200 status code if successful
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RegisterUser([FromBody] object json)
        {
            var JsonString = json.ToString();
            var user = JsonConvert.DeserializeObject<User>(JsonString);

            _timeTrackerService.AddUser(user);

            //Store Session information for this user using Username
            HttpContext.Session.SetObjectAsJson("user", user);
            
            return Ok();
        }

        /// <summary>
        ///     Updates a Course name
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveCourse([FromBody] object json)
        {
            var JsonString = json.ToString();

            var course = JsonConvert.DeserializeObject<Course>(JsonString);

            if (IsAdmin() || IsInstructorForCourse(course.courseID))
            {
                if (DBHelper.SaveCourse(course)) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized(); //Not an Admin or the Instructor for the course, Unauthorized (401)
        }

        /// <summary>
        ///     Updates a Group name.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveGroup([FromBody] object json)
        {
            var JsonString = json.ToString();

            var group = JsonConvert.DeserializeObject<Group>(JsonString);
            var courseID = GetCourseForGroup(group.groupID);

            if (IsAdmin() || IsInstructorForCourse(courseID) || IsActiveStudentInGroup(group.groupID))
            {
                if (DBHelper.SaveGroup(group)) return Ok();
                return StatusCode(500); // Query failed
            }

            return
                Unauthorized(); // Not an Admin or the Instructor for the course, or a student in the group, Unauthorized (401)
        }

        //}
        /// <summary>
        ///     Update a Project name or isActive status.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveProject([FromBody] object json)
        {
            var JsonString = json.ToString();

            var project = JsonConvert.DeserializeObject<Project>(JsonString);

            if (IsAdmin() || IsInstructorForCourse(GetCourseForProject(project.projectID)))
            {
                if (DBHelper.SaveProject(project)) return Ok();
                return StatusCode(500); // Query failed
            }

            return Unauthorized(); // Not an Admin or the Instructor for the course, Unauthorized (401)
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
                if (DBHelper.SaveCategory(category)) return Ok();
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
                if (DBHelper.DeleteCategory(category.evalTemplateQuestionCategoryID)) return Ok();
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
                if (DBHelper.SaveQuestion(question)) return Ok();
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
                if (DBHelper.DeleteQuestion(question.evalTemplateQuestionID)) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized(); //Not an Admin or the Instructor for the course, Unauthorized (401)
        }

        [HttpPost]
        public IActionResult SaveTime([FromBody] object json)
        {
            var JsonString = json.ToString();

            var timecard = JsonConvert.DeserializeObject<TimeCard>(JsonString);

            //  Checks the time input on the server side to stop any manual posts that are not valid.
            DateTime timeIn, timeOut;

            //  Is time in a date?,  is time out a date?
            //  are hours negative?, is time out a future date?     
            if (!DateTime.TryParse(timecard.timeIn, out timeIn) || !DateTime.TryParse(timecard.timeOut, out timeOut) ||
                timeOut.CompareTo(timeIn) < 0 || timeOut > DateTime.Now || timeIn > DateTime.Now)
            {
                return BadRequest("Invalid time entered");
            }

            if (IsAdmin() || GetUserID() == timecard.userID ||
                IsInstructorForCourse(GetCourseForGroup(timecard.groupID)))
            {
                if (DBHelper.SaveTime(timecard)) return Ok();
                return StatusCode(500);
            }

            return Unauthorized();
        }

        [HttpPost]
        public IEnumerable<Project> GetProjects([FromBody] object json)
        {
            var JsonString = json.ToString();
            var course = JsonConvert.DeserializeObject<Course>(JsonString);

            var projects = _projectService.GetProjectsByCourseId(course.courseID);

            return projects;
        }

        [HttpPost]
        public IActionResult GetTemplates([FromBody] object json)
        {
            var JsonString = json.ToString();

            var course = JsonConvert.DeserializeObject<Course>(JsonString);
            var templates = DBHelper.GetTemplates(DBHelper.GetInstructorForCourse(course.courseID));

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
                var templates = DBHelper.GetFullTemplatesForInstructor(user.userID);
                if (templates.Count > 0) return Ok(templates);
            }

            return NoContent();
        }

        [HttpPost]
        public IActionResult GetCoursesForInstructor([FromBody] object json)
        {
            var JsonString = json.ToString();
            var user = JsonConvert.DeserializeObject<User>(JsonString);

            if (IsAdmin() || GetUserID() == user.userID)
            {
                var courses = _courseService.GetCoursesByUser(user);
                return Ok(courses);
            }

            return NoContent();
        }

        [HttpPost]
        public IActionResult AssignEvals([FromBody] object json)
        {
            var JsonString = json.ToString();

            var assignEvals = JsonConvert.DeserializeObject<AssignEvals>(JsonString);

            //call and set the inUse flag with another query 

            if (DBHelper.AssignEvals(assignEvals.projectIDs, assignEvals.evalTemplateID))
            {
                DBHelper.SetInUse(assignEvals.evalTemplateID);
                return Ok();
            }

            return StatusCode(500);
        }

        [HttpPost]
        public IActionResult GetEvaluation([FromBody] object json)
        {
            var JsonString = json.ToString();
            var eval = JsonConvert.DeserializeObject<Eval>(JsonString);
            if (IsAdmin()) return Ok(DBHelper.GetEvaluation(eval.evalID));
            return Unauthorized();
        }

        [HttpPost]
        public IActionResult GetLatestIncompleteEvaluation([FromBody] object json)
        {
            var JsonString = json.ToString();
            var group = JsonConvert.DeserializeObject<Group>(JsonString);

            var evalID = DBHelper.GetLatestIncompleteEvaluationID(group.groupID, GetUserID());
            if (evalID > 0) return Ok(DBHelper.GetEvaluation(evalID));
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
                if (!DBHelper.SaveResponse(response.userID, response.evalID, response.evalTemplateQuestionID,
                    response.response)) failed = true;
            }

            //}
            if (failed) return StatusCode(500);

            if (!DBHelper.CompleteEval(evalID)) return StatusCode(500);

            return Ok();
        }

        [HttpPost]
        public IActionResult GetAllCompleteEvaluations([FromBody] object json)
        {
            var JsonString = json.ToString();

            var group = JsonConvert.DeserializeObject<UserGroup>(JsonString);

            if (IsActiveStudentInGroup(group.groupID))
                //Use logged in users ID if they are a student
                return Ok(DBHelper.RandomizeEvaluations(group.groupID, group.userID));
            if (IsAdmin() || IsInstructorForCourse(GetCourseForGroup(group.groupID)))
                //Get passed userID if they are an Admin/Instructor
                return Ok(DBHelper.EvalResponsesA(group.groupID, group.userID));
            return Unauthorized();
        }

        [HttpGet]
        public IActionResult GetAllEvaluations()
        {
            if (IsAdmin()) return Ok(DBHelper.GetAllEvals());
            return Unauthorized();
        }

        #endregion
    }
}