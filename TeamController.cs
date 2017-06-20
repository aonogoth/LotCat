using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace LotCatSecond.Resource.Controllers
{
    [Authorize]
    [RoutePrefix("api/team")]
    public class TeamController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("GetTest")]
        public JObject GetTest()
        {
            try
            {
                return GetJSONResult(true, "{\"aono\":\"morimiya\"}");
            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetTeamsByLeague")]
        public JObject GetTeamsByLeague(Guid leagueId)
        {
            try
            {
                //dynamic data = obj;
                //string name = data.name.ToString();
                var list = (from t in DbContext.Teams
                            join l in DbContext.Leagues on t.LeagueId equals l.LeagueId
                            where t.LeagueId == leagueId
                            orderby l.LeagueChineseName, t.TeamChineseName
                            select new
                            {
                                t.TeamId,
                                t.TeamChineseName,
                                t.TeamEnglishName,
                                t.LeagueId,
                                l.LeagueChineseName,
                            }).ToList();

                return GetJSONResult(true, JsonConvert.SerializeObject(list));
            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetTeams")]
        public JObject GetTeams()
        {
            try
            {
                //dynamic data = obj;
                //string name = data.name.ToString();
                var list = (from t in DbContext.Teams
                            join l in DbContext.Leagues on t.LeagueId equals l.LeagueId
                            where 1 == 1
                            orderby l.LeagueChineseName, t.TeamChineseName
                            select new
                            {
                                t.TeamId,
                                t.TeamChineseName,
                                t.TeamEnglishName,
                                t.LeagueId,
                                l.LeagueChineseName,
                            }).ToList();

                return GetJSONResult(true, JsonConvert.SerializeObject(list));
            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AddTeams")]
        public JObject AddTeams(JObject obj)
        {
            try
            {
                dynamic json = obj;
                JArray datas = (JArray)json.datas;
                for (int i = 0; i < datas.Count; i++)
                {
                    string Cname = (string)datas[i]["teamCName"];
                    string Ename = (string)datas[i]["teamEName"];
                    string leagueid = (string)datas[i]["leagueId"];
                    Model.Team team = new Model.Team();
                    team.TeamId = Guid.NewGuid();
                    team.TeamChineseName = Cname;
                    team.TeamEnglishName = Ename;
                    team.LeagueId = Guid.Parse(leagueid);
                    DbContext.Teams.Add(team);
                    DbContext.SaveChanges();
                }

                return GetJSONResult(true, "true");
            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("AddTeam")]
        public JObject AddTeam(JObject obj)
        {
            try
            {
                dynamic json = obj;
                string teamCName = (string)json.teamCName;
                string leagueId = (string)json.leagueId;

                //string Cname = (string)datas[i]["teamCName"];
                //string Ename = (string)datas[i]["teamEName"];
                //string leagueid = (string)datas[i]["leagueId"];
                if (DbContext.Teams.Where(c => c.TeamChineseName == teamCName.Trim()).Count() == 0)
                {
                    Model.Team team = new Model.Team();
                    team.TeamId = Guid.NewGuid();
                    team.TeamChineseName = teamCName;
                    team.TeamEnglishName = "";
                    team.LeagueId = Guid.Parse(leagueId);
                    DbContext.Teams.Add(team);
                    DbContext.SaveChanges();
                }

                return GetJSONResult(true, "true");
            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetLeagues")]
        public JObject GetLeagues()
        {
            try
            {
                //dynamic data = obj;
                //string name = data.name.ToString();
                var list = (from l in DbContext.Leagues
                            where 1 == 1
                            select new
                            {
                                l.LeagueChineseName,
                                l.LeagueId,
                            }).ToList();

                return GetJSONResult(true, JsonConvert.SerializeObject(list));
            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AddLeagues")]
        public JObject AddLeagues(JObject obj)
        {
            try
            {
                dynamic json = obj;
                JArray datas = (JArray)json.datas;
                for (int i = 0; i < datas.Count; i++)
                {
                    string Cname = (string)datas[i]["leagueCName"];
                    string Ename = (string)datas[i]["leagueEName"];
                    Model.League league = new Model.League();
                    league.LeagueId = Guid.NewGuid();
                    league.LeagueChineseName = Cname;
                    league.LeagueEnglishName = Ename;
                    DbContext.Leagues.Add(league);
                    DbContext.SaveChanges();
                }


                return GetJSONResult(true, "true");
            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("AddLeague")]
        public JObject AddLeague(JObject obj)
        {
            try
            {
                dynamic json = obj;
                string Cname = (string)json.leagueCName;

                if (DbContext.Leagues.Where(c => c.LeagueChineseName == Cname.Trim()).Count() == 0)
                {
                    Model.League league = new Model.League();
                    league.LeagueId = Guid.NewGuid();
                    league.LeagueChineseName = Cname;
                    league.LeagueEnglishName = "";
                    DbContext.Leagues.Add(league);
                    DbContext.SaveChanges();
                }

                return GetJSONResult(true, "true");
            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AddScore")]
        public JObject AddScore(JObject obj)
        {
            try
            {
                dynamic json = obj;

                string HomerId = (string)json.HomerId;
                string HomerRank = (string)json.HomerRank;
                string HomerScore = (string)json.HomerScore;
                string VisitorId = (string)json.VisitorId;
                string VisitorRank = (string)json.VisitorRank;
                string VisitorScore = (string)json.VisitorScore;
                string MyBid = (string)json.MyBid;
                string TrueBid = (string)json.TrueBid;
                string DrawEnd = (string)json.DrawEnd;
                string DrawStart = (string)json.DrawStart;
                string LostEnd = (string)json.LostEnd;
                string LostStart = (string)json.LostStart;
                string WinEnd = (string)json.WinEnd;
                string WinStart = (string)json.WinStart;
                Model.Score score = new Model.Score();
                score.ScoreId = Guid.NewGuid();
                score.HomerId = Guid.Parse(HomerId);
                if (!String.IsNullOrEmpty(HomerRank))
                {
                    score.HomerRank = int.Parse(HomerRank);
                }
                if (!String.IsNullOrEmpty(HomerScore))
                {
                    score.HomerScore = int.Parse(HomerScore);
                }
                score.VisitorId = Guid.Parse(VisitorId);
                if (!String.IsNullOrEmpty(VisitorRank))
                {
                    score.VisitorRank = int.Parse(VisitorRank);
                }
                if (!String.IsNullOrEmpty(VisitorScore))
                {
                    score.VisitorScore = int.Parse(VisitorScore);
                }
                if (!String.IsNullOrEmpty(MyBid))
                {
                    score.MyBid = int.Parse(MyBid);
                }
                if (!String.IsNullOrEmpty(TrueBid))
                {
                    score.TrueBid = int.Parse(TrueBid);
                }

                score.DrawEnd = decimal.Parse(DrawEnd);
                score.DrawStart = decimal.Parse(DrawStart);
                score.LostEnd = decimal.Parse(LostEnd);
                score.LostStart = decimal.Parse(LostStart);
                score.WinEnd = decimal.Parse(WinEnd);
                score.WinStart = decimal.Parse(WinStart);


                DbContext.Scores.Add(score);
                DbContext.SaveChanges();


                return GetJSONResult(true, "true");
            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("AddScores")]
        public JObject AddScores(JObject obj)
        {
            try
            {
                dynamic json = obj;
                JArray datas = (JArray)json.datas;
                for (int i = 0; i < datas.Count; i++)
                {
                    string result = (string)datas[i]["result"];
                    string Ename = (string)datas[i]["teamEName"];
                    string leagueid = (string)datas[i]["leagueId"];
                    Model.Score score = new Model.Score();
                    score.ScoreId = Guid.NewGuid();
                    //score.HomerId = ;
                    //score.HomerRank = ;
                    //score.HomerScore = ;
                    //score.VisitorId = ;
                    //score.VisitorRank = ;
                    //score.VisitorScore = ;
                    //score.MyBid = ; 
                    //score.TrueBid = ;

                    //score.DrawEnd = ;
                    //score.DrawStart = ; 
                    //score.LostEnd = ;
                    //score.LostStart = ;  
                    //score.WinEnd = ;
                    //score.WinStart = ;


                    DbContext.Scores.Add(score);
                    DbContext.SaveChanges();
                }

                return GetJSONResult(true, "true");
            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetScores")]
        public JObject GetScores()
        {
            try
            {
                //dynamic data = obj;
                //string name = data.name.ToString();
                var list = (from s in DbContext.Scores
                            join t1 in DbContext.Teams on s.HomerId equals t1.TeamId
                            join t2 in DbContext.Teams on s.VisitorId equals t2.TeamId
                            where 1 == 1
                            orderby s.WinStart descending
                            select new
                            {
                                s.ScoreId,
                                s.HomerRank,
                                HomeTeam = t1.TeamChineseName,
                                s.HomerScore,
                                s.VisitorScore,
                                VisitTeam = t2.TeamChineseName,
                                s.VisitorRank,
                                s.WinStart,
                                s.WinEnd,
                                s.DrawStart,
                                s.DrawEnd,
                                s.LostStart,
                                s.LostEnd,
                                s.MyBid,
                                s.TrueBid,
                            }).ToList();

                return GetJSONResult(true, JsonConvert.SerializeObject(list));
            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AnalyseScore")]
        public JObject AnalyseScore(JObject obj)
        {
            try
            {
                dynamic data = obj;
                decimal ws = decimal.Parse((string)data.ws);
                decimal we = decimal.Parse((string)data.we);
                decimal ds = decimal.Parse((string)data.ds);
                decimal de = decimal.Parse((string)data.de);
                decimal ls = decimal.Parse((string)data.ls);
                decimal le = decimal.Parse((string)data.le);
                decimal abs = decimal.Parse((string)data.abs);
                if (ws >= we && ls >= le)
                {
                    var list = (from s in DbContext.Scores
                                join t1 in DbContext.Teams on s.HomerId equals t1.TeamId
                                join t2 in DbContext.Teams on s.VisitorId equals t2.TeamId
                                where s.WinStart > ws - abs && s.WinStart < ws + abs && s.WinEnd > we - abs && s.WinEnd < we + abs
                                    && s.DrawStart > ds - abs && s.DrawStart < ds + abs && s.DrawEnd > de - abs && s.DrawEnd < de + abs
                                    && s.LostStart > ls - abs && s.LostStart < ls + abs && s.LostEnd > le - abs && s.LostEnd < le + abs
                                    && s.WinStart >= s.WinEnd && s.LostStart >= s.LostEnd
                                orderby s.WinStart descending
                                select new
                                {
                                    s.ScoreId,
                                    s.HomerRank,
                                    HomeTeam = t1.TeamChineseName,
                                    s.HomerScore,
                                    s.VisitorScore,
                                    VisitTeam = t2.TeamChineseName,
                                    s.VisitorRank,
                                    s.WinStart,
                                    s.WinEnd,
                                    s.DrawStart,
                                    s.DrawEnd,
                                    s.LostStart,
                                    s.LostEnd,
                                    s.MyBid,
                                    s.TrueBid,
                                }).ToList();

                    return GetJSONResult(true, JsonConvert.SerializeObject(list));
                }
                if (ws >= we && ls <= le)
                {
                    var list = (from s in DbContext.Scores
                                join t1 in DbContext.Teams on s.HomerId equals t1.TeamId
                                join t2 in DbContext.Teams on s.VisitorId equals t2.TeamId
                                where s.WinStart > ws - abs && s.WinStart < ws + abs && s.WinEnd > we - abs && s.WinEnd < we + abs
                                    && s.DrawStart > ds - abs && s.DrawStart < ds + abs && s.DrawEnd > de - abs && s.DrawEnd < de + abs
                                    && s.LostStart > ls - abs && s.LostStart < ls + abs && s.LostEnd > le - abs && s.LostEnd < le + abs
                                    && s.WinStart >= s.WinEnd && s.LostStart <= s.LostEnd
                                orderby s.WinStart descending
                                select new
                                {
                                    s.ScoreId,
                                    s.HomerRank,
                                    HomeTeam = t1.TeamChineseName,
                                    s.HomerScore,
                                    s.VisitorScore,
                                    VisitTeam = t2.TeamChineseName,
                                    s.VisitorRank,
                                    s.WinStart,
                                    s.WinEnd,
                                    s.DrawStart,
                                    s.DrawEnd,
                                    s.LostStart,
                                    s.LostEnd,
                                    s.MyBid,
                                    s.TrueBid,
                                }).ToList();

                    return GetJSONResult(true, JsonConvert.SerializeObject(list));
                }
                if (ws <= we && ls >= le)
                {
                    var list = (from s in DbContext.Scores
                                join t1 in DbContext.Teams on s.HomerId equals t1.TeamId
                                join t2 in DbContext.Teams on s.VisitorId equals t2.TeamId
                                where s.WinStart > ws - abs && s.WinStart < ws + abs && s.WinEnd > we - abs && s.WinEnd < we + abs
                                    && s.DrawStart > ds - abs && s.DrawStart < ds + abs && s.DrawEnd > de - abs && s.DrawEnd < de + abs
                                    && s.LostStart > ls - abs && s.LostStart < ls + abs && s.LostEnd > le - abs && s.LostEnd < le + abs
                                    && s.WinStart <= s.WinEnd && s.LostStart >= s.LostEnd
                                orderby s.WinStart descending
                                select new
                                {
                                    s.ScoreId,
                                    s.HomerRank,
                                    HomeTeam = t1.TeamChineseName,
                                    s.HomerScore,
                                    s.VisitorScore,
                                    VisitTeam = t2.TeamChineseName,
                                    s.VisitorRank,
                                    s.WinStart,
                                    s.WinEnd,
                                    s.DrawStart,
                                    s.DrawEnd,
                                    s.LostStart,
                                    s.LostEnd,
                                    s.MyBid,
                                    s.TrueBid,
                                }).ToList();

                    return GetJSONResult(true, JsonConvert.SerializeObject(list));
                }
                if (ws <= we && ls <= le)
                {
                    var list = (from s in DbContext.Scores
                                join t1 in DbContext.Teams on s.HomerId equals t1.TeamId
                                join t2 in DbContext.Teams on s.VisitorId equals t2.TeamId
                                where s.WinStart > ws - abs && s.WinStart < ws + abs && s.WinEnd > we - abs && s.WinEnd < we + abs
                                    && s.DrawStart > ds - abs && s.DrawStart < ds + abs && s.DrawEnd > de - abs && s.DrawEnd < de + abs
                                    && s.LostStart > ls - abs && s.LostStart < ls + abs && s.LostEnd > le - abs && s.LostEnd < le + abs
                                    && s.WinStart <= s.WinEnd && s.LostStart <= s.LostEnd
                                orderby s.WinStart descending
                                select new
                                {
                                    s.ScoreId,
                                    s.HomerRank,
                                    HomeTeam = t1.TeamChineseName,
                                    s.HomerScore,
                                    s.VisitorScore,
                                    VisitTeam = t2.TeamChineseName,
                                    s.VisitorRank,
                                    s.WinStart,
                                    s.WinEnd,
                                    s.DrawStart,
                                    s.DrawEnd,
                                    s.LostStart,
                                    s.LostEnd,
                                    s.MyBid,
                                    s.TrueBid,
                                }).ToList();

                    return GetJSONResult(true, JsonConvert.SerializeObject(list));
                }
                return GetJSONResult(true, "");

            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AnalyseScoreRank")]
        public JObject AnalyseScoreRank(JObject obj)
        {
            try
            {
                dynamic data = obj;
                decimal ws = decimal.Parse((string)data.ws);
                decimal we = decimal.Parse((string)data.we);
                decimal ds = decimal.Parse((string)data.ds);
                decimal de = decimal.Parse((string)data.de);
                decimal ls = decimal.Parse((string)data.ls);
                decimal le = decimal.Parse((string)data.le);

                int hr = int.Parse((string)data.hr);
                int vr = int.Parse((string)data.vr);

                decimal abs = decimal.Parse((string)data.abs);
                if (ws >= we && ls >= le)
                {
                    var list = (from s in DbContext.Scores
                                join t1 in DbContext.Teams on s.HomerId equals t1.TeamId
                                join t2 in DbContext.Teams on s.VisitorId equals t2.TeamId
                                where s.WinStart > ws - abs && s.WinStart < ws + abs && s.WinEnd > we - abs && s.WinEnd < we + abs
                                    && s.DrawStart > ds - abs && s.DrawStart < ds + abs && s.DrawEnd > de - abs && s.DrawEnd < de + abs
                                    && s.LostStart > ls - abs && s.LostStart < ls + abs && s.LostEnd > le - abs && s.LostEnd < le + abs
                                    && s.WinStart >= s.WinEnd && s.LostStart >= s.LostEnd
                                    && s.HomerRank.Value > hr - 3 && s.HomerRank.Value < hr + 3 && s.VisitorRank.Value > vr - 3 && s.VisitorRank.Value < vr + 3
                                orderby s.WinStart descending
                                select new
                                {
                                    s.ScoreId,
                                    s.HomerRank,
                                    HomeTeam = t1.TeamChineseName,
                                    s.HomerScore,
                                    s.VisitorScore,
                                    VisitTeam = t2.TeamChineseName,
                                    s.VisitorRank,
                                    s.WinStart,
                                    s.WinEnd,
                                    s.DrawStart,
                                    s.DrawEnd,
                                    s.LostStart,
                                    s.LostEnd,
                                    s.MyBid,
                                    s.TrueBid,
                                }).ToList();

                    return GetJSONResult(true, JsonConvert.SerializeObject(list));
                }
                if (ws >= we && ls <= le)
                {
                    var list = (from s in DbContext.Scores
                                join t1 in DbContext.Teams on s.HomerId equals t1.TeamId
                                join t2 in DbContext.Teams on s.VisitorId equals t2.TeamId
                                where s.WinStart > ws - abs && s.WinStart < ws + abs && s.WinEnd > we - abs && s.WinEnd < we + abs
                                    && s.DrawStart > ds - abs && s.DrawStart < ds + abs && s.DrawEnd > de - abs && s.DrawEnd < de + abs
                                    && s.LostStart > ls - abs && s.LostStart < ls + abs && s.LostEnd > le - abs && s.LostEnd < le + abs
                                    && s.WinStart >= s.WinEnd && s.LostStart <= s.LostEnd
                                    && s.HomerRank.Value > hr - 3 && s.HomerRank.Value < hr + 3 && s.VisitorRank.Value > vr - 3 && s.VisitorRank.Value < vr + 3
                                orderby s.WinStart descending
                                select new
                                {
                                    s.ScoreId,
                                    s.HomerRank,
                                    HomeTeam = t1.TeamChineseName,
                                    s.HomerScore,
                                    s.VisitorScore,
                                    VisitTeam = t2.TeamChineseName,
                                    s.VisitorRank,
                                    s.WinStart,
                                    s.WinEnd,
                                    s.DrawStart,
                                    s.DrawEnd,
                                    s.LostStart,
                                    s.LostEnd,
                                    s.MyBid,
                                    s.TrueBid,
                                }).ToList();

                    return GetJSONResult(true, JsonConvert.SerializeObject(list));
                }
                if (ws <= we && ls >= le)
                {
                    var list = (from s in DbContext.Scores
                                join t1 in DbContext.Teams on s.HomerId equals t1.TeamId
                                join t2 in DbContext.Teams on s.VisitorId equals t2.TeamId
                                where s.WinStart > ws - abs && s.WinStart < ws + abs && s.WinEnd > we - abs && s.WinEnd < we + abs
                                    && s.DrawStart > ds - abs && s.DrawStart < ds + abs && s.DrawEnd > de - abs && s.DrawEnd < de + abs
                                    && s.LostStart > ls - abs && s.LostStart < ls + abs && s.LostEnd > le - abs && s.LostEnd < le + abs
                                    && s.WinStart <= s.WinEnd && s.LostStart >= s.LostEnd
                                    && s.HomerRank.Value > hr - 3 && s.HomerRank.Value < hr + 3 && s.VisitorRank.Value > vr - 3 && s.VisitorRank.Value < vr + 3
                                orderby s.WinStart descending
                                select new
                                {
                                    s.ScoreId,
                                    s.HomerRank,
                                    HomeTeam = t1.TeamChineseName,
                                    s.HomerScore,
                                    s.VisitorScore,
                                    VisitTeam = t2.TeamChineseName,
                                    s.VisitorRank,
                                    s.WinStart,
                                    s.WinEnd,
                                    s.DrawStart,
                                    s.DrawEnd,
                                    s.LostStart,
                                    s.LostEnd,
                                    s.MyBid,
                                    s.TrueBid,
                                }).ToList();

                    return GetJSONResult(true, JsonConvert.SerializeObject(list));
                }
                if (ws <= we && ls <= le)
                {
                    var list = (from s in DbContext.Scores
                                join t1 in DbContext.Teams on s.HomerId equals t1.TeamId
                                join t2 in DbContext.Teams on s.VisitorId equals t2.TeamId
                                where s.WinStart > ws - abs && s.WinStart < ws + abs && s.WinEnd > we - abs && s.WinEnd < we + abs
                                    && s.DrawStart > ds - abs && s.DrawStart < ds + abs && s.DrawEnd > de - abs && s.DrawEnd < de + abs
                                    && s.LostStart > ls - abs && s.LostStart < ls + abs && s.LostEnd > le - abs && s.LostEnd < le + abs
                                    && s.WinStart <= s.WinEnd && s.LostStart <= s.LostEnd
                                    && s.HomerRank.Value > hr - 3 && s.HomerRank.Value < hr + 3 && s.VisitorRank.Value > vr - 3 && s.VisitorRank.Value > vr + 3
                                orderby s.WinStart descending
                                select new
                                {
                                    s.ScoreId,
                                    s.HomerRank,
                                    HomeTeam = t1.TeamChineseName,
                                    s.HomerScore,
                                    s.VisitorScore,
                                    VisitTeam = t2.TeamChineseName,
                                    s.VisitorRank,
                                    s.WinStart,
                                    s.WinEnd,
                                    s.DrawStart,
                                    s.DrawEnd,
                                    s.LostStart,
                                    s.LostEnd,
                                    s.MyBid,
                                    s.TrueBid,
                                }).ToList();

                    return GetJSONResult(true, JsonConvert.SerializeObject(list));
                }
                return GetJSONResult(true, "");

            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("EditScore")]
        public JObject EditScore(JObject obj)
        {
            try
            {
                dynamic data = obj;
                string scoreid = (string)data.ScoreId;
                string ws = (string)data.WinStart;
                string we = (string)data.WinEnd;
                string ds = (string)data.DrawStart;
                string de = (string)data.DrawEnd;
                string ls = (string)data.LostStart;
                string le = (string)data.LostEnd;

                string hr = (string)data.HomerRank;
                string hs = (string)data.HomerScore;
                string vr = (string)data.VisitorRank;
                string vs = (string)data.VisitorScore;

                string mybid = (string)data.MyBid;
                string truebid = (string)data.TrueBid;

                LotCatSecond.Model.Score score = new LotCatSecond.Model.Score();
                score = DbContext.Scores.Find(Guid.Parse(scoreid));
                if (!String.IsNullOrEmpty(ws))
                {
                    score.WinStart = decimal.Parse(ws);
                }
                if (!String.IsNullOrEmpty(we))
                {
                    score.WinEnd = decimal.Parse(we);
                }
                if (!String.IsNullOrEmpty(ds))
                {
                    score.DrawStart = decimal.Parse(ds);
                }
                if (!String.IsNullOrEmpty(de))
                {
                    score.DrawEnd = decimal.Parse(de);
                }
                if (!String.IsNullOrEmpty(ls))
                {
                    score.LostStart = decimal.Parse(ls);
                }
                if (!String.IsNullOrEmpty(le))
                {
                    score.LostEnd = decimal.Parse(le);
                }

                if (!String.IsNullOrEmpty(hr))
                {
                    score.HomerRank = int.Parse(hr);
                }
                if (!String.IsNullOrEmpty(hs))
                {
                    score.HomerScore = int.Parse(hs);
                }
                if (!String.IsNullOrEmpty(vr))
                {
                    score.VisitorRank = int.Parse(vr);
                }
                if (!String.IsNullOrEmpty(vs))
                {
                    score.VisitorScore = int.Parse(vs);
                }

                if (!String.IsNullOrEmpty(mybid))
                {
                    score.MyBid = int.Parse(mybid);
                }
                if (!String.IsNullOrEmpty(truebid))
                {
                    score.TrueBid = int.Parse(truebid);
                }

                DbContext.SaveChanges();

                return GetJSONResult(true, "true");
            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("ReturnTeamId")]
        public JObject ReturnTeamId(JObject obj)
        {
            try
            {
                dynamic json = obj;

                string TeamName = (string)json.TeamName;
                var team = DbContext.Teams.Where(t => t.TeamChineseName == TeamName).FirstOrDefault();
                if (team != null)
                {
                    return GetJSONResult(true, "\"" + team.TeamId + "\"");
                }
                else
                {
                    return GetJSONResult(true, "");
                }

            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ReturnLeagueId")]
        public JObject ReturnLeagueId(JObject obj)
        {
            try
            {
                dynamic json = obj;

                string LeagueName = (string)json.LeagueName;
                var league = DbContext.Leagues.Where(t => t.LeagueChineseName == LeagueName).FirstOrDefault();
                if (league != null)
                {
                    return GetJSONResult(true, "\"" + league.LeagueId + "\"");
                }
                else
                {
                    return GetJSONResult(true, "");
                }

            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }








        [AllowAnonymous]
        [HttpPost]
        [Route("AddScoreData")]
        public JObject AddScoreData(JObject obj)
        {
            try
            {
                dynamic json = obj;
                string Period = (string)json.Period;
                string ScoreDate = (string)json.ScoreDate;
                string PageNumber = (string)json.PageNumber;
                string LeagueId = (string)json.LeagueId;

                string HomerId = (string)json.HomerId;
                string HomerRank = (string)json.HomerRank;
                string HomerScore = (string)json.HomerScore;
                string VisitorId = (string)json.VisitorId;
                string VisitorRank = (string)json.VisitorRank;
                string VisitorScore = (string)json.VisitorScore;
                string MyBid = (string)json.MyBid;
                string TrueBid = (string)json.TrueBid;
                string DrawEnd = (string)json.DrawEnd;
                string DrawStart = (string)json.DrawStart;
                string LostEnd = (string)json.LostEnd;
                string LostStart = (string)json.LostStart;
                string WinEnd = (string)json.WinEnd;
                string WinStart = (string)json.WinStart;
                Model.ScoreData score = new Model.ScoreData();
                score.ScoreDataId = Guid.NewGuid();

                score.Period = Period;
                score.ScoreDate = DateTime.Parse(ScoreDate);
                score.PageNumber = int.Parse(PageNumber);
                score.LeagueId = Guid.Parse(LeagueId);

                score.HomerId = Guid.Parse(HomerId);
                if (!String.IsNullOrEmpty(HomerRank))
                {
                    score.HomerRank = int.Parse(HomerRank);
                }
                if (!String.IsNullOrEmpty(HomerScore))
                {
                    score.HomerScore = int.Parse(HomerScore);
                }
                score.VisitorId = Guid.Parse(VisitorId);
                if (!String.IsNullOrEmpty(VisitorRank))
                {
                    score.VisitorRank = int.Parse(VisitorRank);
                }
                if (!String.IsNullOrEmpty(VisitorScore))
                {
                    score.VisitorScore = int.Parse(VisitorScore);
                }
                if (!String.IsNullOrEmpty(MyBid))
                {
                    score.MyBid = int.Parse(MyBid);
                }
                if (!String.IsNullOrEmpty(TrueBid))
                {
                    score.TrueBid = int.Parse(TrueBid);
                }

                score.DrawEnd = decimal.Parse(DrawEnd);
                score.DrawStart = decimal.Parse(DrawStart);
                score.LostEnd = decimal.Parse(LostEnd);
                score.LostStart = decimal.Parse(LostStart);
                score.WinEnd = decimal.Parse(WinEnd);
                score.WinStart = decimal.Parse(WinStart);


                DbContext.ScoreDatas.Add(score);
                DbContext.SaveChanges();


                return GetJSONResult(true, "true");
            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("AnalyseScoreData")]
        public JObject AnalyseScoreData(JObject obj)
        {
            try
            {
                dynamic data = obj;
                decimal ws = decimal.Parse((string)data.ws);
                decimal we = decimal.Parse((string)data.we);
                decimal ds = decimal.Parse((string)data.ds);
                decimal de = decimal.Parse((string)data.de);
                decimal ls = decimal.Parse((string)data.ls);
                decimal le = decimal.Parse((string)data.le);
                decimal abs = decimal.Parse((string)data.abs);
                string leagueId = (string)data.leagueid;
                Guid leagueGuid = Guid.Parse(leagueId);
                if (ws >= we && ls >= le)
                {
                    var list = (from s in DbContext.ScoreDatas
                                join t1 in DbContext.Teams on s.HomerId equals t1.TeamId
                                join t2 in DbContext.Teams on s.VisitorId equals t2.TeamId
                                where s.WinStart > ws - abs && s.WinStart < ws + abs && s.WinEnd > we - abs && s.WinEnd < we + abs
                                    && s.DrawStart > ds - abs && s.DrawStart < ds + abs && s.DrawEnd > de - abs && s.DrawEnd < de + abs
                                    && s.LostStart > ls - abs && s.LostStart < ls + abs && s.LostEnd > le - abs && s.LostEnd < le + abs
                                    && s.WinStart >= s.WinEnd && s.LostStart >= s.LostEnd
                                && s.LeagueId == leagueGuid
                                orderby s.ScoreDate ascending
                                select new
                                {
                                    s.ScoreDataId,
                                    s.ScoreDate,
                                    s.Period,
                                    s.PageNumber,
                                    s.HomerRank,
                                    HomeTeam = t1.TeamChineseName,
                                    s.HomerScore,
                                    s.VisitorScore,
                                    VisitTeam = t2.TeamChineseName,
                                    s.VisitorRank,
                                    s.WinStart,
                                    s.WinEnd,
                                    s.DrawStart,
                                    s.DrawEnd,
                                    s.LostStart,
                                    s.LostEnd,
                                    s.MyBid,
                                    s.TrueBid,
                                }).ToList();

                    return GetJSONResult(true, JsonConvert.SerializeObject(list));
                }
                if (ws >= we && ls <= le)
                {
                    var list = (from s in DbContext.ScoreDatas
                                join t1 in DbContext.Teams on s.HomerId equals t1.TeamId
                                join t2 in DbContext.Teams on s.VisitorId equals t2.TeamId
                                where s.WinStart > ws - abs && s.WinStart < ws + abs && s.WinEnd > we - abs && s.WinEnd < we + abs
                                    && s.DrawStart > ds - abs && s.DrawStart < ds + abs && s.DrawEnd > de - abs && s.DrawEnd < de + abs
                                    && s.LostStart > ls - abs && s.LostStart < ls + abs && s.LostEnd > le - abs && s.LostEnd < le + abs
                                    && s.WinStart >= s.WinEnd && s.LostStart <= s.LostEnd
                                && s.LeagueId == leagueGuid
                                orderby s.ScoreDate ascending
                                select new
                                {
                                    s.ScoreDataId,
                                    s.ScoreDate,
                                    s.Period,
                                    s.PageNumber,
                                    s.HomerRank,
                                    HomeTeam = t1.TeamChineseName,
                                    s.HomerScore,
                                    s.VisitorScore,
                                    VisitTeam = t2.TeamChineseName,
                                    s.VisitorRank,
                                    s.WinStart,
                                    s.WinEnd,
                                    s.DrawStart,
                                    s.DrawEnd,
                                    s.LostStart,
                                    s.LostEnd,
                                    s.MyBid,
                                    s.TrueBid,
                                }).ToList();

                    return GetJSONResult(true, JsonConvert.SerializeObject(list));
                }
                if (ws <= we && ls >= le)
                {
                    var list = (from s in DbContext.ScoreDatas
                                join t1 in DbContext.Teams on s.HomerId equals t1.TeamId
                                join t2 in DbContext.Teams on s.VisitorId equals t2.TeamId
                                where s.WinStart > ws - abs && s.WinStart < ws + abs && s.WinEnd > we - abs && s.WinEnd < we + abs
                                    && s.DrawStart > ds - abs && s.DrawStart < ds + abs && s.DrawEnd > de - abs && s.DrawEnd < de + abs
                                    && s.LostStart > ls - abs && s.LostStart < ls + abs && s.LostEnd > le - abs && s.LostEnd < le + abs
                                    && s.WinStart <= s.WinEnd && s.LostStart >= s.LostEnd
                                && s.LeagueId == leagueGuid
                                orderby s.ScoreDate ascending
                                select new
                                {
                                    s.ScoreDataId,
                                    s.ScoreDate,
                                    s.Period,
                                    s.PageNumber,
                                    s.HomerRank,
                                    HomeTeam = t1.TeamChineseName,
                                    s.HomerScore,
                                    s.VisitorScore,
                                    VisitTeam = t2.TeamChineseName,
                                    s.VisitorRank,
                                    s.WinStart,
                                    s.WinEnd,
                                    s.DrawStart,
                                    s.DrawEnd,
                                    s.LostStart,
                                    s.LostEnd,
                                    s.MyBid,
                                    s.TrueBid,
                                }).ToList();

                    return GetJSONResult(true, JsonConvert.SerializeObject(list));
                }
                if (ws <= we && ls <= le)
                {
                    var list = (from s in DbContext.ScoreDatas
                                join t1 in DbContext.Teams on s.HomerId equals t1.TeamId
                                join t2 in DbContext.Teams on s.VisitorId equals t2.TeamId
                                where s.WinStart > ws - abs && s.WinStart < ws + abs && s.WinEnd > we - abs && s.WinEnd < we + abs
                                    && s.DrawStart > ds - abs && s.DrawStart < ds + abs && s.DrawEnd > de - abs && s.DrawEnd < de + abs
                                    && s.LostStart > ls - abs && s.LostStart < ls + abs && s.LostEnd > le - abs && s.LostEnd < le + abs
                                    && s.WinStart <= s.WinEnd && s.LostStart <= s.LostEnd
                                && s.LeagueId == leagueGuid
                                orderby s.ScoreDate ascending
                                select new
                                {
                                    s.ScoreDataId,
                                    s.ScoreDate,
                                    s.Period,
                                    s.PageNumber,
                                    s.HomerRank,
                                    HomeTeam = t1.TeamChineseName,
                                    s.HomerScore,
                                    s.VisitorScore,
                                    VisitTeam = t2.TeamChineseName,
                                    s.VisitorRank,
                                    s.WinStart,
                                    s.WinEnd,
                                    s.DrawStart,
                                    s.DrawEnd,
                                    s.LostStart,
                                    s.LostEnd,
                                    s.MyBid,
                                    s.TrueBid,
                                }).ToList();

                    return GetJSONResult(true, JsonConvert.SerializeObject(list));
                }
                return GetJSONResult(true, "");

            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("AnalyseScoreDataRank")]
        public JObject AnalyseScoreDataRank(JObject obj)
        {
            try
            {
                dynamic data = obj;
                decimal ws = decimal.Parse((string)data.ws);
                decimal we = decimal.Parse((string)data.we);
                decimal ds = decimal.Parse((string)data.ds);
                decimal de = decimal.Parse((string)data.de);
                decimal ls = decimal.Parse((string)data.ls);
                decimal le = decimal.Parse((string)data.le);

                int hr = int.Parse((string)data.hr);
                int vr = int.Parse((string)data.vr);

                string leagueId = (string)data.leagueid;
                Guid leagueGuid = Guid.Parse(leagueId);

                decimal abs = decimal.Parse((string)data.abs);
                if (ws >= we && ls >= le)
                {
                    var list = (from s in DbContext.ScoreDatas
                                join t1 in DbContext.Teams on s.HomerId equals t1.TeamId
                                join t2 in DbContext.Teams on s.VisitorId equals t2.TeamId
                                where s.WinStart > ws - abs && s.WinStart < ws + abs && s.WinEnd > we - abs && s.WinEnd < we + abs
                                    && s.DrawStart > ds - abs && s.DrawStart < ds + abs && s.DrawEnd > de - abs && s.DrawEnd < de + abs
                                    && s.LostStart > ls - abs && s.LostStart < ls + abs && s.LostEnd > le - abs && s.LostEnd < le + abs
                                    && s.WinStart >= s.WinEnd && s.LostStart >= s.LostEnd
                                    && s.HomerRank.Value > hr - 3 && s.HomerRank.Value < hr + 3 && s.VisitorRank.Value > vr - 3 && s.VisitorRank.Value < vr + 3
                                    && s.LeagueId == leagueGuid
                                orderby s.ScoreDate ascending
                                select new
                                {
                                    s.ScoreDataId,
                                    s.ScoreDate,
                                    s.Period,
                                    s.PageNumber,
                                    s.HomerRank,
                                    HomeTeam = t1.TeamChineseName,
                                    s.HomerScore,
                                    s.VisitorScore,
                                    VisitTeam = t2.TeamChineseName,
                                    s.VisitorRank,
                                    s.WinStart,
                                    s.WinEnd,
                                    s.DrawStart,
                                    s.DrawEnd,
                                    s.LostStart,
                                    s.LostEnd,
                                    s.MyBid,
                                    s.TrueBid,
                                }).ToList();

                    return GetJSONResult(true, JsonConvert.SerializeObject(list));
                }
                if (ws >= we && ls <= le)
                {
                    var list = (from s in DbContext.ScoreDatas
                                join t1 in DbContext.Teams on s.HomerId equals t1.TeamId
                                join t2 in DbContext.Teams on s.VisitorId equals t2.TeamId
                                where s.WinStart > ws - abs && s.WinStart < ws + abs && s.WinEnd > we - abs && s.WinEnd < we + abs
                                    && s.DrawStart > ds - abs && s.DrawStart < ds + abs && s.DrawEnd > de - abs && s.DrawEnd < de + abs
                                    && s.LostStart > ls - abs && s.LostStart < ls + abs && s.LostEnd > le - abs && s.LostEnd < le + abs
                                    && s.WinStart >= s.WinEnd && s.LostStart <= s.LostEnd
                                    && s.HomerRank.Value > hr - 3 && s.HomerRank.Value < hr + 3 && s.VisitorRank.Value > vr - 3 && s.VisitorRank.Value < vr + 3
                                    && s.LeagueId == leagueGuid
                                orderby s.ScoreDate ascending
                                select new
                                {
                                    s.ScoreDataId,
                                    s.ScoreDate,
                                    s.Period,
                                    s.PageNumber,
                                    s.HomerRank,
                                    HomeTeam = t1.TeamChineseName,
                                    s.HomerScore,
                                    s.VisitorScore,
                                    VisitTeam = t2.TeamChineseName,
                                    s.VisitorRank,
                                    s.WinStart,
                                    s.WinEnd,
                                    s.DrawStart,
                                    s.DrawEnd,
                                    s.LostStart,
                                    s.LostEnd,
                                    s.MyBid,
                                    s.TrueBid,
                                }).ToList();

                    return GetJSONResult(true, JsonConvert.SerializeObject(list));
                }
                if (ws <= we && ls >= le)
                {
                    var list = (from s in DbContext.ScoreDatas
                                join t1 in DbContext.Teams on s.HomerId equals t1.TeamId
                                join t2 in DbContext.Teams on s.VisitorId equals t2.TeamId
                                where s.WinStart > ws - abs && s.WinStart < ws + abs && s.WinEnd > we - abs && s.WinEnd < we + abs
                                    && s.DrawStart > ds - abs && s.DrawStart < ds + abs && s.DrawEnd > de - abs && s.DrawEnd < de + abs
                                    && s.LostStart > ls - abs && s.LostStart < ls + abs && s.LostEnd > le - abs && s.LostEnd < le + abs
                                    && s.WinStart <= s.WinEnd && s.LostStart >= s.LostEnd
                                    && s.HomerRank.Value > hr - 3 && s.HomerRank.Value < hr + 3 && s.VisitorRank.Value > vr - 3 && s.VisitorRank.Value < vr + 3
                                && s.LeagueId == leagueGuid
                                orderby s.ScoreDate ascending
                                select new
                                {
                                    s.ScoreDataId,
                                    s.ScoreDate,
                                    s.Period,
                                    s.PageNumber,
                                    s.HomerRank,
                                    HomeTeam = t1.TeamChineseName,
                                    s.HomerScore,
                                    s.VisitorScore,
                                    VisitTeam = t2.TeamChineseName,
                                    s.VisitorRank,
                                    s.WinStart,
                                    s.WinEnd,
                                    s.DrawStart,
                                    s.DrawEnd,
                                    s.LostStart,
                                    s.LostEnd,
                                    s.MyBid,
                                    s.TrueBid,
                                }).ToList();

                    return GetJSONResult(true, JsonConvert.SerializeObject(list));
                }
                if (ws <= we && ls <= le)
                {
                    var list = (from s in DbContext.ScoreDatas
                                join t1 in DbContext.Teams on s.HomerId equals t1.TeamId
                                join t2 in DbContext.Teams on s.VisitorId equals t2.TeamId
                                where s.WinStart > ws - abs && s.WinStart < ws + abs && s.WinEnd > we - abs && s.WinEnd < we + abs
                                    && s.DrawStart > ds - abs && s.DrawStart < ds + abs && s.DrawEnd > de - abs && s.DrawEnd < de + abs
                                    && s.LostStart > ls - abs && s.LostStart < ls + abs && s.LostEnd > le - abs && s.LostEnd < le + abs
                                    && s.WinStart <= s.WinEnd && s.LostStart <= s.LostEnd
                                    && s.HomerRank.Value > hr - 3 && s.HomerRank.Value < hr + 3 && s.VisitorRank.Value > vr - 3 && s.VisitorRank.Value > vr + 3
                                && s.LeagueId == leagueGuid
                                orderby s.ScoreDate ascending
                                select new
                                {
                                    s.ScoreDataId,
                                    s.ScoreDate,
                                    s.Period,
                                    s.PageNumber,
                                    s.HomerRank,
                                    HomeTeam = t1.TeamChineseName,
                                    s.HomerScore,
                                    s.VisitorScore,
                                    VisitTeam = t2.TeamChineseName,
                                    s.VisitorRank,
                                    s.WinStart,
                                    s.WinEnd,
                                    s.DrawStart,
                                    s.DrawEnd,
                                    s.LostStart,
                                    s.LostEnd,
                                    s.MyBid,
                                    s.TrueBid,
                                }).ToList();

                    return GetJSONResult(true, JsonConvert.SerializeObject(list));
                }
                return GetJSONResult(true, "");

            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("IsDataExist")]
        public JObject IsDataExist(JObject obj)
        {
            try
            {
                dynamic data = obj;
                string period = (string)data.period;
                int intpage = int.Parse(period.Trim());
                var q = (from sd in DbContext.ScoreDatas
                         where sd.PageNumber == intpage
                         select sd).ToList();
                if (q.Count > 0)
                {
                    return GetJSONResult(true, "true");
                }
                else
                {
                    return GetJSONResult(true, "false");
                }

            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("AddScoreDataTime")]
        public JObject AddScoreDataTime(JObject obj)
        {
            try
            {
                dynamic json = obj;
                string Period = (string)json.Period;
                string ScoreDate = (string)json.ScoreDate;
                string PageNumber = (string)json.PageNumber;
                string LeagueName = (string)json.LeagueName;

                string HomerName = (string)json.HomerName;
                string HomerRank = (string)json.HomerRank;
                string HomerScore = (string)json.HomerScore;
                string VisitorName = (string)json.VisitorName;
                string VisitorRank = (string)json.VisitorRank;
                string VisitorScore = (string)json.VisitorScore;
                string MyBid = (string)json.MyBid;
                string TrueBid = (string)json.TrueBid;
                string DrawEnd = (string)json.DrawEnd;
                string DrawStart = (string)json.DrawStart;
                string LostEnd = (string)json.LostEnd;
                string LostStart = (string)json.LostStart;
                string WinEnd = (string)json.WinEnd;
                string WinStart = (string)json.WinStart;
                Model.ScoreDataTime score = new Model.ScoreDataTime();
                score.ScoreDataTimeId = Guid.NewGuid();

                score.Period = Period;
                score.ScoreDate = DateTime.Parse(ScoreDate);
                score.PageNumber = int.Parse(PageNumber);
                score.LeagueName = LeagueName;

                score.HomerName = HomerName;
                if (!String.IsNullOrEmpty(HomerRank))
                {
                    score.HomerRank = int.Parse(HomerRank);
                }
                if (!String.IsNullOrEmpty(HomerScore))
                {
                    score.HomerScore = int.Parse(HomerScore);
                }
                score.VisitorName = VisitorName;
                if (!String.IsNullOrEmpty(VisitorRank))
                {
                    score.VisitorRank = int.Parse(VisitorRank);
                }
                if (!String.IsNullOrEmpty(VisitorScore))
                {
                    score.VisitorScore = int.Parse(VisitorScore);
                }
                if (!String.IsNullOrEmpty(MyBid))
                {
                    score.MyBid = int.Parse(MyBid);
                }
                if (!String.IsNullOrEmpty(TrueBid))
                {
                    score.TrueBid = int.Parse(TrueBid);
                }

                score.DrawEnd = decimal.Parse(DrawEnd);
                score.DrawStart = decimal.Parse(DrawStart);
                score.LostEnd = decimal.Parse(LostEnd);
                score.LostStart = decimal.Parse(LostStart);
                score.WinEnd = decimal.Parse(WinEnd);
                score.WinStart = decimal.Parse(WinStart);

                score.DataAddTime = DateTime.Now;


                DbContext.ScoreDataTimes.Add(score);
                DbContext.SaveChanges();


                return GetJSONResult(true, "true");
            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("GetScoreDataTimeBySelf")]
        public JObject GetScoreDataTimeBySelf(JObject obj)
        {
            try
            {
                dynamic json = obj;
                string Period = (string)json.Period;
                int PageNumber = int.Parse(Period.Trim());

                var list = (from s in DbContext.ScoreDataTimes
                            where s.PageNumber == PageNumber
                            orderby s.DataAddTime
                            select new
                            {
                                s.ScoreDataTimeId,
                                s.ScoreDate,
                                s.LeagueName,
                                s.Period,
                                s.PageNumber,
                                s.HomerRank,
                                HomeTeam = s.HomerName,
                                s.HomerScore,
                                s.VisitorScore,
                                VisitTeam = s.VisitorName,
                                s.VisitorRank,
                                s.WinStart,
                                s.WinEnd,
                                s.DrawStart,
                                s.DrawEnd,
                                s.LostStart,
                                s.LostEnd,
                                s.MyBid,
                                s.TrueBid,
                                s.DataAddTime,
                            }).ToList();
                return GetJSONResult(true, JsonConvert.SerializeObject(list));
            }
            catch(Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("GetScoreDataTimeBySimi")]
        public JObject GetScoreDataTimeBySimi(JObject obj)
        {
            try
            {
                dynamic json = obj;
                string Period = (string)json.Period;
                int PageNumber = int.Parse(Period.Trim());
                string Abs = (string)json.Abs;
                decimal abs = decimal.Parse(Abs);

                var sdt = (from s in DbContext.ScoreDataTimes
                           where s.PageNumber == PageNumber
                           orderby s.DataAddTime descending
                           select s).ToList().FirstOrDefault();

                var listPeriod = (from s in DbContext.ScoreDataTimes
                                   where s.WinStart > sdt.WinStart - abs && s.WinStart < sdt.WinStart + abs && s.WinEnd > sdt.WinEnd - abs && s.WinEnd < sdt.WinEnd + abs
                                            && s.DrawStart > sdt.DrawStart - abs && s.DrawStart < sdt.DrawStart + abs && s.DrawEnd > sdt.DrawEnd - abs && s.DrawEnd < sdt.DrawEnd + abs
                                            && s.LostStart > sdt.LostStart - abs && s.LostStart < sdt.LostStart + abs && s.LostEnd > sdt.LostEnd - abs && s.LostEnd < sdt.LostEnd + abs
                                   select s.PageNumber).Distinct().ToList();

                var list = (from s in DbContext.ScoreDataTimes
                            where listPeriod.Contains(s.PageNumber)
                            orderby s.PageNumber , s.DataAddTime
                            select new
                            {
                                s.ScoreDataTimeId,
                                s.ScoreDate,
                                s.Period,
                                s.PageNumber,
                                s.HomerRank,
                                HomeTeam = s.HomerName,
                                s.HomerScore,
                                s.VisitorScore,
                                VisitTeam = s.VisitorName,
                                s.VisitorRank,
                                s.WinStart,
                                s.WinEnd,
                                s.DrawStart,
                                s.DrawEnd,
                                s.LostStart,
                                s.LostEnd,
                                s.MyBid,
                                s.TrueBid,
                                s.DataAddTime,
                            }).ToList();


                return GetJSONResult(true, JsonConvert.SerializeObject(list));
            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("GetScoreDataTimeBySimi2")]
        public JObject GetScoreDataTimeBySimi2(JObject obj)
        {
            try
            {
                dynamic json = obj;
                string Period = (string)json.Period;
                int PageNumber = int.Parse(Period.Trim());


                var sdt = (from s in DbContext.ScoreDataTimes
                           where s.PageNumber == PageNumber
                           orderby s.DataAddTime descending
                           select s).ToList();
                int top = sdt.Count;
                decimal AweSum = 0, AweSumS = 0, AweAve = 0, AweRia = 0;
                decimal AdeSum = 0, AdeSumS = 0, AdeAve = 0, AdeRia = 0;
                decimal AleSum = 0, AleSumS = 0, AleAve = 0, AleRia = 0;
                foreach (var s in sdt)
                {
                    AweSum += s.WinEnd;
                    AweSumS += s.WinEnd * s.WinEnd;
                    AdeSum += s.DrawEnd;
                    AdeSumS += s.DrawEnd * s.DrawEnd;
                    AleSum += s.LostEnd;
                    AleSumS += s.LostEnd * s.LostEnd;
                }
                AweAve = AweSum / top;
                AweRia = (AweSumS / top) - (AweAve * AweAve);
                AdeAve = AdeSum / top;
                AdeRia = (AdeSumS / top) - (AdeAve * AdeAve);
                AleAve = AleSum / top;
                AleRia = (AleSumS / top) - (AleAve * AleAve);
                var singleLists = (from a in DbContext.ScoreDataTimes
                                   select a.PageNumber).Distinct().ToList();
                List<int> listPeriod = new List<int>();
                foreach (int single in singleLists)
                {
                    var every = (from e in DbContext.ScoreDataTimes
                                 where e.PageNumber == single
                                 orderby e.DataAddTime ascending
                                 select e).Take(top).ToList();
                    decimal weSum = 0, weSumS = 0, weAve = 0, weRia = 0;
                    decimal deSum = 0, deSumS = 0, deAve = 0, deRia = 0;
                    decimal leSum = 0, leSumS = 0, leAve = 0, leRia = 0;
                    foreach (var e in every)
                    {
                        weSum += e.WinEnd;
                        weSumS += e.WinEnd * e.WinEnd;
                        deSum += e.DrawEnd;
                        deSumS += e.DrawEnd * e.DrawEnd;
                        leSum += e.LostEnd;
                        leSumS += e.LostEnd * e.LostEnd;
                    }
                    weAve = weSum / top;
                    weRia = (weSumS / top) - (weAve * weAve);
                    deAve = deSum / top;
                    deRia = (deSumS / top) - (deAve * deAve);
                    leAve = leSum / top;
                    leRia = (leSumS / top) - (leAve * leAve);
                    if (Math.Abs(AweAve - weAve) < decimal.Parse("0.444") && Math.Abs(AweRia - weRia) < decimal.Parse("0.0444") &&
                        Math.Abs(AdeAve - deAve) < decimal.Parse("0.444") && Math.Abs(AdeRia - deRia) < decimal.Parse("0.00444") &&
                        Math.Abs(AleAve - leAve) < decimal.Parse("0.444") && Math.Abs(AleRia - leRia) < decimal.Parse("0.0444"))
                    {
                        listPeriod.Add(single);
                    }
                }

                var list = (from s in DbContext.ScoreDataTimes
                            where listPeriod.Contains(s.PageNumber)
                            orderby s.Period, s.DataAddTime
                            select new
                            {
                                s.ScoreDataTimeId,
                                s.ScoreDate,
                                s.LeagueName,
                                s.Period,
                                s.PageNumber,
                                s.HomerRank,
                                HomeTeam = s.HomerName,
                                s.HomerScore,
                                s.VisitorScore,
                                VisitTeam = s.VisitorName,
                                s.VisitorRank,
                                s.WinStart,
                                s.WinEnd,
                                s.DrawStart,
                                s.DrawEnd,
                                s.LostStart,
                                s.LostEnd,
                                s.MyBid,
                                s.TrueBid,
                                s.DataAddTime,
                            }).ToList();


                return GetJSONResult(true, JsonConvert.SerializeObject(list));
            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("GetScoreDataTimeBySimiDoSome")]
        public JObject GetScoreDataTimeBySimiDoSome(JObject obj)
        {
            try
            {
                dynamic json = obj;
                string Period = (string)json.Period;
                int PageNumber = int.Parse(Period.Trim());


                var sdt = (from s in DbContext.ScoreDataTimes
                           where s.PageNumber == PageNumber
                           orderby s.DataAddTime descending
                           select s).ToList();
                int top = sdt.Count;
                decimal AweSum = 0, AweSumS = 0, AweAve = 0, AweRia = 0;
                decimal AdeSum = 0, AdeSumS = 0, AdeAve = 0, AdeRia = 0;
                decimal AleSum = 0, AleSumS = 0, AleAve = 0, AleRia = 0;
                foreach (var s in sdt)
                {
                    AweSum += s.WinEnd;
                    AweSumS += s.WinEnd * s.WinEnd;
                    AdeSum += s.DrawEnd;
                    AdeSumS += s.DrawEnd * s.DrawEnd;
                    AleSum += s.LostEnd;
                    AleSumS += s.LostEnd * s.LostEnd;
                }
                AweAve = AweSum / top;
                AweRia = (AweSumS / top) - (AweAve * AweAve);
                AdeAve = AdeSum / top;
                AdeRia = (AdeSumS / top) - (AdeAve * AdeAve);
                AleAve = AleSum / top;
                AleRia = (AleSumS / top) - (AleAve * AleAve);

                var singleLists = (from a in DbContext.ScoreDataTimes
                                   select a.PageNumber).Distinct().ToList();
                List<List<string>> listPeriod = new List<List<string>>();
                foreach (int single in singleLists)
                {
                    var every = (from e in DbContext.ScoreDataTimes
                                 where e.PageNumber == single
                                 orderby e.DataAddTime ascending
                                 select e).Take(top).ToList();
                    decimal weSum = 0, weSumS = 0, weAve = 0, weRia = 0;
                    decimal deSum = 0, deSumS = 0, deAve = 0, deRia = 0;
                    decimal leSum = 0, leSumS = 0, leAve = 0, leRia = 0;
                    string Home = ""; string Visit = "";
                    DateTime dtDate = DateTime.Now; 
                    foreach (var e in every)
                    {
                        weSum += e.WinEnd;
                        weSumS += e.WinEnd * e.WinEnd;
                        deSum += e.DrawEnd;
                        deSumS += e.DrawEnd * e.DrawEnd;
                        leSum += e.LostEnd;
                        leSumS += e.LostEnd * e.LostEnd;
                        Home = e.HomerName;
                        Visit = e.VisitorName;
                        dtDate = e.ScoreDate;  
                    }
                    weAve = weSum / top;
                    weRia = (weSumS / top) - (weAve * weAve);
                    deAve = deSum / top;
                    deRia = (deSumS / top) - (deAve * deAve);
                    leAve = leSum / top;
                    leRia = (leSumS / top) - (leAve * leAve);
                    if (Math.Abs(AweAve - weAve) < decimal.Parse("0.444") && Math.Abs(AweRia - weRia) < decimal.Parse("0.0444") &&
                        Math.Abs(AdeAve - deAve) < decimal.Parse("0.444") && Math.Abs(AdeRia - deRia) < decimal.Parse("0.00444") &&
                        Math.Abs(AleAve - leAve) < decimal.Parse("0.444") && Math.Abs(AleRia - leRia) < decimal.Parse("0.0444"))
                    {
                        List<string> listP = new List<string>();
                        listP.Add(single + "");
                        listP.Add(weAve.ToString("f7"));
                        listP.Add(weRia.ToString("f7"));
                        listP.Add(deAve.ToString("f7"));
                        listP.Add(deRia.ToString("f7"));
                        listP.Add(leAve.ToString("f7"));
                        listP.Add(leRia.ToString("f7"));
                        listP.Add(Home);
                        listP.Add(Visit);
                        listP.Add(dtDate.ToString("yyyy-MM-dd HH:mm"));
                        var tbid = (from e in DbContext.ScoreDataTimes
                                     where e.PageNumber == single
                                     orderby e.DataAddTime descending
                                     select e).FirstOrDefault();
                        if(tbid.TrueBid != null)
                        {
                            listP.Add(tbid.TrueBid.ToString());
                        } 
                        listPeriod.Add(listP);
                    }
                }

                return GetJSONResult(true, JsonConvert.SerializeObject(listPeriod));
            }
            catch (Exception ex)
            {
                return GetJSONResult(false, "", ex.Message);
            }
        }

    }
}