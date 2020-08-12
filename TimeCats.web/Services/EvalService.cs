//using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TimeCats.Models;


namespace TimeCats.Services
{
    public class EvalService
    {
        private readonly TimeTrackerContext _context;
        private readonly string ConnString;

        public EvalService(TimeTrackerContext context)
        {
            _context = context;
            ConnString = context.Database.GetDbConnection().ConnectionString;
        }

        public bool CompleteEval(int evalID)
        {
            //throw new NotImplementedException();

            var names = _context.Evals
                          .FirstOrDefault(u => u.evalID == evalID);

            names.isComplete = true;
            _context.Evals.Update(names);
            _context.SaveChanges();
            return true;
        }


        public long CreateTemplate(int userID)
        {
            //throw new NotImplementedException();

            var names = new EvalTemplate();

            names.userID = userID;
            names.templateName = "New Template"; //Possibly hardcoded value here?

            _context.EvalTemplates.Add(names);
            _context.SaveChanges();
            return 0;
        }


        public bool SaveTemplateName(EvalTemplate evalTemplate)
        {
            //throw new NotImplementedException();

            _context.EvalTemplates.Update(evalTemplate);
            _context.SaveChanges();
            return true;
        }

        public bool SaveEval(Eval eval)
        {
            //throw new NotImplementedException();

            _context.Evals.Update(eval);
            _context.SaveChanges();
            return true;
        }


        public bool DeleteQuestion(int evalTemplateQuestionID)
        {
            //throw new NotImplementedException();

            var name = _context.EvalTemplateQuestions.Find(evalTemplateQuestionID);
            _context.EvalTemplateQuestions.Remove(name);
            _context.SaveChanges();

            return false;
        }


        public bool SaveCategory(EvalTemplateQuestionCategory category)
        {
            //throw new NotImplementedException();

            _context.EvalTemplateQuestionCategories.Add(category);
            _context.SaveChanges();
            return false;
        }


        public bool DeleteCategory(int evalTemplateQuestionCategoryID)
        {
            //throw new NotImplementedException();

            var names = _context.EvalTemplateQuestionCategories
                .FirstOrDefault(u => u.evalTemplateQuestionCategoryID == evalTemplateQuestionCategoryID); //Was a = changed to ==

            _context.EvalTemplateQuestionCategories.Remove(names);
            _context.SaveChanges();
            return false;
        }


        public bool SaveQuestion(EvalTemplateQuestion question)
        {
            //throw new NotImplementedException();

            _context.EvalTemplateQuestions.Add(question);
            _context.SaveChanges();

            return true;
        }



        public List<EvalTemplate> GetTemplates(int instructorId)
        {
            //throw new NotImplementedException();

            return _context.EvalTemplates
                .Where(u => u.userID == instructorId).ToList();
        }

        //var templates = new List<EvalTemplate>();

        //using (var conn = new MySqlConnection(ConnString.ToString()))
        //{
        //    conn.Open();
        //    using (var cmd = conn.CreateCommand())
        //    {
        //        //SQL and Parameters
        //        cmd.CommandText = "Select * From evalTemplates e Where e.userID = @userID ";

        //        cmd.Parameters.AddWithValue("@userID", instructorId);

        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            //Runs once per record retrieved
        //            while (reader.Read())
        //                templates.Add(new EvalTemplate
        //                {
        //                    evalTemplateID = (int) reader["evalTemplateID"],
        //                    templateName = (string) reader["templateName"],
        //                    inUse = reader.GetBoolean("inUse"),
        //                    userID = (int) reader["userID"]
        //                });
        //        }
        //    }
        //}

        //return templates;


