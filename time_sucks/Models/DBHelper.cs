using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;

namespace time_sucks.Models
{
    public class DBHelper
    {
        //TODO Make this a better system user 
        static private MySqlConnectionStringBuilder connstring = new MySqlConnectionStringBuilder("" +
            "Server=cs4450.cj7o28wmyp47.us-east-2.rds.amazonaws.com;" +
            "UID=Logan;" +
            "password=password;" +
            "database=cs4450");     
        
        public static long AddUser(User user)
        {
            if (user.username != null) user.username = user.username.ToLower();
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "INSERT INTO users (username, password, firstName, lastName, type, isActive) " +
                        "VALUES (@username, @password, @firstName, @lastName, 'S', 1)";
                    cmd.Parameters.AddWithValue("@username", user.username);
                    cmd.Parameters.AddWithValue("@password", user.password);
                    cmd.Parameters.AddWithValue("@firstName", user.firstName);
                    cmd.Parameters.AddWithValue("@lastName", user.lastName);

                    //Return the last inserted ID if successful
                    if (cmd.ExecuteNonQuery() > 0) return cmd.LastInsertedId;

                    return 0;
                }
            }
        }

        //Normal version requires current password to be passed
        public static bool ChangePassword(User user)
        {
            string password = "";
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters

                    cmd.CommandText = "SELECT password FROM users WHERE userID = @userID";
                    cmd.Parameters.AddWithValue("@userID", user.userID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            password = reader.GetString("password");
                        }
                    }

                    if (password == user.password)
                    {
                        cmd.CommandText = "UPDATE users SET password = @password WHERE userID = @userID";
                        cmd.Parameters.AddWithValue("@password", user.newPassword);

                        if (cmd.ExecuteNonQuery() > 0) return true;
                        return false;
                    }
                }
            }
            return false;
        }

        //Admin version doesn't require the current password to be passed
        public static bool ChangePasswordA(User user)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE users SET password = @password WHERE userID = @userID";
                    cmd.Parameters.AddWithValue("@password", user.newPassword);
                    cmd.Parameters.AddWithValue("@userID", user.userID);

                    if (cmd.ExecuteNonQuery() > 0) return true;
                    return false;

                }
            }
        }

        //Normal version doesn't save type or isActive
        public static bool ChangeUser(User user)
        {
            if (user.username != null) user.username = user.username.ToLower();
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "UPDATE users SET username = @username, firstName = @firstName, lastName = @lastName WHERE userID = @userID";
                    cmd.Parameters.AddWithValue("@username", user.username);
                    cmd.Parameters.AddWithValue("@firstName", user.firstName);
                    cmd.Parameters.AddWithValue("@lastName", user.lastName);
                    cmd.Parameters.AddWithValue("@userID", user.userID);

                    if (cmd.ExecuteNonQuery() > 0) return true;
                    return false;
                }
            }
        }

        //Admin version also saves type and isActive
        public static bool ChangeUserA(User user)
        {
            if (user.username != null) user.username = user.username.ToLower();
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "UPDATE users SET username = @username, firstName = @firstName, lastName = @lastName, type = @type, isActive = @isActive WHERE userID = @userID";
                    cmd.Parameters.AddWithValue("@username", user.username);
                    cmd.Parameters.AddWithValue("@firstName", user.firstName);
                    cmd.Parameters.AddWithValue("@lastName", user.lastName);
                    cmd.Parameters.AddWithValue("@type", user.type);
                    cmd.Parameters.AddWithValue("@isActive", user.isActive);
                    cmd.Parameters.AddWithValue("@userID", user.userID);

                    if (cmd.ExecuteNonQuery() > 0) return true;
                    return false;
                }
            }
        }



        public static long CreateCourse(int instructorID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "INSERT INTO courses (courseName, instructorID, isActive, description) " +
                        "VALUES ('New Course', @instructorID, 1, '')";
                    cmd.Parameters.AddWithValue("@instructorID", instructorID);

                    //Return the last inserted ID if successful
                    if (cmd.ExecuteNonQuery() > 0) return cmd.LastInsertedId;

                    return 0;
                }
            }
        }

        public static long CreateProject(int courseID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "INSERT INTO projects (projectName, courseID, isActive, description) " +
                        "VALUES ('New Project', @courseID, 1, '')";
                    cmd.Parameters.AddWithValue("@courseID", courseID);

                    //Return the last inserted ID if successful
                    if (cmd.ExecuteNonQuery() > 0) return cmd.LastInsertedId;

                    return 0;
                }
            }
        }

        public static long CreateGroup(int projectID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "INSERT INTO cs4450.groups (groupName, isActive, evalID, projectID) " +
                                      "VALUES ('New Group', 1, 0, @projectID);";
                    cmd.Parameters.AddWithValue("@projectID", projectID);

                    //Return the last inserted ID if successful
                    if (cmd.ExecuteNonQuery() > 0) return cmd.LastInsertedId;

                    return 0;
                }
            }
        }

        public static long CreateTimeCard(TimeCard timeCard)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "INSERT INTO timeCards (timeIn, timeOut, isEdited, userID, groupID, description)" +
                                      " VALUES (@timeIn, @timeOut, 0, @userID, @groupID, @description);";

                    if (timeCard.timeIn == null || timeCard.timeIn == "") cmd.Parameters.AddWithValue("@timeIn", null);
                    else cmd.Parameters.AddWithValue("@timeIn", Convert.ToDateTime(timeCard.timeIn));
                    if (timeCard.timeOut == null || timeCard.timeOut == "") cmd.Parameters.AddWithValue("@timeOut", null);
                    else cmd.Parameters.AddWithValue("@timeOut", Convert.ToDateTime(timeCard.timeOut));
                    cmd.Parameters.AddWithValue("@userID", timeCard.userID);
                    cmd.Parameters.AddWithValue("@groupID", timeCard.groupID);
                    if (timeCard.description == null) cmd.Parameters.AddWithValue("@description", "");
                    else cmd.Parameters.AddWithValue("@description", timeCard.description);


                    //Return the last inserted ID if successful
                    if (cmd.ExecuteNonQuery() > 0) return cmd.LastInsertedId;

                    return 0;
                }
            }
        }

        /*********************************************** Jamison Edit *****************************************/

        public static long DeleteTimeCard(TimeCard timeCard)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "DELETE FROM timeCards WHERE timeIn = @timeIn AND timeOut = @timeOut AND userID  = @userID AND groupID = @groupID AND description = @description;";

                    if (timeCard.timeIn == null || timeCard.timeIn == "") cmd.Parameters.AddWithValue("@timeIn", null);
                    else cmd.Parameters.AddWithValue("@timeIn", Convert.ToDateTime(timeCard.timeIn));
                    if (timeCard.timeOut == null || timeCard.timeOut == "") cmd.Parameters.AddWithValue("@timeOut", null);
                    else cmd.Parameters.AddWithValue("@timeOut", Convert.ToDateTime(timeCard.timeOut));
                    cmd.Parameters.AddWithValue("@userID", timeCard.userID);
                    cmd.Parameters.AddWithValue("@groupID", timeCard.groupID);
                    if (timeCard.description == null) cmd.Parameters.AddWithValue("@description", "");
                    else cmd.Parameters.AddWithValue("@description", timeCard.description);


                    //Return the last inserted ID if successful
                    if (cmd.ExecuteNonQuery() > 0) return cmd.LastInsertedId;

                    return 0;
                }
            }
        }

        /************************************************* End Edit ***********************************************/

        public static bool DeleteUserCourse(int userID, int courseID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM uCourses WHERE userID = @userID AND courseID = @courseID";
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@courseID", courseID);

                    if (cmd.ExecuteNonQuery() > 0) return true;
                    return false;
                }
            }
        }

        public static Course GetCourse(int courseID)
        {
            Course course = new Course()
            {
                users = new List<User>(),
                projects = new List<Project>()
            };

            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText =
                        "SELECT c.*, uc.isActive AS ucIsActive, u.userID, u.firstName, u.lastName FROM courses c " +
                        "LEFT JOIN uCourses uc ON c.courseID = uc.courseID LEFT JOIN users u ON uc.userID = u.userID " +
                        "WHERE c.courseID = @courseID";
                    cmd.Parameters.AddWithValue("@courseID", courseID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            if (course.courseID == 0)
                            {
                                course.courseID = reader.GetInt32("courseId");
                                course.courseName = reader.GetString("courseName");
                                course.instructorID = reader.GetInt32("instructorId");
                                course.isActive = reader.GetBoolean("isActive");
                                course.description = reader.GetString("description");
                            }

                            if (!reader.IsDBNull(6))
                            {
                                course.users.Add(new User()
                                {
                                    userID = reader.GetInt32("userID"),
                                    firstName = reader.GetString("firstName"),
                                    lastName = reader.GetString("lastName"),
                                    isActive = reader.GetBoolean("ucIsActive"),
                                });

                            }

                        }
                    }

                    cmd.CommandText = "SELECT * FROM projects WHERE courseID = @courseID";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            course.projects.Add(new Project()
                            {
                                projectID = reader.GetInt32("projectID"),
                                projectName = reader.GetString("projectName"),
                                description = reader.GetString("description"),
                                isActive = reader.GetBoolean("isActive"),
                            });
                        }
                    }
                }
            }
            return course;
        }

        public static Project GetProject(int projectID)
        {
            Project project = new Project()
            {
                groups = new List<Group>()
            };

            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText =
                        "Select p.*, g.groupID, g.groupName, g.isActive AS groupIsActive, u.userID, u.firstName, u.lastName, t.groupID AS 'tgroupID', t.timeID, " +
                          "date_format(t.timeIn, '%m/%d/%Y %l:%i %p') AS 'timeIn', date_format(t.timeOut, '%m/%d/%Y %l:%i %p') AS 'timeOut', " +
                          "t.description AS 'timeDescription', t.isEdited, t.userID AS 'tuserID', ug.isActive AS isActiveInGroup " +
                        "FROM projects p " +
                        "Left Join cs4450.groups g On p.projectID = g.projectID " +
                        "Left Join uGroups ug On ug.groupID = g.groupID " +
                        "Left Join users u On u.userID = ug.userID " +
                        "Left Join timeCards t On (u.userID = t.userID AND g.groupID = t.groupID) " +
                        "Where p.projectID = @projectID";
                    cmd.Parameters.AddWithValue("@projectID", projectID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            if (project.projectID == 0)
                            {
                                project.projectID = reader.GetInt32("projectID");
                                project.projectName = reader.GetString("projectName");
                                project.isActive = reader.GetBoolean("isActive");
                                project.description = reader.GetString("description");
                                project.CourseID = reader.GetInt32("courseID");
                            }

                            bool foundGroup = false;

                            foreach (Group group in project.groups)
                            {

                                if (group.groupID == reader.GetInt32("groupID"))
                                {
                                    foundGroup = true;

                                    bool foundUser = false;

                                    if (group.users == null) group.users = new List<User>();

                                    if (group.groupID == 0)
                                    {
                                        group.groupName = reader.GetString("groupName");
                                        group.groupID = reader.GetInt32("groupID");
                                        group.isActive = reader.GetBoolean("groupIsActive");
                                    }

                                    //get each users time info
                                    foreach (User user in group.users)
                                    {
                                        if (user.userID == reader.GetInt32("userID"))
                                        {
                                            foundUser = true;
                                            //Add time slot

                                            if (user.timecards == null) user.timecards = new List<TimeCard>();

                                            if (!reader.IsDBNull(12))
                                            {
                                                user.timecards.Add(new TimeCard()
                                                {
                                                    timeIn = reader.IsDBNull(13) ? "" : reader.GetString("timeIn"),
                                                    timeOut = reader.IsDBNull(14) ? "" : reader.GetString("timeOut"),
                                                    description = reader.GetString("description"),
                                                    groupID = reader.GetInt32("tgroupID"),
                                                    timeslotID = reader.GetInt32("timeID"),
                                                    isEdited = reader.GetBoolean("isEdited"),
                                                    userID = reader.GetInt32("tuserID")
                                                });
                                            }
                                        }
                                    }

                                    if (!foundUser)
                                    {
                                        List<TimeCard> timecardlist = new List<TimeCard>();
                                        if (!reader.IsDBNull(12))
                                        {
                                            timecardlist.Add(new TimeCard()
                                            {
                                                timeIn = reader.IsDBNull(13) ? "" : reader.GetString("timeIn"),
                                                timeOut = reader.IsDBNull(14) ? "" : reader.GetString("timeOut"),
                                                description = reader.GetString("description"),
                                                groupID = reader.GetInt32("tgroupID"),
                                                timeslotID = reader.GetInt32("timeID"),
                                                isEdited = reader.GetBoolean("isEdited"),
                                                userID = reader.GetInt32("tuserID")
                                            });
                                        }

                                        //Add the user and then the time slot
                                        if (!reader.IsDBNull(8))
                                        {
                                            group.users.Add(new User()
                                            {
                                                userID = reader.GetInt32("userID"),
                                                firstName = reader.GetString("firstName"),
                                                lastName = reader.GetString("lastName"),
                                                timecards = timecardlist,
                                                isActive = reader.GetBoolean("isActiveInGroup")
                                            });
                                        }
                                    }
                                }
                            }

                            if (!foundGroup)
                            {
                                List<TimeCard> timecardlist = new List<TimeCard>();
                                if (!reader.IsDBNull(12))
                                {
                                    timecardlist.Add(new TimeCard()
                                    {
                                        timeIn = reader.IsDBNull(13) ? "" : reader.GetString("timeIn"),
                                        timeOut = reader.IsDBNull(14) ? "" : reader.GetString("timeOut"),
                                        description = reader.GetString("timeDescription"),
                                        groupID = reader.GetInt32("tgroupID"),
                                        timeslotID = reader.GetInt32("timeID"),
                                        isEdited = reader.GetBoolean("isEdited")
                                    });
                                }

                                List<User> users = new List<User>();
                                if (!reader.IsDBNull(8))
                                {
                                    users.Add(new User()
                                    {
                                        userID = reader.GetInt32("userID"),
                                        firstName = reader.GetString("firstName"),
                                        lastName = reader.GetString("lastName"),
                                        timecards = timecardlist,
                                        isActive = reader.GetBoolean("isActiveInGroup")
                                    });
                                }

                                if (!reader.IsDBNull(5))
                                {
                                    project.groups.Add(new Group()
                                    {
                                        groupID = reader.GetInt32("groupID"),
                                        groupName = reader.GetString("groupName"),
                                        isActive = reader.GetBoolean("groupIsActive"),
                                        users = users
                                    });
                                }
                            }

                        }
                    }
                }
            }
            return project;
        }

        public static int GetCourseForGroup(int groupID)
        {
            int courseID = 0;
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "SELECT c.courseID FROM courses c LEFT JOIN projects p ON (c.courseID = p.courseID) " +
                        "LEFT JOIN cs4450.groups g ON (p.projectID = g.projectID) WHERE g.groupID = @groupID";
                    cmd.Parameters.AddWithValue("@groupID", groupID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            courseID = reader.GetInt32("courseID");
                        }
                    }
                }
            }
            return courseID;
        }

        public static int GetCourseForProject(int projectID)
        {
            int courseID = 0;
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "SELECT courseID FROM projects WHERE projectID = @projectID";
                    cmd.Parameters.AddWithValue("@projectID", projectID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            courseID = reader.GetInt32("courseID");
                        }
                    }
                }
            }
            return courseID;
        }

        public static List<Course> GetCourses()
        {
            List<Course> course = new List<Course>();
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select c.*, CONCAT(u.firstName, ' ',u.lastName) as instructorName " +
                        "FROM courses c " +
                        "LEFT JOIN users u ON (c.instructorID = u.userID) " +
                        "ORDER BY courseName DESC";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {

                            course.Add(new Course()
                            {
                                courseID = reader.GetInt32("courseID"),
                                courseName = reader.GetString("courseName"),
                                instructorID = reader.GetInt32("instructorID"),
                                isActive = reader.GetBoolean("isActive"),
                                description = reader.GetString("description"),
                                instructorName = reader.GetString("instructorName")
                            });
                        }
                    }
                }
            }
            return course;
        }

        public static List<Course> GetCoursesForInstructor(int instructorID)
        {
            List<Course> course = new List<Course>();
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select c.*, CONCAT(u.firstName, ' ',u.lastName) as instructorName " +
                        "FROM courses c " +
                        "LEFT JOIN users u ON (c.instructorID = u.userID)";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {

                            course.Add(new Course()
                            {
                                courseID = reader.GetInt32("courseID"),
                                courseName = reader.GetString("courseName"),
                                instructorID = reader.GetInt32("instructorID"),
                                isActive = reader.GetBoolean("isActive"),
                                description = reader.GetString("description"),
                                instructorName = reader.GetString("instructorName")
                            });
                        }
                    }
                }
            }
            return course;

        }

        public static Group GetGroup(int groupID)
        {
            Group group = new Group();
            group.users = new List<User>();
            bool foundUser = false;

            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "Select g.*, u.userID, u.firstName, u.lastName, t.groupID AS 'tgroupID', t.timeID, " +
                                      "date_format(t.timeIn, '%m/%d/%Y %l:%i %p') AS 'timeIn', date_format(t.timeOut, '%m/%d/%Y %l:%i %p') AS 'timeOut', " +
                                      "t.description, t.isEdited, t.userID AS 'tuserID', ug.isActive AS isActiveInGroup  " +
                                      "From cs4450.groups g Left Join uGroups ug On " +
                                      "ug.groupID = g.groupID " +
                                      "Left Join users u On " +
                                      "u.userID = ug.userID " +
                                      "Left Join timeCards t On " +
                                      "(u.userID = t.userID AND g.groupID = t.groupID) " +
                                      "Where g.groupID = @groupID";
                    cmd.Parameters.AddWithValue("@groupID", groupID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            foundUser = false;
                            group.groupName = reader.GetString("groupName");
                            group.groupID = reader.GetInt32("groupID");
                            group.isActive = reader.GetBoolean("isActive");
                            group.projectID = reader.GetInt32("projectID");

                            //get each users time info
                            foreach (User user in group.users)
                            {
                                if (user.userID == reader.GetInt32("userID"))
                                {
                                    foundUser = true;
                                    //Add time slot

                                    if (user.timecards == null) user.timecards = new List<TimeCard>();

                                    if (!reader.IsDBNull(9))
                                    {
                                        user.timecards.Add(new TimeCard()
                                        {
                                            timeIn = reader.IsDBNull(10) ? "" : reader.GetString("timeIn"),
                                            timeOut = reader.IsDBNull(11) ? "" : reader.GetString("timeOut"),
                                            description = reader.GetString("description"),
                                            groupID = reader.GetInt32("tgroupID"),
                                            timeslotID = reader.GetInt32("timeID"),
                                            isEdited = reader.GetBoolean("isEdited"),
                                            userID = reader.GetInt32("tuserID")
                                        });
                                    }
                                }
                            }

                            if (!foundUser)
                            {
                                List<TimeCard> timecardlist = new List<TimeCard>();
                                if (!reader.IsDBNull(9))
                                {
                                    timecardlist.Add(new TimeCard()
                                    {
                                        timeIn = reader.IsDBNull(10) ? "" : reader.GetString("timeIn"),
                                        timeOut = reader.IsDBNull(11) ? "" : reader.GetString("timeOut"),
                                        description = reader.GetString("description"),
                                        groupID = reader.GetInt32("tgroupID"),
                                        timeslotID = reader.GetInt32("timeID"),
                                        isEdited = reader.GetBoolean("isEdited"),
                                        userID = reader.GetInt32("tuserID")
                                    });
                                }

                                //Add the user and then the time slot
                                if (!reader.IsDBNull(5))
                                {
                                    group.users.Add(new User()
                                    {
                                        userID = reader.GetInt32("userID"),
                                        firstName = reader.GetString("firstName"),
                                        lastName = reader.GetString("lastName"),
                                        timecards = timecardlist,
                                        isActive = reader.GetBoolean("isActiveInGroup")
                                    });
                                }
                            }
                        }
                    }
                }
            }
            return group;
        }

        public static long CreateCategory(int evalTemplateID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "INSERT INTO evalTemplateQuestionCategories (evalTemplateID, categoryName) " +
                        "VALUES (@evalTemplateID, 'New Category')";
                    cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplateID);

                    //Return the last inserted ID if successful
                    if (cmd.ExecuteNonQuery() > 0) return cmd.LastInsertedId;
                    return 0;
                }
            }
        }

        public static int GetInstructorForCourse(int courseID)
        {
            int instructorID = 0;
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "SELECT instructorID FROM courses WHERE courseID = @courseID";
                    cmd.Parameters.AddWithValue("@courseID", courseID);

                    // cmd.CommandText = "SELECT firstName, lastName FROM courses WHERE "
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            instructorID = reader.GetInt32("instructorID");
                        }
                    }
                }
            }
            return instructorID;
        }

        public static int GetInstructorForEval(int evalTemplateID)
        {
            int instructorID = 0;
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "SELECT userID FROM evalTemplates WHERE evalTemplateID = @evalTemplateID";
                    cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplateID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            instructorID = reader.GetInt32("userID");
                        }
                    }
                }
            }
            return instructorID;
        }

        public static long CreateTemplateQuestion(int evalTemplateQuestionCategoryID, int evalTemplateID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "INSERT INTO evalTemplateQuestions (evalTemplateID, evalTemplateQuestionCategoryID, questionType, questionText, number) " +
                        "VALUES (@evalTemplateID, @evalTemplateQuestionCategoryID, 'N', '', 0)";
                    cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplateID);
                    cmd.Parameters.AddWithValue("@evalTemplateQuestionCategoryID", evalTemplateQuestionCategoryID);

                    //Return the last inserted ID if successful
                    if (cmd.ExecuteNonQuery() > 0) return cmd.LastInsertedId;
                    return 0;
                }
            }
        }

        public static List<Dashboard> GetDashboard(int userID)
        {
            List<Dashboard> dashboard = new List<Dashboard>();
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "SELECT g.groupID, g.groupName, p.projectID, p.projectName, c.courseID, c.courseName, " +
                        "u.userID AS 'instructorID', CONCAT(u.firstName, ' ',u.lastName) as instructorName FROM uGroups uG " +
                        "LEFT JOIN cs4450.groups g ON g.groupID = uG.groupID " +
                        "LEFT JOIN projects p ON g.projectID = p.projectID " +
                        "LEFT JOIN courses c on p.courseID = c.courseID " +
                        "LEFT JOIN users u ON c.instructorID = u.userID " +
                        "WHERE uG.userID = @userID " +
                        "AND uG.isActive = 1 " +
                        "AND g.isActive = 1 " +
                        "AND p.isActive = 1 " +
                        "AND c.isActive = 1 ";
                    cmd.Parameters.AddWithValue("@userID", userID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            dashboard.Add(new Dashboard()
                            {
                                groupID = reader.GetInt32("groupID"),
                                groupName = reader.GetString("groupName"),
                                projectID = reader.GetInt32("projectID"),
                                projectName = reader.GetString("projectName"),
                                courseID = reader.GetInt32("courseID"),
                                courseName = reader.GetString("courseName"),
                                instructorID = reader.GetInt32("instructorID"),
                                instructorName = reader.GetString("instructorName")
                            });
                        }
                    }
                }
            }
            return dashboard;
        }

        public static User GetUser(string username)
        {
            username = username.ToLower();
            User user = null;
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "SELECT * FROM users WHERE username = @username";
                    cmd.Parameters.AddWithValue("@username", username);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            user = new User()
                            {
                                userID = reader.GetInt32("userID"),
                                username = reader.GetString("username"),
                                firstName = reader.GetString("firstName"),
                                lastName = reader.GetString("lastName"),
                                type = reader.GetChar("type"),
                                isActive = reader.GetBoolean("isActive")
                            };
                        }
                    }
                }
            }
            return user;
        }

        public static List<AdminEval> GetAllEvals()
        {
            List<AdminEval> evals = new List<AdminEval>();
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "SELECT e.*, CONCAT(u.firstName, ' ', u.lastName) AS usersName, g.groupName, p.projectID, p.projectName, " +
                                          "c.courseID, c.courseName, et.templateName, c.instructorID, CONCAT(ui.firstName, ' ', ui.lastName) AS instructorName " +
                                        "FROM evals e " +
                                        "LEFT JOIN cs4450.groups g on e.groupID = g.groupID " +
                                        "LEFT JOIN users u on e.userID = u.userID " +
                                        "LEFT JOIN projects p on g.projectID = p.projectID " +
                                        "LEFT JOIN courses c on p.courseID = c.courseID " +
                                        "LEFT JOIN evalTemplates et on e.evalTemplateID = et.evalTemplateID " +
                                        "LEFT JOIN users ui on c.instructorID = ui.userID";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            evals.Add(new AdminEval()
                            {
                                evalID = reader.GetInt32("evalID"),
                                evalTemplateID = reader.GetInt32("evalTemplateID"),
                                groupID = reader.GetInt32("groupID"),
                                userID = reader.GetInt32("userID"),
                                number = reader.GetInt32("number"),
                                isComplete = reader.GetBoolean("isComplete"),
                                usersName = reader.GetString("usersName"),
                                groupName = reader.GetString("groupName"),
                                projectID = reader.GetInt32("projectID"),
                                projectName = reader.GetString("projectName"),
                                courseID = reader.GetInt32("courseID"),
                                courseName = reader.GetString("courseName"),
                                templateName = reader.GetString("templateName"),
                                instructorID = reader.GetInt32("instructorID"),
                                instructorName = reader.GetString("instructorName")
                            });
                        }
                    }
                }
            }
            return evals;
        }

        public static List<Eval> EvalResponsesA(int groupID, int userID)
        {
            List<Eval> evals = new List<Eval>();

            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT er.*, u.firstName, e.number AS 'evalNumber', etq.number AS 'questionNumber', " +
                                    "u.lastName, etq.questionText, etq.evalTemplateID, etq.questionType, etq.evalTemplateQuestionCategoryID, " +
                                    "etqc.categoryName, etqc.number AS 'categoryNumber' " +
                                    "FROM evalResponses er " +
                                    "  INNER JOIN evals e ON er.evalID = e.evalID " +
                                    "  INNER JOIN users u ON u.userID = e.userID " +
                                    "  INNER JOIN evalTemplateQuestions etq ON etq.evalTemplateQuestionID = er.evalTemplateQuestionID " +
                                    "  LEFT JOIN evalTemplateQuestionCategories etqc ON etqc.evalTemplateQuestionCategoryID = etq.evalTemplateQuestionCategoryID " +
                                    "WHERE groupID = @groupID AND er.userID = @userID";
                    cmd.Parameters.AddWithValue("@groupID", groupID);
                    cmd.Parameters.AddWithValue("@userID", userID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bool foundEval = false;
                            foreach (Eval eval in evals)
                            {
                                if (eval.number != reader.GetInt32("evalNumber"))
                                {
                                    continue;
                                }
                                foundEval = true;

                                //Adding Eval entries
                                bool foundEvalColumn = false;
                                foreach (EvalColumn evalColumn in eval.evals)
                                {
                                    if (evalColumn.evalID == reader.GetInt32("evalID"))
                                    {
                                        foundEvalColumn = true;
                                        break;
                                    }
                                }

                                if (!foundEvalColumn)
                                {
                                    eval.evals.Add(new EvalColumn()
                                    {
                                        evalID = reader.GetInt32("evalID"),
                                        firstName = reader.GetString("firstName"), //Name is Team Member for anonymity
                                        lastName = reader.GetString("lastName")
                                    });
                                }

                                //Adding Template Questions
                                bool foundTemplateQuestion = false;
                                foreach (EvalTemplateQuestion tq in eval.templateQuestions)
                                {
                                    if (tq.evalTemplateQuestionID == reader.GetInt32("evalTemplateQuestionID"))
                                    {
                                        foundTemplateQuestion = true;
                                        break;
                                    }
                                }

                                if (!foundTemplateQuestion)
                                {
                                    eval.templateQuestions.Add(new EvalTemplateQuestion()
                                    {
                                        questionText = reader.GetString("questionText"),
                                        evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
                                        questionType = reader.GetChar("questionType"),
                                        evalTemplateQuestionCategoryID = reader.GetInt32("evalTemplateQuestionCategoryID"),
                                        number = reader.GetInt32("questionNumber")
                                    });
                                }

                                //Adding Categories if they're there
                                if (!reader.IsDBNull(13)) //column 13 = categoryName
                                {
                                    bool foundCategory = false;
                                    foreach (EvalTemplateQuestionCategory tqc in eval.categories)
                                    {
                                        if (tqc.evalTemplateQuestionCategoryID == reader.GetInt32("evalTemplateQuestionCategoryID"))
                                        {
                                            foundCategory = true;
                                            break;
                                        }
                                    }

                                    if (!foundCategory)
                                    {
                                        eval.categories.Add(new EvalTemplateQuestionCategory()
                                        {
                                            evalTemplateQuestionCategoryID = reader.GetInt32("evalTemplateQuestionCategoryID"),
                                            categoryName = reader.GetString("categoryName"),
                                            number = reader.GetInt32("categoryNumber")
                                        });
                                    }
                                }

                                //Every row is a unique response, so we don't need to worry about existing ones
                                eval.responses.Add(new EvalResponse()
                                {
                                    evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
                                    evalID = reader.GetInt32("evalID"),
                                    response = reader.GetString("response"),
                                    evalResponseID = reader.GetInt32("evalResponseID"),
                                    userID = reader.GetInt32("userID")
                                });
                            }

                            if (!foundEval)
                            {
                                Eval eval = new Eval();
                                eval.evalTemplateID = reader.GetInt32("evalTemplateID");
                                eval.userID = reader.GetInt32("userID");
                                eval.groupID = groupID;
                                eval.number = reader.GetInt32("evalNumber");

                                //Adding evalColumn
                                eval.evals = new List<EvalColumn>();
                                eval.evals.Add(new EvalColumn()
                                {
                                    evalID = reader.GetInt32("evalID"),
                                    firstName = reader.GetString("firstName"), //Name is Team Member for anonymity
                                    lastName = reader.GetString("lastName")
                                });

                                //Adding templateQuestion
                                eval.templateQuestions = new List<EvalTemplateQuestion>();
                                eval.templateQuestions.Add(new EvalTemplateQuestion()
                                {
                                    questionText = reader.GetString("questionText"),
                                    evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
                                    questionType = reader.GetChar("questionType"),
                                    evalTemplateQuestionCategoryID = reader.GetInt32("evalTemplateQuestionCategoryID"),
                                    number = reader.GetInt32("questionNumber")
                                });

                                //Adding Categories if they're there
                                eval.categories = new List<EvalTemplateQuestionCategory>();
                                if (!reader.IsDBNull(13)) //column 13 = categoryName
                                {
                                    eval.categories.Add(new EvalTemplateQuestionCategory()
                                    {
                                        evalTemplateQuestionCategoryID = reader.GetInt32("evalTemplateQuestionCategoryID"),
                                        categoryName = reader.GetString("categoryName"),
                                        number = reader.GetInt32("categoryNumber")
                                    });
                                }

                                //Adding Response
                                eval.responses = new List<EvalResponse>();
                                eval.responses.Add(new EvalResponse()
                                {
                                    evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
                                    evalID = reader.GetInt32("evalID"),
                                    response = reader.GetString("response"),
                                    evalResponseID = reader.GetInt32("evalResponseID"),
                                    userID = reader.GetInt32("userID")
                                });

                                evals.Add(eval); //Adding new eval to the list
                            }
                        }
                    }
                }
            }
            TeammateStats(ref evals);
            return evals;
        }

        public static List<Eval> EvalResponses(int groupID, int userID)
        {
            List<Eval> evals = new List<Eval>();

            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT er.*, u.firstName, e.number AS 'evalNumber', etq.number AS 'questionNumber', " +
                                    "u.lastName, etq.questionText, etq.evalTemplateID, etq.questionType, etq.evalTemplateQuestionCategoryID, " +
                                    "etqc.categoryName, etqc.number AS 'categoryNumber' " +
                                    "FROM evalResponses er " +
                                    "  INNER JOIN evals e ON er.evalID = e.evalID " +
                                    "  INNER JOIN users u ON u.userID = e.userID " +
                                    "  INNER JOIN evalTemplateQuestions etq ON etq.evalTemplateQuestionID = er.evalTemplateQuestionID " +
                                    "  LEFT JOIN evalTemplateQuestionCategories etqc ON etqc.evalTemplateQuestionCategoryID = etq.evalTemplateQuestionCategoryID " +
                                    "WHERE groupID = @groupID AND er.userID = @userID";
                    cmd.Parameters.AddWithValue("@groupID", groupID);
                    cmd.Parameters.AddWithValue("@userID", userID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bool foundEval = false;
                            foreach (Eval eval in evals)
                            {
                                if (eval.number != reader.GetInt32("evalNumber"))
                                {
                                    continue;
                                }
                                foundEval = true;

                                //Adding Eval entries
                                bool foundEvalColumn = false;
                                foreach (EvalColumn evalColumn in eval.evals)
                                {
                                    if (evalColumn.evalID == reader.GetInt32("evalID"))
                                    {
                                        foundEvalColumn = true;
                                        break;
                                    }
                                }

                                if (!foundEvalColumn)
                                {
                                    eval.evals.Add(new EvalColumn()
                                    {
                                        evalID = reader.GetInt32("evalID"),
                                        firstName = "Team", //Name is Team Member for anonymity
                                        lastName = "Member"
                                    });
                                }

                                //Adding Template Questions
                                bool foundTemplateQuestion = false;
                                foreach (EvalTemplateQuestion tq in eval.templateQuestions)
                                {
                                    if (tq.evalTemplateQuestionID == reader.GetInt32("evalTemplateQuestionID"))
                                    {
                                        foundTemplateQuestion = true;
                                        break;
                                    }
                                }

                                if (!foundTemplateQuestion)
                                {
                                    eval.templateQuestions.Add(new EvalTemplateQuestion()
                                    {
                                        questionText = reader.GetString("questionText"),
                                        evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
                                        questionType = reader.GetChar("questionType"),
                                        evalTemplateQuestionCategoryID = reader.GetInt32("evalTemplateQuestionCategoryID"),
                                        number = reader.GetInt32("questionNumber")
                                    });
                                }

                                //Adding Categories if they're there
                                if (!reader.IsDBNull(13)) //column 13 = categoryName
                                {
                                    bool foundCategory = false;
                                    foreach (EvalTemplateQuestionCategory tqc in eval.categories)
                                    {
                                        if (tqc.evalTemplateQuestionCategoryID == reader.GetInt32("evalTemplateQuestionCategoryID"))
                                        {
                                            foundCategory = true;
                                            break;
                                        }
                                    }

                                    if (!foundCategory)
                                    {
                                        eval.categories.Add(new EvalTemplateQuestionCategory()
                                        {
                                            evalTemplateQuestionCategoryID = reader.GetInt32("evalTemplateQuestionCategoryID"),
                                            categoryName = reader.GetString("categoryName"),
                                            number = reader.GetInt32("categoryNumber")
                                        });
                                    }
                                }

                                //Every row is a unique response, so we don't need to worry about existing ones
                                eval.responses.Add(new EvalResponse()
                                {
                                    evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
                                    evalID = reader.GetInt32("evalID"),
                                    response = reader.GetString("response"),
                                    evalResponseID = reader.GetInt32("evalResponseID"),
                                    userID = reader.GetInt32("userID")
                                });
                            }

                            if (!foundEval)
                            {
                                Eval eval = new Eval();
                                eval.evalTemplateID = reader.GetInt32("evalTemplateID");
                                eval.userID = reader.GetInt32("userID");
                                eval.groupID = groupID;
                                eval.number = reader.GetInt32("evalNumber");

                                //Adding evalColumn
                                eval.evals = new List<EvalColumn>();
                                eval.evals.Add(new EvalColumn()
                                {
                                    evalID = reader.GetInt32("evalID"),
                                    firstName = "Team", //Name is Team Member for anonymity
                                    lastName = "Member"
                                });

                                //Adding templateQuestion
                                eval.templateQuestions = new List<EvalTemplateQuestion>();
                                eval.templateQuestions.Add(new EvalTemplateQuestion()
                                {
                                    questionText = reader.GetString("questionText"),
                                    evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
                                    questionType = reader.GetChar("questionType"),
                                    evalTemplateQuestionCategoryID = reader.GetInt32("evalTemplateQuestionCategoryID"),
                                    number = reader.GetInt32("questionNumber")
                                });

                                //Adding Categories if they're there
                                eval.categories = new List<EvalTemplateQuestionCategory>();
                                if (!reader.IsDBNull(13)) //column 13 = categoryName
                                {
                                    eval.categories.Add(new EvalTemplateQuestionCategory()
                                    {
                                        evalTemplateQuestionCategoryID = reader.GetInt32("evalTemplateQuestionCategoryID"),
                                        categoryName = reader.GetString("categoryName"),
                                        number = reader.GetInt32("categoryNumber")
                                    });
                                }

                                //Adding Response
                                eval.responses = new List<EvalResponse>();
                                eval.responses.Add(new EvalResponse()
                                {
                                    evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
                                    evalID = reader.GetInt32("evalID"),
                                    response = reader.GetString("response"),
                                    evalResponseID = reader.GetInt32("evalResponseID"),
                                    userID = reader.GetInt32("userID")
                                });

                                evals.Add(eval); //Adding new eval to the list
                            }
                        }
                    }
                }
            }
            TeammateStats(ref evals);
            return evals;
        }

        //  Helper function that will assist with total a score per each teammate
        private static void TeammateStats( ref List<Eval> eval)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                foreach(Eval e in eval)
                {
                    foreach (EvalResponse r in e.responses)
                    {
                        conn.Open();
                        //  Get avg score student gave for the specific eval
                        using (MySqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText =
                            "SELECT u.userID, u.firstName, u.lastName, AVG(er.response) AS avg " +
                            "FROM evalResponses er INNER JOIN evals e ON er.evalID = e.evalID " +
                            "INNER JOIN users u ON u.userID = e.userID " +
                            "INNER JOIN evalTemplateQuestions etq ON etq.evalTemplateQuestionID = er.evalTemplateQuestionID " +
                            "LEFT JOIN evalTemplateQuestionCategories etqc ON etqc.evalTemplateQuestionCategoryID = etq.evalTemplateQuestionCategoryID " +
                            "WHERE groupID = @groupID " +
                            "AND e.evalID = @evalID " +
                            "GROUP BY u.userID;"; 

                            cmd.Parameters.AddWithValue("@evalID", r.evalID);
                            cmd.Parameters.AddWithValue("@groupID", e.groupID);

                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    r.userAvgerage = reader.GetDouble("avg");
                                }
                            }
                        }
                        conn.Close();
                        
                    }
                }
            }
            return;
        }
        

        public static User GetUser(string username, string password)
        {
            username = username.ToLower();
            User user = null;
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "SELECT * FROM users WHERE username = @username AND password = @password";
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            user = new User()
                            {
                                userID = reader.GetInt32("userID"),
                                username = reader.GetString("username"),
                                firstName = reader.GetString("firstName"),
                                lastName = reader.GetString("lastName"),
                                type = reader.GetChar("type"),
                                isActive = reader.GetBoolean("isActive")
                            };
                        }
                    }
                }
            }
            return user;
        }

        public static User GetUserByID(int ID)
        {
            User user = null;
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "SELECT * FROM users WHERE userID = @ID";
                    cmd.Parameters.AddWithValue("@ID", ID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            user = new User()
                            {
                                userID = reader.GetInt32("userID"),
                                username = reader.GetString("username"),
                                firstName = reader.GetString("firstName"),
                                lastName = reader.GetString("lastName"),
                                type = reader.GetChar("type"),
                                isActive = reader.GetBoolean("isActive")
                            };
                        }
                    }
                }
            }
            return user;
        }
        
        
        public static long CreateTemplate(int userID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "INSERT INTO evalTemplates (userID, templateName) " +
                        "VALUES (@userID, 'New Template')";
                    cmd.Parameters.AddWithValue("@userID", userID);

                    //Return the last inserted ID if successful
                    if (cmd.ExecuteNonQuery() > 0) return cmd.LastInsertedId;
                    return 0;
                }
            }
        }

        public static bool SaveTemplateName(EvalTemplate evalTemplate)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "UPDATE evalTemplates SET templateName = @templateName " +
                        "WHERE evalTemplateID = @evalTemplateID";
                    cmd.Parameters.AddWithValue("@templateName", evalTemplate.templateName);
                    cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplate.evalTemplateID);
                    
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool SaveEval(AdminEval eval)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "UPDATE evals SET isComplete = @isComplete " +
                        "WHERE evalID = @evalID";
                    cmd.Parameters.AddWithValue("@evalId", eval.evalID);
                    cmd.Parameters.AddWithValue("@isComplete", eval.isComplete);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool CreateTemplateCopy(int userID, int evalTemplateID)
        {
            string templateName = "";
            string catName = "";
            string temp = "";

            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT templateName FROM evalTemplates WHERE evalTemplateID = @evalTemplateID";
                    cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplateID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            templateName = reader.GetString("templateName");
                        }
                    }

                    //SQL and Parameters
                    cmd.CommandText = "INSERT INTO evalTemplates (userID, templateName) " +
                        "VALUES (@userID, '@TempName')";
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@TempName", templateName);

                    //Return the last inserted ID if successful
                    cmd.ExecuteNonQuery();
                    string higher = "0";
                    cmd.Parameters.AddWithValue("@questionType", higher);
                    cmd.Parameters.AddWithValue("@questionText", higher);
                    cmd.Parameters.AddWithValue("@number", higher);
                    cmd.CommandText = "SELECT * FROM evalTemplateQuestions AS 'ETQ' INNER JOIN evalTemplateQuestionCategories AS 'ETC' "
                    + "ON ETC.evalTemplateQuestionCategoryID = ETQ.evalTemplateQuestionCategoryID WHERE ETQ.evalTemplateID = " 
                    + "@evalTemplateID ORDER BY ETC.categoryName";
                    cmd.Parameters.AddWithValue("@evalTemplateID", cmd.LastInsertedId);
                    cmd.Parameters.AddWithValue("@evalTemplateQuestionCategoryID", higher);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int catNum = 0;
                        while (reader.Read())
                        {
                            catName = reader.GetString("ETC.categoryName");
                            if (catName != temp)
                            {
                                cmd.CommandText = "INSERT INTO evalTemplateQuestionCategory (evalTemplateID, categoryName) "
                                + "VALUES (@evalTemplateID, @categoryName)";
                                cmd.ExecuteNonQuery();
                                cmd.Parameters["@evalTemplateQuestionCategoryID"].Value = cmd.LastInsertedId;
                            }
                            temp = catName;
                            catNum = reader.GetInt32("ETQ.evalTemplateQuestionCategoryID");

                            cmd.CommandText = "INSERT INTO evalTemplateQuestions (evalTemplateID, evalTemplateQuestionCategoryID, "
                            + "questionType, questionText, number) VALUES (@evalTemplateID, @evalTemplateQuestionCategoryID, "
                            + "@questionType, @questionText, @number)";
                            cmd.Parameters["@questionType"].Value = reader.GetString("ETQ.questionType");
                            cmd.Parameters["@questionText"].Value = reader.GetString("ETQ.questionText");
                            cmd.Parameters["@number"].Value = reader.GetString("ETQ.number");

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            return true;
        }

        public static bool SaveResponse(int userID, int evalID, int evalTemplateQuestionID, string response)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "INSERT INTO evalResponses (evalID, evalTemplateQuestionID, userID, response) " +
                        "VALUES (@evalID, @evalTemplateQuestionID, @userID, @response)";
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@evalID", evalID);
                    cmd.Parameters.AddWithValue("@evalTemplateQuestionID", evalTemplateQuestionID);
                    cmd.Parameters.AddWithValue("@response", response);

                    //Return the last inserted ID if successful
                    if (cmd.ExecuteNonQuery() > 0) return true;
                    return false;
                }
            }
        }

        public static bool CompleteEval(int evalID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "UPDATE evals SET isComplete = 1 WHERE evalID = @evalID";
                    cmd.Parameters.AddWithValue("@evalID", evalID);

                    //Return the last inserted ID if successful
                    if (cmd.ExecuteNonQuery() > 0) return true;
                    return false;
                }
            }
        }

        public static List<User> GetUsers()
        {
            List<User> user = new List<User>();
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "SELECT * FROM users";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {

                            user.Add(new User()
                            {
                                userID = reader.GetInt32("userID"),
                                username = reader.GetString("username"),
                                firstName = reader.GetString("firstName"),
                                lastName = reader.GetString("lastName"),
                                type = reader.GetChar("type"),
                                isActive = reader.GetBoolean("isActive"),
                            });
                        }
                    }
                }
            }
            return user;
        }

        public static List<User> GetUsersForGroup(int groupID)
        {
            List<User> users = new List<User>();
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "SELECT * FROM uGroups ug LEFT JOIN users u ON ug.userID = u.userID WHERE ug.groupID = @groupID";
                    cmd.Parameters.AddWithValue("@groupID", groupID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            users.Add(new User()
                            {
                                userID = reader.GetInt32("userID"),
                                firstName = reader.GetString("firstName"),
                                lastName = reader.GetString("lastName"),
                            });
                        }
                    }
                }
            }
            return users;
        }

        public static List<User> GetInactiveUsersForCourse(int courseID)
        {
            List<User> users = new List<User>();
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM uCourses uc INNER JOIN users u ON uc.userID = u.userID WHERE uc.courseID = @courseID AND uc.isActive = 0";
                    cmd.Parameters.Add("@courseID", MySqlDbType.Int32).Value = courseID;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User()
                            {
                                userID = reader.GetInt32("userID"),
                                firstName = reader.GetString("firstName"),
                                lastName = reader.GetString("lastName")
                            });
                        }
                    }
                }
            }
            return users;
        }

        public static bool IsUserInGroup(int userID, int groupID)
        {
            bool isInGroup = false;
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = " SELECT * FROM uGroups WHERE userID = @userID AND groupID = @groupID";
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@groupID", groupID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            if (!isInGroup) isInGroup = true;
                        }
                    }
                }
            }
            return isInGroup;
        }

        public static bool IsActiveUserInGroup(int userID, int groupID)
        {
            bool isInGroup = false;
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = " SELECT * FROM uGroups WHERE userID = @userID AND groupID = @groupID AND isActive = 1";
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@groupID", groupID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            if (!isInGroup) isInGroup = true;
                        }
                    }
                }
            }
            return isInGroup;
        }

        public static bool IsUserInOtherGroup(int userID, int groupID)
        {
            bool isInGroup = false;
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = " Select u.userID, u.firstName, u.lastName, ug.groupID From users u " +
                        "Inner Join uGroups ug On u.userID = ug.userID Inner Join cs4450.groups g On ug.groupID = g.groupID Where u.userID = @userID " +
                        "And g.projectID = 	(SELECT projectID FROM cs4450.groups WHERE groupID = @groupID) " +
                        "And ug.isActive = 1";
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@groupID", groupID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            if (!isInGroup) isInGroup = true;
                        }
                    }
                }
            }
            return isInGroup;
        }

        public static bool IsUserInGroupForProject(int userID, int projectID)
        {
            bool isInGroup = false;
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = " Select u.userID, u.firstName, u.lastName, ug.groupID From users u " +
                                      "Inner Join uGroups ug On u.userID = ug.userID Inner Join cs4450.groups g On ug.groupID = g.groupID Where u.userID = @userID " +
                                      "And g.projectID = @projectID " +
                                      "And ug.isActive = 1";
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@projectID", projectID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            if (!isInGroup) isInGroup = true;
                        }
                    }
                }
            }
            return isInGroup;
        }

        public static bool UserHasTimeInGroup(int userID, int groupID)
        {
            bool hasTimeInGroup = false;
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "SELECT * FROM timeCards WHERE userID = @userID AND groupID = @groupID";
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@groupID", groupID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            if (!hasTimeInGroup) hasTimeInGroup = true;
                        }
                    }
                }
            }
            return hasTimeInGroup;
        }

        public static bool JoinCourse(int courseID, int userID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO uCourses (userID, courseID, isActive) VALUES (@userID, @courseID, 0)";
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@courseID", courseID);
                    if (cmd.ExecuteNonQuery() > 0) return true;

                    return false;
                }
            }
        }

        public static long JoinGroup(int userID, int groupID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "INSERT INTO uGroups (userID, groupID, isActive) " +
                        "VALUES (@userID, @groupID, 1)";
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@groupID", groupID);

                    //Return the last inserted ID if successful
                    if (cmd.ExecuteNonQuery() > 0) return cmd.LastInsertedId;

                    return 0;
                }
            }
        }

        public static bool ReJoinGroup(int userID, int groupID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "UPDATE uGroups SET isActive = 1 WHERE userID = @userID AND groupID = @groupID";
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@groupID", groupID);

                    if (cmd.ExecuteNonQuery() > 0) return true;

                    return false;
                }
            }
        }

        public static bool LeaveCourse(int courseID, int userID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    if (UserIsInCourse(courseID, userID))
                    {
                        cmd.CommandText = "UPDATE uCourses SET isActive = 0 WHERE courseID = @courseID AND userID = @userID";
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Parameters.AddWithValue("@courseID", courseID);
                        if (cmd.ExecuteNonQuery() > 0) return true;
                    }
                    return false;
                }
            }
        }

        public static bool SaveUserInCourse(uCourse uCourse)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE uCourses SET isActive = @isActive WHERE courseID = @courseID AND userID = @userID";
                    cmd.Parameters.AddWithValue("@userID", uCourse.userID);
                    cmd.Parameters.AddWithValue("@courseID", uCourse.courseID);
                    cmd.Parameters.AddWithValue("@isActive", uCourse.isActive);
                    if (cmd.ExecuteNonQuery() > 0) return true;
                    return false;
                }
            }
        }

        public static bool DeleteFromCourse(int courseID, int userID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    if (UserIsInCourse(courseID, userID))
                    {
                        cmd.CommandText = "DELETE FROM uCourses WHERE courseID = @courseID AND userID = @userID";
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Parameters.AddWithValue("@courseID", courseID);
                        if (cmd.ExecuteNonQuery() > 0) return true;
                    }
                    return false;
                }
            }
        }

        public static bool SaveCourse(Course course)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    // SQL and Parameters
                    cmd.CommandText = "UPDATE courses SET courseName = @courseName, instructorID = @instructorID, " +
                                      "isActive = @isActive, description = @description WHERE courseID = @courseID";
                    cmd.Parameters.AddWithValue("@courseName", course.courseName);
                    cmd.Parameters.AddWithValue("@instructorID", course.instructorID);
                    cmd.Parameters.AddWithValue("@isActive", course.isActive);
                    cmd.Parameters.AddWithValue("@description", course.description);
                    cmd.Parameters.AddWithValue("@courseID", course.courseID);

                    if (cmd.ExecuteNonQuery() > 0) return true;
                    return false;
                }
            }
        }

        public static bool SaveGroup(Group group)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    // SQL and Parameters
                    cmd.CommandText = "UPDATE cs4450.groups SET groupName = @groupName, " +
                                      "isActive = @isActive, evalID = @evalID, projectID = @projectID WHERE groupID = @groupID";
                    cmd.Parameters.AddWithValue("@groupName", group.groupName);
                    cmd.Parameters.AddWithValue("@isActive", group.isActive);
                    cmd.Parameters.AddWithValue("@evalID", group.evalID);
                    cmd.Parameters.AddWithValue("@projectID", group.projectID);
                    cmd.Parameters.AddWithValue("@groupID", group.groupID);

                    if (cmd.ExecuteNonQuery() > 0) return true;
                    return false;
                }
            }
        }

        public static bool DeleteFromGroup(int userID, int groupID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM uGroups WHERE userID = @userID AND groupID = @groupID";
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@groupID", groupID);
                    if (cmd.ExecuteNonQuery() > 0) return true;

                    return false;
                }
            }
        }

        public static bool LeaveGroup(int userID, int groupID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE uGroups SET isActive = 0 WHERE userID = @userID AND groupID = @groupID";
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@groupID", groupID);
                    if (cmd.ExecuteNonQuery() > 0) return true;

                    return false;
                }
            }
        }

        public static bool SaveProject(Project project)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    // SQL and Parameters
                    cmd.CommandText = "UPDATE projects SET projectName = @projectName, " +
                                      "isActive = @isActive, description = @description WHERE projectID = @projectID";
                    cmd.Parameters.AddWithValue("@projectName", project.projectName);
                    cmd.Parameters.AddWithValue("@isActive", project.isActive);
                    cmd.Parameters.AddWithValue("@description", project.description);
                    cmd.Parameters.AddWithValue("@projectID", project.projectID);

                    if (cmd.ExecuteNonQuery() > 0) return true;
                    return false;
                }
            }
        }

        public static bool SaveCategory(EvalTemplateQuestionCategory category)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    // SQL and Parameters
                    cmd.CommandText = "UPDATE evalTemplateQuestionCategories SET evalTemplateID = @evalTemplateID, " +
                                      "categoryName = @categoryName WHERE evalTemplateQuestionCategoryID = @evalTemplateQuestionCategoryID";
                    cmd.Parameters.AddWithValue("@evalTemplateID", category.evalTemplateID);
                    cmd.Parameters.AddWithValue("@categoryName", category.categoryName);
                    cmd.Parameters.AddWithValue("@evalTemplateQuestionCategoryID", category.evalTemplateQuestionCategoryID);

                    if (cmd.ExecuteNonQuery() > 0) return true;
                    return false;
                }
            }
        }

        public static bool DeleteCategory(int evalTemplateQuestionCategoryID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM evalTemplateQuestionCategories " +
                                      "WHERE evalTemplateQuestionCategoryID = @evalTemplateQuestionCategoryID";
                    cmd.Parameters.AddWithValue("@evalTemplateQuestionCategoryID", evalTemplateQuestionCategoryID);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        //  Added to delete all questions that below to a deleted category.
                        cmd.CommandText = "DELETE FROM evalTemplateQuestions " +
                                      "WHERE evalTemplateQuestionCategoryID = @evalTemplateQuestionCategoryID";
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
                return false;}
        }

        public static bool SaveQuestion(EvalTemplateQuestion question)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    // SQL and Parameters
                    cmd.CommandText = "UPDATE evalTemplateQuestions SET evalTemplateID = @evalTemplateID, " +
                                      "evalTemplateQuestionCategoryID = @evalTemplateQuestionCategoryID, " +
                                      "number = @number, questionType = @questionType, " +
                                      "questionText = @questionText WHERE evalTemplateQuestionID = @evalTemplateQuestionID";
                    cmd.Parameters.AddWithValue("@evalTemplateID", question.evalTemplateID);
                    cmd.Parameters.AddWithValue("@evalTemplateQuestionCategoryID", question.evalTemplateQuestionCategoryID);
                    cmd.Parameters.AddWithValue("@number", question.number);
                    cmd.Parameters.AddWithValue("@questionType", question.questionType);
                    cmd.Parameters.AddWithValue("@questionText", question.questionText);
                    cmd.Parameters.AddWithValue("@evalTemplateQuestionID", question.evalTemplateQuestionID);

                    if (cmd.ExecuteNonQuery() > 0) return true;
                    return false;
                }
            }
        }

        public static bool DeleteQuestion(int evalTemplateQuestionID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM evalTemplateQuestions WHERE evalTemplateQuestionID = @evalTemplateQuestionID";
                    cmd.Parameters.AddWithValue("@evalTemplateQuestionID", evalTemplateQuestionID);

                    if (cmd.ExecuteNonQuery() > 0) return true;
                    return false;
                }
            }
        }

        public static bool SaveTime(TimeCard timecard)
        {
            String edited = "";
            DateTime before;
            DateTime after;

            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT timeID, createdOn FROM timeCards WHERE timeID = @timeID";
                    cmd.Parameters.AddWithValue("@timeID", timecard.timeslotID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            edited = reader.GetString("createdOn");
                        }
                    }

                    after = Convert.ToDateTime(edited);
                    before = after.AddDays(-7);

                    if (after < before)
                    {
                        cmd.CommandText = "UPDATE timeCards SET timeIn = @timeIn, timeOut = @timeOut, isEdited = 1, description = @description WHERE timeID = @timeID";
                        if (timecard.timeIn == null || timecard.timeIn == "") cmd.Parameters.AddWithValue("@timeIn", null);
                        else cmd.Parameters.AddWithValue("@timeIn", Convert.ToDateTime(timecard.timeIn));

                        if (timecard.timeOut == null || timecard.timeOut == "") cmd.Parameters.AddWithValue("@timeOut", null);
                        else cmd.Parameters.AddWithValue("@timeOut", Convert.ToDateTime(timecard.timeOut));

                        if (timecard.description == null) cmd.Parameters.AddWithValue("@description", "");
                        else cmd.Parameters.AddWithValue("@description", timecard.description);
                        if (cmd.ExecuteNonQuery() > 0) return true;
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE timeCards SET timeIn = @timeIn, timeOut = @timeOut, description = @description WHERE timeID = @timeID";
                        if (timecard.timeIn == "" || timecard.timeIn == "") cmd.Parameters.AddWithValue("@timeIn", null);
                        else cmd.Parameters.AddWithValue("@timeIn", Convert.ToDateTime(timecard.timeIn));

                        if (timecard.timeOut == "" || timecard.timeOut == "") cmd.Parameters.AddWithValue("@timeOut", null);
                        else cmd.Parameters.AddWithValue("@timeOut", Convert.ToDateTime(timecard.timeOut));

                        if (timecard.description == null) cmd.Parameters.AddWithValue("@description", "");
                        else cmd.Parameters.AddWithValue("@description", timecard.description);
                        if (cmd.ExecuteNonQuery() > 0) return true;
                    }
                    return false;
                }
            }
        }

        public static bool UserIsInCourse(int courseID, int userID)
        {
            bool isInCourse = false;
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "SELECT userID FROM uCourses WHERE courseID = @courseID AND userID = @userID";
                    cmd.Parameters.AddWithValue("@courseID", courseID);
                    cmd.Parameters.AddWithValue("@userID", userID);

                    // cmd.CommandText = "SELECT firstName, lastName FROM courses WHERE "
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            if (!isInCourse) isInCourse = true;
                        }
                    }
                }
            }
            return isInCourse;
        }

        public static List<Project> GetProjects(int courseID)
        {

            List<Project> projects = new List<Project>();

            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "SELECT * FROM projects WHERE courseID = @courseID AND isActive = true";
                    cmd.Parameters.AddWithValue("@courseID", courseID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {

                            projects.Add(new Project()
                            {
                                projectID = reader.GetInt32("projectID"),
                                projectName = reader.GetString("projectName")

                            });

                        }
                    }
                }
            }
            return projects;

        }

        public static List<EvalTemplate> GetTemplates(int instructorID)
        {
            List<EvalTemplate> templates = new List<EvalTemplate>();

            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "Select * From evalTemplates e Where e.userID = @userID ";

                    cmd.Parameters.AddWithValue("@userID", instructorID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            templates.Add(new EvalTemplate()
                            {
                                evalTemplateID = reader.GetInt32("evalTemplateID"),
                                templateName = reader.GetString("templateName"),
                                inUse = reader.GetBoolean("inUse"),
                                userID = reader.GetInt32("userID"),
                            });
                        }
                    }
                }
            }
            return templates;
        }

        public static List<EvalTemplate> GetFullTemplatesForInstructor(int instructorID)
        {
            List<EvalTemplate> templates = new List<EvalTemplate>();
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "SELECT eT.*, eTQC.evalTemplateQuestionCategoryID, eTQC.categoryName, eTQC.number AS categoryNumber, " +
                                        "eTQ.evalTemplateQuestionID, eTQ.evalTemplateQuestionCategoryID AS qevalTemplateQuestionCategoryID, " +
                                        "eTQ.questionType, eTQ.questionText, eTQ.number AS questionNumber " +
                                    "FROM evalTemplates eT " +
                                    "LEFT JOIN evalTemplateQuestionCategories eTQC on eT.evalTemplateID = eTQC.evalTemplateID " +
                                    "LEFT JOIN evalTemplateQuestions eTQ on eT.evalTemplateID = eTQ.evalTemplateID " +
                                    "WHERE eT.userID = @userID ";
                    cmd.Parameters.AddWithValue("@userID", instructorID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        EvalTemplate template = new EvalTemplate();
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            if(template.evalTemplateID != reader.GetInt32("evalTemplateID"))
                            {
                                if (template.evalTemplateID > 0) templates.Add(template); //Adds the previous template before making a new one

                                template = new EvalTemplate() {
                                    evalTemplateID = reader.GetInt32("evalTemplateID"),
                                    templateName = reader.GetString("templateName"),
                                    inUse = reader.GetBoolean("inUse"),
                                    userID = reader.GetInt32("userID"),
                                    categories = new List<EvalTemplateQuestionCategory>(),
                                    templateQuestions = new List<EvalTemplateQuestion>()
                                };
                            }

                            if(!reader.IsDBNull(4))//column 4 = evalTemplateQuestionCategoryID
                            {
                                template.categories.Add(new EvalTemplateQuestionCategory()
                                {
                                    evalTemplateQuestionCategoryID = reader.GetInt32("evalTemplateQuestionCategoryID"),
                                    evalTemplateID = reader.GetInt32("evalTemplateID"),
                                    categoryName = reader.GetString("categoryName"),
                                    number = reader.GetInt32("categoryNumber")
                                });
                            }

                            if (!reader.IsDBNull(7))//column 8 = 
                            {
                                template.templateQuestions.Add(new EvalTemplateQuestion()
                                {
                                    evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
                                    evalTemplateID = reader.GetInt32("evalTemplateID"),
                                    evalTemplateQuestionCategoryID = reader.GetInt32("qevalTemplateQuestionCategoryID"),
                                    questionType = reader.GetChar("questionType"),
                                    questionText = reader.GetString("questionText"),
                                    number = reader.GetInt32("questionNumber")
                                });
                            }

                        }
                        if (template.evalTemplateID > 0) templates.Add(template); //Adds the last template because it wouldn't have been added previously
                    }
                }
            }
            return templates;
        }

        public static bool AssignEvals(List<int> projectIDs, int evalTemplateID)
        {
            int temp = 0;
            foreach (int projectID in projectIDs)
            {
                Group tempGroup = new Group();

                using (var conn = new MySqlConnection(connstring.ToString()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = conn.CreateCommand())
                    {
                        //SQL and Parameters
                        cmd.CommandText = "Select g.groupID From cs4450.groups g Inner Join uGroups ug On g.groupID = ug.groupID " +
                            "INNER Join users u On ug.userID = u.userID Where projectID = @projectID AND g.isActive = 1 AND ug.isActive = 1 " +
                            "GROUP BY g.groupID";

                        cmd.Parameters.AddWithValue("@projectID", projectID);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {

                            //Runs once per record retrieved
                            while (reader.Read())
                            {
                                tempGroup.groupID = reader.GetInt32("groupID");

                                tempGroup = GetGroup(tempGroup.groupID); //get all the users in group

                                if (AssignEvalsForGroup(tempGroup, evalTemplateID, GetLastEvalNumber(tempGroup.groupID)+1))
                                    temp++;

                            }
                        }
                    }
                }
            } //end foreach
            return (temp > 0);
        }

        public static bool AssignEvalsForGroup(Group group, int evalTemplateID, int number)
        {
            int temp = 0;
            foreach (User user in group.users)
            {
                int userID = user.userID;
                int groupID = group.groupID;

                using (var conn = new MySqlConnection(connstring.ToString()))
                {
                    conn.Open();
                    using (MySqlCommand cmd = conn.CreateCommand())
                    {
                        //SQL and Parameters
                        cmd.CommandText = "INSERT INTO evals (evalTemplateID, groupID, userID, number, isComplete) " +
                        "VALUES (@evalTemplateID, @groupID, @userID, @number, 0) ";

                        cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplateID);
                        cmd.Parameters.AddWithValue("@groupID", groupID);
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Parameters.AddWithValue("@number", number);

                        //Return the last inserted ID if successful
                        if (cmd.ExecuteNonQuery() > 0) temp++;
                    }
                }
            }
            return (temp > 0);

        }

        public static int GetLastEvalNumber(int groupID)
        {
            int number = 0;
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "Select MAX(number) AS number From evals e WHERE groupID = @groupID";

                    cmd.Parameters.AddWithValue("@groupID", groupID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {

                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            if(!reader.IsDBNull(0)) number = reader.GetInt32("number");
                        }
                    }
                }
            }
            return number;
        }

        public static bool SetInUse(int evalTemplateID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "Update evalTemplates Set inUse = 1 Where evalTemplateID = @evalTemplateID";

                    // cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplateID);
                    cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplateID);

                    //Return the last inserted ID if successful
                    if (cmd.ExecuteNonQuery() > 0) return true;
                    return false;
                }
            }




        }

        public static int GetLatestIncompleteEvaluationID(int groupID, int userID)
        {
            int evalID = 0;
            using (var conn = new MySqlConnection(connstring.ToString()))
            {
                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "Select evalID From evals WHERE groupID = @groupID AND userID = @userID AND isComplete = 0 ORDER BY number DESC";

                    cmd.Parameters.AddWithValue("@groupID", groupID);
                    cmd.Parameters.AddWithValue("@userID", userID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {

                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            if (evalID == 0)
                            {
                                evalID = reader.GetInt32("evalID");
                                break;
                            }
                        }
                    }
                }
            }
            return evalID;
        }

        public static Eval GetEvaluation(int evalID)
        {
            using (var conn = new MySqlConnection(connstring.ToString()))
            {

                Eval eval = new Eval();
                eval.templateQuestions = new List<EvalTemplateQuestion>();
                eval.categories = new List<EvalTemplateQuestionCategory>();
                eval.responses = new List<EvalResponse>();
                eval.users = new List<User>();

                conn.Open();
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "Select * From evals Where evalID = @evalID";

                    // cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplateID);
                    cmd.Parameters.AddWithValue("@evalID", evalID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            eval.number = reader.GetInt32("number");
                            eval.evalID = reader.GetInt32("evalID");
                            eval.evalTemplateID = reader.GetInt32("evalTemplateID");
                            eval.groupID = reader.GetInt32("groupID");
                            eval.isComplete = reader.GetBoolean("isComplete");
                        }
                    }


                    //SQL and Parameters
                    cmd.CommandText = "Select * From evalTemplateQuestions Where evalTemplateID = @evalTemplateID";
                    cmd.Parameters.AddWithValue("@evalTemplateID", eval.evalTemplateID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {                           
                            eval.templateQuestions.Add(new EvalTemplateQuestion()
                            {
                                questionText = reader.GetString("questionText"),
                                questionType = reader.GetChar("questionType"),
                                evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
                                evalTemplateQuestionCategoryID = reader.GetInt32("evalTemplateQuestionCategoryID"),
                                number = reader.GetInt32("number")
                            });
                        }                        
                    }

                    cmd.CommandText = "Select * From evalTemplateQuestionCategories Where evalTemplateID = @evalTemplateID";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            eval.categories.Add(new EvalTemplateQuestionCategory()
                            {
                                categoryName = reader.GetString("categoryName"),
                                evalTemplateQuestionCategoryID = reader.GetInt32("evalTemplateQuestionCategoryID")
                            });
                        }
                    }

                    cmd.CommandText = "Select * From evalResponses Where evalID = @evalID";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            eval.responses.Add(new EvalResponse()
                            {
                                evalResponseID = reader.GetInt32("evalResponseID"),
                                evalID = reader.GetInt32("evalID"),
                                evalTemplateQuestionID = reader.GetInt32("evalTemplateQuestionID"),
                                userID = reader.GetInt32("userID"),
                                response = reader.GetString("response")
                            });
                        }
                    }

                    cmd.CommandText = "Select * From uGroups uG LEFT JOIN users u on uG.userID = u.userID WHERE uG.groupID = @groupID AND uG.isActive = 1 AND u.isActive";
                    cmd.Parameters.AddWithValue("@groupID", eval.groupID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            eval.users.Add(new User()
                            {
                                firstName = reader.GetString("firstName"),
                                lastName = reader.GetString("lastName"),
                                userID = reader.GetInt32("userID")
                            });
                        }
                    }
                    return eval;
                }
            }
        }

        public static List<Eval> RandomizeEvaluations(int groupID, int userID)
        {
            
            Random randNum = new Random();
            List<Eval> userEvalResponses = EvalResponses(groupID, userID);
            List<int> evalIDs = new List<int>();
            int temp = -1;
            int[] arr = new int[100];
            arr[0] = temp;
            int count = 0;
            bool repeat = false;

            foreach(Eval eval in userEvalResponses)
            {
                List<EvalColumn> tempEvalColumns = eval.evals;
                foreach (EvalColumn evalColumn in tempEvalColumns)
                {
                    count++;
                    do
                    {
                        repeat = false;
                        temp = randNum.Next(1, 1000);
                        for (int i = 0; i < 99; i++)
                        {
                            if (arr[i] == temp)
                            {
                                repeat = true;
                                continue;
                            }
                        }
                    } while (repeat);

                    evalColumn.originalID = evalColumn.evalID;
                    evalColumn.evalID = temp;
                    arr[count] = temp;
                }

                //puts each evalID in list
                foreach (EvalColumn evalColumn in eval.evals)
                {
                    foreach (EvalColumn tempEvalColumn in tempEvalColumns)
                    {
                        if (evalColumn.evalID == tempEvalColumn.originalID)
                            evalColumn.evalID = tempEvalColumn.evalID;
                    }
                }

                foreach (EvalResponse evalResponse in eval.responses)
                {
                    foreach (EvalColumn tempEvalColumn in tempEvalColumns)
                    {
                        if (evalResponse.evalID == tempEvalColumn.originalID)
                            evalResponse.evalID = tempEvalColumn.evalID;
                    }
                }
            }

            return userEvalResponses;
        }
    }
}