        public List<EvalTemplate> GetFullTemplatesForInstructor(int instructorId)
        {
            //throw new NotImplementedException();
            var templates = new List<EvalTemplate>();
            using (var conn = new NpgsqlConnection(ConnString.ToString()))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText =
                        "SELECT eT.*, eTQC.evalTemplateQuestionCategoryID, eTQC.categoryName, eTQC.number AS categoryNumber, " +
                        "eTQ.evalTemplateQuestionID, eTQ.evalTemplateQuestionCategoryID AS qevalTemplateQuestionCategoryID, " +
                        "eTQ.questionType, eTQ.questionText, eTQ.number AS questionNumber " +
                        "FROM EvalTemplates eT " +
                        "LEFT JOIN EvalTemplateQuestionCategories eTQC on eT.evalTemplateID = eTQC.evalTemplateID " +
                        "LEFT JOIN EvalTemplateQuestions eTQ on eT.evalTemplateID = eTQ.evalTemplateID " +
                        "WHERE eT.userID = @userID ";
                    cmd.Parameters.AddWithValue("@userID", instructorId);
                    
                    using (var reader = cmd.ExecuteReader()) //EXCEPTION HERE: I believe the problem is Npgsql is turining the CommandText lowercase and that's why it can't find the table
                    {
                        var template = new EvalTemplate();
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            if (template.evalTemplateID != (int) reader["evalTemplateID"])
                            {
                                if (template.evalTemplateID > 0)
                                    templates.Add(template); //Adds the previous template before making a new one

                                template = new EvalTemplate
                                {
                                    evalTemplateID = (int) reader["evalTemplateID"],
                                    templateName = (string) reader["templateName"],
                                    inUse = (bool) reader["inUse"],
                                    userID = (int) reader["userID"],
                                    categories = new List<EvalTemplateQuestionCategory>(),
                                    templateQuestions = new List<EvalTemplateQuestion>()
                                };
                            }

                            if (!reader.IsDBNull(4)) //column 4 = evalTemplateQuestionCategoryID
                                template.categories.Add(new EvalTemplateQuestionCategory
                                {
                                    evalTemplateQuestionCategoryID = (int) reader["evalTemplateQuestionCategoryID"],
                                    evalTemplateID = (int) reader["evalTemplateID"],
                                    categoryName = (string) reader["categoryName"],
                                    number = (int) reader["categoryNumber"]
                                });

                            if (!reader.IsDBNull(7)) //column 8 =
                                template.templateQuestions.Add(new EvalTemplateQuestion
                                {
                                    evalTemplateQuestionID = (int) reader["evalTemplateQuestionID"],
                                    evalTemplateID = (int) reader["evalTemplateID"],
                                    evalTemplateQuestionCategoryID = (int) reader["qevalTemplateQuestionCategoryID"],
                                    questionType = (char) reader["questionType"],
                                    questionText = (string) reader["questionText"],
                                    number = (int) reader["questionNumber"]
                                });
                        }

                        if (template.evalTemplateID > 0)
                            templates.Add(
                                template); //Adds the last template because it wouldn't have been added previously
                    }
                }
            }

            return templates;
        }

        public Group GetGroup(int groupID)
        {
            return _context.Groups
                .Include(g => g.Project)
                .Include(g => g.UserGroups)
                .ThenInclude(ug => ug.User)
                .ThenInclude(u => u.timecards)
                .FirstOrDefault(g => g.groupID == groupID);
        }

        public bool AssignEvals(List<int> projectIDs, int evalTemplateID)
        {
            //throw new NotImplementedException();

            var temp = 0;
            foreach (var projectID in projectIDs)
            {
                var tempGroup = new Group();

                using (var conn = new NpgsqlConnection(ConnString.ToString()))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        //SQL and Parameters
                        cmd.CommandText =
                            "Select g.groupID From Groups g Inner Join UserGroups ug On g.groupID = ug.groupID " +
                            "INNER Join users u On ug.userID = u.userID Where projectID = @projectID AND g.isActive = 1 AND ug.isActive = 1 " +
                            "GROUP BY g.groupID";

                        cmd.Parameters.AddWithValue("@projectID", projectID);

                        using (var reader = cmd.ExecuteReader())
                        {
                            //Runs once per record retrieved
                            while (reader.Read())
                            {
                                tempGroup.groupID = (int) reader["groupID"];

                                tempGroup = GetGroup(tempGroup.groupID); //get all the users in group

                                if (AssignEvalsForGroup(tempGroup, evalTemplateID,
                                    GetLastEvalNumber(tempGroup.groupID) + 1))
                                    temp++;
                            }
                        }
                    }
                }
            } //end foreach

            return temp > 0;
        }

        public bool AssignEvalsForGroup(Group group, int evalTemplateID, int number)
        {
            //throw new NotImplementedException();

            var temp = 0;
            foreach (var user in group.users)
            {
                var userID = user.userID;
                var groupID = group.groupID;

                using (var conn = new NpgsqlConnection(ConnString.ToString()))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
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

            return temp > 0;
        }

        public int GetLastEvalNumber(int groupID)
        {
            //throw new NotImplementedException();

            var number = 0;
            using (var conn = new NpgsqlConnection(ConnString.ToString()))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "Select MAX(number) AS number From Evals e WHERE groupID = @groupID";

                    cmd.Parameters.AddWithValue("@groupID", groupID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                            if (!reader.IsDBNull(0))
                                number = (int) reader["number"];
                    }
                }
            }

            return number;
        }

        public long CreateCategory(int evalTemplateID)
        {
            //throw new NotImplementedException();

            var names = new EvalTemplateQuestionCategory();

            names.evalTemplateID = evalTemplateID;
            names.categoryName = "New Category"; //Possibly another hardcoded value?

            _context.EvalTemplateQuestionCategories.Add(names);
            _context.SaveChanges();

            return 0;
        }




        public int GetInstructorForEval(int evalTemplateID)
        {
            //throw new NotImplementedException();

            return _context.EvalTemplates
                      .Where(u => u.evalTemplateID == evalTemplateID)
                      .Select(u => u.userID)
                      .FirstOrDefault();
        }


        public long CreateTemplateQuestion(int evalTemplateQuestionCategoryID, int evalTemplateID)
        {
            //throw new NotImplementedException();

            var names = new EvalTemplateQuestion();

            names.evalTemplateID = evalTemplateID;
            names.evalTemplateQuestionCategoryID = evalTemplateQuestionCategoryID;
            names.questionType = 'N';
            names.questionText = null; //Might want something other than null here
            names.number = 0;

            _context.EvalTemplateQuestions.Add(names);
            _context.SaveChanges();

            return 0;
        }

        public bool SetInUse(int evalTemplateID)
        {
            //throw new NotImplementedException();

            using (var conn = new NpgsqlConnection(ConnString.ToString()))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "Update EvalTemplates Set inUse = 1 Where evalTemplateID = @evalTemplateID";

                    // cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplateID);
                    cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplateID);

                    //Return the last inserted ID if successful
                    if (cmd.ExecuteNonQuery() > 0) return true;
                    return false;
                }
            }
        }

        public int GetLatestIncompleteEvaluationID(int groupID, int userID)
        {

            //throw new NotImplementedException();

            var evalID = 0;
            using (var conn = new NpgsqlConnection(ConnString.ToString()))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText =
                        "Select evalID From Evals WHERE groupID = @groupID AND userID = @userID AND isComplete = 0 ORDER BY number DESC";

                    cmd.Parameters.AddWithValue("@groupID", groupID);
                    cmd.Parameters.AddWithValue("@userID", userID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                            if (evalID == 0)
                            {
                                evalID = (int) reader["evalID"];
                                break;
                            }
                    }
                }
            }

            return evalID;
        }

        public Eval GetEvaluation(int evalID)
        {
            //throw new NotImplementedException();

            using (var conn = new NpgsqlConnection(ConnString.ToString()))
            {
                var eval = new Eval();
                eval.templateQuestions = new List<EvalTemplateQuestion>();
                eval.categories = new List<EvalTemplateQuestionCategory>();
                eval.responses = new List<EvalResponse>();
                eval.users = new List<User>();

                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "Select * From Evals Where evalID = @evalID";

                    // cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplateID);
                    cmd.Parameters.AddWithValue("@evalID", evalID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                        {
                            eval.number = (int) reader["number"];
                            eval.evalID = (int) reader["evalID"];
                            eval.evalTemplateID = (int) reader["evalTemplateID"];
                            eval.groupID = (int) reader["groupID"];
                            eval.isComplete = (bool) reader["isComplete"];
                        }
                    }


                    //SQL and Parameters
                    cmd.CommandText = "Select * From EvalTemplateQuestions Where evalTemplateID = @evalTemplateID";
                    cmd.Parameters.AddWithValue("@evalTemplateID", eval.evalTemplateID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            eval.templateQuestions.Add(new EvalTemplateQuestion
                            {
                                questionText = (string) reader["questionText"],
                                questionType = (char) reader["questionType"],
                                evalTemplateQuestionID = (int) reader["evalTemplateQuestionID"],
                                evalTemplateQuestionCategoryID = (int) reader["evalTemplateQuestionCategoryID"],
                                number = (int) reader["number"]
                            });
                    }

                    cmd.CommandText =
                        "Select * From EvalTemplateQuestionCategories Where evalTemplateID = @evalTemplateID";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            eval.categories.Add(new EvalTemplateQuestionCategory
                            {
                                categoryName = (string) reader["categoryName"],
                                evalTemplateQuestionCategoryID = (int) reader["evalTemplateQuestionCategoryID"]
                            });
                    }

                    cmd.CommandText = "Select * From EvalResponses Where evalID = @evalID";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            eval.responses.Add(new EvalResponse
                            {
                                evalResponseID = (int) reader["evalResponseID"],
                                evalID = (int) reader["evalID"],
                                evalTemplateQuestionID = (int) reader["evalTemplateQuestionID"],
                                userID = (int) reader["userID"],
                                response = (string) reader["response"]
                            });
                    }

                    cmd.CommandText =
                        "Select * From UserGroups uG LEFT JOIN Users u on uG.userID = u.userID WHERE uG.groupID = @groupID AND uG.isActive = 1 AND u.isActive";
                    cmd.Parameters.AddWithValue("@groupID", eval.groupID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            eval.users.Add(new User
                            {
                                firstName = (string) reader["firstName"],
                                lastName = (string) reader["lastName"],
                                userID = (int) reader["userID"]
                            });
                    }

                    return eval;
                }
            }
        }

        public List<Eval> RandomizeEvaluations(int groupID, int userID)
        {
            //throw new NotImplementedException();

            var randNum = new Random();
            var userEvalResponses = EvalResponses(groupID, userID);
            var evalIDs = new List<int>();
            var temp = -1;
            var arr = new int[100];
            arr[0] = temp;
            var count = 0;
            var repeat = false;

            foreach (var eval in userEvalResponses)
            {
                var tempEvalColumns = eval.evals;
                foreach (var evalColumn in tempEvalColumns)
                {
                    count++;
                    do
                    {
                        repeat = false;
                        temp = randNum.Next(1, 1000);
                        for (var i = 0; i < 99; i++)
                            if (arr[i] == temp)
                                repeat = true;
                    } while (repeat);

                    evalColumn.originalID = evalColumn.evalID;
                    evalColumn.evalID = temp;
                    arr[count] = temp;
                }

                //puts each evalID in list
                foreach (var evalColumn in eval.evals)
                    foreach (var tempEvalColumn in tempEvalColumns)
                        if (evalColumn.evalID == tempEvalColumn.originalID)
                            evalColumn.evalID = tempEvalColumn.evalID;

                foreach (var evalResponse in eval.responses)
                    foreach (var tempEvalColumn in tempEvalColumns)
                        if (evalResponse.evalID == tempEvalColumn.originalID)
                            evalResponse.evalID = tempEvalColumn.evalID;
            }

            return userEvalResponses;
        }

        public List<AdminEval> GetAllEvals()
        {
            //throw new NotImplementedException();

            var evals = new List<AdminEval>();
            using (var conn = new NpgsqlConnection(ConnString.ToString()))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText =
                        "SELECT e.*, CONCAT(u.firstName, ' ', u.lastName) AS usersName, g.groupName, p.projectID, p.projectName, " +
                        "c.courseID, c.courseName, et.templateName, c.instructorId, CONCAT(ui.firstName, ' ', ui.lastName) AS instructorName " +
                        "FROM Evals e " +
                        "LEFT JOIN Groups g on e.groupID = g.groupID " +
                        "LEFT JOIN Users u on e.userID = u.userID " +
                        "LEFT JOIN Projects p on g.projectID = p.projectID " +
                        "LEFT JOIN Courses c on p.courseID = c.courseID " +
                        "LEFT JOIN EvalTemplates et on e.evalTemplateID = et.evalTemplateID " +
                        "LEFT JOIN Users ui on c.instructorId = ui.userID";

                    using (var reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read())
                            evals.Add(new AdminEval
                            {
                                evalID = (int) reader["evalID"],
                                evalTemplateID = (int) reader["evalTemplateID"],
                                groupID = (int) reader["groupID"],
                                userID = (int) reader["userID"],
                                number = (int) reader["number"],
                                isComplete = (bool) reader["isComplete"],
                                usersName = (string) reader["usersName"],
                                groupName = (string) reader["groupName"],
                                projectID = (int) reader["projectID"],
                                projectName = (string) reader["projectName"],
                                courseID = (int) reader["courseID"],
                                courseName = (string) reader["courseName"],
                                templateName = (string) reader["templateName"],
                                instructorId = (int) reader["instructorId"],
                                instructorName = (string) reader["instructorName"]
                            });
                    }
                }
            }

            return evals;
        }

        public List<Eval> EvalResponsesA(int groupID, int userID)
        {
            //throw new NotImplementedException();

            var evals = new List<Eval>();

            using (var conn = new NpgsqlConnection(ConnString.ToString()))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        "SELECT er.*, u.firstName, e.number AS 'evalNumber', etq.number AS 'questionNumber', " +
                        "u.lastName, etq.questionText, etq.evalTemplateID, etq.questionType, etq.evalTemplateQuestionCategoryID, " +
                        "etqc.categoryName, etqc.number AS 'categoryNumber' " +
                        "FROM EvalResponses er " +
                        "  INNER JOIN Evals e ON er.evalID = e.evalID " +
                        "  INNER JOIN Users u ON u.userID = e.userID " +
                        "  INNER JOIN EvalTemplateQuestions etq ON etq.evalTemplateQuestionID = er.evalTemplateQuestionID " +
                        "  LEFT JOIN EvalTemplateQuestionCategories etqc ON etqc.evalTemplateQuestionCategoryID = etq.evalTemplateQuestionCategoryID " +
                        "WHERE groupID = @groupID AND er.userID = @userID";
                    cmd.Parameters.AddWithValue("@groupID", groupID);
                    cmd.Parameters.AddWithValue("@userID", userID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var foundEval = false;
                            foreach (var eval in evals)
                            {
                                if (eval.number != (int) reader["evalNumber"]) continue;
                                foundEval = true;

                                //Adding Eval entries
                                var foundEvalColumn = false;
                                foreach (var evalColumn in eval.evals)
                                    if (evalColumn.evalID == (int) reader["evalID"])
                                    {
                                        foundEvalColumn = true;
                                        break;
                                    }

                                if (!foundEvalColumn)
                                    eval.evals.Add(new EvalColumn
                                    {
                                        evalID = (int) reader["evalID"],
                                        firstName = (string) reader["firstName"], //Name is Team Member for anonymity
                                        lastName = (string) reader["lastName"]
                                    });

                                //Adding Template Questions
                                var foundTemplateQuestion = false;
                                foreach (var tq in eval.templateQuestions)
                                    if (tq.evalTemplateQuestionID == (int) reader["evalTemplateQuestionID"])
                                    {
                                        foundTemplateQuestion = true;
                                        break;
                                    }

                                if (!foundTemplateQuestion)
                                    eval.templateQuestions.Add(new EvalTemplateQuestion
                                    {
                                        questionText = (string) reader["questionText"],
                                        evalTemplateQuestionID = (int) reader["evalTemplateQuestionID"],
                                        questionType = (char) reader["questionType"],
                                        evalTemplateQuestionCategoryID = (int) reader["evalTemplateQuestionCategoryID"],
                                        number = (int) reader["questionNumber"]
                                    });

                                //Adding Categories if they're there
                                if (!reader.IsDBNull(13)) //column 13 = categoryName
                                {
                                    var foundCategory = false;
                                    foreach (var tqc in eval.categories)
                                        if (tqc.evalTemplateQuestionCategoryID == (int) reader["evalTemplateQuestionCategoryID"])
                                        {
                                            foundCategory = true;
                                            break;
                                        }

                                    if (!foundCategory)
                                        eval.categories.Add(new EvalTemplateQuestionCategory
                                        {
                                            evalTemplateQuestionCategoryID = (int) reader["evalTemplateQuestionCategoryID"],
                                            categoryName = (string) reader["categoryName"],
                                            number = (int) reader["categoryNumber"]
                                        });
                                }

                                //Every row is a unique response, so we don't need to worry about existing ones
                                eval.responses.Add(new EvalResponse
                                {
                                    evalTemplateQuestionID = (int) reader["evalTemplateQuestionID"],
                                    evalID = (int) reader["evalID"],
                                    response = (string) reader["response"],
                                    evalResponseID = (int) reader["evalResponseID"],
                                    userID = (int) reader["userID"]
                                });
                            }

                            if (!foundEval)
                            {
                                var eval = new Eval();
                                eval.evalTemplateID = (int) reader["evalTemplateID"];
                                eval.userID = (int) reader["userID"];
                                eval.groupID = groupID;
                                eval.number = (int) reader["evalNumber"];

                                //Adding evalColumn
                                eval.evals = new List<EvalColumn>();
                                eval.evals.Add(new EvalColumn
                                {
                                    evalID = (int) reader["evalID"],
                                    firstName = (string) reader["firstName"], //Name is Team Member for anonymity ??? no idea what this comment is about
                                    lastName = (string) reader["lastName"]
                                });

                                //Adding templateQuestion
                                eval.templateQuestions = new List<EvalTemplateQuestion>();
                                eval.templateQuestions.Add(new EvalTemplateQuestion
                                {
                                    questionText = (string) reader["questionText"],
                                    evalTemplateQuestionID = (int) reader["evalTemplateQuestionID"],
                                    questionType = (char) reader["questionType"],
                                    evalTemplateQuestionCategoryID = (int) reader["evalTemplateQuestionCategoryID"],
                                    number = (int) reader["questionNumber"]
                                });

                                //Adding Categories if they're there
                                eval.categories = new List<EvalTemplateQuestionCategory>();
                                if (!reader.IsDBNull(13)) //column 13 = categoryName
                                    eval.categories.Add(new EvalTemplateQuestionCategory
                                    {
                                        evalTemplateQuestionCategoryID = (int) reader["evalTemplateQuestionCategoryID"],
                                        categoryName = (string) reader["categoryName"],
                                        number = (int) reader["categoryNumber"]
                                    });

                                //Adding Response
                                eval.responses = new List<EvalResponse>();
                                eval.responses.Add(new EvalResponse
                                {
                                    evalTemplateQuestionID = (int) reader["evalTemplateQuestionID"],
                                    evalID = (int) reader["evalID"],
                                    response = (string) reader["response"],
                                    evalResponseID = (int) reader["evalResponseID"],
                                    userID = (int) reader["userID"]
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

        public List<Eval> EvalResponses(int groupID, int userID)
        {
            //throw new NotImplementedException();

            var evals = new List<Eval>();

            using (var conn = new NpgsqlConnection(ConnString.ToString()))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        "SELECT er.*, u.firstName, e.number AS 'evalNumber', etq.number AS 'questionNumber', " +
                        "u.lastName, etq.questionText, etq.evalTemplateID, etq.questionType, etq.evalTemplateQuestionCategoryID, " +
                        "etqc.categoryName, etqc.number AS 'categoryNumber' " +
                        "FROM EvalResponses er " +
                        "  INNER JOIN Evals e ON er.evalID = e.evalID " +
                        "  INNER JOIN Users u ON u.userID = e.userID " +
                        "  INNER JOIN EvalTemplateQuestions etq ON etq.evalTemplateQuestionID = er.evalTemplateQuestionID " +
                        "  LEFT JOIN EvalTemplateQuestionCategories etqc ON etqc.evalTemplateQuestionCategoryID = etq.evalTemplateQuestionCategoryID " +
                        "WHERE groupID = @groupID AND er.userID = @userID";
                    cmd.Parameters.AddWithValue("@groupID", groupID);
                    cmd.Parameters.AddWithValue("@userID", userID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var foundEval = false;
                            foreach (var eval in evals)
                            {
                                if (eval.number != (int) reader["evalNumber"]) continue;
                                foundEval = true;

                                //Adding Eval entries
                                var foundEvalColumn = false;
                                foreach (var evalColumn in eval.evals)
                                    if (evalColumn.evalID == (int) reader["evalID"])
                                    {
                                        foundEvalColumn = true;
                                        break;
                                    }

                                if (!foundEvalColumn)
                                    eval.evals.Add(new EvalColumn
                                    {
                                        evalID = (int) reader["evalID"],
                                        firstName = "Team", //Name is Team Member for anonymity
                                        lastName = "Member"
                                    });

                                //Adding Template Questions
                                var foundTemplateQuestion = false;
                                foreach (var tq in eval.templateQuestions)
                                    if (tq.evalTemplateQuestionID == (int) reader["evalTemplateQuestionID"])
                                    {
                                        foundTemplateQuestion = true;
                                        break;
                                    }

                                if (!foundTemplateQuestion)
                                    eval.templateQuestions.Add(new EvalTemplateQuestion
                                    {
                                        questionText = (string) reader["questionText"],
                                        evalTemplateQuestionID = (int) reader["evalTemplateQuestionID"],
                                        questionType = (char) reader["questionType"],
                                        evalTemplateQuestionCategoryID =
                                            (int) reader["evalTemplateQuestionCategoryID"],
                                        number = (int) reader["questionNumber"]
                                    });

                                //Adding Categories if they're there
                                if (!reader.IsDBNull(13)) //column 13 = categoryName
                                {
                                    var foundCategory = false;
                                    foreach (var tqc in eval.categories)
                                        if (tqc.evalTemplateQuestionCategoryID ==
                                            (int) reader["evalTemplateQuestionCategoryID"])
                                        {
                                            foundCategory = true;
                                            break;
                                        }

                                    if (!foundCategory)
                                        eval.categories.Add(new EvalTemplateQuestionCategory
                                        {
                                            evalTemplateQuestionCategoryID =
                                                (int) reader["evalTemplateQuestionCategoryID"],
                                            categoryName = (string) reader["categoryName"],
                                            number = (int) reader["categoryNumber"]
                                        });
                                }

                                //Every row is a unique response, so we don't need to worry about existing ones
                                eval.responses.Add(new EvalResponse
                                {
                                    evalTemplateQuestionID = (int) reader["evalTemplateQuestionID"],
                                    evalID = (int) reader["evalID"],
                                    response = (string) reader["response"],
                                    evalResponseID = (int) reader["evalResponseID"],
                                    userID = (int) reader["userID"]
                                });
                            }

                            if (!foundEval)
                            {
                                var eval = new Eval();
                                eval.evalTemplateID = (int) reader["evalTemplateID"];
                                eval.userID = (int) reader["userID"];
                                eval.groupID = groupID;
                                eval.number = (int) reader["evalNumber"];

                                //Adding evalColumn
                                eval.evals = new List<EvalColumn>();
                                eval.evals.Add(new EvalColumn
                                {
                                    evalID = (int) reader["evalID"],
                                    firstName = "Team", //Name is Team Member for anonymity
                                    lastName = "Member"
                                });

                                //Adding templateQuestion
                                eval.templateQuestions = new List<EvalTemplateQuestion>();
                                eval.templateQuestions.Add(new EvalTemplateQuestion
                                {
                                    questionText = (string) reader["questionText"],
                                    evalTemplateQuestionID = (int) reader["evalTemplateQuestionID"],
                                    questionType = (char) reader["questionType"],
                                    evalTemplateQuestionCategoryID = (int) reader["evalTemplateQuestionCategoryID"],
                                    number = (int) reader["questionNumber"]
                                });

                                //Adding Categories if they're there
                                eval.categories = new List<EvalTemplateQuestionCategory>();
                                if (!reader.IsDBNull(13)) //column 13 = categoryName
                                    eval.categories.Add(new EvalTemplateQuestionCategory
                                    {
                                        evalTemplateQuestionCategoryID =
                                            (int) reader["evalTemplateQuestionCategoryID"],
                                        categoryName = (string) reader["categoryName"],
                                        number = (int) reader["categoryNumber"]
                                    });

                                //Adding Response
                                eval.responses = new List<EvalResponse>();
                                eval.responses.Add(new EvalResponse
                                {
                                    evalTemplateQuestionID = (int) reader["evalTemplateQuestionID"],
                                    evalID = (int) reader["evalID"],
                                    response = (string) reader["response"],
                                    evalResponseID = (int) reader["evalResponseID"],
                                    userID = (int) reader["userID"]
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
        private void TeammateStats(ref List<Eval> eval)
        {
            //throw new NotImplementedException();

            using (var conn = new NpgsqlConnection(ConnString.ToString()))
            {
                foreach (var e in eval)
                    foreach (var r in e.responses)
                    {
                        conn.Open();
                        //  Get avg score student gave for the specific eval
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText =
                                "SELECT u.userID, u.firstName, u.lastName, AVG(er.response) AS avg " +
                                "FROM EvalResponses er INNER JOIN Evals e ON er.evalID = e.evalID " +
                                "INNER JOIN Users u ON u.userID = e.userID " +
                                "INNER JOIN EvalTemplateQuestions etq ON etq.evalTemplateQuestionID = er.evalTemplateQuestionID " +
                                "LEFT JOIN EvalTemplateQuestionCategories etqc ON etqc.evalTemplateQuestionCategoryID = etq.evalTemplateQuestionCategoryID " +
                                "WHERE groupID = @groupID " +
                                "AND e.evalID = @evalID " +
                                "GROUP BY u.userID;";

                            cmd.Parameters.AddWithValue("@evalID", r.evalID);
                            cmd.Parameters.AddWithValue("@groupID", e.groupID);

                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read()) r.userAvgerage = (double) reader["avg"];
                            }
                        }

                        conn.Close();
                    }
            }
        }

        public bool SaveResponse(int userID, int evalID, int evalTemplateQuestionID, string response)
        {
            //throw new NotImplementedException();

            using (var conn = new NpgsqlConnection(ConnString.ToString()))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    //SQL and Parameters
                    cmd.CommandText = "INSERT INTO EvalResponses (evalID, evalTemplateQuestionID, userID, response) " +
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

        public bool CreateTemplateCopy(int userID, int evalTemplateID)
        {
            //throw new NotImplementedException();

            var templateName = "";
            var catName = "";
            var temp = "";

            using (var conn = new NpgsqlConnection(ConnString.ToString()))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT templateName FROM EvalTemplates WHERE evalTemplateID = @evalTemplateID";
                    cmd.Parameters.AddWithValue("@evalTemplateID", evalTemplateID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        //Runs once per record retrieved
                        while (reader.Read()) templateName = (string) reader["templateName"];
                    }

                    //SQL and Parameters
                    cmd.CommandText = "INSERT INTO EvalTemplates (userID, templateName) " +
                                      "VALUES (@userID, '@TempName')";
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@TempName", templateName);

                    //Return the last inserted ID if successful
                    cmd.ExecuteNonQuery();
                    var higher = "0";
                    cmd.Parameters.AddWithValue("@questionType", higher);
                    cmd.Parameters.AddWithValue("@questionText", higher);
                    cmd.Parameters.AddWithValue("@number", higher);
                    cmd.CommandText =
                        "SELECT * FROM EvalTemplateQuestions AS 'ETQ' INNER JOIN EvalTemplateQuestionCategories AS 'ETC' "
                        + "ON ETC.evalTemplateQuestionCategoryID = ETQ.evalTemplateQuestionCategoryID WHERE ETQ.evalTemplateID = "
                        + "@evalTemplateID ORDER BY ETC.categoryName"
                        + "RETURNING id";
                    cmd.Parameters.AddWithValue("@evalTemplateID", (int) cmd.ExecuteScalar());
                    cmd.Parameters.AddWithValue("@evalTemplateQuestionCategoryID", higher);

                    using (var reader = cmd.ExecuteReader())
                    {
                        var catNum = 0;
                        while (reader.Read())
                        {
                            catName = (string) reader["ETC.categoryName"];
                            if (catName != temp)
                            {
                                cmd.CommandText =
                                    "INSERT INTO EvalTemplateQuestionCategory (evalTemplateID, categoryName) "
                                    + "VALUES (@evalTemplateID, @categoryName)"
                                    + "RETURNING id";
                                cmd.ExecuteNonQuery();
                                cmd.Parameters["@evalTemplateQuestionCategoryID"].Value = (int) cmd.ExecuteScalar();
                            }

                            temp = catName;
                            catNum = (int) reader["ETQ.evalTemplateQuestionCategoryID"];

                            cmd.CommandText =
                                "INSERT INTO EvalTemplateQuestions (evalTemplateID, evalTemplateQuestionCategoryID, "
                                + "questionType, questionText, number) VALUES (@evalTemplateID, @evalTemplateQuestionCategoryID, "
                                + "@questionType, @questionText, @number)";
                            cmd.Parameters["@questionType"].Value = (string) reader["ETQ.questionType"];
                            cmd.Parameters["@questionText"].Value = (string) reader["ETQ.questionText"];
                            cmd.Parameters["@number"].Value = (string) reader["ETQ.number"]; //maybe supposed to be int instead of string?

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            return true;
        }


        //Implemented elsewhere
        //public Project GetProject(int projectID)
        //{
        //    //TODO
        //    var names = (from p in _TimeTrackerContext.Projects
        //                join g in _TimeTrackerContext.Groups on p.projectID equals g.projectID
        //                join ug in _TimeTrackerContext.UserGroups on g.groupID equals ug.groupID
        //                join u in _TimeTrackerContext.Users on ug.userID equals u.userID
        //                join t in _TimeTrackerContext.TimeCards on
        //                    new
        //                    {
        //                        key1 = u.userID,
        //                        key2 = g.groupID
        //                    }
        //                    equals
        //                    new
        //                    {
        //                        key1 = t.userID,
        //                        key2 = t.groupID
        //                    }
        //                where (p.projectID == projectID)
        //                select new
        //                {
        //                    p.projectID,
        //                    p.projectName,
        //                    p.isActive,
        //                    p.description,
        //                    p.CourseID,
        //                    p.Course,
        //                    g.groupID,
        //                    g.groupName,
        //                    groupIsActive = g.isActive,
        //                    u.userID,
        //                    u.firstName,
        //                    u.lastName,
        //                    tgroupID = t.groupID,
        //                    t.timeslotID,
        //                    timeDescription = t.description,
        //                    t.isEdited,
        //                    tuserID = t.userID,
        //                    isActiveInGroup = ug.isActive
        //                })
        //                .ToList();




        //    var project = new Project
        //    {
        //        groups = new List<Group>()
        //    };

        //    using (var conn = new mysqlconnection(connstring.tostring()))
        //    {
        //        conn.open();
        //        using (var cmd = conn.createcommand())
        //        {
        //            //sql and parameters
        //            cmd.commandtext =
        //                "select p.*, g.groupid, g.groupname, g.isactive as groupisactive, u.userid, u.firstname, u.lastname, t.groupid as 'tgroupid', t.timeid, " +
        //                "date_format(t.timein, '%m/%d/%y %l:%i %p') as 'timein', date_format(t.timeout, '%m/%d/%y %l:%i %p') as 'timeout', " +
        //                "t.description as 'timedescription', t.isedited, t.userid as 'tuserid', ug.isactive as isactiveingroup " +
        //                "from projects p " +
        //                "left join cs4450.groups g on p.projectid = g.projectid " +
        //                "left join ugroups ug on ug.groupid = g.groupid " +
        //                "left join users u on u.userid = ug.userid " +
        //                "left join timecards t on (u.userid = t.userid and g.groupid = t.groupid) " +
        //                "where p.projectid = @projectid";
        //            cmd.parameters.addwithvalue("@projectid", projectid);

        //            using (var reader = cmd.executereader())
        //            {
        //                //runs once per record retrieved
        //                while (reader.read())
        //                {
        //                    if (project.projectID == 0)
        //                    {
        //                        project.projectID = reader.GetInt32("projectID");
        //                        project.projectName = reader.GetString("projectName");
        //                        project.isActive = reader.GetBoolean("isActive");
        //                        project.description = reader.GetString("description");
        //                        project.CourseID = reader.GetInt32("courseID");
        //                    }

        //                    var foundGroup = false;

        //                    foreach (var group in project.groups)
        //                        if (group.groupID == reader.GetInt32("groupID"))
        //                        {
        //                            foundGroup = true;

        //                            var foundUser = false;

        //                            if (group.users == null) group.users = new List<User>();

        //                            if (group.groupID == 0)
        //                            {
        //                                group.groupName = reader.GetString("groupName");
        //                                group.groupID = reader.GetInt32("groupID");
        //                                group.isActive = reader.GetBoolean("groupIsActive");
        //                            }

        //                            //get each users time info
        //                            foreach (var user in group.users)
        //                                if (user.userID == (int) reader["userID"])
        //                                {
        //                                    foundUser = true;
        //                                    //Add time slot

        //                                    if (user.timecards == null) user.timecards = new List<TimeCard>();

        //                                    if (!reader.IsDBNull(12))
        //                                        user.timecards.Add(new TimeCard
        //                                        {
        //                                            timeIn = reader.IsDBNull(13) ? "" : reader.GetString("timeIn"),
        //                                            timeOut = reader.IsDBNull(14) ? "" : reader.GetString("timeOut"),
        //                                            description = reader.GetString("description"),
        //                                            groupID = reader.GetInt32("tgroupID"),
        //                                            timeslotID = reader.GetInt32("timeID"),
        //                                            isEdited = reader.GetBoolean("isEdited"),
        //                                            userID = reader.GetInt32("tuserID")
        //                                        });
        //                                }

        //                            if (!foundUser)
        //                            {
        //                                var timecardlist = new List<TimeCard>();
        //                                if (!reader.IsDBNull(12))
        //                                    timecardlist.Add(new TimeCard
        //                                    {
        //                                        timeIn = reader.IsDBNull(13) ? "" : reader.GetString("timeIn"),
        //                                        timeOut = reader.IsDBNull(14) ? "" : reader.GetString("timeOut"),
        //                                        description = reader.GetString("description"),
        //                                        groupID = reader.GetInt32("tgroupID"),
        //                                        timeslotID = reader.GetInt32("timeID"),
        //                                        isEdited = reader.GetBoolean("isEdited"),
        //                                        userID = reader.GetInt32("tuserID")
        //                                    });

        //                                //Add the user and then the time slot
        //                                if (!reader.IsDBNull(8))
        //                                    group.users.Add(new User
        //                                    {
        //                                        userID = (int) reader["userID"],
        //                                        firstName = (string) reader["firstName"],
        //                                        lastName = (string) reader["lastName"],
        //                                        timecards = timecardlist,
        //                                        isActive = reader.GetBoolean("isActiveInGroup")
        //                                    });
        //                            }
        //                        }

        //                    if (!foundGroup)
        //                    {
        //                        var timecardlist = new List<TimeCard>();
        //                        if (!reader.IsDBNull(12))
        //                            timecardlist.Add(new TimeCard
        //                            {
        //                                timeIn = reader.IsDBNull(13) ? "" : reader.GetString("timeIn"),
        //                                timeOut = reader.IsDBNull(14) ? "" : reader.GetString("timeOut"),
        //                                description = reader.GetString("timeDescription"),
        //                                groupID = reader.GetInt32("tgroupID"),
        //                                timeslotID = reader.GetInt32("timeID"),
        //                                isEdited = reader.GetBoolean("isEdited")
        //                            });

        //                        var users = new List<User>();
        //                        if (!reader.IsDBNull(8))
        //                            users.Add(new User
        //                            {
        //                                userID = (int) reader["userID"],
        //                                firstName = (string) reader["firstName"],
        //                                lastName = (string) reader["lastName"],
        //                                timecards = timecardlist,
        //                                isActive = reader.GetBoolean("isActiveInGroup")
        //                            });

        //                        if (!reader.IsDBNull(5))
        //                            project.groups.Add(new Group
        //                            {
        //                                groupID = reader.GetInt32("groupID"),
        //                                groupName = reader.GetString("groupName"),
        //                                isActive = reader.GetBoolean("groupIsActive"),
        //                                users = users
        //                            });
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return project;
        //}
    }
}
